import { createContext, useState, useEffect, type ReactNode } from 'react';
import { jwtDecode } from 'jwt-decode';
import api from '../api';
import { fetchRoles, type SystemRole } from '../services/roleService';

export interface User {
  email: string;
  role: SystemRole;
  companyId: number;
}

export interface AuthContextType {
  token: string | null;
  user: User | null;
  roles: SystemRole[];
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
  isRoleLoaded: boolean;
}

export const AuthContext = createContext<AuthContextType>({
  token: null,
  user: null,
  roles: [],
  login: async () => {},
  logout: () => {},
  isRoleLoaded: false,
});

export const AuthProvider = ({ children }: { children: ReactNode }) => {
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

  // Token varsa user'ı set et
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
        logout();
      }
    } else {
      delete api.defaults.headers.common['Authorization'];
      setUser(null);
    }
  }, [token, roles]);

  const login = async (email: string, password: string) => {
    const res = await api.post('/auth/login', { email, password });
    const receivedToken = res.data.token;

    localStorage.setItem('token', receivedToken);
    setToken(receivedToken);
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