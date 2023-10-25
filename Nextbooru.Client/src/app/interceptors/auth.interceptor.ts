import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse,
  HttpContextToken
} from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { BackendEndpoints } from '../backend/backend-enpoints';
import { Store } from '@ngxs/store';
import { Logout, SilentLogout } from '../store/actions/auth.actions';

export const SILENT_LOGOUT_ON_401 = new HttpContextToken(() => false);

/**
 *  Adds `withCredentials: true` option and redirects to `/auth`
 */
@Injectable()
export class AuthInterceptor implements HttpInterceptor {

  constructor(private store: Store) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    const newReq = request.clone({
      withCredentials: true
    });

    if (this.ignoreErrorResponse(request)) {
      return next.handle(newReq);
    }

    return next.handle(newReq)
      .pipe(
        tap({
          error: err => err instanceof HttpErrorResponse && this.handleError(err, request.context.get(SILENT_LOGOUT_ON_401))
        })
      );
  }

  private static readonly skipEndpoints = [
    BackendEndpoints.auth.login,
    BackendEndpoints.auth.register,
    BackendEndpoints.auth.logout
  ]

  private ignoreErrorResponse(req: HttpRequest<unknown>) {
    return AuthInterceptor.skipEndpoints.some(endpoint => {
      return req.url.startsWith(endpoint);
    });
  }

  private handleError(err: HttpErrorResponse, silentLogout = false) {
    if (err.status && err.status == 401) {
      this.store.dispatch(silentLogout ? new SilentLogout() : new Logout());
    }
  }
}
