import { createContext } from "react";

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