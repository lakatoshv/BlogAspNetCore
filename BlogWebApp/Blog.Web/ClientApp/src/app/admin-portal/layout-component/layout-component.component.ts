import { ChangeDetectionStrategy, Component, OnInit } from '@angular/core';
import { GlobalService } from './../../core/services/global-service/global-service.service';
import { User } from './../../core/models/User';
import { UsersService } from '../../core/services/users-services/users-service.service';

@Component({
  selector: 'app-layout-component',
  templateUrl: './layout-component.component.html',
  styleUrls: ['./layout-component.component.css'],
  standalone: false,
  changeDetection: ChangeDetectionStrategy.OnPush
})
export class LayoutComponentComponent implements OnInit {
  /**
   * @param user User | undefined
   */
  user: User | undefined;

  /**
   * @param loggedIn boolean
   */
  loggedIn = false;

  /**
   * @param navbarOpen boolean
   */
  navbarOpen = false;

  /**
   * @param _usersService UsersService
   * @param _globalService GlobalService
   */
  constructor(
    private _usersService: UsersService,
    private _globalService: GlobalService
  ) {
  }

  /**
   * @inheritdoc
   */
  ngOnInit() {
    this._initUser();

    this._globalService.userDataChanged.subscribe(
      () => {
        this._initUser();
      }
    );
  }

  /**
   * Toggle navbar menu
   *
   * @returns void
   */
  toggleNavbar(): void {
    this.navbarOpen = !this.navbarOpen;
  }

  /**
   * Init user data.
   *
   * @returns void
   */
  private _initUser(): void {
    this.loggedIn = this._usersService.isLoggedIn();
    if (this.loggedIn) {
      this._globalService.resetUserData();
      this.user = this._globalService._currentUser;
    }
  }
}
