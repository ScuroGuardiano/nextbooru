export interface ILoginPayload {
  username: string;
  password: string;
}

export class Login {
  static readonly type = '[Auth] Login';
  constructor(public payload: ILoginPayload) {}
}

export interface IRegisterPayload {
  username: string;
  password: string;
  email?: string;
}

export class Register {
  static readonly type = '[Auth] Register';
  constructor(public payload: IRegisterPayload) {}
}

export class Logout {
  static readonly type = '[Auth] Logout';
}

/**
 * The same as Logout but won't redirect user to log in page and won't set `logoutLoading` state to true.
 */
export class SilentLogout {
  static readonly type = `[Auth] Silent Logout`
}
