import { Injectable } from '@angular/core';
import { Observable, Subject, ReplaySubject, from, of, range , throwError } from 'rxjs';
import { map, filter, switchMap, catchError } from 'rxjs/operators';
import { JwtToken } from '../../models/JwtToken';
import * as moment from 'moment';
import { JwtHelperService } from '@auth0/angular-jwt';
import { HttpClientService } from '../global-service/http-client-services/http-client.service';
import { GlobalService } from '../global-service/global-service.service';
import { User } from '../../models/User';

@Injectable()
export class UsersService {
  /**
   * @param _jwt JwtHelperService
   */
  private _jwt = new JwtHelperService();

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
   * @param roles string[]
   */
  public roles: string[];

  /**
   * @param _httpClient HttpClientService
   * @param _globalService GlobalService
   */
  public constructor(
    private _httpClient: HttpClientService,
    private _globalService: GlobalService
  ) {
  }

  /**
   * Register user.
   * @param model any
   * @returns Observable<any>
   */
  public registration(model: any): Observable<any> {
    return this._httpClient.post(HttpClientService.REGISTER_USER, model, null, false, true);
  }

  /**
   * Save user token.
   * @param token string
   * @param refreshToken string
   * @returns void
   */
  public saveToken(token: string, refreshToken: string): void {
    if (token) {
      localStorage.setItem('token', token);
      localStorage.setItem('refresh_token', refreshToken);
      sessionStorage.setItem('token', token);
      this._globalService.resetUserData();
    }
  }

  /**
   * Login user.
   * @param credentials any
   * @returns Observable<JwtToken>
   */
  public login(credentials: any): Observable<JwtToken> {
    return this._httpClient.post(HttpClientService.LOGIN_USER, credentials, null, false, true);
  }

  /**
   * Logout user.
   */
  logout() {
    localStorage.removeItem('token');
    localStorage.removeItem('refresh_token');
  }

  /**
   * Check if user is login.
   */
  isLoggedIn() {
    const token: string = localStorage.getItem('token');
    if (token != null && !this._jwt.isTokenExpired(token)) {
      return true;
    }
    return false;
  }

  /**
   * Get user token from local storage.
   */
  getToken() {
    return localStorage.getItem('token');
  }

  /*
  private setSession(authResult) {
    const expiresAt = moment().add(authResult.expiresIn,'second');
    localStorage.setItem('id_token', authResult.idToken);
    localStorage.setItem("expires_at", JSON.stringify(expiresAt.valueOf()) );
  }

  logout() {
    localStorage.removeItem("id_token");
    localStorage.removeItem("expires_at");
  }

  public isLoggedIn() {
    return moment().isBefore(this.getExpiration());
  }

  isLoggedOut() {
    return !this.isLoggedIn();
  }

  getExpiration() {
    const expiration = localStorage.getItem("expires_at");
    const expiresAt = JSON.parse(expiration);
    return moment(expiresAt);
  }
  */
}
