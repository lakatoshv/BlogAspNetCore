import { Injectable } from '@angular/core';
import { ActivatedRoute, ActivatedRouteSnapshot } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class GeneralServiceService {
  /**
   * @inheritdoc
   */
  constructor() { }

  /**
   * Return property value by URL
   * @param paramName string
   * @param activatedRoute ActivatedRoute
   */
  public getRouteParam(paramName: string, activatedRoute: ActivatedRoute): string | null {
    const snapshot = activatedRoute.snapshot;
    return this._getParamValue(paramName, snapshot);
  }

  /**
   * Return property value by URL
   * @param paramName string
   * @param routeObject ActivatedRouteSnapshot
   */
  private _getParamValue(paramName: string, routeObject: ActivatedRouteSnapshot | null): string | null{
    if (!routeObject) {
      return null;
    }

    if (paramName === null) {
      return null;
    }

    if (routeObject.paramMap.get(paramName) !== null) {
      return routeObject?.paramMap?.get(paramName);
    } else {
      return this._getParamValue(paramName, routeObject.parent);
    }
  }
}
