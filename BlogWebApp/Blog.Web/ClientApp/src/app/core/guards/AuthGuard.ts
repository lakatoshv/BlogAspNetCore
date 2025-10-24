import { Injectable } from "@angular/core";
import { ActivatedRouteSnapshot, Router, RouterStateSnapshot, UrlTree } from "@angular/router";
import { Observable } from "rxjs";
import { UsersService } from "../services/users-services/users-service.service";

@Injectable({
  providedIn: 'root'
})
export class AuthGuard  {

    /**
     * @inheritdoc
     * 
     * @param _usersService UsersService
     * @param _router Router
     */
    constructor(
        private _usersService: UsersService,
        private _router: Router) {
    }

    /**
     * @inheritdoc
     */
    canActivate(
        _: ActivatedRouteSnapshot, 
        state: RouterStateSnapshot
    ): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        
        debugger
        const authenticated = this._usersService.isLoggedIn();
        if(authenticated) {
            return true;
        }

        this._router.navigate(['/blog/authorization'], { queryParams: { returnUrl: state.url } });
        
        return false;
    }

    /**
     * @inheritdoc
     */
    canActivateChild(
        childRoute: ActivatedRouteSnapshot, 
        state: RouterStateSnapshot
    ): boolean | UrlTree | Observable<boolean | UrlTree> | Promise<boolean | UrlTree> {
        return this.canActivate(childRoute, state);
    }
}
