import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { Observable} from 'rxjs';

@Injectable()
export class HttpClientService {
  /**
   * API url.
   * @param API string
   */
  private static readonly API: string = 'api';

  // Controllers

  /**
   * Accounts controller url.
   * @param ACCOUNTS_CONTROLLER string
   */
  public static readonly ACCOUNTS_CONTROLLER = HttpClientService.API + '/accounts';

  // Methods

  // Accounts controller methods
  /**
   * Initialize user method.
   * @param ACCOUNTS_INITIALIZE string
   */
  public static readonly ACCOUNTS_INITIALIZE = HttpClientService.ACCOUNTS_CONTROLLER + '/initialize';

  /**
   * Login user method
   * @param API string
   */
  public static readonly LOGIN_USER = HttpClientService.ACCOUNTS_CONTROLLER + '/login';

  /**
   * Register user method.
   * @param REGISTER_USER string
   */
  public static readonly REGISTER_USER = HttpClientService.ACCOUNTS_CONTROLLER + '/register';

  /**
   * Get all users method.
   * @param GET_ALL_USERS string
   */
  public static readonly GET_ALL_USERS = HttpClientService.ACCOUNTS_CONTROLLER + '/get-all-users';

  /**
   * @param _httpClient HttpClient
   * @param _router Router
   */
  constructor(
    private _httpClient: HttpClient,
    private _router: Router,
  ) {
  }

  /**
   * HTTP GET method.
   * @param url string
   * @param params any
   * @returns Observable<any>
   */
  public get(
    url: string,
    params?: any | null
  ): Observable<any> {
    return this._httpClient.get(url, this._getOptions(params));
  }

  /**
   * HTTP POST method.
   * @param url string
   * @param body any | null
   * @param params any | null
   * @param isFormData boolean | false
   * @param isNonAuthRequest boolean | false
   * @returns Observable<any>
   */
  public post(
    url: string,
    body?: any | null,
    params?: any | null,
    isFormData?: boolean | false,
    isNonAuthRequest?: boolean | false
  ): Observable<any> {
    return this._httpClient.post(url, body, this._getOptions(params));
  }

  /**
   * HTTP PUT method.
   * @param url string
   * @param body any | null
   * @returns Observable<any>
   */
  public put(
    url: string,
    body?: any | null
  ): Observable<any> {
    return this._httpClient.put(url, body, this._getOptions());
  }

  /**
   * HTTP DELETE method.
   * @param url string
   * @param params any | null
   * @returns Observable<any>
   */
  public delete(
    url: string,
    params?: any | null
  ): Observable<any> {
    return this._httpClient.delete(url, this._getOptions(params));
  }

  /**
   * Get options.
   * @param params params
   * @returns object
   */
  private _getOptions(params?: any): object {
    const options = {
      params: params
    };
    return options;
  }
}
