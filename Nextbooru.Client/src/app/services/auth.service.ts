import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest, RegisterRequest, SessionResponse } from '../backend/backend-types';
import { BackendEndpoints } from '../backend/backend-enpoints';

@Injectable({
  providedIn: 'root'
})
export class AuthService {

  constructor(public httpClient: HttpClient) {}

  login(credentials: LoginRequest) {
    return this.httpClient.post<SessionResponse>(
      BackendEndpoints.auth.login,
      credentials
    );
  }

  register(credentials: RegisterRequest) {
    return this.httpClient.post<SessionResponse>(
      BackendEndpoints.auth.register,
      credentials
    );
  }

  logout() {
    return this.httpClient.delete<void>(BackendEndpoints.auth.logout);
  }
}
