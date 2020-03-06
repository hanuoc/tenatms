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
import { DaterangepickerConfig } from '../../core/services/config.service';
import { DateTimeConstants } from 'app/core/common/datetime.constants';
import { CommonConstants } from 'app/core/common/common.constants';
import { FunctionConstants } from 'app/core/common/function.constants';
import { DefaultColunmConstants } from 'app/core/common/defaultColunm.constant';
import { filterDelegationAssigned } from '../../core/models/filterDelegationAssigned';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { StatusConstants } from 'app/core/common/status.constants';
import { PageConstants } from 'app/core/common/page.constans';
import { Announcement } from '../../core/models/announcement';
import { DateRangePickerModel } from '../../core/models/date-range-picker';
import { SendMail } from 'app/core/models/sendMail';
import { SendMailModel } from '../../core/models/sendMailModel';
import { MailConstants } from 'app/core/common/mail.constants';
import { SendMailModelNew } from '../../core/models/SendMailModelNew';
@Component({
  selector: 'app-delegation-explanation-request',
  templateUrl: './delegation-explanation-request.component.html',
  styleUrls: ['./delegation-explanation-request.component.css']
})
export class DelegationExplanationRequestComponent implements OnInit {
  //#region "Declare variable, innit some value and so on"
  @ViewChild('addEditModal') public addEditModal: ModalDirective;
  public baseFolder: string = SystemConstants.BASE_API;
  public explanations: any[];
  public statusRequest: any[];
  public user: any;
  public value: any = {
    end : '',
    start : ''
  };
  public toEmail: string[];
  SendMailModel: SendMailModel = null;
  groupLead:any;
  public isDesc: boolean = false;
  isActived :boolean =false;
  column: string = DefaultColunmConstants.DelegationRequestColunm;
  pagingSize: number = PageConstants.pagingSize;
  public pageIndex: number = PageConstants.pageIndex;
  public pageSize: number = PageConstants.pageSize;
  public listDelegationUserOption: IMultiSelectOption[] = [];
  public totalRow: number;
  public ListFilter: filterDelegationAssigned = null;
  private startDate: string;
  private endDate: string;
  public announcement: Announcement = null;
  public totalEntries: number;
  public statusRequestOption: IMultiSelectOption[] = [];
  public statusFilterOptions: IMultiSelectOption[] = [];
  public reasonFilterOptions: IMultiSelectOption[] = [];

  public usernameFilter: string[];
  public statusRequestFilter: string[] = [];
  public isUnauthorizedLeave: boolean = false;
  public entity: any;
  SendMailModelCreated: SendMailModelNew = null;
  public ExplanationDate :string;
  //#endregion

  // Stauts filter setting
  StatusFilterSettings: IMultiSelectSettings = {
    enableSearch: false,
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 1,
    displayAllSelectedText: true,
    showCheckAll: true,
    showUncheckAll: true,
    closeOnClickOutside: true,
    maxHeight:'200px',
  };

  // User filter setting
  UserFiltersettings: IMultiSelectSettings = {
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 0,
    autoUnselect: true,
    maxHeight: '200px',
    enableSearch: true,
    displayAllSelectedText: true,
    showCheckAll: true,
    showUncheckAll: true,
  };

