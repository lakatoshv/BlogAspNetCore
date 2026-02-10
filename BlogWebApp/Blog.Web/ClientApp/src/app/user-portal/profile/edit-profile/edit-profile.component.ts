import { ChangeDetectionStrategy, ChangeDetectorRef, Component, OnInit } from '@angular/core';
import { User } from './../../../core/models/User';
import { Router } from '@angular/router';
import { GlobalService } from './../../../core/services/global-service/global-service.service';
import { UsersService } from '../../../core/services/users-services/users-service.service';
import { FormGroup } from '@angular/forms';
import { ProfileForm } from './../../../core/forms/user/ProfileForm';
import { TinyMCEOptionsObject } from './../../../core/models/TinyMCEOptionsObject';
import { TinyMCEOptions } from './../../../core/data/TinyMCEOptions';
import { Messages } from './../../../core/data/Mesages';
import { CustomToastrService } from './../../../core/services/custom-toastr.service';
import { ProfileViewDto } from '../../../core/Dto/ProfileViewDto';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class EditProfileComponent implements OnInit {
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
   * Change user data.
   * @param profileModel any
   * @returns void
   */
  edit(profileModel: any): void {
    if(this.user && this._globalService._currentUser?.profile) {
      const profile = new ProfileViewDto(
        this.user.email ?? '',
        profileModel.firstName,
        profileModel.lastName,
        this.user.phoneNumber,
        undefined,
        profileModel.about);
      this._usersService.updateProfile(this._globalService._currentUser.profile.id, profile).subscribe(
        (result: any) => {
          if(this._globalService._currentUser?.profile) {
            this._globalService._currentUser.userName = result.firstName + ' ' + result.lastName;
            this._globalService._currentUser.email = result.email;
            this._globalService._currentUser.firstName = result.firstName;
            this._globalService._currentUser.lastName = result.lastName;
            this._globalService._currentUser.phoneNumber = result.phoneNumber;
            this._globalService._currentUser.profile.about = result.about;
          }
          // this._usersService.saveUser(JSON.stringify(this._globalService._currentUser));*/
          this._customToastrService.displaySuccessMessage(Messages.PROFILE_EDITED_SUCCESSFULLY);
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
    this.profileForm.get('userName')?.setValue(this.user?.firstName + ' ' + this.user?.lastName);
    this.profileForm.get('email')?.setValue(this.user?.email);
    this.profileForm.get('firstName')?.setValue(this.user?.firstName);
    this.profileForm.get('lastName')?.setValue(this.user?.lastName);
    this.profileForm.get('about')?.setValue(this.user?.profile?.about);
  }

  /**
   * Clear form data
   * @returns void
   */
  private clearFormData(): void {
  }
}
