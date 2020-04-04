import { Injectable } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';
import { User } from '../../models/User';
import { JwtHelperService } from '@auth0/angular-jwt';

@Injectable({
  providedIn: 'root'
})
export class GlobalService {
  /**
   * @param _isLoadedData boolean
   */
  public _isLoadedData = false;

  /**
   * @param _currentUser User
   */
  public _currentUser: User;

  /**
   * @param _avatarUrl string
   */
  public _avatarUrl: string;

  /**
   * @param _roles string[]
   */
  public _roles: string[];

  /**
   * @param _jwt JwtHelperService
   */
  private _jwt = new JwtHelperService();

  /**
   * @inheritdoc
   */
  constructor() { }

  /**
   * Return property value by URL
   * @param idName string
   * @param activatedRoute ActivatedRoute
   * @returns string|null
   */
  public getRouteParam(
    idName: string,
    activatedRoute: ActivatedRoute
  ): string|null {
    const snapshot = activatedRoute.snapshot;
    return this._getId(idName, snapshot);
  }

  /**
   * Initialize current user data from response.
   * @param response any
   * @returns void
   */
  public initializeData(response: any): void {
    if (response.currentUser) {
        this._currentUser = response.currentUser;
    }
  }

  /**
   * Reset user data.
   * @returns void
   */
  public resetUserData(): void {
    const token = localStorage.getItem('token');
    if (token) {
      this._currentUser = this.decode(token);
      // this._avatarUrl = this._currentUser.AvatarUrl;
      this._roles = this._currentUser.roles;
      // this.onAvatarChanged.next(this._currentUser.AvatarUrl);

    } else {
      this._isLoadedData = true;
    }
  }

  /**
   * @param token string
   * @returns User
   */
  public decode(token: string): User {
    const decoded = this._jwt.decodeToken(token);

    const id = decoded['id'];
    const username = decoded['sub'];
    let user = new User(id, username);

    user.email = decoded['email'];
    user.firstName = decoded['firstName'];
    user.lastName = decoded['lastName'];
    user.phoneNumber = decoded['phoneNumber'];
    // user.IsEmailVerified = decoded['isEmailVerified'] == 'True';
    // user. = decoded['avatarUrl'];
    user.roles = decoded['roles'];

    return user;
  }

  /**
   * Return property value by URL
   * @param idName string
   * @param routeObject ActivatedRouteSnapshot
   * @returns string|null
   */
  private _getId(idName: string, routeObject: ActivatedRouteSnapshot): string|null {
    if (!routeObject) {
      return null;
    }

    if (routeObject.paramMap.get(idName) !== null) {
      return routeObject.paramMap.get(idName);
    } else {
      return this._getId(idName, routeObject.parent);
    }
  }
}
