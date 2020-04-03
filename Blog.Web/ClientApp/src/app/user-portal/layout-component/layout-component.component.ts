import { UsersService } from 'src/app/core/services/users-services/users.service';
import { Component, OnInit } from '@angular/core';
import { User } from 'src/app/core/models/User';
import { GlobalService } from 'src/app/core/services/global-service/global-service.service';

@Component({
  selector: 'app-layout-component',
  templateUrl: './layout-component.component.html',
  styleUrls: ['./layout-component.component.css']
})
export class LayoutComponentComponent implements OnInit {
  /**
   * @param user User
   */
  user: User;

  /**
   * @param loggedIn boolean
   */
  loggedIn = false;

  /**
   * @param _navbarOpen boolean
   */
  private _navbarOpen = false;

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
    this.loggedIn = this._usersService.isLoggedIn();
    if (this.loggedIn) {
      this._globalService.resetUserData();
      this.user = this._globalService._currentUser;
    }
  }

  /**
   * Toggle navbar menu
   */
  toggleNavbar() {
    this._navbarOpen = !this._navbarOpen;
  }
}
