import { Injectable } from '@angular/core';
import { Http, Response, Headers } from '@angular/http';
import { Router } from '@angular/router';
import { SystemConstants } from './../common/system.constants';
import { AuthenService } from './authen.service';
import { NotificationService } from './notification.service';
import { UtilityService } from './utility.service';

import { Observable } from 'rxjs/Observable';
import { MessageConstants } from './../common/message.constants';

@Injectable()
export class DataService {
  private headers: Headers;
  constructor(private _http: Http, private _router: Router, private _authenService: AuthenService,
    private _notificationService: NotificationService, private _utilityService: UtilityService) {
    this.headers = new Headers();
    this.headers.append('Content-Type', 'application/json');
  }

  get(uri: string) {
    this.headers.delete("Authorization");
    this.headers.append("Authorization", "Bearer " + this._authenService.getLoggedInUser().access_token);
    return this._http.get(SystemConstants.BASE_API + uri, { headers: this.headers }).map(this.extractData);
  }
  post(uri: string, data?: any) {
    this.headers.delete("Authorization");
    this.headers.append("Authorization", "Bearer " + this._authenService.getLoggedInUser().access_token);
    return this._http.post(SystemConstants.BASE_API + uri, data, { headers: this.headers }).map(this.extractData);
  }
  put(uri: string, data?: any) {
    this.headers.delete("Authorization");
    this.headers.append("Authorization", "Bearer " + this._authenService.getLoggedInUser().access_token);
    return this._http.put(SystemConstants.BASE_API + uri, data, { headers: this.headers }).map(this.extractData);
  }
  delete(uri: string, key: string, id: string) {
    this.headers.delete("Authorization");
    this.headers.append("Authorization", "Bearer " + this._authenService.getLoggedInUser().access_token);
    return this._http.delete(SystemConstants.BASE_API + uri + "/?" + key + "=" + id, { headers: this.headers })
      .map(this.extractData);
  }
  deleteWithMultiParams(uri: string, params) {
    this.headers.delete('Authorization');

    this.headers.append("Authorization", "Bearer " + this._authenService.getLoggedInUser().access_token);
    var paramStr: string = '';
    for (let param in params) {
      paramStr += param + "=" + params[param] + '&';
    }
    return this._http.delete(SystemConstants.BASE_API + uri + "/?" + paramStr, { headers: this.headers })
      .map(this.extractData);

  }
  postFile(uri: string, data?: any) {
    let newHeader = new Headers();
    newHeader.append("Authorization", "Bearer " + this._authenService.getLoggedInUser().access_token);
    return this._http.post(SystemConstants.BASE_API + uri, data, { headers: newHeader })
      .map(this.extractData);
  }

  private extractData(res: Response) {
    let body = res.json();
    return body || {};
  }
  public handleError(error: any) {
    if (error.status == 401) {
      localStorage.removeItem(SystemConstants.CURRENT_USER);
      this._notificationService.printErrorMessage(MessageConstants.LOGIN_AGAIN_MSG);
      this._utilityService.navigateToLogin();
    }
    else if (error.status == 403) {
      localStorage.removeItem(SystemConstants.CURRENT_USER);
      this._notificationService.printErrorMessage(MessageConstants.FORBIDDEN);
      this._utilityService.navigateToLogin();
    }
    if (error.status == 0) {
      this._notificationService.printErrorMessage(MessageConstants.SYSTEM_ERROR_MSG);
    }
    if (error.status == 405) {
      this._notificationService.printErrorMessage(MessageConstants.NO_SUPPORT_FILE);
    }
    if (error.status == 411) {
      this._notificationService.printErrorMessage(MessageConstants.FILE_MAX_LENG);
    }
    if (error.status == 415) {
      this._notificationService.printErrorMessage(MessageConstants.FILE_TYPE_NOT_SUPPORT);
    }
    if (error.status == 502) {
      this._notificationService.printErrorMessage(MessageConstants.SAVE_ERROR);
    }
    if (error.status == 400) {
      if(JSON.parse(error._body).Message==="Error_Edit_By_Admin"){
        localStorage.removeItem(SystemConstants.CURRENT_USER);
        this._utilityService.navigateToLogin();
        this._notificationService.printErrorMessage(MessageConstants.Error_Edit_By_Admin);
        return;
      }
      if(JSON.parse(error._body)==="HNN"){
        this._notificationService.printErrorMessage(MessageConstants.ERROR_CREATE_MSG);
        return;
      }
      let servermsg: any = JSON.parse(error._body)["ModelState"];
      let servermsgBody: any = JSON.parse(error._body)["Message"];
      if (servermsg != null) {
        for (var key in servermsg) {
          for (var i = 0; i < servermsg[key].length; i++) {
            this._notificationService.printErrorMessage(servermsg[key][i]);
          }
        }
        if (servermsg == null) {
          let errMsg = JSON.parse(error._body).Message;
          this._notificationService.printErrorMessage(errMsg);
        }
      }
      else if(servermsgBody != null){
        this._notificationService.printErrorMessage(servermsgBody);
      }
      else {
        let errMsg = JSON.parse(error._body);
        this._notificationService.printErrorMessage(errMsg);
      //  return Observable.throw(errMsg);
      }
    }
    if (error.status == 0) {
      this._notificationService.printErrorMessage(MessageConstants.SYSTEM_ERROR_MSG);
    }
  }
}
