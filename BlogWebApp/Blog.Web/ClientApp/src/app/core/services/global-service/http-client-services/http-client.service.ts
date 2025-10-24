import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import {HttpClient, HttpHeaders} from '@angular/common/http';
import { Observable} from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class HttpClientService {
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
    params?: any,
    isFormData?: any,
    isNonAuthRequest?: any): object {
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
      'Authorization': 'Bearer ' + JSON.parse(JSON.stringify(token)),
      'Access-Control-Allow-Origin': '*',
      'Access-Control-Allow-Methods': 'GET, POST, OPTIONS, PUT, PATCH, DELETE',
      'Access-Control-Allow-Headers': 'X-Requested-With,content-type',
      'Access-Control-Allow-Credentials': 'true',
    });

    return headers;
  }
}
