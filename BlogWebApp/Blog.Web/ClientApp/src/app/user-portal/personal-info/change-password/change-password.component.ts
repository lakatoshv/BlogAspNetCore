import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { User } from './../../../core/models/User';
import { FormGroup } from '@angular/forms';
import { TinyMCEOptionsObject } from './../../../core/models/TinyMCEOptionsObject';
import { TinyMCEOptions } from './../../../core/data/TinyMCEOptions';
import { Router } from '@angular/router';
import { GlobalService } from './../../../core/services/global-service/global-service.service';
import { UsersService } from '../../../core/services/users-services/users-service.service';
import { Messages } from './../../../core/data/Mesages';
import { CustomToastrService } from './../../../core/services/custom-toastr.service';
import { ChangePasswordDto } from '../../../core/Dto/ChangePasswordDto';
import { ChangePasswordForm } from '../../../core/forms/user/ChangePasswordForm';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';
import { finalize } from 'rxjs';

@Component({
  selector: 'app-change-password',
  templateUrl: './change-password.component.html',
  styleUrls: ['./change-password.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
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
   * @param _changeDetectorRef: ChangeDetectorRef
   */
  constructor(
    private _router: Router,
    private _globalService: GlobalService,
    private _usersService: UsersService,
    private _customToastrService: CustomToastrService,
    private _changeDetectorRef: ChangeDetectorRef
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
  private async _getProfile(id: number): Promise<void> {
    this._usersService.getProfile(id)
      .pipe(
        finalize(() => {
          this._changeDetectorRef.markForCheck();
        })
      )
      .subscribe({
        next: (response: any) => {
          this.user = response;
          this._setFormData();
        },
        error: (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
        }
      });
  }

  /**
   * Change user email.
   * @param profileModel any
   */
  async edit(profileModel: any): Promise<void> {
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
      this._usersService.changePassword(profile)
        .subscribe({
          next: (response: any) => {
            // this._usersService.saveUser(JSON.stringify(this._globalService._currentUser));*/
            this._customToastrService.displaySuccessMessage(Messages.PASSWORD_CHANGED_SUCCESSFULLY);
          },
          error: (error: ErrorResponse) => {
            this._customToastrService.displayErrorMessage(error);
          }
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
