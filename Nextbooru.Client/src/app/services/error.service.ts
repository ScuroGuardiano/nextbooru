import { HttpErrorResponse } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { ApiErrorResponse } from '../backend/backend-types';

@Injectable({
  providedIn: 'root'
})
export class ErrorService {

  constructor() { }

  errorToHuman(err: any, extended = false) {
    const apiError = this.apiErrorResponseFromError(err);

    let message = "";

    message = extended ?
      this.apiErrorToMessageExtended(apiError, message)
      : this.apiErrorToMessage(apiError, message);

    if (message != "") {
      return message;
    }

    if (err instanceof HttpErrorResponse) {
      switch (err.status) {
        case 0:
          return "Uknown error, check your internet connection or try again later.";
        case 504:
          return "Gateway timeout, server is unreachable. Try again later or contact administrator."
        default:
          return err.statusText;
      }
    }

    if (typeof err.message == 'string') {
      return err.message;
    }
    return "Uknown error";
  }

  private apiErrorToMessage(apiError: ApiErrorResponse | null, message: string) {
    if (apiError && apiError.message) {
      message = apiError.message;
    }
    return message;
  }

  private apiErrorToMessageExtended(apiError: ApiErrorResponse | null, message: string) {
    if (apiError) {
      if (apiError.statusCode) {
        message += `Status Code: ${apiError.statusCode};`;
      }
      if (apiError.errorCode) {
        message += `Error Code: ${apiError.errorCode};`;
      }
      if (apiError.message) {
        message += `Message: ${apiError.message};`;
      }
      if (apiError.errorClrType) {
        message += `CLRType: ${apiError.errorClrType};`;
      }
    }
    return message;
  }

  apiErrorResponseFromError(err: any): ApiErrorResponse | null {
    if (!(err instanceof HttpErrorResponse)) {
      return null;
    }

    if (err.error && err.error.type == "ApiErrorResponse") {
      return err.error as ApiErrorResponse;
    }

    return null;
  }
}
