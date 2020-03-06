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
import { Response } from '@angular/http/src/static_response';
import { SendMailModel } from 'app/core/models/sendMailModel';
import { MailConstants } from 'app/core/common/mail.constants';
import { DateTimeConstants } from 'app/core/common/datetime.constants';

@Component({
  selector: 'app-management-delegation-explanation-request',
  templateUrl: './management-delegation-explanation-request.component.html',
  styleUrls: ['./management-delegation-explanation-request.component.css']
})
export class ManagementDelegationExplanationRequestComponent implements OnInit {

  //#region "Decalre variable, innit some value and so on"
  public baseFolder: string = SystemConstants.BASE_API;
  public user: any;
  public toEmail: string[];
  public SendMailModel: SendMailModel = null;
  public totalRow: number;
  pagingSize: number = PageConstants.pagingSize;
  public pageIndex: number = PageConstants.pageIndex;
  public pageSize: number = PageConstants.pageSize;
  public listDelegationExplanationRequest: any[];
  public currentMaxEntries: number;
  isDesc: boolean = false;
  column: string = DefaultColunmConstants.CreatedBy;
  constructor(
    public _authenService: AuthenService,
    private _dataService: DataService,
    private _notificationService: NotificationService,
    private _utilityService: UtilityService,
    private _uploadService: UploadService) {
      if(_authenService.getLoggedInUser().groupId.length == 0){
        _utilityService.navigateToMain();
      }
      if (!_authenService.hasPermission(FunctionConstants.DelegationExplanation,FunctionConstants.Read)) {
        _utilityService.navigateToMain();
      } 
  }

  ngOnInit() {
    this.user = this._authenService.getLoggedInUser();
    this.getAllDelegationExplanationRequest();
    this.toEmail = [];
  }

  //#region  "Innit data"
  public getAllDelegationExplanationRequest() {
    this._dataService.post('/api/delegationexplanationrequest/getalldelegationexplanationrequest?userID=' + this.user.id + '&groupID=' + this.user.groupId + '&column=' + this.column + '&isDesc=' + this.isDesc + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize)
      .subscribe((response: any) => {
        this.listDelegationExplanationRequest = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.calShowingPage()
      }, error => this._dataService.handleError(error));
  }
  //#endregion

  //cancel request
  cancelRequest(id: any, title: string, Delegate: any) {
    this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_CANCEL_REQUEST_MSG, () => this.cancelDelegationExplanationRequest(id, title, Delegate));
  }

  //method display message
  cancelDelegationExplanationRequest(id: any, title: string, Delegate: any) {
    this.toEmail.push(Delegate.Email);
    this.SendMailModel = null;
    this.SendMailModel = new SendMailModel(title, this.user.id,this.toEmail,MailConstants.Cancel,MailConstants.DelegationExplanationRequest,MailConstants.DelegationExplanationRequestSubject,null,MailConstants.DelegationManagement);
    this._dataService.post('/api/delegationexplanationrequest/changestatus?requestID=' + id + '&action=' + StatusConstants.Pending + '&changeStatusBy=' + this.user.id)
      .subscribe((response: Response) => {
        this._notificationService.printSuccessMessage(MessageConstants.CANCEL_OK_MSG);
        this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModel)).subscribe((response: any) => {});
        this.toEmail = [];
        this.getAllDelegationExplanationRequest();
      }, error => {
        if(error.status == 416){
          this._notificationService.printErrorMessage(MessageConstants.REJECTED_OK_MSG);
          this.getAllDelegationExplanationRequest();
        }
        else if(error.status == 414){
          this._notificationService.printErrorMessage(MessageConstants.APPROVED_OK_MSG);
          this.getAllDelegationExplanationRequest();
        }
      });
  }

  //method change page
  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.getAllDelegationExplanationRequest();
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
    let j = ( moment().startOf('day'));
    let i = moment(new Date(entity.CreatedDate)).startOf('day').add(2, DateTimeConstants.DAY)
    // let diffInMs: number = (Date.parse(moment()) - Date.parse(entity.CreatedDate));
    // let date :number = (diffInMs / 1000 / 60 / 60);
    // let abDate = moment(new Date(abnormaldate)).add(DateTimeConstants.LimitTimeApproveRejectRequest, DateTimeConstants.HOURS);
    if ((entity.StatusRequest.Name == CommonConstants.Approved || entity.StatusRequest.Name == CommonConstants.Rejected ||  entity.StatusRequest.Name == CommonConstants.Cancelled)|| j>i){
      return true;
    }
    else{
      return false;
    }
  }
}
