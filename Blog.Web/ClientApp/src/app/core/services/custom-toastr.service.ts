import { ErrorResponse } from './../responses/ErrorResponse';
import { Injectable } from '@angular/core';
import { IndividualConfig, ToastrService } from 'ngx-toastr';
import { Status } from '../enums/Status';

/** Custom toastr service. */
@Injectable({
  providedIn: 'root'
})
export class CustomToastrService {
  /**
   * @param options IndividualConfig
   */
  options: IndividualConfig;

  /**
   * @param _toastrService ToastrService
   */
  constructor(private _toastrService: ToastrService) {
    this.options = this._toastrService.toastrConfig;
    this.options.timeOut = 3000;
    this.options.progressBar = true;
    this.options.progressAnimation = 'increasing';
  }

  /**
   * Display custom toastr message.
   * @param title string
   * @param message string
   * @param type string
   *
   * @returns void
   */
  displayCustomToastrMessage(title: string, message: string, type: string): void {
    this._toastrService.show(message, title, this.options, 'toast-' + type);
  }

  /**
   * Display success message.
   * @param title string
   *
   * @returns void
   */
  displaySuccessMessage(title: string): void {
    this._toastrService.success(title, Status[Status.Success], this.options);
  }

  // Errors.

  /**
   * Display error message.
   * @param error ErrorResponse
   *
   * @returns void
   */
  displayErrorMessage(error: ErrorResponse): void {
    if(error.error !== undefined){
      // Not found error.
      if(error.error.title !== undefined){
        this.notFoundError(error.error.title, error.status);
      }

      // Model fields error.
      if(error.error.errors !== undefined){
        this.modelFieldError(error.error.errors, error.status);
      }

      // custom error or bad request
      if(error.error.errorMessage !== undefined){
        this.customErrorOrBadRequest(error.error.errorMessage, error.status);
      }
    }
  }

  /**
   * Not found error.
   * @param title string
   * @param status any
   *
   * @returns void
   */
  notFoundError(title: string, status: any): void {
    this._toastrService.error(title, status + ' ' + Status[Status.Error], this.options);
  }

  /**
   * Model field error.
   * @param errors object
   * @param status any
   *
   * @returns void
   */
  modelFieldError(errors: object, status: any): void {
    for (const [key, value] of Object.entries(errors)) {
      this._toastrService.error(`${key}: ${value}`, status + ' ' + Status[Status.Error], this.options);
    }
  }

  /**
   * Custom errorOr bad request.
   * @param error string
   * @param status any
   *
   * @returns void
   */
  customErrorOrBadRequest(error: string, status: any): void {
    this._toastrService.error(error, status + ' ' + Status[Status.Error], this.options);
  }
}
