
import { Injectable } from '@angular/core';
import { Observable, Subject, ReplaySubject, from, of, range , throwError } from 'rxjs';
import { map, filter, switchMap, catchError } from 'rxjs/operators';
import { HttpClientService } from '../global-service/http-client-services/http-client.service';

/**
 * Post service.
 */
@Injectable()
export class PostService {
  /**
   * @param _httpClient HttpClientService
   */
  constructor(private _httpClient: HttpClientService) { }

  /**
   * Get posts list by model.
   * @param model any
   * @returns Observable<any>
   */
  public list(model?: any): Observable<any> {
    if (model) {
      return this._httpClient.post(HttpClientService.GET_POSTS, model);
    }

    return this._httpClient.get(HttpClientService.POSTS_CONTROLLER);
  }

  /**
   * Sort posts list by model.
   * @param model any
   * @returns Observable<any>
   */
  public sort(model?: any): Observable<any> {
    if (model) {
      return this._httpClient.post(HttpClientService.GET_POSTS, model);
    }

    return this._httpClient.post(HttpClientService.GET_POSTS);
  }
}
