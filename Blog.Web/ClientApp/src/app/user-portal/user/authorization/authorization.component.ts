import { AuthorizationForm } from 'src/app/core/forms/user/AuthorizationForm';
import { Component, OnInit } from '@angular/core';
import { FormGroup } from '@angular/forms';
import { UsersService } from 'src/app/core/services/users-services/users.service';
import { Router } from '@angular/router';
import { JwtToken } from 'src/app/core/models/JwtToken';
import { AccountsService } from 'src/app/core/services/users-services/account.sevice';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';

@Component({
  selector: 'app-authorization',
  templateUrl: './authorization.component.html',
  styleUrls: ['./authorization.component.css']
})
export class AuthorizationComponent implements OnInit {
  /**
   * @param authorizationForm FormGroup
   */
  private _authorizationForm: FormGroup = new AuthorizationForm().authorizationForm;

  /**
   * @param _globalService GlobalService
   * @param _usersService UsersService
   * @param _accountService AccountsService
   * @param _router Router
   */
  constructor(
    private _globalService: GlobalService,
    private _usersService: UsersService,
    private _accountService: AccountsService,
    private _router: Router
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
    if (this._authorizationForm.valid) {
      this._usersService.login(dataForAuthorize)
        .subscribe(
          (jwt: JwtToken) => {
            if (jwt) {
              this.succesLogin(jwt);
            }
          },
          (errorMessage) => {}
        );
    }
  }

  /**
   * Save user data if login success
   * @param jwt JwtToken
   */
  public succesLogin(jwt: JwtToken): void {
    this._usersService.saveToken(jwt['auth_token'], jwt['refresh_token']);
    const initializeSubscription = this._accountService.initialize(this._globalService._currentUser.Id).subscribe(
        (initializationData: any) => {
            this._globalService.initializeData(initializationData);
            this._router.navigate(['/']);
        },
        (errorMessage) => {}
    );
    // this._subscription.add(initializeSubscription);
    // this._globalService.setIsLoadedData(false);
  }
}
