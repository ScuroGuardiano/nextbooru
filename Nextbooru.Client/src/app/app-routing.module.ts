import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';
import { authGuard } from './guards/auth.guard';

const routes: Routes = [
  {
    path: "",
    pathMatch: "full",
    loadComponent: () => import("./pages/search-page/search-page.component")
      .then(c => c.SearchPageComponent)
  },
  {
    path: "auth",
    loadComponent: () => import("./pages/auth-page/auth-page.component")
      .then(c => c.AuthPageComponent)
  },
  {
    path: "upload",
    loadComponent: () => import("./pages/upload-page/upload-page.component")
      .then(c => c.UploadPageComponent)
  },
  {
    path: "settings",
    loadComponent: () => import("./pages/settings-page/settings-page.component")
      .then(c => c.SettingsPageComponent),
    canActivate: [authGuard]
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
