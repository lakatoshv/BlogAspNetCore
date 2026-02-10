import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { AuthorizationForm } from './../../../core/forms/user/AuthorizationForm';
import { Router } from '@angular/router';
import { UsersService } from '../../../core/services/users-services/users-service.service';
import { Messages } from './../../../core/data/Mesages';
import { CustomToastrService } from './../../../core/services/custom-toastr.service';
import { JwtToken } from '../../../core/models/JwtToken';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';
import { GlobalService } from '../../../core/services/global-service/global-service.service';
import { AccountsService } from '../../../core/services/users-services/account.sevice';

@Component({
  selector: 'app-authorization',
  templateUrl: './authorization.component.html',
  styleUrls: ['./authorization.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
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
  async authorization(dataForAuthorize: any): Promise<void> {
    if (this.authorizationForm.valid) {
      this._usersService.login(dataForAuthorize)
        .subscribe({
          next: (jwt: JwtToken) => {
            if (jwt) {
              this.succesLogin(jwt);
            }
          },
          error: (error: ErrorResponse) => {
            this._customToastrService.displayErrorMessage(error);
          }
        });
    }
  }

  /**
   * Save user data if login success
   * @param jwt JwtToken
   */
  public async succesLogin(jwt: JwtToken | null): Promise<void> {
    if(typeof jwt === 'string') {
      const jwtString = jwt;
      jwt = null;
      const parsedJwtString = JSON.parse(jwtString);
      jwt = new JwtToken(parsedJwtString['auth_token'], parsedJwtString['refresh_token'], parsedJwtString['expires_in']);
    }
    if(jwt) {
      this._usersService.saveToken(jwt.AccessToken, jwt.RefreshToken);
      if(this._globalService._currentUser) {
        this._accountService.initialize(this._globalService._currentUser.id)
          .subscribe({
            next: (initializationData: any) => {
              this._customToastrService.displaySuccessMessage(Messages.AUTHORIZED_SUCCESSFULLY);
              this._router.navigate(['/'])
                .then(() => {
                  this._globalService.initializeData(initializationData);
                });
            },
            error: (error: ErrorResponse) => {
              this._customToastrService.displayErrorMessage(error);
            }
          });
      }
    }
    // this._subscription.add(initializeSubscription);
    // this._globalService.setIsLoadedData(false);
  }
}
