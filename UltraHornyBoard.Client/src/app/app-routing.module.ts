import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

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
  }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
