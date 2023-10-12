import { environment } from "src/environments/environment"

// Make sure that URL doesn't end with backslash
const apiUrl = environment.backendUrl.endsWith('/')
  ? environment.backendUrl.substring(0, environment.backendUrl.length - 1)
  : environment.backendUrl;

export const BackendEndpoints = {
  auth: {
    login: `${apiUrl}/auth/login`,
    register: `${apiUrl}/auth/register`,
    logout: `${apiUrl}/auth/logout`,
    currentSession: `${apiUrl}/auth/currentSession`
  },
  upload: `${apiUrl}/upload`
} as const;
