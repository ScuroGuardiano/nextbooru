import { Injectable, NgZone } from '@angular/core';
import {
  HttpRequest,
  HttpHandler,
  HttpEvent,
  HttpInterceptor
} from '@angular/common/http';
import { Observable, finalize } from 'rxjs';
import { LoadingIndicatorService } from '../services/loading-indicator.service';

@Injectable()
export class LoadingIndicatorInterceptor implements HttpInterceptor {

  constructor(private loadingIndicatorService: LoadingIndicatorService) {}

  intercept(request: HttpRequest<unknown>, next: HttpHandler): Observable<HttpEvent<unknown>> {
    this.loadingIndicatorService.addLoading();
    return next.handle(request)
      .pipe(
        finalize(() => this.loadingIndicatorService.removeLoading())
      );
  }
}
