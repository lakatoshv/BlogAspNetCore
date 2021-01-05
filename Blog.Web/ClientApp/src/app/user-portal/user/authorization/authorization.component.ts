import { AuthorizationForm } from 'src/app/core/forms/user/AuthorizationForm';
import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { UsersService } from 'src/app/core/services/users-services/users.service';
import { Router } from '@angular/router';
import { JwtToken } from 'src/app/core/models/JwtToken';
import { AccountsService } from 'src/app/core/services/users-services/account.sevice';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';
import { CustomToastrService } from 'src/app/core/services/custom-toastr.service';
import { ErrorResponse } from 'src/app/core/responses/ErrorResponse';
import { Messages } from 'src/app/core/data/Messages';

@Component({
  selector: 'app-authorization',
  templateUrl: './authorization.component.html',
  styleUrls: ['./authorization.component.css']
})
export class AuthorizationComponent implements OnInit {
  /**
   * @param authorizationForm FormGroup
   */
  authorizationForm: FormGroup = new AuthorizationForm().authorizationForm;

  /**
   * @param _globalService GlobalService
   * @param _usersService UsersService
   * @param _accountService AccountsService
   * @param _router Router
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _globalService: GlobalService,
    private _usersService: UsersService,
    private _accountService: AccountsService,
    private _router: Router,
    private _customToastrService: CustomToastrService
  ) { }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    if (this._usersService.isLoggedIn()) {
      this._router.navigate(['/']);
    }
  }

  /**
   * Authorization event
   * @param dataForAuthorize any
   * @returns void
   */
  authorization(dataForAuthorize: any): void {
    if (this.authorizationForm.valid) {
      this._usersService.login(dataForAuthorize)
        .subscribe(
          (jwt: JwtToken) => {
            if (jwt) {
              this.succesLogin(jwt);
            }
          },
          (error: ErrorResponse) => {
            this._customToastrService.displayErrorMessage(error);
          });
    }
  }

  /**
   * Save user data if login success
   * @param jwt JwtToken
   */
  public succesLogin(jwt: JwtToken): void {
    this._usersService.saveToken(jwt['auth_token'], jwt['refresh_token']);
    const initializeSubscription = this._accountService.initialize(this._globalService._currentUser.id).subscribe(
        (initializationData: any) => {
            this._globalService.initializeData(initializationData);
            this._customToastrService.displaySuccessMessage(Messages.AUTHORIZED_SUCCESSFULLY);
            this._router.navigate(['/']);
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
        });
    // this._subscription.add(initializeSubscription);
    // this._globalService.setIsLoadedData(false);
  }
}
