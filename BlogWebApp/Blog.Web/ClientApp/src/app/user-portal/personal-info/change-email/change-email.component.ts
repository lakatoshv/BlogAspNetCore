import { AccountsService } from 'src/app/core/services/users-services/account.sevice';
import { FormGroup } from '@angular/forms';
import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/core/models/User';
import { ProfileForm } from 'src/app/core/forms/user/ProfileForm';
import { TinyMCEOptionsObject } from 'src/app/core/models/TinyMCEOptionsObject';
import { TinyMCEOptions } from 'src/app/core/data/TinyMCEOptions';
import { Router } from '@angular/router';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';
import { UsersService } from 'src/app/core/services/users-services/users.service';
import { ProfileViewDto } from 'src/app/core/Dto/ProfileViewDto';
import { ErrorResponse } from 'src/app/core/responses/ErrorResponse';
import { CustomToastrService } from 'src/app/core/services/custom-toastr.service';
import { Messages } from 'src/app/core/data/Messages';

@Component({
  selector: 'app-change-email',
  templateUrl: './change-email.component.html',
  styleUrls: ['./change-email.component.scss']
})
export class ChangeEmailComponent implements OnInit {
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
  public profileForm: FormGroup = new ProfileForm().profileForm;

  /**
   * @param tinyMCEOptions TinyMCEOptionsObject
   */
  public tinyMCEOptions: TinyMCEOptionsObject = TinyMCEOptions;

  /**
   * @param _router Router
   * @param _globalService GlobalService
   * @param _usersService UsersService
   * @param _accountsService: AccountsService
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _router: Router,
    private _globalService: GlobalService,
    private _usersService: UsersService,
    private _accountsService: AccountsService,
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
    if(this.user && this._globalService._currentUser && this._globalService._currentUser?.profile) {
      const profile = new ProfileViewDto(
        profileModel.email,
        this.user.firstName,
        this.user.lastName,
        this.user.phoneNumber,
        this._globalService._currentUser?.password,
        this.user.profile?.about);
      this._usersService.updateProfile(this._globalService._currentUser?.profile?.id, profile).subscribe(
        (result: any) => {
          if(this._globalService._currentUser && this._globalService._currentUser.profile) {
            this._globalService._currentUser.userName = result.firstName + ' ' + result.lastName;
            this._globalService._currentUser.email = result.email;
            this._globalService._currentUser.firstName = result.firstName;
            this._globalService._currentUser.lastName = result.lastName;
            this._globalService._currentUser.phoneNumber = result.phoneNumber;
            this._globalService._currentUser.profile.about = result.about;
          }
          // this._usersService.saveUser(JSON.stringify(this._globalService._currentUser));*/
          this._customToastrService.displaySuccessMessage(Messages.EMAIL_CHANGED_SUCCESSFULLY);
        },
        (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
        });
    }
  }

  /**
   * Verify email.
   * @returns void.
   */
  public verifyEmail(): void {
    this._accountsService.sendConfirmationEmail().subscribe(
      () => {
        this._customToastrService.displaySuccessMessage(Messages.EMAIL_VERIFIED_SUCCESSFULLY);
      },
      (error: ErrorResponse) => {
        this._customToastrService.displayErrorMessage(error);
      });
  }

  /**
   * Set form data.
   * @returns void
   */
  private _setFormData(): void {
    this.profileForm.get('email')?.setValue(this.user?.email);
  }

  /**
   * Clear form data.
   * @returns void
   */
  private clearFormData(): void {
  }
}
