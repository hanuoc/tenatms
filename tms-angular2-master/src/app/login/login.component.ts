import { Component, OnInit, ViewChild } from '@angular/core';
import { NotificationService } from '../core/services/notification.service';
import { AuthenService } from '../core/services/authen.service';
import { MessageConstants } from '../core/common/message.constants';
import { UrlConstants } from '../core/common/url.constants';
import { Router } from '@angular/router';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { SystemConstants } from 'app/core/common/system.constants';
import { clearTimeout, clearInterval, setInterval } from 'timers';
import { fail } from 'assert';
@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  //#region "Decalre variable, innit some value and so on"
  @ViewChild('modalForgotPassword') public modalForgotPassword: ModalDirective;
  loading = false;
  model: any = {};
  public entity: any;
  //#endregion

  //#region  Contructor init of 
  constructor(private authenService: AuthenService,
    private notificationService: NotificationService,
    private router: Router) {
    // if exist session then automatically page home
    let interval = setInterval(() => {
      if (localStorage.getItem(SystemConstants.CURRENT_USER)) {
        this.router.navigate([UrlConstants.HOME]);
        clearInterval(interval);
      }
    }, 1000);
  }

  //#endregion contructor
  //#region Innit data on function
  ngOnInit() {
  }
  //#endregion Innit data

  //#region Login
  login() {
    this.loading = true;
    this.authenService.login(this.model.username.trim(), this.model.password).subscribe(data => {
      this.router.navigate([UrlConstants.HOME]);
    }, error => {
      if (error.status == 400) {
        let errMsg = JSON.parse(error._body).error_description;
        this.notificationService.printErrorMessage(errMsg);
        this.loading = false;
      }
      else if (error.status == 0) {
        this.notificationService.printErrorMessage(MessageConstants.SYSTEM_ERROR_MSG);
        this.loading = false;
      }
    });
  }
  //#endregion login
}
