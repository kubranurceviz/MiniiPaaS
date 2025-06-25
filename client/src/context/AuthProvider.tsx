import { useState, useEffect } from 'react';
import { jwtDecode } from 'jwt-decode';
import api from '../api';
import { fetchRoles, type SystemRole } from '../services/roleService';
import { AuthContext, type User } from './AuthContext';

export const AuthProvider = ({ children }: { children: React.ReactNode }) => {
  const [token, setToken] = useState<string | null>(localStorage.getItem('token'));
  const [user, setUser] = useState<User | null>(null);
  const [roles, setRoles] = useState<SystemRole[]>([]);
  const [isRoleLoaded, setIsRoleLoaded] = useState(false);

  // Roller backend'den çekilir
  useEffect(() => {
    const loadRoles = async () => {
      try {
        const systemRoles = await fetchRoles();
        setRoles(systemRoles);
      } catch (error) {
        console.error('Roller yüklenemedi:', error);
      } finally {
        setIsRoleLoaded(true);
      }
    };
    loadRoles();
  }, []);

  // Token değiştiğinde axios header ve user state güncellenir
  useEffect(() => {
    if (token) {
      api.defaults.headers.common['Authorization'] = `Bearer ${token}`;
      try {
        const decoded = jwtDecode<{ email: string; Role: SystemRole; CompanyId: number }>(token);

        if (!roles.includes(decoded.Role)) {
          throw new Error('Geçersiz rol atandı');
        }

        setUser({
          email: decoded.email,
          role: decoded.Role,
          companyId: decoded.CompanyId,
        });
      } catch (error) {
        console.error('Token çözümlenirken hata:', error);
        setUser(null);
        setToken(null);
        localStorage.removeItem('token');
      }
    } else {
      delete api.defaults.headers.common['Authorization'];
      setUser(null);
    }
  }, [token, roles]);

  const login = async (email: string, password: string) => {
    const res = await api.post('/auth/login', { email, password });
    const token = res.data.token;
    localStorage.setItem('token', token);
    setToken(token);
  };

  const logout = () => {
    localStorage.removeItem('token');
    setToken(null);
    setUser(null);
  };

  return (
    <AuthContext.Provider value={{ token, user, roles, login, logout, isRoleLoaded }}>
      {children}
    </AuthContext.Provider>
  );
};