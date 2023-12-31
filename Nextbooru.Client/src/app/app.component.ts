import { Component } from '@angular/core';
import { NavigationCancel, NavigationEnd, NavigationError, NavigationStart, Router } from '@angular/router';
import { LoadingIndicatorService } from './services/loading-indicator.service';
import { AuthService } from './services/auth.service';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent {
  constructor(router: Router, loadingIndicatorService: LoadingIndicatorService, private authService: AuthService) {
    router.events.subscribe(e => {
      if (e instanceof NavigationStart) {
        loadingIndicatorService.addLoading();
      }

      if (
        e instanceof NavigationEnd
        || e instanceof NavigationCancel
        || e instanceof NavigationError
      ) {
        loadingIndicatorService.removeLoading();
      }
    });
  }
}
