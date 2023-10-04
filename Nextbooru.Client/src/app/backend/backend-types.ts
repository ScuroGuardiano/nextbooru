export interface LoginRequest {
  username: string;
  password: string;
}

export interface RegisterRequest {
  username: string;
  password: string;
  email?: string;
}

export interface SessionResponse {
  user:       SessionUser;
  loggedInIP: string;
  lastIP:     string;
  userAgent:  string;
  createdAt:  string;
  lastAccess: string;
}

export interface SessionUser {
  username:    string;
  displayName: string;
}

export interface ApiErrorResponse {
  type: "ApiErrorResponse",
  statusCode: number;
  message?: string;
  errorClrType?: string;
  errorCode?: string;
}
