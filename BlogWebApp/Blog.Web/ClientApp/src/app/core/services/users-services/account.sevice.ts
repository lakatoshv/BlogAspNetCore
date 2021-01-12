import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { APiRoutes } from 'src/app/contracts/v1/ApiRoutes';
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
     * @returns Observable<any>
     */
    public initialize(id: string): Observable<any> {
        return this._httpClient.get(APiRoutes.ACCOUNTS_INITIALIZE.concat('/', id.toString()));
    }

    /**
     * Send confirmation email.
     * @returns Observable<any>
     */
    public sendConfirmationEmail(): Observable<any> {
      return this._httpClient.get(APiRoutes.SEND_CONFIRMATION_EMAIL);
  }
}
