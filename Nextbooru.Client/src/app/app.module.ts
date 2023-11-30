import { APP_INITIALIZER, NgModule, isDevMode } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutModule } from './layout/layout.module';
import { NgxsModule, Store } from '@ngxs/store';
import { NgxsStoragePluginModule } from '@ngxs/storage-plugin';
import { NgxsReduxDevtoolsPluginModule } from '@ngxs/devtools-plugin';
import { AUTH_STATE_TOKEN, AuthState } from './store/state/auth.state';
import { httpInterceptorProviders } from './interceptors/interceptors';
import { AuthService } from './services/auth.service';
import { PostsState } from './store/state/posts.state';

function appInitFactory(authService: AuthService, store: Store) {
  return () => {
    if (store.selectSnapshot(AUTH_STATE_TOKEN).isLoggedIn) {
      authService.checkSessionValidity(true);
    }
  }
}

@NgModule({
  declarations: [
    AppComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule,
    LayoutModule,
    HttpClientModule,
    NgxsModule.forRoot(
      [ AuthState, PostsState ],
      { developmentMode: isDevMode() }
    ),
    NgxsStoragePluginModule.forRoot({
      key: AuthState
    }),
    NgxsReduxDevtoolsPluginModule.forRoot()
  ],
  providers: [
    httpInterceptorProviders,
    {
      provide: APP_INITIALIZER,
      multi: true,
      useFactory: appInitFactory,
      deps: [AuthService, Store]
    }
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
