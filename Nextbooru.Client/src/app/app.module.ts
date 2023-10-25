import { APP_INITIALIZER, NgModule, isDevMode } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutModule } from './layout/layout.module';
import { NgxsModule } from '@ngxs/store';
import { NgxsStoragePluginModule } from '@ngxs/storage-plugin';
import { AuthState } from './store/state/auth.state';
import { httpInterceptorProviders } from './interceptors/interceptors';
import { AuthService } from './services/auth.service';

function appInitFactory(authService: AuthService) {
  return () => authService.checkSessionValidity(true);
}

@NgModule({
  declarations: [
    AppComponent,
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    LayoutModule,
    HttpClientModule,
    NgxsModule.forRoot(
      [ AuthState ],
      { developmentMode: isDevMode() }
    ),
    NgxsStoragePluginModule.forRoot({
      key: AuthState
    })
  ],
  providers: [
    httpInterceptorProviders,
    {
      provide: APP_INITIALIZER,
      multi: true,
      useFactory: appInitFactory,
      deps: [AuthService]
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
