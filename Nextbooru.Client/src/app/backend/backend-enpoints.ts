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
  upload: `${apiUrl}/upload`,
  images: {
    list: `${apiUrl}/images`,
    get: (id: number) => `${apiUrl}/images/${id}`,
    upvote: (id: number) => `${apiUrl}/images/${id}/upvote`,
    downvote: (id: number) => `${apiUrl}/images/${id}/downvote`,
    makePublic: (id: number) => `${apiUrl}/images/${id}/make-public`,
    makeNonPublic: (id: number) => `${apiUrl}/images/${id}/make-non-public`
  },
  tags: {
    autocomplete: `${apiUrl}/tags/autocomplete`,
    orderableFields: `${apiUrl}/tags/orderable-fields`,
    list: `${apiUrl}/tags`,
  }
} as const;
