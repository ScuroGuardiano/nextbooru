import { inject } from '@angular/core';
import { CanActivateFn, Router } from '@angular/router';
import { Store } from '@ngxs/store';
import { AUTH_STATE_TOKEN } from '../store/state/auth.state';

export const authGuard: CanActivateFn = (route, state) => {
  const store = inject(Store);
  const router = inject(Router);

  if (store.selectSnapshot(AUTH_STATE_TOKEN).isLoggedIn) {
    return true;
  }
  return router.parseUrl("/auth");
};
