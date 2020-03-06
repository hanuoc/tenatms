import { Component, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../core/services/data.service';
import { NotificationService } from '../../core/services/notification.service';
import { UtilityService } from '../../core/services/utility.service';
import { AuthenService } from '../../core/services/authen.service';

import { MessageConstants } from '../../core/common/message.constants';
import { SystemConstants } from '../../core/common/system.constants';
import { UploadService } from '../../core/services/upload.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Router } from '@angular/router';
import { LoggedInUser } from '../../core/domain/loggedin.user';
import { CommonConstants } from 'app/core/common/common.constants';
import { FunctionConstants } from 'app/core/common/function.constants';
import { DefaultColunmConstants } from 'app/core/common/defaultColunm.constant';
import { StatusConstants } from 'app/core/common/status.constants';
import { PageConstants } from 'app/core/common/page.constans';
import { Console } from '@angular/core/src/console';
import { DateTimeConstants } from 'app/core/common/datetime.constants';
import { SendMailModel } from 'app/core/models/sendMailModel';
import { MailConstants } from 'app/core/common/mail.constants';

@Component({
  selector: 'app-management-delegation-request',
  templateUrl: './management-delegation-request.component.html',
  styleUrls: ['./management-delegation-request.component.css']
})
export class ManagementDelegationRequestComponent implements OnInit {

   //#region "Decalre variable, innit some value and so on"
   public baseFolder: string = SystemConstants.BASE_API;
   public user: any;
   public toEmail: string[];
   public SendMailModel: SendMailModel = null;
   public totalRow: number;
   pagingSize: number = PageConstants.pagingSize;
   public pageIndex: number = PageConstants.pageIndex;
   public pageSize: number = PageConstants.pageSize;
   public listDelegationRequest: any[];
   public currentMaxEntries: number;
   isDesc: boolean = false;
   public entity : any;
   column: string = DefaultColunmConstants.DelegationRequestColunm;
   
   
   constructor(
    public _authenService: AuthenService,
    private _dataService: DataService,
    private _notificationService: NotificationService,
    private _utilityService: UtilityService,
    private _uploadService: UploadService) { 
      if(_authenService.getLoggedInUser().groupId.length == 0){
        _utilityService.navigateToMain();
      }
      if (!_authenService.hasPermission(FunctionConstants.DelegationRequestGL,FunctionConstants.Read)) {
        _utilityService.navigateToMain();
      } 
    }

  ngOnInit() {
    this.user = this._authenService.getLoggedInUser();
    this.getAllDelegationRequest();
    this.toEmail = [];
  }

  //#region "Innit data"
  public getAllDelegationRequest() {
    this._dataService.post('/api/delegationrequest/getalldelegationrequest?userID=' + this.user.id + '&groupId='
    + this.user.groupId + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize + '&column=' + this.column + '&isDesc=' + this.isDesc)
      .subscribe((response: any) => {
        this.listDelegationRequest = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.calShowingPage();
      }, error => this._dataService.handleError(error));
  }
  //#endregion

  //cancel request
  cancelRequest(request: any, title: string, AppUserAssign: any) {
    this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_CANCEL_REQUEST_MSG, () => this.cancelDelegationRequest(request, title, AppUserAssign));
  }

  //method message want to cancel request
  cancelDelegationRequest(request: any, title: string, AppUserAssign: any) {
    this.toEmail.push(AppUserAssign.Email);
    this.SendMailModel = null;
    this.SendMailModel = new SendMailModel(title,this.user.id,this.toEmail,MailConstants.Cancel,MailConstants.DelegationRequest,MailConstants.DelegationRequestSubject,null,MailConstants.DelegationManagement);
    this._dataService.post('/api/delegationrequest/changeStatus?&action=' + StatusConstants.Pending + '&changeStatusBy=' + this.user.id, JSON.stringify(request)).
      subscribe((response: Response) => {
        this._notificationService.printSuccessMessage(MessageConstants.CANCEL_OK_MSG);
        this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModel)).subscribe((response: any) => {});
        this.toEmail = [];
        this.getAllDelegationRequest();
      },  error => {
        if(error.status == 416){
          this._notificationService.printErrorMessage(MessageConstants.REJECTED_OK_MSG);
          this.getAllDelegationRequest();
        }
        else if(error.status == 414){
          this._notificationService.printErrorMessage(MessageConstants.APPROVED_OK_MSG);
          this.getAllDelegationRequest();
        }
      });
  }

  //method change page
  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.getAllDelegationRequest();
    this.calShowingPage();
  }

  //method show current page
  calShowingPage() {
    this.currentMaxEntries = this.pageIndex * this.pageSize;
    if (this.currentMaxEntries >= this.totalRow) {
      this.currentMaxEntries = this.totalRow;
    }
  }
  public checkCancel(entity: any): boolean {
    let x : number = Date.parse(entity.StartDate);
    let x1 :number = Date.parse( moment().startOf('day'));
    let y = moment().startOf('day');
    let z=moment(new Date(entity.StartDate));
    let diffInMs: number = Date.parse( moment().startOf('day')) -  Date.parse(entity.StartDate);
    let date :number = (diffInMs / 1000 / 60 / 60);
    var currentDate = moment().format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    var requestStartDate = moment(new Date(entity.StartDate)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    if ((entity.StatusRequest.Name == CommonConstants.Approved || entity.StatusRequest.Name == CommonConstants.Rejected ||  entity.StatusRequest.Name == CommonConstants.Cancelled)|| x < x1){
      return true;
    }
    else{
      return false;
    }
  }
}
