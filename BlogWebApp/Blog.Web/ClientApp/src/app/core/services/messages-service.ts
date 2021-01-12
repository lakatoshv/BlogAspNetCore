import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from './global-service/http-client-services/http-client.service';
import { map } from 'rxjs/operators';
import { APiRoutes } from 'src/app/contracts/v1/ApiRoutes';


@Injectable()
/**
 * Messages service
 */
export class MessagesService {
    /**
     * @param _httpClient HttpClientService
     */
    constructor(private _httpClient: HttpClientService) {
    }

    /**
     * Send message.
     * @param model any
     * @returns Observable<any>
     */
    public sendMessage(model: any): Observable<any> {
      return this._httpClient.post(APiRoutes.MESSAGES_CONTROLLER, model).pipe(map(res => res.json()));
  }
}
