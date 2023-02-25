import { ChangePasswordDto } from './../../../core/Dto/ChangePasswordDto';
import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/core/models/User';
import { FormGroup } from '@angular/forms';
import { TinyMCEOptionsObject } from 'src/app/core/models/TinyMCEOptionsObject';
import { TinyMCEOptions } from 'src/app/core/data/TinyMCEOptions';
import { Router } from '@angular/router';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';
import { UsersService } from 'src/app/core/services/users-services/users.service';
import { ChangePasswordForm } from 'src/app/core/forms/user/ChangePasswordForm';
import { ErrorResponse } from 'src/app/core/responses/ErrorResponse';
import { CustomToastrService } from 'src/app/core/services/custom-toastr.service';
import { Messages } from 'src/app/core/data/Messages';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css']
})
export class ChangePasswordComponent implements OnInit {
  /**
   * @param user User | undefined
   */
  public user: User | undefined;

  /**
   * @param isLoggedIn boolean
   */
  public isLoggedIn = false;

  /**
   * @param profileForm FormGroup
   */
  public profileForm: FormGroup = new ChangePasswordForm().profileForm;

  /**
   * @param tinyMCEOptions TinyMCEOptionsObject
   */
  public tinyMCEOptions: TinyMCEOptionsObject = TinyMCEOptions;

  /**
   * @param _router Router
   * @param _globalService GlobalService
   * @param _usersService UsersService
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _router: Router,
    private _globalService: GlobalService,
    private _usersService: UsersService,
    private _customToastrService: CustomToastrService
  ) { }

  /**
   * @inheritdoc
   */
  public ngOnInit() {
    this.isLoggedIn = this._usersService.isLoggedIn();
    if (this._usersService.isLoggedIn()) {
      this._globalService.resetUserData();
      if(this._globalService._currentUser?.profile) {
        this._getProfile(this._globalService._currentUser.profile.id);
      }

    } else {
      this._router.navigateByUrl('/authorization');
    }
  }

  /**
   * Get user profile by id.
   * @param id number
   * @returns void
   */
  private _getProfile(id: number): void {
    this._usersService.getProfile(id).subscribe(
      (response: any) => {
        this.user = response;
        this._setFormData();
      },
      (error: ErrorResponse) => {
        this._customToastrService.displayErrorMessage(error);
      });
  }

  /**
   * Change user email.
   * @param profileModel any
   */
  edit(profileModel: any): void {
    if (profileModel.oldPassword !== null
        && profileModel.newPassword !== null
        && profileModel.confirmPassword != null
        && profileModel.newPassword === profileModel.confirmPassword) {
      // if (profileModel.oldPassword === this.user.password) {
      //  password = profileModel.newPassword;
      // } else { console.error('Different passwords'); }
      const profile = new ChangePasswordDto(
        profileModel.oldPassword,
        profileModel.newPassword);
      this._usersService.changePassword(profile).subscribe(
        () => {
          // this._usersService.saveUser(JSON.stringify(this._globalService._currentUser));*/
          this._customToastrService.displaySuccessMessage(Messages.PASSWORD_CHANGED_SUCCESSFULLY);
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
        });
    }
  }

  /**
   * Set form data.
   * @returns void
   */
  private _setFormData(): void {
  }

  /**
   * Clear form data.
   * @returns void
   */
  private clearFormData(): void {
  }
}