  // Text configuration dropdown status request
  StatusDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectStatusType,
    checked: CommonConstants.CheckStatusType,
    checkedPlural:CommonConstants.CheckStatusType,
    defaultTitle :CommonConstants.StatusRequestTitle
  };

  // Text configuration dropdown user delegation request
  UserDelegationDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectFullName,
    checked: CommonConstants.CheckFullName,
    checkedPlural: CommonConstants.CheckFullName,
    defaultTitle: CommonConstants.FullName

  };
  //#endregion

  //Contructor to inject services
  constructor(public _authenService: AuthenService,
    private _dataService: DataService,
    private _notificationService: NotificationService,
    private daterangepickerOptions: DaterangepickerConfig,
    private _utilityService:UtilityService) {
      if(_authenService.getLoggedInUser().groupId.length == 0){
        _utilityService.navigateToMain();
      }
      this.daterangepickerOptions.settings = {
        locale: DateTimeConstants.Locale,
        alwaysShowCalendars: true,
        "opens": "left",
        ranges: DateTimeConstants.Range,
    };
  }

  // Innit data or function
  ngOnInit() {
    this.CheckDelegationStatus();
    this.user = this._authenService.getLoggedInUser();
    this.loadData();
    this.loadStatus();
    this.monitorPageChange();
    this.loadUserDelegation();
    this.SendMailModel = null;
    this.toEmail = [];
  }

  //#region "Init data"
  public loadData() {
    this._dataService.post('/api/explanation/requestassigned?userId=' + this.user.id + '&groupId='
      + this.user.groupId + '&column=' + this.column + '&isDesc=' + this.isDesc + '&page='
      + this.pageIndex + '&pageSize=' + this.pageSize, JSON.stringify(this.ListFilter))
      .subscribe((response: any) => {
        this.explanations = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.monitorPageChange();
      }, error => this._dataService.handleError(error));
  }

  // method load status of request
  public loadStatus() {
    this._dataService.get('/api/statusrequest/getall')
      .subscribe((response: any) => {
        this.statusRequestOption = [];
        for (let status of response) {
          if (status.Name == StatusConstants.Approved || status.Name == StatusConstants.Delegation || status.Name == StatusConstants.Rejected)
            this.statusRequestOption.push({ id: status.ID, name: status.Name })
        }
      }, error => this._dataService.handleError(error));
  }

  // method get username assign
  public loadUserDelegation() {
    this._dataService.get('/api/appUser/getuserbygroup')
      .subscribe((response: any) => {
        this.groupLead = response;
        this.listDelegationUserOption = [];
        for (let user of response) {
          this.listDelegationUserOption.push({ id: user.Id, name: user.FullName + " ( " + user.UserName + " ) "  })
        }
      }, error => this._dataService.handleError(error));
  }
 // method check delegation status
 public CheckDelegationStatus() {
  this._dataService.get('/api/delegationrequest/check-delegation-status')
    .subscribe((response: any) => {
      if (response === "Actived") {
        this.isActived = true;
      } else {
        this.isActived = false;
      }
    }, error => this._dataService.handleError(error));
}
  // event chosenData
  public chosenDate: any = {
    start: '',
    end: '',
  };

  // date picker
  public picker = {
    opens: CommonConstants.LEFT,
    startDate: moment().subtract(3, DateTimeConstants.DAY),
    endDate: moment(),
    isInvalidDate: function (date) {
      if (date.isSame(Date.toString(), DateTimeConstants.DAY))
        return 'mystyle';
      return false;
    }
  }
  //#endregion

  //#region "Business logic"

  //Approve Request
  approveDelegationExplanationRequest(id: any, Title: string, email: string) {
    // this.toEmail.push(email);
    // this.toEmail.push(this.groupLead[0].Email);
    this.showDetailData(id);
    // this.SendMailModel = new SendMailModel(Title,this.user.id,this.toEmail,MailConstants.Approve,MailConstants.Request,MailConstants.DelegationExplanationRequestSubject,null,MailConstants.ExplanationRequestManagement);
    this._dataService.post('/api/explanation/changestatus?explanationId=' + id + '&statusName=' + CommonConstants.Approved + '&delegateId=' + this.user.id).
      subscribe((response: Response) => {
        this.ExplanationDate = moment(new Date(this.entity.ExplanationDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY)
        this.toEmail.push(email + "," + this.entity.User.FullName);
        this.SendMailModelCreated = new SendMailModelNew(this.entity.GroupName,null,this.entity.DetailReason,this.entity.StartDate,this.entity.EndDate,null,null,null,null,null,this.ExplanationDate,this.entity.ReasonList,this.entity.Actual,this.entity.ReasonDetail,Title, this.user.id, this.toEmail,null,  MailConstants.Approve, MailConstants.ExplanationRequest, MailConstants.ExplanationRequestSubject, null, MailConstants.ExplanationRequestManagement);
        this._notificationService.printSuccessMessage(MessageConstants.APPROVED_SUCCES_REQUEST_MSG);
        this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModelCreated)).subscribe((response: any) => {})
        this.resetEmail();
        this.addEditModal.hide();
        this.loadData();
      });
  }

  //Reject Request
  rejectDelegationExplanationRequest(id: any,Title: string, email: string,  userId: string) {
    this._notificationService.printPromptDialog("", MessageConstants.TEXTAREA_COMMENT_REJECT_EXPLANATION_MSG, (value: string) => this.rejectDelegationRequestConfirm(value, id, email, Title, userId));
  }

  //Display message comment reason reject
  rejectDelegationRequestConfirm(value: any, id: any, email: any, Title: string, userId: string) {
    if (value != '') {
      this.showDetailData(id);
      this._dataService.post('/api/explanation/changeStatus?explanationId=' + id + '&statusName=' + CommonConstants.Rejected+ '&delegateId=' + this.user.id).
        subscribe((response: Response) => {
          this.ExplanationDate = moment(new Date(this.entity.ExplanationDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY)
          this.toEmail.push(email + "," + this.entity.User.FullName);
          this.SendMailModelCreated = new SendMailModelNew(this.entity.GroupName,null,this.entity.DetailReason,this.entity.StartDate,this.entity.EndDate,null,null,null,null,null,this.ExplanationDate,this.entity.ReasonList,this.entity.Actual,this.entity.ReasonDetail,Title, this.user.id, this.toEmail,null,  MailConstants.Reject, MailConstants.ExplanationRequest, MailConstants.ExplanationRequestSubject, value, MailConstants.ExplanationRequestManagement);
          this._notificationService.printSuccessMessage(MessageConstants.REJECTED_SUCCES_EXPLANATION_REQUEST_MSG);
          this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModelCreated)).subscribe((response: any) => {})
          this.resetEmail();
          this.loadData();
          this.addEditModal.hide();
        });
    } else {
      this._notificationService.printErrorMessage(MessageConstants.VALID_COMMENT_REJECT_MSG);
    }
  }

  //Check User login can Approve request
  public checkApprove(entity: any): boolean {
    return ((entity.StatusRequest.Name == CommonConstants.Delegated || entity.StatusRequest.Name == CommonConstants.Rejected) && !entity.IsExpiredDate);
  }

  //Check User login can Reject request
  public checkReject(entity: any): boolean {
    return ((entity.StatusRequest.Name == CommonConstants.Delegated || entity.StatusRequest.Name == CommonConstants.Approved) && !entity.IsExpiredDate);
  }

  public getAllowRangeDate(date: any): boolean {
    let diffInMs: number = (Date.parse(moment().format(DateTimeConstants.FORMAT_DATE_MMDDYYYY)) - Date.parse(date));
    return (diffInMs / 1000 / 60 / 60) <= CommonConstants.DATE_RANGE_CHANGE_STATUS_EXPLANATION;
  }

  // check permission of user when load delegation explanation list
  public checkPermisson(statusRequest: string, userId: string): boolean {
    return (userId == this.user.id && (statusRequest == StatusConstants.Delegation))
  }

  // check permission of user when load delegation explanation list
  public hasPermission(functionId: string, action: string): boolean {
    return this._authenService.hasPermission(functionId, action);
  }

  // reset all chonsen filter and get list explanation request
  reset() {
    this.usernameFilter = [];
    this.ListFilter = null;
    this.statusRequestFilter = [];
    this.daterangepickerOptions.settings = {
      ranges: DateTimeConstants.Range,
      locale: DateTimeConstants.Locale,
      autoUpdateInput: true,
      alwaysShowCalendars: true,
      "opens": "left",
    };
        this.value.start = moment();
        this.value.end = moment();
        this.selectedDateRangePicker(this.value, this.chosenDate);
        this.chosenDate.start = '';
        this.chosenDate.end = '';
    this.loadData();
  }
  
  // method reset email
  resetEmail(){
    this.toEmail = [];
  }

  // method filter
  filterRequestIsAssign() {
    if(this.chosenDate.start != '' && this.chosenDate.end != ''){
      this.startDate = moment(new Date(this.chosenDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      this.endDate = moment(new Date(this.chosenDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      }else{
        this.startDate = null;
        this.endDate = null;
      }
    this.ListFilter = new filterDelegationAssigned(this.usernameFilter, this.statusRequestFilter, this.startDate, this.endDate);
    this.loadData();
  }

  // call when next/previous page
  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.loadData();
    this.monitorPageChange();
  }

  // calculate total showing entries when page change
  public monitorPageChange() {
    this.totalEntries = this.pageIndex * this.pageSize;
    if (this.totalEntries >= this.totalRow) {
      this.totalEntries = this.totalRow;
    }
  }

  // call event when chosen date
  public selectedDatePicker(value: any, dateInput: any) {
    dateInput.start = value.start.format();
    dateInput.end = value.end;
  }

  // call event when chosen date
  public selectedDateRangePicker(value: any, dateInput: any) {
    this.value = value;
    dateInput.start = value.start.format();
    dateInput.end = value.end;
  }
  // Active/inactive delegation
  ActiveDelegation() {
    this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_Active_Delegation,
      () => this.Active());
  }
  Active() {
    this._dataService.get('/api/delegationrequest/active_inactive-delegation?action=' + 'Active')
      .subscribe((response: any) => {
        this.isActived = true;
        this._notificationService.printSuccessMessage("Delegation is activated!")
      }, error => this._dataService.handleError(error));
  }
  DeactiveDelegation() {
    this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_Inactive_Delegation,
      () => this.Deactive());
  }
  Deactive(){
    this._dataService.get('/api/delegationrequest/active_inactive-delegation?action=' + 'Inactive')
    .subscribe((response: any) => {
      this.isActived = false;
      this._notificationService.printSuccessMessage("Delegation is Deactivated !")
    }, error => this._dataService.handleError(error));
  }
  public showDetail(id: number) {
    this._dataService.get('/api/explanation/detail/' + id).subscribe((response: any) => {
      if (response.ReasonList.includes('Unauthorized Leave')) {
        this.isUnauthorizedLeave = true;
      } else {
        this.isUnauthorizedLeave = false;
      }
      this.entity = response;
      this.addEditModal.show();
    }, error => this._dataService.handleError(error));
  }

  //show data for sendmail
  public showDetailData(id: number) {
    this._dataService.get('/api/explanation/detail/' + id).subscribe((response: any) => {
      this.entity = response;
    }, error => this._dataService.handleError(error));
  }
  //#endregion
}