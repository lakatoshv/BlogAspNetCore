import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { HttpClientService } from '../global-service/http-client-services/http-client.service';
import { APiRoutes } from 'src/app/contracts/v1/ApiRoutes';

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
  public list(postId: number | null, model?: any): Observable<any> {
    if (model && postId !== null) {
      return this._httpClient.post(APiRoutes.GET_COMMENTS_BY_POST.concat("/", postId.toString()), model);
    }
    else if (model) {
      return this._httpClient.post(APiRoutes.GET_COMMENTS_BY_FILTER, model);
    }

    return this._httpClient.get(APiRoutes.COMMENTS_CONTROLLER);
  }

  /**
   * Get posts activity.
   * @returns Observable<any>
   */
  public commentsActivity(): Observable<any> {
    return this._httpClient.get(APiRoutes.COMMENTS_ACTIVITY);
  }

  /**
   * Add new comment.
   * @param model any
   * @returns Observable<any>
   */
  public add(model?: any): Observable<any> {
    return this._httpClient.post(APiRoutes.CREATE_COMMENT, model).pipe(map(res => res.json()));
  }

  /**
   * Edit comment by id.
   * @param id number
   * @param model any
   * @returns Observable<any>
   */
  public edit(id: number, model?: any): Observable<any> {
    return this._httpClient.put(APiRoutes.COMMENTS_CONTROLLER.concat('/', id.toString()), model);
  }

  /**
   * Delete comment by id.
   * @param id number
   * @returns Observable<any>
   */
  public delete(id: number): Observable<any> {
    return this._httpClient.delete(APiRoutes.COMMENTS_CONTROLLER.concat("/", id.toString()));
  }
}
