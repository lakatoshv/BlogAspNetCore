import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { User } from './../../../core/models/User';
import { FormGroup } from '@angular/forms';
import { ProfileForm } from './../../../core/forms/user/ProfileForm';
import { TinyMCEOptionsObject } from './../../../core/models/TinyMCEOptionsObject';
import { TinyMCEOptions } from './../../../core/data/TinyMCEOptions';
import { Router } from '@angular/router';
import { GlobalService } from './../../../core/services/global-service/global-service.service';
import { UsersService } from '../../../core/services/users-services/users-service.service';
import { CustomToastrService } from './../../../core/services/custom-toastr.service';
import { Messages } from './../../../core/data/Mesages';
import { ProfileViewDto } from '../../../core/Dto/ProfileViewDto';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';
import { AccountsService } from '../../../core/services/users-services/account.sevice';

@Component({
  selector: 'app-change-email',
  templateUrl: './change-email.component.html',
  styleUrls: ['./change-email.component.scss'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
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
   * @param _changeDetectorRef: ChangeDetectorRef
   */
  constructor(
    private _router: Router,
    private _globalService: GlobalService,
    private _usersService: UsersService,
    private _accountsService: AccountsService,
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
    if(this.user && this._globalService._currentUser && this._globalService._currentUser?.profile) {
      const profile = new ProfileViewDto(
        profileModel.email,
        this.user.firstName,
        this.user.lastName,
        this.user.phoneNumber,
        this._globalService._currentUser?.password,
        this.user.profile?.about);

      this._usersService.updateProfile(this._globalService._currentUser?.profile?.id, profile)

        .subscribe({
          next: (result: any) => {
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
          error: (error: ErrorResponse) => {
          this._customToastrService.displayErrorMessage(error);
          }
        });
    }
  }

  /**
   * Verify email.
   * @returns void.
   */
  public async verifyEmail(): Promise<void> {
    this._accountsService.sendConfirmationEmail()
      .subscribe({
        next: () => {
        this._customToastrService.displaySuccessMessage(Messages.EMAIL_VERIFIED_SUCCESSFULLY);
      },
        error: (error: ErrorResponse) => {
        this._customToastrService.displayErrorMessage(error);
        }
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
