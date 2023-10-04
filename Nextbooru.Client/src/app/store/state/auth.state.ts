import { Injectable, NgZone } from "@angular/core";
import { Action, State, StateContext, StateToken } from "@ngxs/store";
import { finalize, retry, tap } from "rxjs";
import { AuthService } from "src/app/services/auth.service";
import { ErrorService } from "src/app/services/error.service";
import { Login, Logout, Register } from "../actions/auth.actions";
import { LoggerService } from "src/app/services/logger.service";
import { Router } from "@angular/router";
import { ResetState, ResettableState } from "../resettable-state/resettable-state";

export const AUTH_STATE_TOKEN = new StateToken<AuthStateModel>('auth');

export interface AuthStateModel {
  isLoggedIn?: boolean;
  username?: string;
  loading?: boolean;
  logoutLoading?: boolean;
  loginError?: string;
  registerError?: string;
}

type Context = StateContext<AuthStateModel>;

@ResettableState<AuthStateModel>({
  name: AUTH_STATE_TOKEN,
  defaults: {}
})
@Injectable()
export class AuthState {
  constructor(
    private authService: AuthService,
    private errorService: ErrorService,
    private logger: LoggerService,
    private router: Router,
    private ngZone: NgZone
  ) {}

  @Action(Login, { cancelUncompleted: true })
  private login(ctx: Context, action: Login) {
    ctx.patchState({
      loading: true,
    })

    return this.authService.login({...action.payload}).pipe(
      tap({
        next: res => {
          ctx.setState({
            username: res.user.displayName,
            isLoggedIn: true
          });
          this.ngZone.run(() => this.router.navigateByUrl("/"));
        },
        error: err => {
          ctx.setState({
            loginError: this.errorService.errorToHuman(err)
          });
          this.logger.error(err);
        }
      }),
      finalize(() => ctx.patchState({ loading: false }))
    );
  }

  @Action(Register, { cancelUncompleted: true })
  private register(ctx: Context, action: Register) {
    ctx.patchState({
      loading: true
    });

    return this.authService.register({ ...action.payload }).pipe(
      tap({
        next: res => {
          ctx.setState({
            username: res.user.displayName,
            isLoggedIn: true
          });
          this.ngZone.run(() => this.router.navigateByUrl("/"));
        },
        error: err => {
          ctx.setState({
            registerError: this.errorService.errorToHuman(err)
          });
          this.logger.error(err);
        }
      }),
      finalize(() => ctx.patchState({ loading: false }))
    );
  }

  @Action(Logout)
  private logout(ctx: Context) {
    ctx.patchState({
      logoutLoading: true
    });

    return this.authService.logout()
      .pipe(
        tap({
          error: err => {
            this.logger.error("Logout request failed.")
            this.logger.error(err);
          }
        }),
        finalize(() => {
          // ctx.patchState({ logoutLoading: false }) useless coz we're resetting store anyways.
          ctx.dispatch(new ResetState());
          this.ngZone.run(() => this.router.navigateByUrl("/auth"));
        })
      )
  }
}
