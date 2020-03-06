import { Component, OnInit, NgZone, ViewChild, Input, EventEmitter, group } from '@angular/core';
import { NgModule } from '@angular/core';
import { LoggedInUser } from '../../core/domain/loggedin.user';
import { AuthenService } from '../../core/services/authen.service';
import { SystemConstants } from '../../core/common/system.constants';
import { UrlConstants } from '../../core/common/url.constants';

import { SignalrService } from '../../core/services/signalr.service';
import { DataService } from '../../core/services/data.service';
import { UtilityService } from '../../core/services/utility.service';
import { Router } from '@angular/router/src/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { NotificationService } from '../../core/services/notification.service';
import { MessageConstants } from 'app/core/common/message.constants';
import { read } from 'fs';
import { Idle, DEFAULT_INTERRUPTSOURCES } from '@ng-idle/core';
import { Keepalive } from '@ng-idle/keepalive';
import { NgForm } from '@angular/forms';

@Component({
  selector: 'app-top-menu',
  templateUrl: './top-menu.component.html',
  styleUrls: ['./top-menu.component.css']
})
export class TopMenuComponent implements OnInit {
  private route: Router;

  //#region "Decalre variable, innit some value and so on"
  @ViewChild('ChangePassword') public ChangePassword: ModalDirective;
  
  public user: any;
  public baseFolder: string = SystemConstants.BASE_API;
  public canSendMessage: Boolean;
  public announcements: any[];
  public loadtime: boolean = true;
  //#endregion
  //#region "Constructor to inject Service"
  public entity: any;
  private password: string;
  private newPassword: string;
  private confirmPassword: string;
  public id: number;
  private regexPassword: string = "^(?=.*[A-Za-z])(?=.*\d)(?=.*[$@$!%*?&])[A-Za-z\d$@$!%*?&]{8,16}$";
  //#endregion

  //#region constructor to inject Service
  constructor(private _authenService: AuthenService,
    private utilityService: UtilityService,
    private _signalRService: SignalrService,
    private _dataService: DataService,
    private _notificationService: NotificationService,
    private _ngZone: NgZone, private idle: Idle, private keepalive: Keepalive) {
    // this can subscribe for events  
    this.subscribeToEvents();
    // this can check for conenction exist or not.  
    this.canSendMessage = _signalRService.connectionExists;
    setInterval(() => {
      this.user = JSON.parse(localStorage.getItem(SystemConstants.CURRENT_USER));
      if (localStorage.getItem(SystemConstants.CURRENT_USER) == null) {
        this.utilityService.navigateToLogin();
      }
    }, 1000)
    window.onload = function () {
      if (!localStorage.getItem(SystemConstants.CURRENT_USER)) {
        return utilityService.navigateToLogin();
      }
    }
  }
  //#endregion

  // Innit data or function
  ngOnInit() {
    this.user = this._authenService.getLoggedInUser();
    if (localStorage.getItem(SystemConstants.CURRENT_USER)) {
      this.idle.setIdle(5);
      this.idle.setTimeout(7200);
      this.idle.setInterrupts(DEFAULT_INTERRUPTSOURCES);
      this.idle.onTimeout.subscribe(() => {
        localStorage.clear();
        this.utilityService.navigateToLogin();
        this._notificationService.printErrorMessage(MessageConstants.LOGIN_AGAIN_MSG);
      });
      this.idle.watch();
    }
  }
  //#region "Popup or other"
  public showAdd() {
    this.newPassword = null;
    this.password = null;
    this.confirmPassword = null;
    this.entity = { Content: '' };
    this.ChangePassword.show()
  }
  //#endregion

  //#region "Log Out"
  logout() {
    localStorage.removeItem(SystemConstants.CURRENT_USER);
    this.utilityService.navigate(UrlConstants.LOGIN);
  }
  //#region "CRUD"
  //Function change password
  saveChanges() {
    if (this.validatePassword()) {
      if (this.password == this.newPassword) {
        this._notificationService.printErrorMessage(MessageConstants.DUPLICATE_OLD_PASSWORD);
      }
      else if (this.newPassword != this.confirmPassword) {
        this._notificationService.printErrorMessage(MessageConstants.PASSWORD_NOT_MATCH);
      }
      else {
        this._dataService.put('/api/appUser/changepassword?id=' + this.user.id + '&password=' + this.password + '&newPassword=' + this.newPassword)
          .subscribe((response: any) => {
            this._notificationService.printSuccessMessage(MessageConstants.CHANGE_PASSWORD_OK);
            this.logout();
          },
          error => this._dataService.handleError(error));
      }
    }
  }
  //#endregion

  //#region "Business logic"
  //Validate Password
  validatePassword(): boolean {
    if ( this.newPassword.search(/^(?=.*[A-Za-z])(?=.*\d)(?=.*?[^\w\s])[A-Za-z\d$@$!%*?&^#()+.,]{8,16}$/) < 0
      || this.confirmPassword.search(/^(?=.*[A-Za-z])(?=.*\d)(?=.*?[^\w\s])[A-Za-z\d$@$!%*?&^#()+.,]{8,16}$/) < 0) {
      this._notificationService.printErrorMessage(MessageConstants.CONSTRAINT_NEW_PASSWORD)
      return false;
    }
    else {
      return true;
    }
  }
  //#endregion

  //#region function
  hideModal(form: NgForm){
    this.ChangePassword.hide();
  }

  private subscribeToEvents(): void {

    var self = this;
    self.announcements = [];
    // if connection exists it can call of method.  
    this._signalRService.connectionEstablished.subscribe(() => {
      this.canSendMessage = true;
    });

    // finally our service method to call when response received from server event and transfer response to some variable to be shwon on the browser.  
    this._signalRService.announcementReceived.subscribe((announcement: any) => {
      this._ngZone.run(() => {
        moment.locale('vi');
        announcement.CreatedDate = moment(announcement.CreatedDate).fromNow();
        self.announcements.push(announcement);
      });
    });
  }
  // #endregion

  //JS
  toggleClicked(event: MouseEvent)
  {
      var target = event.target;
      var body = $('body');
      var menu = $('#sidebar-menu');
      
      // toggle small or large menu
      if (body.hasClass('nav-md')) {
          menu.find('li.active ul').hide();
          menu.find('li.active').addClass('active-sm').removeClass('active');
      } else {
          menu.find('li.active-sm ul').show();
          menu.find('li.active-sm').addClass('active').removeClass('active-sm');
      }
      body.toggleClass('nav-md nav-sm');

  }
}
