import { Component, OnInit } from '@angular/core';
import { GeneralServiceService } from './../../../core';
import { ActivatedRoute, Router } from '@angular/router';
import { User } from './../../../core/models/User';
import { GlobalService } from './../../../core/services/global-service/global-service.service';
import { UsersService } from '../../../core/services/users-services/users-service.service';
import { CustomToastrService } from './../../../core/services/custom-toastr.service';
import { Messages } from './../../../core/data/Mesages';
import { ErrorResponse } from '../../../core/responses/ErrorResponse';
import { AccountsService } from '../../../core/services/users-services/account.sevice';

@Component({
  selector: 'app-profile-page',
  templateUrl: './profile-page.component.html',
  styleUrls: ['./profile-page.component.css'],
  standalone: false
})
export class ProfilePageComponent implements OnInit {
  /**
   * @param user User | undefined
   */
  public user: User | undefined;

  /**
   * @param topTab string
   */
  public topTab = 'main-info';

  /**
   * @param postsTab string
   */
  public postsTab = 'my';

  /**
   * @param isForCurrentUser boolean
   */
  public isForCurrentUser = false;

  /**
   * @param isLoggedIn boolean
   */
  public isLoggedIn = false;

  /**
   * @param isLoaded boolean
   */
  public isLoaded = false;

  public postsCount = 0;


  /**
   * @param _profileId number
   */
  private _profileId?: number;

  /**
   * @param _generalService GeneralServiceService
   * @param _activatedRoute ActivatedRoute
   * @param _router Router
   * @param _globalService GlobalService
   * @param _usersService UsersService
   * @param _accountsService: AccountsService
   * @param _customToastrService CustomToastrService
   */
  constructor(
    private _generalService: GeneralServiceService,
    private _activatedRoute: ActivatedRoute,
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
    this._profileId = parseInt(this._generalService.getRouteParam('profile-id', this._activatedRoute) ?? '', undefined);
    this.isLoggedIn = this._usersService.isLoggedIn();
    if (this._usersService.isLoggedIn()) {
      this._globalService.resetUserData();
      this.user = this._globalService._currentUser;
    }
    this.isForCurrentUser =
      this._router.url.includes('/my-profile') ||
      (this._profileId !== null && this.user !== null && this.user?.profile?.id === this._profileId);

    if (!this.isForCurrentUser) {
      if (this._profileId !== null) {
        this._getProfile(this._profileId);
      } else {
        this._router.navigateByUrl('/');
      }
    } else if (!this.isLoggedIn) {
      this._router.navigateByUrl('/authorization');
    } else {
      this._getProfile(this.user?.profile?.id ?? 0);
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
      },
      (error: ErrorResponse) => {
        this._customToastrService.displayErrorMessage(error);
      });
  }

  /**
   * Select tab
   * @param selectedTab string
   * @param level string
   */
  public selectTab(selectedTab: string, level: string): void {
    switch (level) {
      case 'top':
        this.topTab = selectedTab;
        break;
      case 'posts':
        this.postsTab = selectedTab;
        break;
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
}
