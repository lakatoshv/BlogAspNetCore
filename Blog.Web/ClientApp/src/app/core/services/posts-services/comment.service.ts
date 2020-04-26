import { Comment } from './../../models/Comment';
import { Injectable } from '@angular/core';
import { Observable, Subject, ReplaySubject, from, of, range , throwError } from 'rxjs';
import { map, filter, switchMap, catchError, mergeMap, retry } from 'rxjs/operators';
import { HttpClientService } from '../global-service/http-client-services/http-client.service';

/**
 * Comments service.
 */
@Injectable()
export class CommentService {
  /**
   * @param _httpClient HttpClientService
   */
  constructor(private _httpClient: HttpClientService) { }

  /**
   * Get comments list by post id.
   * @param postId number
   * @param model any
   * @returns Observable<any>
   */
  public list(postId: number, model?: any): Observable<any> {
    if (model) {
      return this._httpClient.post(HttpClientService.GET_COMMENTS_BY_POST.concat("/", postId.toString()), model);
    }

    return this._httpClient.get(HttpClientService.COMMENTS_CONTROLLER);
  }

  /**
   * Add new comment.
   * @param model any
   * @returns Observable<any>
   */
  public add(model?: any): Observable<any> {
    return this._httpClient.post(HttpClientService.CREATE_COMMENT, model).pipe(map(res => res.json()));
  }

  /**
   * Edit comment by id.
   * @param id number
   * @param model any
   * @returns Observable<any>
   */
  public edit(id: number, model?: any): Observable<any> {
    return this._httpClient.put(HttpClientService.COMMENTS_CONTROLLER.concat('/', id.toString()), model);
  }
}
