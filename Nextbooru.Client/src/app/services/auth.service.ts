import { HttpClient, HttpContext } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { LoginRequest, RegisterRequest, SessionResponse } from '../backend/backend-types';
import { BackendEndpoints } from '../backend/backend-enpoints';
import { firstValueFrom } from 'rxjs';
import { SILENT_LOGOUT_ON_401 } from '../interceptors/auth.interceptor';

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

  async checkSessionValidity(logoutSilently = false): Promise<SessionResponse | null> {
    try {
      return await firstValueFrom(this.httpClient.get<SessionResponse>(
        BackendEndpoints.auth.currentSession,
        {
          context: new HttpContext().set(SILENT_LOGOUT_ON_401, logoutSilently)
        }
      ));
    }
    catch(err) {
      // TODO: Add handling for other codes than 401
      return null;
    }
  }
}
