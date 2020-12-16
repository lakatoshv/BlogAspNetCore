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

/**
 * VERSION url.
 * @param VERSION string
 */
  private static readonly VERSION: string = 'v1';

/**
 * BASE url.
 * @param BASE string
 */
  private static readonly BASE: string = HttpClientService.API + '/' + HttpClientService.VERSION;

  // Controllers

  /**
   * Accounts controller url.
   * @param ACCOUNTS_CONTROLLER string
   */
  public static readonly ACCOUNTS_CONTROLLER = HttpClientService.BASE + '/accounts';

  /**
   * Posts controller url.
   * @param POSTS_CONTROLLER string
   */
  public static readonly POSTS_CONTROLLER = HttpClientService.BASE + '/posts';

  /**
   * Comments controller url.
   * @param COMMENTS_CONTROLLER string
   */
  public static readonly COMMENTS_CONTROLLER = HttpClientService.BASE + '/comments';

  /**
   * Comments controller url.
   * @param PROFILE_CONTROLLER string
   */
  public static readonly PROFILE_CONTROLLER = HttpClientService.BASE + '/profile';

  /**
   * Messages controller url.
   * @param MESSAGES_CONTROLLER string
   */
  public static readonly MESSAGES_CONTROLLER = HttpClientService.BASE + '/messages';

  /**
   * Tags controller url.
   * @param TAGS_CONTROLLER string
   */
  public static readonly TAGS_CONTROLLER = HttpClientService.BASE + '/tags';

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
   * Change password method.
   * @param CHANGE_PASSWORD string
   */
  public static readonly CHANGE_PASSWORD = HttpClientService.ACCOUNTS_CONTROLLER + '/change-password';

  /**
   * Send confirmation email.
   * @param SEND_CONFIRMATION_EMAIL string
   */
  public static readonly SEND_CONFIRMATION_EMAIL = HttpClientService.ACCOUNTS_CONTROLLER + '/send-confirmation-email';

  // Posts controller methods
  /**
   * Create post method.
   * @param CREATE_POST string
   */
  public static readonly CREATE_POST = HttpClientService.POSTS_CONTROLLER + '/create';

  /**
   * Get post method.
   * @param GET_POSTS string
   */
  public static readonly GET_POSTS = HttpClientService.POSTS_CONTROLLER + '/get-posts';

  /**
   * Show post method.
   * @param SHOW_POST string
   */
  public static readonly SHOW_POST = HttpClientService.POSTS_CONTROLLER + '/show';

  /**
   * Like post method.
   * @param LIKE_POST string
   */
  public static readonly LIKE_POST = HttpClientService.POSTS_CONTROLLER + '/like';

  /**
   * Dislike post method.
   * @param DISLIKE_POST string
   */
  public static readonly DISLIKE_POST = HttpClientService.POSTS_CONTROLLER + '/dislike';

  /**
   * Users posts method.
   * @param USER_POSTS string
   */
  public static readonly USER_POSTS = HttpClientService.POSTS_CONTROLLER + '/user-posts';

  // Comments controller methods
  /**
   * Get comment by post method.
   * @param GET_COMMENTS_BY_POST string
   */
  public static readonly GET_COMMENTS_BY_POST = HttpClientService.COMMENTS_CONTROLLER + '/get-comments-by-post';

  /**
   * Create new comment.
   * @param CREATE_COMMENT string
   */
  public static readonly CREATE_COMMENT = HttpClientService.COMMENTS_CONTROLLER + '/create';

  /**
   * Get comment by id.
   * @param GET_COMMENT string
   */
  public static readonly GET_COMMENT = HttpClientService.COMMENTS_CONTROLLER + '/get-comment';

   // Tags controller methods
  /**
   * Get tags.
   * @param GET_TAGS string
   */
  public static readonly GET_TAGS = HttpClientService.TAGS_CONTROLLER + '/get-tags';

  /**
   * Create new comment.
   * @param CREATE_TAG string
   */
  public static readonly CREATE_TAG = HttpClientService.TAGS_CONTROLLER + '/create';

  /**
   * Get comment by id.
   * @param GET_TAG string
   */
  public static readonly GET_TAG = HttpClientService.TAGS_CONTROLLER + '/get-tag';

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


    return this._httpClient.get(url);
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
   * @param params any
   * @param isFormData any
   * @param isNonAuthRequest any
   * @returns object
   */
  private _getOptions(
    params?,
    isFormData?,
    isNonAuthRequest?): object {
      const options = {
        headers: this._getAuthHeaders(isFormData, isNonAuthRequest),
        params: params
      };
      return options;
  }

  /**
   * Get auth headers.
   * @param isFormData boolean
   * @param isNonAuthRequest boolean
   */
  private _getAuthHeaders(isFormData?: boolean, isNonAuthRequest?: boolean): HttpHeaders {
    const token = localStorage.getItem('token');
    const headers = new HttpHeaders({
      'Content-Type': 'application/json',
      'Authorization': 'Bearer ' + JSON.parse(JSON.stringify(token))
    });

    return headers;
  }
}
