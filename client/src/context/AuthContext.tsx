import React, { createContext, useState, useEffect, ReactNode } from "react";
import axios from "axios";

interface AuthContextType {
  token: string | null;
  login: (email: string, password: string) => Promise<void>;
  logout: () => void;
}

export const AuthContext = createContext<AuthContextType>({
  token: null,
  login: async () => {},
  logout: () => {},
});

export const AuthProvider = ({ children }: { children: ReactNode }) => {
  const [token, setToken] = useState<string | null>(() => {
    return localStorage.getItem("token");
  });

  useEffect(() => {
    if (token) {
      axios.defaults.headers.common["Authorization"] = Bearer ${token};
    } else {
      delete axios.defaults.headers.common["Authorization"];
    }
  }, [token]);

  const login = async (email: string, password: string) => {
    const response = await axios.post("http://localhost:5000/api/auth/login", {
      email,
      password,
    });
    const receivedToken = response.data.token;
    setToken(receivedToken);
    localStorage.setItem("token", receivedToken);
  };

  const logout = () => {
    setToken(null);
    localStorage.removeItem("token");
  };

  return (
    <AuthContext.Provider value={{ token, login, logout }}>
      {children}
    </AuthContext.Provider>
  );
};