import { Component, OnInit } from '@angular/core';
import { DataService } from '../../../core/services/data.service';
import { AuthenService } from '../../../core/services/authen.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { NgForm } from '@angular/forms';
import { UrlConstants } from '../../../core/common/url.constants';
import { element } from 'protractor';
import { NgModel } from '@angular/forms/src/directives/ng_model';
import { SystemConstants } from '../../../core/common/system.constants';
import { UserGroup } from '../../../core/models/usergroup';
import { NotificationService } from '../../../core/services/notification.service';
import { MessageConstants } from '../../../core/common/message.constants';
import { error } from 'util';
import { DateTimeConstants } from '../../../core/common/datetime.constants';
import { LoggedInUser } from '../../../core/domain/loggedin.user';
@Component({
  selector: 'app-user-detail',
  templateUrl: './user-detail.component.html',
  styleUrls: ['./user-detail.component.css']
})
export class UserDetailComponent implements OnInit {

  //#region "Decalre variable, innit some value and so on"
  public user: any;
  public isEdit: boolean;
  //#endregion

  // Constructor to inject services.
  constructor(
    private _dataService: DataService,
    private _authenService: AuthenService,
    private _notificationService: NotificationService) {
  }

  // Init data
  ngOnInit(): void {
    this.isEdit = false;
    this.reset();
  }

  //#region "Init data"
  loadDataDetail(id: string) {
    this._dataService.get('/api/appUser/detail/' + this._authenService.getLoggedInUser().id).subscribe((response: any) => {
      this.user = response;
      this.user.BirthDay = moment(new Date(this.user.BirthDay));
    }, error => this._dataService.handleError(error));
  }
  //#endregion

  //#region "CRUD"
  private saveData(form: NgForm) {
    this.user.FullName = this.user.FullName.trim();
    this._dataService.put('/api/appUser/update-profile', JSON.stringify(this.user))
      .subscribe((response: any) => {
        this._notificationService.printSuccessMessage(MessageConstants.UPDATED_OK_MSG);
        this.isEdit = false;
        this.resetLocalStorage();
        this.reset();
      }, error => {
        this._dataService.handleError(error);
        this.reset();
      });
  }
  //#endregion

  //#region "Business logic"

  resetLocalStorage() {
    let userUpdated = JSON.parse(localStorage.getItem(SystemConstants.CURRENT_USER));
    userUpdated.fullName = this.user.FullName;
    userUpdated.email = this.user.Email;
    localStorage.setItem(SystemConstants.CURRENT_USER, JSON.stringify(userUpdated));
  }
  reset() {
    this.user = this._authenService.getLoggedInUser();
    this.loadDataDetail(this.user.id);
  }
  showEdit() {
    this.isEdit = true;
  }
  cancelEdit() {
    this.isEdit = false;
    this.reset();
  }
  //#endregion
}
