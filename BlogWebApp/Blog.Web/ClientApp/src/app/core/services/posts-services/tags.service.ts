import { Injectable } from '@angular/core';
import { HttpClientService } from '../global-service/http-client-services/http-client.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';
import { APiRoutes } from '../../../contracts/v1/APiRoutes';

/**
 * Tags service.
 */
@Injectable({
  providedIn: 'root'
})
export class TagsService {
  /**
   * @param _httpClient HttpClientService
   */
  constructor(private _httpClient: HttpClientService) { }

  /**
   * Get tags list.
   * @returns Observable<any>
   */
  public list(model?: any): Observable<any> {
    if (model) {
      return this._httpClient.post(APiRoutes.GET_TAGS_BY_FILTER, model);
    }

    return this._httpClient.get(APiRoutes.GET_TAGS);
  }

  /**
   * Add new tag.
   * @param model any
   * @returns Observable<any>
   */
  public add(model?: any): Observable<any> {
    return this._httpClient.post(APiRoutes.CREATE_TAG, model).pipe(map(res => res.json()));
  }

  /**
   * Edit tag by id.
   * @param id number
   * @param model any
   * @returns Observable<any>
   */
  public edit(id: number, model?: any): Observable<any> {
    return this._httpClient.put(APiRoutes.TAGS_CONTROLLER.concat('/', id.toString()), model);
  }

  /**
   * Delete tag by id.
   * @param id number
   * @returns Observable<any>
   */
  public delete(id: number): Observable<any> {
    return this._httpClient.delete(APiRoutes.TAGS_CONTROLLER.concat('/', id.toString()));
  }

  /**
   * Get tags activity.
   * @returns Observable<any>
   */
  public tagsActivity(): Observable<any> {
    return this._httpClient.get(APiRoutes.USERS_ACTIVITY);
  }

  /**
   * Get tag by id.
   * @param id number
   * @returns Observable<any>
   */
  public getTag(id: number): Observable<any> {
    return this._httpClient.get(APiRoutes.GET_TAG.concat('/', id.toString()));
  }
}
