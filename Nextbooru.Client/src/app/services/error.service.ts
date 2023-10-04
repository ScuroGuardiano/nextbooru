import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  constructor() { }

  errorToHuman(err: any) {
    if (err instanceof HttpErrorResponse) {
      return this.httpErrorResponseToHuman(err);
    }
    if (typeof err.message == 'string') {
      return err.message;
    }
    return "Uknown error";
  }

  httpErrorResponseToHuman(err: HttpErrorResponse) {
    return "ScuroGuardiano you idiot, implement ErrorService uwu";
  }
}
