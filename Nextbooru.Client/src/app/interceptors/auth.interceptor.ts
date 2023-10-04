import { Injectable } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor,
  HttpErrorResponse
} from '@angular/common/http';
import { Observable, tap } from 'rxjs';
import { BackendEndpoints } from '../backend/backend-enpoints';
import { Store } from '@ngxs/store';
import { Logout } from '../store/actions/auth.actions';

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

    if (this.skipRedirect(request)) {
      return next.handle(request);
    }

    return next.handle(newReq)
      .pipe(
        tap({
          error: err => err instanceof HttpErrorResponse ?? this.handleError(err)
        })
      );
  }

  private static readonly skipEndpoints = [
    BackendEndpoints.auth.login,
    BackendEndpoints.auth.register,
    BackendEndpoints.auth.logout
  ]

  private skipRedirect(req: HttpRequest<unknown>) {
    return AuthInterceptor.skipEndpoints.some(endpoint => {
      return req.url.startsWith(endpoint);
    });
  }

  private handleError(err: HttpErrorResponse) {
    if (err.status && err.status == 401) {
      this.store.dispatch(new Logout());
    }
  }
}
