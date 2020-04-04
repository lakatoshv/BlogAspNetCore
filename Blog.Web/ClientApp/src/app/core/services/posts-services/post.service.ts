
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

  /**
   * Get user posts list by user id and model.
   * @param userId string
   * @param model any
   * @returns Observable<any>
   */
  public userPosts(userId: string, model?: any): Observable<any> {
    if (model) {
      return this._httpClient.post(HttpClientService.USER_POSTS.concat('/', userId.toString()), model);
    }

    return this._httpClient.post(HttpClientService.USER_POSTS.concat('/', userId.toString()));
  }

  /**
   * Get post by id.
   * @param id number
   * @returns Observable<any>
   */
  public showPost(id: number): Observable<any> {
    return this._httpClient.get(HttpClientService.SHOW_POST.concat('/', id.toString()));
  }

  /**
   * Edit post by id.
   * @param id number
   * @param model any
   * @returns Observable<any>
   */
  public edit(id: number, model: any): Observable<any> {
    return this._httpClient.put(HttpClientService.POSTS_CONTROLLER.concat('/', id.toString()), model);
  }

  /**
   * Add new post.
   * @param model any
   * @returns Observable<any>
   */
  public add(model: any): Observable<any> {
    return this._httpClient.post(HttpClientService.POSTS_CONTROLLER, model)
        .pipe(
          map((response: any) => response),
          catchError((e: Response) => throwError(e))
        );
  }

  /**
   * Delete post by id and author id.
   * @param id number
   * @param authorId string
   * @returns Observable<any>
   */
  public delete(id: number, authorId: string): Observable<any> {
    const params = {
      authorId: authorId
    };
    return this._httpClient.delete(HttpClientService.POSTS_CONTROLLER.concat('/', id.toString()), params);
  }
}
