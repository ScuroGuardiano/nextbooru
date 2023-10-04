import { NgModule, isDevMode } from '@angular/core';
import { HttpClientModule } from '@angular/common/http';
import { BrowserModule } from '@angular/platform-browser';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { LayoutModule } from './layout/layout.module';
import { NGXS_PLUGINS, NgxsModule } from '@ngxs/store';
import { NgxsStoragePluginModule } from '@ngxs/storage-plugin';
import { AuthState } from './store/state/auth.state';

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
      [ AuthState ],
      { developmentMode: isDevMode() }
    ),
    NgxsStoragePluginModule.forRoot({
      key: AuthState
    })
  ],
  bootstrap: [AppComponent]
})
export class AppModule { }
