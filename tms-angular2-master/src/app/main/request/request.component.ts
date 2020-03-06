import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm, Validator } from '@angular/forms';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { AuthenService } from 'app/core/services/authen.service';
import { DataService } from 'app/core/services/data.service';
import { error } from 'util';
import { DateTimeConstants } from 'app/core/common/datetime.constants';
import { CommonConstants } from 'app/core/common/common.constants';
import { IMultiSelectOption, IMultiSelectTexts, IMultiSelectSettings } from 'angular-2-dropdown-multiselect';
import { FilterRequest } from '../../core/models/filterRequest';
import { filter } from 'rxjs/operator/filter';
import { DaterangepickerConfig } from 'app/core/services/config.service';
import { NotificationService } from 'app/core/services/notification.service';
import { MessageConstants } from 'app/core/common/message.constants';
import { LoggedInUser } from 'app/core/domain/loggedin.user';
import { fail } from 'assert';
import { DefaultColunmConstants } from '../../core/common/defaultColunm.constant';
import { Announcement } from '../../core/models/announcement';
import { DateRangePickerModel } from '../../core/models/date-range-picker';
import { PageConstants } from 'app/core/common/page.constans';
import { SendMailModel } from '../../core/models/sendMailModel';
import { MailConstants } from '../../core/common/mail.constants';
import { UtilityService } from '../../core/services/utility.service';
import { Hero } from '../../core/models/dashboardModel';
import { PassDataService } from '../../core/services/passData.service';
import { SendMailModelTest } from 'app/core/models/SendMailModelTest';
import { SendMailModelNew } from '../../core/models/SendMailModelNew';
import { forEach } from '@angular/router/src/utils/collection';
import { SendMailDelegateRequest } from '../../core/models/SendMailDelegateRequest';
import { create } from 'domain';
@Component({
  selector: 'app-request',
  templateUrl: './request.component.html',
  styleUrls: ['./request.component.css']
})
export class RequestComponent implements OnInit {

