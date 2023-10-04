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
