import { Injectable } from '@angular/core';
import { HttpClientService } from '../global-service/http-client-services/http-client.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

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
      return this._httpClient.post(HttpClientService.GET_TAGS_BY_FILTER, model);
    }

    return this._httpClient.get(HttpClientService.GET_TAGS);
  }

  /**
   * Add new tag.
   * @param model any
   * @returns Observable<any>
   */
  public add(model?: any): Observable<any> {
    return this._httpClient.post(HttpClientService.CREATE_TAG, model).pipe(map(res => res.json()));
  }

  /**
   * Edit tag by id.
   * @param id number
   * @param model any
   * @returns Observable<any>
   */
  public edit(id: number, model?: any): Observable<any> {
    return this._httpClient.put(HttpClientService.TAGS_CONTROLLER.concat('/', id.toString()), model);
  }

  /**
   * Delete tag by id.
   * @param id number
   * @returns Observable<any>
   */
  public delete(id: number): Observable<any> {
    return this._httpClient.delete(HttpClientService.TAGS_CONTROLLER.concat('/', id.toString()));
  }
}