  //#region "Declare variable, innit some value and so on"
  // Settings configuration
  settingAngularMulti: IMultiSelectSettings = {
    enableSearch: false,
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 0,
    displayAllSelectedText: true,
    showCheckAll: true,
    showUncheckAll: true,
    closeOnClickOutside: true,
    maxHeight: '200px',
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
  settingAngularSingleSearch: IMultiSelectSettings = {
    checkedStyle: 'glyphicon',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 1,
    autoUnselect: true,
    closeOnSelect: true,
    selectionLimit: 1,
    enableSearch: true,
    maxHeight: '200px',
  };
  settingAngularMultiSearch: IMultiSelectSettings = {
    enableSearch: true,
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    displayAllSelectedText: true,
    showCheckAll: true,
    dynamicTitleMaxItems: 0,
    showUncheckAll: true,
    closeOnClickOutside: true,
    maxHeight: '200px',
  };

  dropdownSearchFilterSettings: IMultiSelectSettings = {
    enableSearch: true,
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 0,
    displayAllSelectedText: true,
    showCheckAll: true,
    showUncheckAll: true,
    closeOnClickOutside: true,
    maxHeight: '200px',
  };
  // Text configuration
  textStatusRequest: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectStatusType,
    checked: CommonConstants.CheckStatusType,
    checkedPlural: CommonConstants.CheckStatusType,
    defaultTitle: CommonConstants.StatusRequestTitle,
  };
  textCreatorRequest: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectFullName,
    checked: CommonConstants.CheckFullName,
    checkedPlural: CommonConstants.CheckFullName,
    defaultTitle: CommonConstants.FullName,
  };
  textRequestType: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectRequestType,
    checked: CommonConstants.CheckRequestType,
    checkedPlural: CommonConstants.CheckRequestType,
    defaultTitle: CommonConstants.requestType,
  };
  textRequestReasonType: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectReasonType,
    checked: CommonConstants.CheckReasonType,
    checkedPlural: CommonConstants.CheckReasonType,
    defaultTitle: CommonConstants.requestReasonType,
  };
  textDelegatorRequest: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectDelegate,
    checked: CommonConstants.CheckDelegateType,
    checkedPlural: CommonConstants.CheckDelegateType,
    defaultTitle: CommonConstants.DelegatorTitle,
  };

  FullNameDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectFullName,
    checked: CommonConstants.CheckFullName,
    checkedPlural: CommonConstants.CheckFullName,
    defaultTitle: CommonConstants.FullName,
  };

  @ViewChild('modalAddEdit') public modalAddEdit: ModalDirective;
  @ViewChild('modalDelegate') public modalDelegate: ModalDirective;
  @ViewChild('addEditForm') public addEditForm: NgForm;

  public entity: any;
  public toEmail: string[];
  public ccMail: string[];
  public requests: any[];
  public requestssuperadmin: any[];
  public DetailRequest: any;
  public userLogin: LoggedInUser;
  public totalRow: number;
  pagingSize: number = PageConstants.pagingSize;
  public pageIndex: number = PageConstants.pageIndex;
  public pageSize: number = PageConstants.pageSize;
  public filterCreators: IMultiSelectOption[] = [];
  public filterCreatorsSuperAdmin: IMultiSelectOption[] = [];
  public filterStatusRequests: IMultiSelectOption[] = [];
  public filterRequestTypes: IMultiSelectOption[] = [];
  public filterRequestReasonTypes: IMultiSelectOption[] = [];
  public filterRequestReasonTypesEntitleDay: IMultiSelectOption[] = [];
  public filterDelegator: IMultiSelectOption[] = [];
  public myCreator: string[];
  public myFilterStatusRequests: string[] = [];
  public myFilterRequestTypes: string[] = [];
  public myFilterRequestReasonTypes: string[] = [];
  public myFilterDelegators: string[] = [];
  public listGrantUser: IMultiSelectOption[] = [];
  public loadGroupRequest: IMultiSelectOption[] = [];
  public createRequestTypes: IMultiSelectOption[] = [];
  public myRequestType: string;
  public myRequestReasonType: string;
  public currentMaxEntries: number;
  SendMailModel: SendMailModel = null;
  SendMailModelCreated: SendMailModelNew = null;
  listMail: Array<SendMailModel> = [];
  public FullNameOption: IMultiSelectOption[] = [];
  public requestObject: any;
  public value: any = {
    end: '',
    start: ''
  };
  public valueDate: any = {
    end: '',
    start: ''
  };
  DashBoard: Hero;
  //lvtung
  public isDisable: boolean = false;
  loadsuccess: boolean = false;
  public isUser: boolean = false;

  public isEdit: boolean = false;
  public isCreateAndUpdate: boolean = false;
  public isCreate: boolean = false;

  public filters: FilterRequest = null;
  public dateRange: string = '';
  public groupLeadFilter: string[] = [];
  public listRequestId: string[] = [];
  //Declare Announment
  public announcement: Announcement = null;
  //Declare variable
  isDesc: boolean = true;
  column: string = DefaultColunmConstants.StartDateColunm;
  direction: number;
  isGreater0: boolean = false;
  public listcheck: any[] = [];
  private startDate: string;
  private endDate: string;
  private isSelectRequestType: boolean = false;
  private checkApproveRequest: boolean = false;
  private checkRejectRequest: boolean = false;
  private checkDelegateRequest: boolean = false;
  private isCheckAll: boolean = false;
  private listRequest: any[] = [];
  public dateOptionSingle: any = {
    locale: { format: DateTimeConstants.FORMAT_DATE_DDMMYYYY },
    alwaysShowCalendars: true,
    singleDatePicker: true,
    minDate: moment(),
  };
  private groupLead: any;
  private FullNameFilter: any[] = [];
  public startDateRequest: string;
  public endDateRequest: string;
  public requestId: string;
  public ReasonName: string;
  public ToEmailSendMail: string[] = [];
  public dataRequest: any;
  public isSuccess: boolean = true;
  public chosenDate: any = {
    start: '',
    end: '',
  };
  public chosenDateDefault: any = {
    start: moment(),
    end: moment(),
  };
  public chosenRequestDate: any = {
    start: moment(),
    end: moment().add(3, DateTimeConstants.DAY),
  };
  public chosenCreateDate: any = {
    start: '',
  };

  //#endregion
  public pickerRequest: any = {
    locale: DateTimeConstants.Locale,
    autoUpdateInput: true,
    opens: CommonConstants.RIGHT,
    minDate: moment(),
    autoUnselect: true,
    alwaysShowCalendars: true,
    ranges: DateTimeConstants.Range,
    isInvalidDate: function (date) {
      if (date.isSame(Date.toString(), DateTimeConstants.DAY))
        return 'mystyle';
      return false;
    }
  };
  //constructor inject request service
  constructor(public _authenService: AuthenService,
    private _dataService: DataService,
    private _notificationService: NotificationService,
    private daterangepickerOptions: DaterangepickerConfig,
    private _utilityService: UtilityService,
    private _passDataService: PassDataService) {
    if (_authenService.getLoggedInUser().groupId.length == 0) {
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
    this.DashBoard = this._passDataService.dashboard;
    this.userLogin = this._authenService.getLoggedInUser();
    if (this.DashBoard == undefined) {
      this.loadData();
    }
    if (this.DashBoard != undefined) {
      if (this.DashBoard.clickFromDashBoard == false) {
        this.loadData();
      }
      if (this.DashBoard.clickFromDashBoard == true) {
        this.loadPersonalData();
        this.DashBoard.clickFromDashBoard = false;
      }
    }
    this.getListForDropdown();
    this.SendMailModel = null;
    this.toEmail = [];
    this.loadFullName();
  }

  //#region "Init data"
  public loadData() {
    var roles: any[] = JSON.parse(this.userLogin.roles);
    if (roles.findIndex(x => x == "Admin") != -1 || roles.findIndex(x => x == "SuperAdmin") != -1) {
      this._dataService.post('/api/request/getallbyusersuperadmin?' + '&column=' + this.column + '&isDesc=' + this.isDesc
        + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize, JSON.stringify(this.filters))
        .subscribe((response: any) => {
          this.requestssuperadmin = response.Items;
          this.pageIndex = response.PageIndex;
          this.pageSize = response.PageSize;
          this.totalRow = response.TotalRows;
          this.loadsuccess = true;
          this.calShowingPage();
        }, error => this._dataService.handleError(error));
    }
    else {
      this._dataService.post('/api/request/getallbyuser?userId=' + this.userLogin.id + '&groupId=' + this.userLogin.groupId
        + '&column=' + this.column + '&isDesc=' + this.isDesc
        + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize, JSON.stringify(this.filters))
        .subscribe((response: any) => {
          this.requests = response.Items;
          this.pageIndex = response.PageIndex;
          this.pageSize = response.PageSize;
          this.totalRow = response.TotalRows;
          this.checkCheckBox();
          this.isGreater0 = this.getCheckCondition().length > 0 ? true : false;
          this.loadsuccess = true;
          this.calShowingPage();
        }, error => this._dataService.handleError(error));
    }
  }

  public loadPersonalData() {
    this.filters = new FilterRequest([this.userLogin.id], this.myFilterStatusRequests, this.myFilterRequestTypes, this.myFilterRequestReasonTypes, this.startDate, this.endDate)
    this.myCreator = [this.userLogin.id];
    this._dataService.post('/api/request/getallbyuser?userId=' + this.userLogin.id + '&groupId=' + this.userLogin.groupId
      + '&column=' + this.column + '&isDesc=' + this.isDesc
      + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize, JSON.stringify(this.filters))
      .subscribe((response: any) => {
        this.requests = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.loadsuccess = true;
        this.calShowingPage();
      }, error => this._dataService.handleError(error));
  }
  public getListForDropdown() {
    this._dataService.get('/api/appUser/getuserbygroup?groupID=' + this.userLogin.groupId)
      .subscribe((response: any[]) => {
        this.filterCreators = [];
        for (let creator of response) {
          this.filterCreators.push({ id: creator.Id, name: creator.FullName + " ( " + creator.UserName + " ) " });
        }
      }, error => this._dataService.handleError(error));

    this._dataService.get('/api/appUser/getuserbygroup')
      .subscribe((response: any[]) => {
        this.filterCreatorsSuperAdmin = [];
        for (let creator of response) {
          this.filterCreatorsSuperAdmin.push({ id: creator.Id, name: creator.FullName + " ( " + creator.UserName + " ) " });
        }
      }, error => this._dataService.handleError(error));

    this._dataService.get('/api/statusrequest/getall')
      .subscribe((response: any[]) => {
        this.filterStatusRequests = [];
        for (let statusRequest of response) {
          this.filterStatusRequests.push({ id: statusRequest.ID, name: statusRequest.Name });
        }
      }, error => this._dataService.handleError(error));

    this._dataService.get('/api/statusrequest/getall')
      .subscribe((response: any[]) => {
        this.filterStatusRequests = [];
        for (let statusRequest of response) {
          this.filterStatusRequests.push({ id: statusRequest.ID, name: statusRequest.Name });
        }
      }, error => this._dataService.handleError(error));

    this._dataService.get('/api/requesttype/getall')
      .subscribe((response: any[]) => {
        this.filterRequestTypes = [];
        for (let requestType of response) {
          this.filterRequestTypes.push({ id: requestType.ID, name: requestType.Name });
        }
      }, error => this._dataService.handleError(error));
    //Get All
    this._dataService.get('/api/entitleday/GetAllTypeFilter?UserID=' + this.userLogin.id)
      .subscribe((response: any[]) => {
        this.filterRequestReasonTypesEntitleDay = [];
        for (let entitledayType of response) {
          this.filterRequestReasonTypesEntitleDay.push({ id: entitledayType.ID, name: entitledayType.HolidayType });
        }
      }, error => this._dataService.handleError(error));
    //Get User Entitle day
    this._dataService.get('/api/entitleday/getalltype?UserID=' + this.userLogin.id)
      .subscribe((response: any[]) => {
        this.filterRequestReasonTypes = [];
        for (let holidayType of response) {
          this.filterRequestReasonTypes.push({ id: holidayType.ID, name: holidayType.HolidayType });
        }
      }, error => this._dataService.handleError(error));

  }

  loadRequestDetail(id: any) {
    this._dataService.get('/api/request/detail/' + id)
      .subscribe((response: any) => {
        this.entity = response;
        this.entity.FullName = response.AppUser.FullName;
        this.entity.GroupName = response.AppUser.Group.Name;
        this.chosenRequestDate.start = response.StartDate;
        this.chosenRequestDate.end = response.EndDate;
        this.chosenCreateDate.start = response.CreatedDate;
        if (this.entity.RequestType.Name.includes(CommonConstants.BREAK)) {
          this.isSelectRequestType = true;
        }
        else {
          this.isSelectRequestType = false;
        }
        if (this.userLogin.username == "admin" || this.userLogin.roles.indexOf('Admin') !== -1) {
          this.groupLeadFilter = [];
          this._dataService.get('/api/appUser/getgrouplead?groupId=' + this.entity.AppUser.GroupId)
            .subscribe((response: any) => {
              this.listGrantUser = [];
              for (let userGroup of response) {
                this.groupLeadFilter.push(userGroup.Id);
                this.listGrantUser.push({ id: userGroup.Id, name: userGroup.FullName });
              }
            }, error => this._dataService.handleError(error));
        }
        if (this.entity.UserId == this.userLogin.id) {
          this.isUser = true
        }
        else {
          this.isUser = false;
        }
        this.daterangepickerOptions.settings = {
          locale: DateTimeConstants.Locale,
          alwaysShowCalendars: true,
          "opens": "left",
          minDate: moment(),
          autoUpdateInput: true,
          startDate: moment(this.chosenRequestDate.start),
          endDate: moment(this.chosenRequestDate.end),
          ranges: DateTimeConstants.Range,
        };
        this.valueDate.start = moment(this.chosenRequestDate.start);
        this.valueDate.end = moment(this.chosenRequestDate.end);
        this.selectedAddRequestDatePicker(this.valueDate, this.chosenRequestDate);
      });
  }
  loadGroupLead() {
    this._dataService.get('/api/appUser/getgrouplead?groupId=' + this.userLogin.groupId)
      .subscribe((response: any) => {
        this.groupLead = response;
      }, error => this._dataService.handleError(error));
  }

  //#endregion  

  //#region "CRUD"
  public saveChanges(form: NgForm) {
    this.entity.CreatedDate = moment(new Date).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this.entity.StartDate = moment(new Date(this.chosenRequestDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this.entity.EndDate = moment(new Date(this.chosenRequestDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this.startDateRequest = moment(new Date(this.chosenRequestDate.start)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
    this.endDateRequest = moment(new Date(this.chosenRequestDate.end)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
    if (moment().startOf('day') > moment(new Date(this.entity.StartDate))) {
      this._notificationService.printErrorMessage(MessageConstants.CREATE_ERROR_REQUEST_CREATEDATE);
    }
    else {
      this.saveData(form);
    }
  }
  //save data
  private saveData(form: NgForm) {
    this.entity.Title = this.entity.Title.trim();
    this.listcheck = [];
    if (Array.isArray(this.entity.RequestTypeId)) {
      if (this.entity.RequestTypeId != null) {
        this.entity.RequestTypeId = this.entity.RequestTypeId[0];
      } else {
        this._notificationService.printErrorMessage(MessageConstants.CREATE_ERROR_REQUEST_TYPE);
        return;
      }
    }
    if (this.entity.RequestTypeId != null) {
      var checkBreak = this.filterRequestTypes.find(x => x.id == this.entity.RequestTypeId).name.includes(CommonConstants.BREAK);
      if (checkBreak == true) {
        this.entity.EntitleDayId = Array.isArray(this.entity.EntitleDayId) ? this.entity.EntitleDayId[0] : this.entity.EntitleDayId;
        //this.entity.EntitleDayId == null ? this._notificationService.printErrorMessage(MessageConstants.CREATE_ERROR_REQUEST_REASONTYPE) : this.entity.EntitleDayId;
        if (this.entity.EntitleDayId == null) {
          this._notificationService.printErrorMessage(MessageConstants.CREATE_ERROR_REQUEST_REASONTYPE);
          return;
        } else {
          this.entity.EntitleDayId
        }
      }
      this.entity.EntitleDayId = Array.isArray(this.entity.EntitleDayId) ? this.entity.EntitleDayId[0] : this.entity.EntitleDayId;
      this.entity.EntitleDayId == this.entity.EntitleDayId == undefined ? null : this.entity.EntitleDayId;
    }
    if (this.groupLead[0] == null) {
      this._notificationService.printErrorMessage(MessageConstants.CREATED_FAIL_NO_GROUPLEAD_MSG);
      return;
    }
    //Check Limit Time Create Moring Leave and Late Going
    var dateNow = moment();
    if (dateNow.format(DateTimeConstants.FORMAT_HOUR_HH) >= DateTimeConstants.LimitTimeCreateMorningLeave && this.entity.StartDate == dateNow.format(DateTimeConstants.FORMAT_DATE_MMDDYYYY) && (this.entity.RequestTypeId == CommonConstants.MorningLeaveStatus || this.entity.RequestTypeId == CommonConstants.LateComingStatus)) {
      this._notificationService.printErrorMessage(MessageConstants.LIMIT_TIME_CREATE_REQUEST_MONRING_MSG);
      return;
    }
    if (dateNow.format(DateTimeConstants.FORMAT_HOUR_HH) >= DateTimeConstants.LimitTimeCreateAfternoonLeave && this.entity.StartDate == dateNow.format(DateTimeConstants.FORMAT_DATE_MMDDYYYY)) {
      this._notificationService.printErrorMessage(MessageConstants.LIMIT_TIME_CREATE_REQUEST_AFTERNOON_MSG);
      return;
    }

    this.entity.toEmail = [];
    this.entity.ccMail = [];

    for (let item of this.FullNameFilter) {
      this.entity.ccMail.push(item);
    }

    if ((this.userLogin.email.localeCompare(this.groupLead[0].Email) == 0)) {
      if (this.groupLead[0] != null) {
        this.entity.toEmail.push(this.groupLead[0].Email + "," + this.groupLead[0].FullName);
      }
    } else {
      this.entity.toEmail.push(this.userLogin.email + "," + this.entity.FullName);
      if (this.groupLead[0] != null) {
        this.entity.toEmail.push(this.groupLead[0].Email + "," + this.groupLead[0].FullName);
      }
    }
    this.isDisable = true;
    this.SendMailModelCreated = new SendMailModelNew(this.entity.GroupName, this.filterRequestTypes.find(x => x.id == this.entity.RequestTypeId).name, this.entity.DetailReason, this.startDateRequest, this.endDateRequest, null, null, null, null, null, null, null, null, null, this.entity.Title, this.userLogin.id, this.entity.toEmail, this.entity.ccMail, MailConstants.Create, MailConstants.Request, MailConstants.RequestSubject + this.entity.Title, null, MailConstants.RequestManagement);

    if (this.entity.ID == undefined && this.entity.RequestTypeId != null) {
      if (!this.filterRequestTypes.find(x => x.id == this.entity.RequestTypeId).name.includes(CommonConstants.BREAK) || (this.filterRequestTypes.find(x => x.id == this.entity.RequestTypeId).name.includes(CommonConstants.BREAK) && this.entity.EntitleDayId != undefined)) {
        this._dataService.post('/api/request/add', JSON.stringify(this.entity))
          .subscribe((response: any) => {
            this.ToEmailSendMail = [];
            this.dataRequest = response;
            this.requestId = response.ID;
            this.isDisable = false;
            this.setDefaultControl();
            this._notificationService.printSuccessMessage(MessageConstants.CREATED_OK_MSG);
            this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModelCreated)).subscribe((response: any) => {
              if (this.dataRequest.CheckConfigDelegateDefault == true) {
                this.ToEmailSendMail.push(this.dataRequest.AssignConfigDelegate);
                this.SendMailModel = new SendMailDelegateRequest(this.dataRequest.GroupName, this.dataRequest.RequestType.Name, this.dataRequest.DetailReason,
                  this.dataRequest.StartDate, this.dataRequest.EndDate, 'Title', this.dataRequest.AppUser.Id, this.ToEmailSendMail, MailConstants.Delegation, MailConstants.DelegationAssignedList, MailConstants.RequestSubject, null, MailConstants.DelegationManagement, this.dataRequest.DelegateId, this.dataRequest.AssignToId);
                this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModel)).subscribe((response: any) => { });
              }
              if (this.dataRequest.CheckGroupDelegateDefault == true) {
                this.ToEmailSendMail.push(this.dataRequest.AssignGroupDelegate);
                this.SendMailModel = new SendMailDelegateRequest(this.dataRequest.GroupName, this.dataRequest.RequestType.Name, this.dataRequest.DetailReason,
                  this.dataRequest.StartDate, this.dataRequest.EndDate, 'Title', this.dataRequest.AppUser.Id, this.ToEmailSendMail, MailConstants.Delegation, MailConstants.DelegationAssignedList, MailConstants.RequestSubject, null, MailConstants.DelegationManagement, this.dataRequest.DelegateId, this.dataRequest.AssignToId);
                this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModel)).subscribe((response: any) => { });
              }

            });
            this.daterangepickerOptions.settings = {
              locale: DateTimeConstants.Locale,
              autoUpdateInput: true,
              alwaysShowCalendars: true,
              "opens": "left",
              minDate: false,
              ranges: DateTimeConstants.Range,
              startDate: moment(),
              endDate: moment(),
            };
            this.loadData();
            form.resetForm();
          }, error => {
            this.isDisable = false;
            this._dataService.handleError(error)
          });
      }
    }
    if (this.entity.ID != undefined && this.entity.RequestTypeId != null) {
      this._dataService.put('/api/request/updaterequest', JSON.stringify(this.entity))
        .subscribe((response: any) => {
          this.isDisable = false;
          this.setDefaultControl();
          this._notificationService.printSuccessMessage(MessageConstants.UPDATED_OK_MSG);
          this.daterangepickerOptions.settings = {
            locale: DateTimeConstants.Locale,
            autoUpdateInput: true,
            alwaysShowCalendars: true,
            "opens": "left",
            minDate: false,
            ranges: DateTimeConstants.Range,
            startDate: moment(),
            endDate: moment(),
          };
          this.loadData();
          form.resetForm();
        }, error => {
          this.isDisable = false;
          this._dataService.handleError(error)
        });
    }

  }
  //set default control
  private setDefaultControl() {
    this.modalAddEdit.hide();
    this.modalDelegate.hide();
    this.checkRejectRequest = false;
    this.checkApproveRequest = false;
    this.checkDelegateRequest = false;
    this.loadData();
  }
  //cancel request
  cancelRequest(id: any, title: string) {
    this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_CANCEL_REQUEST_MSG, () => this.cancelRequestConfirm(id, title));
  }
  ///confirm cancel request
  cancelRequestConfirm(id: any, title: string) {
    this._dataService.post('/api/request/changestatus?requestId=' + id + '&typeStatus=' + CommonConstants.Cancelled + '&userIdDelegate=' + null).subscribe((response: Response) => {
      this.setDefaultControl();
      this._notificationService.printSuccessMessage(MessageConstants.CANCEL_OK_MSG);
    });
  }
  //Approve Request
  approvedRequest(id: any, Title: any, AppUser: any) {
    this.entity = {};
    this.isSuccess = false;
    this.entity = this.requests.find(x => x.ID == id);
    this._dataService.post('/api/request/changeStatus?requestId=' + id + '&typeStatus=' + CommonConstants.Approved + '&userIdDelegate=' + null).
      subscribe((response: Response) => {
        this.entity.StartDate = moment(new Date(this.entity.StartDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
        this.entity.EndDate = moment(new Date(this.entity.EndDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
        this.toEmail.push(AppUser.Email + "," + this.entity.AppUser.FullName);
        this.SendMailModelCreated = new SendMailModelNew(this.entity.AppUser.Group.Name, this.entity.RequestType.Name, this.entity.DetailReason, this.entity.StartDate, this.entity.EndDate, null, null, null, null, null, null, null, null, null, Title, this.userLogin.id, this.toEmail, null, MailConstants.Approve, MailConstants.Request, MailConstants.RequestSubject, null, MailConstants.RequestManagement);
        this.setDefaultControl();
        this.listcheck = [];
        this._notificationService.printSuccessMessage(MessageConstants.APPROVED_SUCCES_REQUEST_MSG);
        this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModelCreated)).subscribe((response: any) => { });
        this.resetEmail();
        this.checkApproveRequest = false;
        this.loadData();
        this.isSuccess = true;
      }, error => {
        this.isSuccess = true;
        this._dataService.handleError(error)

      });
  }
  //Reject Request
  rejectedRequest(id: any, title: string, userId: string, AppUser: any) {
    this._notificationService.printPromptDialog("", MessageConstants.TEXTAREA_COMMENT_REQUEST_MSG, (value: string) => this.rejectedRequestConfirm(value, id, title, userId, AppUser));
  }
  rejectedRequestConfirm(value: any, id: any, title: string, userId: string, AppUser: any) {
    if (value.trim() != '') {
      this.entity = this.requests.find(x => x.ID == id);
      this._dataService.post('/api/request/changeStatus?requestId=' + id + '&typeStatus=' + CommonConstants.Rejected + '&userIdDelegate=' + null).
        subscribe((response: Response) => {
          this.entity.StartDate = moment(new Date(this.entity.StartDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
          this.entity.EndDate = moment(new Date(this.entity.EndDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
          this.toEmail.push(AppUser.Email + "," + this.entity.AppUser.FullName);
          this.SendMailModelCreated = new SendMailModelNew(this.entity.AppUser.Group.Name, this.entity.RequestType.Name, this.entity.DetailReason, this.entity.StartDate, this.entity.EndDate, null, null, null, null, null, null, null, null, null, title, this.userLogin.id, this.toEmail, null, MailConstants.Reject, MailConstants.Request, MailConstants.RequestSubject, value, MailConstants.RequestManagement);
          this.setDefaultControl();
          this.listcheck = [];
          this._notificationService.printSuccessMessage(MessageConstants.REJECTED_SUCCES_REQUEST_MSG);
          this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModelCreated)).subscribe((response: any) => { });
          this.resetEmail();
        }, error => this._dataService.handleError(error));
    } else {
      this._notificationService.printErrorMessage(MessageConstants.VALID_COMMENT_REJECT_MSG);
    }
  }
  public saveDelegate(valid: boolean) {
    this.delegateRequest();
  }
  //save data
  //Delegate Request
  delegateRequest() {
    if (this.checkDelegateRequest) {
      this.listRequestId = [];
      this.listMail = [];
      for (let item of this.listcheck) {
        this.listRequestId.push(item.ID);
        this.listMail.push(new SendMailDelegateRequest(item.AppUser.Group.Name, item.RequestType.Name, item.DetailReason,
          item.StartDate, item.EndDate, 'Title', item.AppUser.Id, this.myFilterDelegators, MailConstants.Delegation, MailConstants.DelegationAssignedList, MailConstants.RequestSubject, null, MailConstants.DelegationManagement, item.DelegateId, item.toEmail));
      }
      this._dataService.post('/api/request/changeStatusMulti?typeStatus=' + CommonConstants.Delegated + '&userIdDelegate=' + this.myFilterDelegators[0], JSON.stringify(this.listRequestId)).
        subscribe((response: Response) => {
          this.setDefaultControl();
          this.listcheck = [];
          this._notificationService.printSuccessMessage(MessageConstants.DELEGATE_SUCCES_REQUEST_MSG);
          this._dataService.post('/api/System/sendMailMultiFix', JSON.stringify(this.listMail)).subscribe((response: any) => { });
          this.resetEmail();
        }, error => this._dataService.handleError(error));
    }
    else {
      this.SendMailModel = new SendMailDelegateRequest(this.DetailRequest.AppUser.Group.Name, this.DetailRequest.RequestType.Name, this.DetailRequest.DetailReason,
        this.DetailRequest.StartDate, this.DetailRequest.EndDate, 'Title', this.DetailRequest.AppUser.Id, this.myFilterDelegators, MailConstants.Delegation, MailConstants.DelegationAssignedList, MailConstants.RequestSubject, null, MailConstants.DelegationManagement, this.DetailRequest.DelegateId, this.DetailRequest.toEmail);
      this._dataService.post('/api/request/changeStatus?requestId=' + this.listRequestId[0]['ID'] + '&typeStatus=' + CommonConstants.Delegated + '&userIdDelegate=' + this.myFilterDelegators[0]).
        subscribe((response: Response) => {
          this.setDefaultControl();
          this.listcheck = [];
          this._notificationService.printSuccessMessage(MessageConstants.DELEGATE_SUCCES_REQUEST_MSG);
          this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModel)).subscribe((response: any) => { });
          this.resetEmail();
        }, error => this._dataService.handleError(error));
    }
  }
  //Approve all Request
  approvedAllRequest() {
    this.listRequestId = [];
    this.listMail = [];
    for (let item of this.listcheck) {
      this.toEmail = [];
      item.StartDate = moment(new Date(item.StartDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
      item.EndDate = moment(new Date(item.EndDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
      this.listRequestId.push(item.ID);
      this.toEmail.push(item.AppUser.Email + "," + item.AppUser.FullName);
      this.listMail.push(new SendMailModelNew(item.AppUser.Group.Name, item.RequestType.Name, item.DetailReason, item.StartDate, item.EndDate,
        null, null, null, null, null, null, null, null, null, item.Title, this.userLogin.id, this.toEmail,
        null, MailConstants.Approve, MailConstants.Request, MailConstants.RequestSubject, null, MailConstants.RequestManagement));
    }
    this._dataService.post('/api/request/changeStatusMulti?typeStatus=' +
      CommonConstants.Approved + '&userIdDelegate=' + null, JSON.stringify(this.listRequestId)).
      subscribe((response: Response) => {
        this.setDefaultControl();
        this.listcheck = [];
        this._notificationService.printSuccessMessage(MessageConstants.APPROVED_SUCCES_REQUEST_MSG);
        this._dataService.post('/api/System/sendMailMultiFix', JSON.stringify(this.listMail)).subscribe((response: any) => { });
        this.resetEmail();
      }, error => this._dataService.handleError(error));
  }
  //Reject confirm all Request
  rejectedAll() {
    this._notificationService.printPromptDialog("", MessageConstants.TEXTAREA_COMMENT_REQUEST_MSG,
      (value: string) => {
        this.rejectAllRequest(value);
      });
  }
  //reject all request
  rejectAllRequest(value: any) {
    this.listRequestId = [];
    this.listMail = [];
    for (let item of this.listcheck) {
      this.toEmail = [];
      item.StartDate = moment(new Date(item.StartDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
      item.EndDate = moment(new Date(item.EndDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
      this.listRequestId.push(item.ID);
      this.toEmail.push(item.AppUser.Email + "," + item.AppUser.FullName);
      this.listMail.push(new SendMailModelNew(item.AppUser.Group.Name, item.RequestType.Name, item.DetailReason, item.StartDate, item.EndDate, null, null, null, null, null, null, null, null, null, item.Title, this.userLogin.id, this.toEmail, null, MailConstants.Reject, MailConstants.Request, MailConstants.RequestSubject, value, MailConstants.RequestManagement));
    }
    if (value.trim() != '') {
      this._dataService.post('/api/request/changeStatusMulti?typeStatus=' + CommonConstants.Rejected + '&userIdDelegate=' + null, JSON.stringify(this.listRequestId)).
        subscribe((response: Response) => {
          this.listcheck = [];
          this.setDefaultControl();
          this._notificationService.printSuccessMessage(MessageConstants.REJECTED_SUCCES_REQUEST_MSG);
          this._dataService.post('/api/System/sendMailMultiFix', JSON.stringify(this.listMail)).subscribe((response: any) => { });
          this.resetEmail();
        }, error => this._dataService.handleError(error));
    } else {
      this._notificationService.printErrorMessage(MessageConstants.VALID_COMMENT_REJECT_MSG);
    }
  }
  //Delegate all Request
  delegateAllRequest() {
    this.showDelegate(undefined, true);
  }
  //get infomation for popup
  getInformationForPopup() {
    if (this.userLogin.username === 'admin' || this.userLogin.roles.indexOf('Admin') !== -1) {
      return;
    }
    this.getGroupLeadByID();
  }
  //#endregion
  //#region "Poppu or other"

  // method reset email
  resetEmail() {
    this.toEmail = [];
    this.listMail = [];
  }
  // Show add modal
  hideModal(form: NgForm) {
    if (form.dirty) {
      this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_CLOSE_REQUEST_MSG, () => {
        form.reset();
        this.modalAddEdit.hide();
        this.daterangepickerOptions.settings = {
          locale: DateTimeConstants.Locale,
          autoUpdateInput: true,
          alwaysShowCalendars: true,
          minDate: false,
          "opens": "left",
          ranges: DateTimeConstants.Range,
          startDate: moment(),
          endDate: moment(),
        };
      });

    } else {
      this.modalAddEdit.hide();
    }
  }


  showAddModal() {
    this.loadGroupLead();
    this.FullNameFilter = [];
    this.isCreate = true;
    this.isCreateAndUpdate = true;
    this.chosenRequestDate.start = moment();
    this.chosenRequestDate.end = moment();
    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: true,
      "opens": "left",
      autoUpdateInput: true,
      startDate: this.chosenRequestDate.start,
      endDate: this.chosenRequestDate.end,
      minDate: moment(),
      ranges: DateTimeConstants.Range,
    };

    this.valueDate.end = moment(new Date(this.chosenRequestDate.end));
    this.valueDate.start = moment(new Date(this.chosenRequestDate.start));
    this.selectedAddRequestDatePicker(this.valueDate, this.chosenRequestDate);
    this.isSelectRequestType = false;
    this.entity = {};
    this.entity.FullName = this.userLogin.fullName;
    this.getGroupLeadByID();
    this._dataService.get('/api/group/getgroupbyid?groupId=' + this.userLogin.groupId)
      .subscribe((response: any) => {
        this.entity.GroupName = response.Name
      }, error => this._dataService.handleError(error));
    this.modalAddEdit.show();
  }
  getGroupLeadByID() {
    this.groupLeadFilter = [];
    this._dataService.get('/api/appUser/getgrouplead?groupId=' + this.userLogin.groupId)
      .subscribe((response: any) => {
        this.listGrantUser = [];
        for (let userGroup of response) {
          this.groupLeadFilter.push(userGroup.Id);
          this.listGrantUser.push({ id: userGroup.Id, name: userGroup.FullName });
        }
        this.entity.DelegateId = this.listGrantUser.length > 0 ? this.listGrantUser[0].id : null;
      }, error => this._dataService.handleError(error));
  }
  // Show edit modal

  showEditModal(id: any) {
    this.loadGroupLead();
    this.entity = {};
    this.isCreate = false;
    this.isEdit = false;
    this.isCreateAndUpdate = false;
    this.getInformationForPopup();
    this.loadRequestDetail(id);
    this.modalAddEdit.show();

    this.valueDate.end = moment(new Date(this.chosenRequestDate.end));
    this.valueDate.start = moment(new Date(this.chosenRequestDate.start));
    this.selectedAddRequestDatePicker(this.valueDate, this.chosenRequestDate);
  }
  showUpdateModal(id: any) {
    this.loadGroupLead();
    this.entity = {};
    this.isCreate = false;
    this.isEdit = true;
    this.isCreateAndUpdate = true;
    this.getInformationForPopup();
    this.loadRequestDetail(id);
    this.modalAddEdit.show();
    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: true,
      "opens": "left",
      autoUpdateInput: true,
      minDate: moment(),
      startDate: moment(this.chosenRequestDate.end),
      endDate: moment(this.chosenRequestDate.start),
      ranges: DateTimeConstants.Range,
    };
  }
  showEditRequest() {
    this.isCreateAndUpdate = true;
    this.isEdit = true;
    this.isCreate = false;
    this.isSelectRequestType = true;
  }
  showResetModal(id: any) {
    this._dataService.get('/api/request/detail/' + id)
      .subscribe((response: any) => {
        this.entity = response;
        this.entity.FullName = response.AppUser.FullName;
        this.entity.GroupName = response.AppUser.Group.Name;
        this.chosenRequestDate.start = response.StartDate;
        this.chosenRequestDate.end = response.EndDate;
        this.chosenCreateDate.start = response.CreatedDate;
        this.entity.StartDate = moment(new Date(this.chosenRequestDate.start)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
        this.entity.EndDate = moment(new Date(this.chosenRequestDate.end)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
        if (this.entity.UserId == this.userLogin.id) {
          this.isUser = true;
        }
        else {
          this.isUser = false;
        }
        this.daterangepickerOptions.settings = {
          locale: DateTimeConstants.Locale,
          minDate: moment(),
          alwaysShowCalendars: true,
          "opens": "left",
          ranges: DateTimeConstants.Range,
          autoUpdateInput: true,
          startDate: moment(this.chosenRequestDate.start),
          endDate: moment(this.chosenRequestDate.end),
        };
        this.valueDate.end = moment(new Date(this.chosenRequestDate.end));
        this.valueDate.start = moment(new Date(this.chosenRequestDate.start));
        this.selectedAddRequestDatePicker(this.valueDate, this.chosenRequestDate);
      });
  }
  //Show delegate
  showDelegate(item: any, isMulti: boolean) {
    this.DetailRequest = item;
    this.listRequestId = [];
    if (!isMulti) {
      this.listRequestId.push(item);
    }
    var groupLead: '';
    this._dataService.get('/api/appUser/getgrouplead?groupId=' + this.userLogin.groupId)
      .subscribe((response: any) => {
        this.listGrantUser = [];
        for (let userGroup of response) {
          this.groupLeadFilter.push(userGroup.Id);
          this.listGrantUser.push({ id: userGroup.Id, name: userGroup.FullName });
        }
        groupLead = this.listGrantUser.length > 0 ? this.listGrantUser[0].id : null;
      }, error => this._dataService.handleError(error));
    this._dataService.get('/api/appUser/getuserbygroup')
      .subscribe((response: any) => {
        this.filterDelegator = [];
        this.myFilterDelegators = [];
        for (let userGroup of response) {
          if (this.userLogin.id != userGroup.Id) {
            this.filterDelegator.push({ id: userGroup.Id, name: userGroup.FullName + " ( " + userGroup.UserName + " ) " });
          }
        }
        var index = this.filterDelegator.findIndex(x => x.id == groupLead);
        this.filterDelegator.splice(index, 1);
      }, error => this._dataService.handleError(error));
    this.modalDelegate.show();
  }
  //reset data in form create
  public requestReset(NgForm) {

    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: true,
      minDate: moment(),
      "opens": "left",
      ranges: DateTimeConstants.Range,
      autoUpdateInput: true,
    };
    this.valueDate.start = moment();
    this.valueDate.end = moment();
    this.selectedAddRequestDatePicker(this.valueDate, this.chosenRequestDate);
    this.showAddModal();
  }
  //close want to close
  public closeForm(modal: ModalDirective) {

    modal.hide();
  }
  //event change data
  onChange(event: any) {
    var selected = this.filterRequestTypes.find(function (element) {
      return element.id == event;
    });
    if (selected != undefined) {
      if (selected.name.includes(CommonConstants.BREAK)) {
        this.isSelectRequestType = true;
        this.entity.EntitleDayId = this.filterRequestReasonTypes.find(x => x.name == CommonConstants.AuthorizedLeave).id;
      }
      else {
        this.isSelectRequestType = false;
        this.entity.EntitleDayId = [];
      }
    }
    else {
      this.isSelectRequestType = false;
      this.entity.EntitleDayId = [];
    }
  }
  //#endregion

  //#region "Business logic"
  // check permission of user when load list request
  public checkPermisson(statusRequest: string, userId): boolean {
    return (userId == this.userLogin.id
      && (statusRequest == CommonConstants.Pending || statusRequest == CommonConstants.Delegated))
  }
  // check User login can Cancel request
  public checkCancel(entity: any): boolean {
    var currentDate = moment();
    var requestStartDate = moment(new Date(entity.StartDate)).add(1, DateTimeConstants.DAY);
    return ((entity.UserId == this.userLogin.id)
      && (entity.StatusRequest.Name == CommonConstants.Pending || entity.StatusRequest.Name == CommonConstants.Delegated)
      && requestStartDate >= currentDate
    )
  }
  // Check User login can Approve request
  public checkApprove(entity: any): boolean {
    var currentDate = moment();
    var requestStartDate = moment(new Date(entity.StartDate)).add(1, DateTimeConstants.DAY);
    if (entity.StatusRequest.Name == CommonConstants.Delegated) {
      return ((entity.AssignToId == this.userLogin.id)
        && requestStartDate.diff(currentDate) > 0)
    }
    else {
      return ((entity.StatusRequest.Name == CommonConstants.Pending || entity.StatusRequest.Name == CommonConstants.Rejected)
        && requestStartDate.diff(currentDate) > 0)
    }
  }
  //Check User login can Reject request
  public checkReject(entity: any): boolean {
    var currentDate = moment();
    var requestStartDate = moment(new Date(entity.StartDate)).add(1, DateTimeConstants.DAY);
    if (entity.StatusRequest.Name == CommonConstants.Delegated) {
      return ((entity.AssignToId == this.userLogin.id)
        && requestStartDate.diff(currentDate) > 0)
    }
    else {
      return ((entity.StatusRequest.Name == CommonConstants.Pending || entity.StatusRequest.Name == CommonConstants.Approved)
        && requestStartDate.diff(currentDate) > 0)
    }
  }
  //Check User login can Delegate request
  public checkDelegate(entity: any): boolean {
    var currentDate = moment();
    var requestStartDate = moment(new Date(entity.StartDate)).add(1, DateTimeConstants.DAY);
    return ((entity.StatusRequest.Name == CommonConstants.Pending) && requestStartDate.diff(currentDate) > 0)
  }
  //check show Approver
  public checkShowAprroved(entity: any): boolean {
    return ((entity.StatusRequest.Name == CommonConstants.Approved || entity.StatusRequest.Name == CommonConstants.Rejected) && entity.ChangeStatusById != null)
  }
  //Change on click textbox
  checkBoxList(event: any, item: any) {
    if (event.target.checked) {
      if (!this.IsArrayIncludeItem(this.listcheck, item)) {
        this.listcheck.push(item);
      }
    } else {
      if (this.IsArrayIncludeItem(this.listcheck, item)) {
        this.removeItemArray(item, this.listcheck);
      }
    }
    this.checkApprovedAll();
    this.checkRejectedAll();
    this.checkDelegateAll();
  }
  //check approve all request
  checkApprovedAll() {

    if (this.listcheck != null) {
      var list = [];
      var list = this.listcheck.filter(function (data) {
        var currentDate = moment();
        var startDate = moment(new Date(data.StartDate)).add(1, DateTimeConstants.DAY);
        if ((data.StatusRequest.Name == CommonConstants.Pending || data.StatusRequest.Name == CommonConstants.Rejected)
          && startDate.diff(currentDate) > 0) {
          return data;
        }
      });
      if (list.length == this.listcheck.length) {
        this.checkApproveRequest = true;
      }
      if (list.length != this.listcheck.length) {
        this.checkApproveRequest = false;
      }
      if (this.listcheck.length == 0) {
        this.checkApproveRequest = false;
      }
    }
  }
  //check reject all request
  checkRejectedAll() {
    if (this.listcheck != null) {
      var list = [];
      var list = this.listcheck.filter(function (data) {
        var currentDate = moment();
        var startDate = moment(new Date(data.StartDate)).add(1, DateTimeConstants.DAY);
        if ((data.StatusRequest.Name == CommonConstants.Pending || data.StatusRequest.Name == CommonConstants.Approved) && startDate.diff(currentDate) > 0) {
          return data;
        }
      });
      if (list.length == this.listcheck.length) {
        this.checkRejectRequest = true;
      }
      if (list.length != this.listcheck.length) {
        this.checkRejectRequest = false;
      }
      if (this.listcheck.length == 0) {
        this.checkRejectRequest = false;
      }
    }
  }
  //check delegate all request
  checkDelegateAll() {
    if (this.listcheck != null) {
      var list = [];
      var list = this.listcheck.filter(function (data) {
        var currentDate = moment();
        var requestStartDate = moment(new Date(data.StartDate)).add(1, DateTimeConstants.DAY);
        if ((data.StatusRequest.Name == CommonConstants.Pending) && requestStartDate.diff(currentDate) > 0) {
          return data;
        }
      });
      if (list.length == this.listcheck.length) {
        this.checkDelegateRequest = true;
      }
      if (list.length != this.listcheck.length) {
        this.checkDelegateRequest = false;
      }
      if (this.listcheck.length == 0) {
        this.checkDelegateRequest = false;
      }
    }
  }
  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.loadsuccess = false;
    this.loadData();
    this.calShowingPage();
    this.checkApprovedAll();
    this.checkRejectedAll();
    this.checkDelegateAll();
  }
  calShowingPage() {
    this.currentMaxEntries = this.pageIndex * this.pageSize;
    if (this.currentMaxEntries >= this.totalRow) {
      this.currentMaxEntries = this.totalRow;
    }
  }
  checkDisabledControlDetail(): boolean {
    return (this.entity.ID != null);
  }
  //sort column
  sort(property) {
    this.pageIndex = 1;
    this.isDesc = !this.isDesc; //change the direction    
    this.column = property;
    let direction = this.isDesc ? 1 : -1;
    this.loadData();
  };
  //reset
  public reset() {
    this.pageIndex = 1;
    this.myCreator = [];
    this.myFilterStatusRequests = [];
    this.myFilterRequestTypes = [];
    this.myFilterRequestReasonTypes = [];
    this.listcheck = [];
    this.checkApproveRequest = false;
    this.checkRejectRequest = false;
    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      ranges: DateTimeConstants.Range,
      autoUpdateInput: true,
      alwaysShowCalendars: true,
      "opens": "left",
    };
    this.value.start = moment();
    this.value.end = moment();
    this.selectedDateRangePicker(this.value, this.chosenDate);
    this.chosenDate.start = '';
    this.chosenDate.end = '';
    this.filters = null;
    this.loadData();
  }

  // filter data
  public filter() {
    this.pageIndex = 1;
    if (this.chosenDate.start != '' && this.chosenDate.end != '') {
      this.startDate = moment(new Date(this.chosenDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      this.endDate = moment(new Date(this.chosenDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    } else {
      this.startDate = null;
      this.endDate = null;
    }
    this.listcheck = [];
    this.checkApproveRequest = false;
    this.checkRejectRequest = false;
    this.filters = new FilterRequest(this.myCreator, this.myFilterStatusRequests, this.myFilterRequestTypes, this.myFilterRequestReasonTypes, this.startDate, this.endDate)
    this.loadData();
  }
  getCheckCondition(): any {
    if (this.requests != null) {
      var list = this.requests.filter(function (data) {
        var currentDate = moment();
        var startDate = moment(new Date(data.StartDate)).add(DateTimeConstants.LimitTimeApproveRejectRequest, DateTimeConstants.HOURS);
        if (data.StatusRequest.Name != CommonConstants.Cancelled && data.StatusRequest.Name != CommonConstants.Delegated) {
          if (startDate.diff(currentDate) > 0) {
            return data;
          }
        }
      });
      return list;
    }
  }
  //check checkAll checked or not
  checkAllchecked() {
    var result = this.getCheckCondition();
    return this.getCheckCondition().length == 0;
  }

  //change checkAll on click
  public checkAll(ev: any) {
    this.getCheckCondition().forEach(x => x.Checked = ev.target.checked);
    if (ev.target.checked) {
      for (let item of this.getCheckCondition()) {
        if (!this.IsArrayIncludeItem(this.listcheck, item)) {
          this.listcheck.push(item);
        }
      }
    } else {
      for (let item of this.getCheckCondition()) {
        if (this.IsArrayIncludeItem(this.listcheck, item)) {
          this.listcheck = this.removeItemArray(item, this.listcheck);
        }
      }
    }
    this.checkApprovedAll();
    this.checkRejectedAll();
    this.checkDelegateAll();
  }

  //condition of check all
  isAllChecked() {
    return this.getCheckCondition().every(_ => _.Checked);
  }
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
  // tslint:disable-next-line:member-ordering


  public selectedAddRequestDatePicker(value: any, dateInput: any) {
    this.valueDate = value;
    dateInput.start = value.start.format();
    dateInput.end = value.end.format();
  }
  public selectedDateRangePicker(value: any, dateInput: any) {
    this.value = value;
    dateInput.start = value.start.format();
    dateInput.end = value.end;
  }
  public toggleDirection(direction: string) {
    this.picker.opens = direction;
  }
  public updateSettings() {
    this.daterangepickerOptions.settings.locale = { format: DateTimeConstants.FORMAT_DATE_DDMMYYYY };
  }
  clearText(event: any, dropdownSearch: any) {
    dropdownSearch.clearSearch(event);
  };
  //remove item in array
  removeItemArray(item: any, array: any): any {
    //var index = array.indexOf(item);
    var index = this.findWithAttr(array, "ID", item.ID);
    if (index > -1) {
      array.splice(index, 1);
    }
    return array;
  }
  findWithAttr(array, attr, value) {
    for (var i = 0; i < array.length; i += 1) {
      if (array[i][attr] === value) {
        return i;
      }
    }
    return -1;
  }
  checkCheckBox() {
    for (let item of this.requests) {
      for (let itemDelete of this.listcheck) {
        if (item.ID === itemDelete.ID)
          item.Checked = true;
      }
    }
  }
  IsArrayIncludeItem(array: any, item: any): boolean {
    for (let value of array) {
      if (value.ID === item.ID) {
        return true;
      }
    }
    return false;
  }
  //#endregion

  loadFullName() {
    this._dataService.get('/api/appUser/getallappuser?userlogin=' + this.userLogin.id + '&groupId=' + this.userLogin.groupId)
      .subscribe((response: any[]) => {
        this.FullNameOption = [];
        for (let user of response) {
          this.FullNameOption.push({ id: user.Email, name: user.FullName + " ( " + user.UserName + " ) " });
        }
      }, error => this._dataService.handleError(error));
  }

  public checkEditPendding(entity: any): boolean {
    let j = (moment().startOf('day'));
    let i = moment(new Date(entity.CreatedDate)).startOf('day').add(2, DateTimeConstants.DAY)
    if ((entity.StatusRequest.Name == CommonConstants.Approved || entity.StatusRequest.Name == CommonConstants.Rejected || entity.StatusRequest.Name == CommonConstants.Cancelled) || j > i) {
      return true;
    }
    else {
      return false;
    }
  }
}
