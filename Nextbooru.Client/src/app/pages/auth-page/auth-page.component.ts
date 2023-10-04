import { ChangeDetectionStrategy, Component, computed, signal } from '@angular/core';
import { SharedModule } from 'src/app/shared/shared.module';
import { AbstractControl, FormControl, FormGroup, ValidationErrors, ValidatorFn, Validators } from '@angular/forms';
import { Observable, map } from 'rxjs';
import { Select, Store } from '@ngxs/store';
import { Login, Register } from 'src/app/store/actions/auth.actions';
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
  registerError$ = this.authState$.pipe(
    map(s => s.registerError)
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
    // TODO: Add better validation for usernames.
    username: new FormControl('', [
      Validators.required,
      Validators.minLength(3),
      Validators.maxLength(16)
    ]),
    password: new FormControl('', [
      Validators.required,
      Validators.minLength(8),
      Validators.maxLength(72)
    ]),
    repeatPassword: new FormControl('', [
      Validators.required
    ]),
    email: new FormControl('', [
      Validators.email
    ])
  }, { validators: [ this.confirmPasswordValidator ] });

  registerFormValid$ = this.registerForm.statusChanges.pipe(
    map(() => {
      return this.registerForm.valid;
    })
  );

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
    if (this.registerForm.invalid) {
      return;
    }

    let email = this.registerForm.controls.password.value == ""
      ? null : this.registerForm.controls.password.value;

    this.store.dispatch(new Register({
      username: this.registerForm.controls.username.value!,
      password: this.registerForm.controls.password.value!,
      email: email!
    }));
  }

  private confirmPasswordValidator(group: AbstractControl): ValidationErrors | null {
    const password = group.get('password')?.value;
    const repeatPassword = group.get('repeatPassword')?.value;
    return password === repeatPassword ? null : { passwordsNotMatching: true }
  }
}
