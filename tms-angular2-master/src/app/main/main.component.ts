import { Component, OnInit, ElementRef } from '@angular/core';
import { SystemConstants } from '../core/common/system.constants';
import { UrlConstants } from '../core/common/url.constants';
import { UtilityService } from '../core/services/utility.service';
import { AuthenService } from '../core/services/authen.service';
import { LoggedInUser } from '../core/domain/loggedin.user';
import { setInterval } from 'timers';
import { Validators } from '@angular/forms';
import { ValidatorHelper } from '../shared/validator/ValidatorHelper';
@Component({
  selector: 'app-main',
  templateUrl: './main.component.html',
  styleUrls: ['./main.component.css']
})
export class MainComponent implements OnInit {
  public user: LoggedInUser;
  public baseFolder: string = SystemConstants.BASE_API;
  constructor(private utilityService: UtilityService, private authenService: AuthenService,private elementRef: ElementRef) {
    setInterval(() => {
      this.user = JSON.parse(localStorage.getItem(SystemConstants.CURRENT_USER));
    }, 1000);

   }

  ngOnInit() {
    var s = document.createElement("script");
    s.type = "text/javascript";
    s.src = "../assets/js/custom.js";
    this.elementRef.nativeElement.appendChild(s);
    this.user = JSON.parse(localStorage.getItem(SystemConstants.CURRENT_USER));
    Validators.minLength = ValidatorHelper.minLength;
  }
 
  logout() {
    localStorage.removeItem(SystemConstants.CURRENT_USER);
    this.utilityService.navigate(UrlConstants.LOGIN);
  }
  
}
