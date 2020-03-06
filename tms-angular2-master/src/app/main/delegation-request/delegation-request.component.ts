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
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { FunctionConstants } from 'app/core/common/function.constants';
import { DefaultColunmConstants } from 'app/core/common/defaultColunm.constant';
import { filterDelegationAssigned } from '../../core/models/filterDelegationAssigned';
import { StatusConstants } from 'app/core/common/status.constants';
import { PageConstants } from 'app/core/common/page.constans';
import { Announcement } from '../../core/models/announcement';
import { DateRangePickerModel } from '../../core/models/date-range-picker';
import { SendMailModel } from 'app/core/models/sendMailModel';
import { MailConstants } from 'app/core/common/mail.constants';
import { SendMailModelNew } from '../../core/models/SendMailModelNew';
@Component({
  selector: 'app-delegation-request',
  templateUrl: './delegation-request.component.html',
  styleUrls: ['./delegation-request.component.css']
})
export class DelegationRequestComponent implements OnInit {

  //#region "Decalre variable, innit some value and so on"
  @ViewChild('modalAddEdit') public modalAddEdit: ModalDirective;
  public baseFolder: string = SystemConstants.BASE_API;
  public userID: string;
  public toEmail: string[];
  public user: any;
  public entity: any;
  public valueDate: any = {
    end: '',
    start: ''
  }
  SendMailModel: SendMailModel = null;
  groupLead: any;
  public totalRow: number;
  pagingSize: number = PageConstants.pagingSize;
  public pageIndex: number = PageConstants.pageIndex;
  public pageSize: number = PageConstants.pageSize;
  public listDelegationUserOption: any[];
  public listDelegationRequest: any[];
  public ListFilter: filterDelegationAssigned = null;
  public currentMaxEntries: number;
  private startDate: string;
  private endDate: string;
  isDesc: boolean = true;
  column: string = DefaultColunmConstants.DelegationRequestColunm;
  isActived: boolean = false;
  //Declare filter
  public usersOption: IMultiSelectOption[] = [];
  public usersFilter: string;
  //Declare Announment
  public announcement: Announcement = null;
  public statusRequestOption: IMultiSelectOption[] = [];
  public statusRequestFilter: string[] = [];
  public usernameFilter: string[] = [];
  public groupLeadFilter: string[] = [];
  public listGrantUser: IMultiSelectOption[] = [];
  SendMailModelCreated: SendMailModelNew = null;

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
    maxHeight: '200px',
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
    checkedPlural: CommonConstants.CheckStatusType,
    defaultTitle: CommonConstants.StatusRequestTitle

  };

  // Text configuration dropdown user delegation request
  UserDelegationDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectFullName,
    checked: CommonConstants.CheckFullName,
    checkedPlural: CommonConstants.CheckFullName,
    defaultTitle: CommonConstants.FullName
  };

  settingAngularSingle: IMultiSelectSettings = {
    checkedStyle: 'glyphicon',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 1,
    autoUnselect: true,
    closeOnSelect: true,
    selectionLimit: 1,
    maxHeight: '200px',
  };
  //#endregion

  //Contructor to inject services.
  constructor(public _authenService: AuthenService,
    private _dataService: DataService,
    private _notificationService: NotificationService,
    private _utilityService: UtilityService,
    private _uploadService: UploadService,
    private daterangepickerOptions: DaterangepickerConfig) {
    if (_authenService.getLoggedInUser().groupId.length == 0) {
      _utilityService.navigateToMain();
    }
    if (!_authenService.hasPermission(FunctionConstants.DelegationAssign, FunctionConstants.Read)) {
      _utilityService.navigateToMain();
    }
    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: true,
      "opens": "left",
      minDate: false,
      ranges: DateTimeConstants.Range,
      startDate: moment(),
      endDate: moment(),
    };
  }

  // Innit data or function   
  ngOnInit() {
    this.CheckDelegationStatus();
    this.user = this._authenService.getLoggedInUser();
    this.getAllDelegationRequest();
    this.loadUserDelegation();
    this.loadStatus();
    this.toEmail = [];
  }

  //#region "Innit data"
  public getAllDelegationRequest() {
    this.userID = this._authenService.getLoggedInUser().id;
    this._dataService.post('/api/request/getallrequestisassignedforuser?userID=' + this.userID + '&groupId='
      + this.user.groupId + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize + '&column=' + this.column + '&isDesc=' + this.isDesc, JSON.stringify(this.ListFilter))
      .subscribe((response: any) => {
        this.listDelegationRequest = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.calShowingPage();
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
    
  //get infomation for popup
  getInformationForPopup() {
    this.groupLeadFilter = [];
    this._dataService.get('/api/appUser/getgrouplead?groupId=' + this.user.groupId)
      .subscribe((response: any) => {
        this.listGrantUser = [];
        for (let userGroup of response) {
          this.groupLeadFilter.push(userGroup.Id);
          this.listGrantUser.push({ id: userGroup.Id, name: userGroup.FullName });
        }
      }, error => this._dataService.handleError(error));
  }

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
  // check permission of user when load explanation list
  public checkPermisson(statusRequest: string, userId: string): boolean {
    return (userId == this.user.id && (statusRequest == StatusConstants.Approved || statusRequest == StatusConstants.Rejected))
  }
  public hasPermission(functionId: string, action: string): boolean {
    return this._authenService.hasPermission(functionId, action);
  }
  // call event when chosen date
  public selectedDateRangePicker(value: any, dateInput: any) {
    this.valueDate = value;
    dateInput.start = value.start.format();
    dateInput.end = value.end;
  }

  // event chosenData
  public chosenDate: any = {
    start: '',
    end: '',
  };
  public chosenRequestDate: any = {
    start: moment(),
    end: moment().add(3, DateTimeConstants.DAY),
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
  approveDelegationRequest(request: any, Title: any, AppUser: any) {
    this.loadRequestDetailData(request.ID);
    this._dataService.post('/api/delegationrequest/changeStatus?&action=' + CommonConstants.Approved + '&changeStatusBy=' + this.user.id, JSON.stringify(request)).
      subscribe((response: Response) => {
        this.toEmail.push(AppUser.Email + "," + this.entity.AppUser.FullName);
        this.SendMailModelCreated = new SendMailModelNew(this.entity.AppUser.Group.Name, this.entity.RequestType.Name, this.entity.DetailReason, this.entity.StartDate, this.entity.EndDate, null, null, null, null, null, null, null, null, null, Title, this.user.id, this.toEmail, null, MailConstants.Approve, MailConstants.Request, MailConstants.RequestSubject, null, MailConstants.RequestManagement);
        this._notificationService.printSuccessMessage(MessageConstants.APPROVED_SUCCES_REQUEST_MSG);
        this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModelCreated)).subscribe((response: any) => { });
        this.resetEmail();
        this.modalAddEdit.hide();
        this.getAllDelegationRequest();
      }, error => {
        if (error.status == 400) {
          this._notificationService.printErrorMessage(MessageConstants.APPROVED_DELEGATION_REQUEST_MSG);
          this.getAllDelegationRequest();
        }
      });
  }

  //Reject Request
  rejectDelegationRequest(request: any, Title: string, AppUser: any) {
    this._notificationService.printPromptDialog("", MessageConstants.TEXTAREA_COMMENT_REQUEST_MSG, (value: string) => this.rejectDelegationRequestConfirm(value, request, Title, AppUser));
  }

  //Display message comment reason reject
  rejectDelegationRequestConfirm(value: any, request: any, Title: string, AppUser: any) {
    if (value != '') {
      this.loadRequestDetailData(request.ID);
      this._dataService.post('/api/delegationrequest/changeStatus?action=' + CommonConstants.Rejected + '&changeStatusBy=' + this.user.id, JSON.stringify(request)).
        subscribe((response: Response) => {
          this.toEmail.push(AppUser.Email + "," + this.entity.AppUser.FullName);
          this.SendMailModelCreated = new SendMailModelNew(this.entity.AppUser.Group.Name, this.entity.RequestType.Name, this.entity.DetailReason, this.entity.StartDate, this.entity.EndDate, null, null, null, null, null, null, null, null, null, Title, this.user.id, this.toEmail, null, MailConstants.Reject, MailConstants.Request, MailConstants.RequestSubject, value, MailConstants.RequestManagement);
          this._notificationService.printSuccessMessage(MessageConstants.REJECTED_SUCCES_REQUEST_MSG);
          this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModelCreated)).subscribe((response: any) => { });
          this.resetEmail();
          this.modalAddEdit.hide();
          this.getAllDelegationRequest();
        });
    } else {
      this._notificationService.printErrorMessage(MessageConstants.VALID_COMMENT_REJECT_MSG);
    }
  }

  // method reset email
  resetEmail() {
    this.toEmail = [];
  }
  //Check User login can Approve request
  public checkApprove(entity: any): boolean {
    var currentDate = moment().format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    var requestStartDate = moment(new Date(entity.StartDate)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    return ((entity.StatusRequest.Name == CommonConstants.Delegated || entity.StatusRequest.Name == CommonConstants.Rejected) && requestStartDate >= currentDate)
  }
  //Check User login can Reject request
  public checkReject(entity: any): boolean {
    var currentDate = moment().format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    var requestStartDate = moment(new Date(entity.StartDate)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    return ((entity.StatusRequest.Name == CommonConstants.Delegated || entity.StatusRequest.Name == CommonConstants.Approved) && requestStartDate >= currentDate)
  }

  // method reset funct of filter
  public reset() {
    this.usernameFilter = [];
    this.statusRequestFilter = [];
    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: true,
      "opens": "left",
      minDate: false,
      ranges: DateTimeConstants.Range,
      startDate: moment(),
      endDate: moment(),
    };
    this.valueDate.end = moment();
    this.valueDate.start = moment();
    this.selectedDateRangePicker(this.valueDate, this.chosenDate);
    this.chosenDate.start = '';
    this.chosenDate.end = '';
    this.ListFilter = null;
    this.pageIndex = PageConstants.pageIndex;
    this.getAllDelegationRequest();
  }

  // method filter request assign
  public filterRequestIsAssign() {
    this.pageIndex = PageConstants.pageIndex;
    if (this.chosenDate.start != '' && this.chosenDate.end != '') {
      this.startDate = moment(new Date(this.chosenDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      this.endDate = moment(new Date(this.chosenDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    } else {
      this.startDate = null;
      this.endDate = null;
    }
    this.ListFilter = new filterDelegationAssigned(this.usernameFilter, this.statusRequestFilter, this.startDate, this.endDate);
    this.getAllDelegationRequest();
  }

  // method change page
  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.getAllDelegationRequest();
    this.calShowingPage();
  }

  // method show current page
  calShowingPage() {
    this.currentMaxEntries = this.pageIndex * this.pageSize;
    if (this.currentMaxEntries >= this.totalRow) {
      this.currentMaxEntries = this.totalRow;
    }
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
  Deactive() {
    this._dataService.get('/api/delegationrequest/active_inactive-delegation?action=' + 'Inactive')
      .subscribe((response: any) => {
        this.isActived = false;
        this._notificationService.printSuccessMessage("Delegation is Deactivated !")
      }, error => this._dataService.handleError(error));
  }

  // Modal detail
  loadRequestDetail(id: number) {
    this._dataService.get('/api/request/detail/' + id)
      .subscribe((response: any) => {
        this.entity = response;
        this.modalAddEdit.show();
        this.getInformationForPopup();
        this.chosenRequestDate.start = response.StartDate;
        this.chosenRequestDate.end = response.EndDate;
        this.entity.StartDate = moment(new Date(this.chosenRequestDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
        this.entity.EndDate = moment(new Date(this.chosenRequestDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      }, error => this._dataService.handleError(error));
  }

  // Modal detail data for sendmail
  loadRequestDetailData(id: number) {
    this._dataService.get('/api/request/detail/' + id)
      .subscribe((response: any) => {
        this.entity = response;
        this.entity.StartDate = moment(new Date(this.chosenRequestDate.start)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
        this.entity.EndDate = moment(new Date(this.chosenRequestDate.end)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
      }, error => this._dataService.handleError(error));
  }
  //#endregion
}