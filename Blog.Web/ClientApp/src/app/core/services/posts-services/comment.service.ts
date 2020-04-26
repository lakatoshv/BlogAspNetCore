import { Comment } from './../../models/Comment';
import { Injectable } from '@angular/core';
import { Observable, Subject, ReplaySubject, from, of, range , throwError } from 'rxjs';
import { map, filter, switchMap, catchError } from 'rxjs/operators';
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
}
