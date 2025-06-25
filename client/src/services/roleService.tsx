import api from '../api';

export const fetchRoles = async (): Promise<string[]> => {
  const response = await api.get('/utils/roles');
  return response.data;
};

export type SystemRole = string;