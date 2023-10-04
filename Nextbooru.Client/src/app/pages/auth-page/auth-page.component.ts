import { ChangeDetectionStrategy, Component, computed, signal } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { Observable, map } from 'rxjs';
import { Select, Store } from '@ngxs/store';
import { Login } from 'src/app/store/actions/auth.actions';
import { AuthState, AuthStateModel } from 'src/app/store/state/auth.state';
import { SpinnerOverlayComponent } from 'src/app/components/spinner-overlay/spinner-overlay.component';

@Component({
  selector: 'app-auth-page',
  standalone: true,
  imports: [SharedModule, SpinnerOverlayComponent],
  templateUrl: './auth-page.component.html',
  styleUrls: ['./auth-page.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AuthPageComponent {
  constructor(private store: Store) {}

  activeTab = signal("login");

  loginHeaderClassess = computed(() => ({ active: this.activeTab() === "login" }));
  registerHeaderClasses = computed(() => ({ active: this.activeTab() === "register" }));

  @Select(AuthState) authState$!: Observable<AuthStateModel>;

  loading$ = this.authState$.pipe(
    map(s => s.loading ?? false)
  );
  loginError$ = this.authState$.pipe(
    map(s => s.loginError)
  );

  loginForm = new FormGroup({
    username: new FormControl('', [
      Validators.required
    ]),
    password: new FormControl('', [
      Validators.required
    ])
  });

  loginFormValid$ = this.loginForm.statusChanges.pipe(
    map(() => this.loginForm.valid)
  );

  registerForm = new FormGroup({
    username: new FormControl('', [
      Validators.required
    ]),
    password: new FormControl('', [
      Validators.required
    ]),
    repeatPassword: new FormControl('', [
      Validators.required
    ]),
    email: new FormControl('', [
      Validators.email
    ])
  });

  login(event: Event) {
    event.preventDefault();
    if (this.loginForm.invalid) {
      return;
    }

    this.store.dispatch(new Login({
      username: this.loginForm.controls.username.value!,
      password: this.loginForm.controls.password.value!
    }));
  }

  register(event: Event) {
    event.preventDefault();
  }
}
