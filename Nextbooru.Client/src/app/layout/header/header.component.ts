import { Component } from '@angular/core';
import { Select, Store } from '@ngxs/store';
import { Observable, filter, map } from 'rxjs';
import { Logout } from 'src/app/store/actions/auth.actions';
import { AuthState, AuthStateModel } from 'src/app/store/state/auth.state';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.scss']
})
export class HeaderComponent {
  constructor(private store: Store) {}

  @Select(AuthState) authState$!: Observable<AuthStateModel>;

  isLoggedIn$ = this.authState$.pipe(
    map(s => s.isLoggedIn)
  );

  username$ = this.authState$.pipe(
    map(s => s.username)
  );

  logout(event: Event) {
    event.preventDefault();
    this.store.dispatch(new Logout());
  }
}
