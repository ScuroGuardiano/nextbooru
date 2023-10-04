import { ChangeDetectionStrategy, Component, computed, signal } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { FormControl, FormGroup, Validators } from '@angular/forms';
import { map } from 'rxjs';
import { Store } from '@ngxs/store';
import { Login } from 'src/app/store/actions/auth.actions';

@Component({
  selector: 'app-auth-page',
  standalone: true,
  imports: [SharedModule],
  templateUrl: './auth-page.component.html',
  styleUrls: ['./auth-page.component.scss'],
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class AuthPageComponent {
  constructor(private store: Store) {}

  activeTab = signal("login");

  loginHeaderClassess = computed(() => ({ active: this.activeTab() === "login" }));
  registerHeaderClasses = computed(() => ({ active: this.activeTab() === "register" }));

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
