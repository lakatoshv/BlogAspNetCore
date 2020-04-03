import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClientService } from '../global-service/http-client-services/http-client.service';


@Injectable()
/**
 * Accounts service
 */
export class AccountsService {
    /**
     * @param _httpClient HttpClientService
     */
    constructor(private _httpClient: HttpClientService) {
    }

    /**
     * Initialize user data
     * @param id string
     */
    public initialize(id: string): Observable<any> {
        return this._httpClient.get(HttpClientService.ACCOUNTS_INITIALIZE.concat('/', id.toString()));
    }
}
