import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/core/models/User';
import { Router } from '@angular/router';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';
import { FormGroup } from '@angular/forms';
import { ProfileForm } from 'src/app/core/forms/user/ProfileForm';
import { TinyMCEOptionsObject } from 'src/app/core/models/TinyMCEOptionsObject';
import { TinyMCEOptions } from 'src/app/core/data/TinyMCEOptions';
import { UsersService } from 'src/app/core/services/users-services/users.service';
import { ProfileViewDto } from 'src/app/core/Dto/ProfileViewDto';

@Component({
  selector: 'app-edit-profile',
  templateUrl: './edit-profile.component.html',
  styleUrls: ['./edit-profile.component.css']
})
export class EditProfileComponent implements OnInit {
  /**
   * @param user User
   */
  public user: User = null;

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
   */
  constructor(
    private _router: Router,
    private _globalService: GlobalService,
    private _usersService: UsersService
  ) { }

  /**
   * @inheritdoc
   */
  public ngOnInit() {
    this.isLoggedIn = this._usersService.isLoggedIn();
    if (this._usersService.isLoggedIn()) {
      this._globalService.resetUserData();
      this._getProfile(this._globalService._currentUser.profile.id);

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
      () => {}
    );
  }

  /**
   *
   * @param profileModel any
   */
  edit(profileModel: any): void {
    if (profileModel.oldPassword !== null && profileModel.newPassword !== null) {
      if (profileModel.oldPassword === this._globalService._currentUser.password) {
        this._globalService._currentUser.password = profileModel.newPassword;
      } else { console.error('Different passwords'); }
    }

    this._usersService.updateProfile(this._globalService._currentUser.id, profileModel)
    /*this._globalService._currentUser.userName = profileModel.firstName + ' ' + profileModel.lastName;
    this._globalService._currentUser.email = profileModel.email;
    this._globalService._currentUser.firstName = profileModel.firstName;
    this._globalService._currentUser.lastName = profileModel.lastName;
    this._globalService._currentUser.phoneNumber = profileModel.phoneNumber;
    this._globalService._currentUser.profile.about = profileModel.about;
    // this._usersService.saveUser(JSON.stringify(this._globalService._currentUser));*/
  }

  /**
   * Set form data.
   */
  private _setFormData() {
    this.profileForm.get('userName').setValue(this.user.firstName + ' ' + this.user.lastName);
    this.profileForm.get('email').setValue(this.user.email);
    this.profileForm.get('firstName').setValue(this.user.firstName);
    this.profileForm.get('lastName').setValue(this.user.lastName);
    this.profileForm.get('phoneNumber').setValue(this.user.phoneNumber);
    this.profileForm.get('about').setValue(this.user.profile.about);

    this._usersService.
  }

  /**
   * Clear form data
   */
  private clearFormData() {
  }

}
