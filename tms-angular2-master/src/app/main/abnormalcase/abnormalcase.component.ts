import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { MailConstants } from 'app/core/common/mail.constants';
import { PageConstants } from 'app/core/common/page.constans';
import { SystemConstants } from 'app/core/common/system.constants';
import { ExplanationRequest } from 'app/core/models/explanationRequestModel';
import { FilterAbnormal } from 'app/core/models/filterAbnormal';
import { SendMailModel } from 'app/core/models/sendMailModel';
import { AuthenService } from 'app/core/services/authen.service';
import { DataService } from 'app/core/services/data.service';
import { ModalDirective } from 'ngx-bootstrap';
import { CommonConstants } from '../../core/common/common.constants';
import { DateTimeConstants } from '../../core/common/datetime.constants';
import { DefaultColunmConstants } from '../../core/common/defaultColunm.constant';
import { MessageConstants } from '../../core/common/message.constants';
import { Hero } from '../../core/models/dashboardModel';
import { SendMailDelegateModel } from '../../core/models/SendMailDelegateModel';
import { SendMailModelNew } from '../../core/models/SendMailModelNew';
import { DaterangepickerConfig } from '../../core/services/config.service';
import { NotificationService } from '../../core/services/notification.service';
import { PassDataService } from '../../core/services/passData.service';
import { UtilityService } from '../../core/services/utility.service';
@Component({
  selector: 'app-abnormalcase',
  templateUrl: './abnormalcase.component.html',
  styleUrls: ['./abnormalcase.component.css']
})
//#region "Decalre variable, innit some value and so on"
export class AbnormalcaseComponent implements OnInit {
  @ViewChild('addEditModal') public addEditModal: ModalDirective;
  @ViewChild('addEditForm') public addEditForm: NgForm;
  currentMaxEntries: number;
  isDesc: boolean = true;
  column: string = DefaultColunmConstants.UserNameColunm;
  public abnormalcase: any[];
  public user: any;
  public value: any = {
    end: '',
    start: ''
  };
  public userIdLogin: string;
  public totalRow: number;
  public pageIndex: number = PageConstants.pageIndex;
  public pageSize: number = PageConstants.pageSize;
  public pagingSize: number = PageConstants.pagingSize;
  public dateOptions: any = {
    locale: { format: 'DD/MM/YYYY' },
    alwaysShowCalendars: false,
    singleDatePicker: true
  };
  public otRequest: any;
  public AbnormalFilter: string[] = [];
  public AbnormalFilterName: string[] = [];
  public StatusFilter: string[] = [];
  public AbnormalReasonTypeFilter: string[] = [];
  public FullNameFilter: string[] = [];
  public fromDate: Date;
  public toDate: Date;
  public settings;
  public settings3;
  public setting1;
  public listFilter: FilterAbnormal = null;
  public listFilterPersonal: FilterAbnormal = null;
  private startDate: string;
  private endDate: string;
  public Exportfilter: any
  SendMailModel: SendMailModel = null;
  SendMailModelCreated: SendMailModelNew = null;
  groupLead: any;
  public toEmail: string[];
  public timeSheetID: any = null;
  public OTCheckIn: string = '';
  public OTCheckOut: string = '';
  public groupLeadFilterOptions: IMultiSelectOption[] = [];
  public chosenGroupLead: string[] = [];
  public entity: any;
  public isNotCheck: boolean = false;
  public isNotCheckIn: boolean = false;
  public isNotCheckOut: boolean = false;
  loadsuccess: boolean = false;
  public explanationTitle: string;
  public explanationDescription: string;
  public receiver: string;
  public explanationObject: ExplanationRequest;
  public isUnauthorizedLeave = false;
  public AbnormalDate: string;
  public ToEmailSendMail: string[] = [];
  public addsuccess: boolean = true;
  public checkDelegate : any;
  DashBoard: Hero;
  settingReceiver: IMultiSelectSettings = {
    checkedStyle: 'glyphicon',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 1,
    autoUnselect: true,
    closeOnSelect: true,
    selectionLimit: 1,
    maxHeight: '200px',
  };
  mySettings: IMultiSelectSettings = {
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
  statusDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectStatusType,
    checked: CommonConstants.CheckStatusType,
    checkedPlural: CommonConstants.CheckStatusType,
    defaultTitle: CommonConstants.StatusRequestTitle,
    // checkedPlural: CommonConstants.checked,
  };
  reasonDropdownTexts: IMultiSelectTexts = {
    // checkAll: CommonConstants.checkAll,
    // uncheckAll: CommonConstants.uncheckAll,
    defaultTitle: CommonConstants.ReasonRequestTitle,
    allSelected: CommonConstants.AllSelectReasonType,
    checked: CommonConstants.CheckReasonType,
    checkedPlural: CommonConstants.CheckReasonType,
    // allSelected: CommonConstants.allSelected,
    // checked: CommonConstants.checked,
    // checkedPlural: CommonConstants.checked,
  }
  abnormalDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectAbsentType,
    checked: CommonConstants.CheckAbsentType,
    checkedPlural: CommonConstants.CheckAbsentType,
    defaultTitle: CommonConstants.AbnormalTimeSheetType,
    // allSelected: CommonConstants.allSelected,
    // checked: CommonConstants.checked,
    // checkedPlural: CommonConstants.checked,
  }
  fullNameDropdowTexts: IMultiSelectTexts = {
    defaultTitle: CommonConstants.FullName,
  }
  public datePickerOptions: any = {
    locale: DateTimeConstants.Locale,
    alwaysShowCalendars: false,
    singleDatePicker: true,
    minDate: moment(),
  };
  public statusFilterOptions: IMultiSelectOption[] = [];
  public abnormalReasonFilterOptions: IMultiSelectOption[] = [];
  public abnormalReasonTypeFilterOptions: IMultiSelectOption[] = [];
  public FullNameOption: IMultiSelectOption[] = [];
  // public FullNameOptionSuperAdmin: IMultiSelectOption[] = [];

  //#endregion

  //#region  constructor
  constructor(public _authenService: AuthenService, private _dataService: DataService,
    private daterangepickerOptions: DaterangepickerConfig,
    private _notificationService: NotificationService,
    private _utilityService: UtilityService,
    private _passDataService: PassDataService) {
    if (_authenService.getLoggedInUser().groupId.length == 0) {
      _utilityService.navigateToMain();
    }
    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: true,
      "opens": "left",
      ranges: DateTimeConstants.Range,
    };
    //#endregion
  }
  //#region  oninnit
  ngOnInit() {
    this.DashBoard = this._passDataService.dashboard;
    this.userIdLogin = this._authenService.getLoggedInUser().id;
    this.user = this._authenService.getLoggedInUser();
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
    // this.loadFullNameSuperAdmin();

    this.loadAbnormalReason();
    this.loadAbnormalReasonType();
    this.loadStatus();
    this.loadFullName();
    this.getGroupLeadList();
    this.settings = {
      bigBanner: true,
      timePicker: false,
      format: 'dd-MM-yyyy',
      defaultOpen: false
    }
    this.setting1 = {
      bigBanner: true,
      timePicker: false,
      format: 'dd-MM-yyyy',
      defaultOpen: false
    }
    //#endregion
  }
  resetForm(form: NgForm) {
    form.resetForm();
  }

  // //#region load data
  public loadData() {
    this._dataService.post('/api/abnormalcase/getallabnormalcasebyuser?userID=' + this.userIdLogin
      + '&groupId=' + this._authenService.getLoggedInUser().groupId
      + '&column=' + this.column + '&isDesc=' + this.isDesc
      + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize, JSON.stringify(this.listFilter))
      .subscribe((response: any) => {
        this.abnormalcase = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.pageIndex = response.PageIndex;
        this.loadsuccess = true;
        this.callShowingPage();
      }, error => this._dataService.handleError(error));
  }
  public loadPersonalData() {
    this.listFilter = new FilterAbnormal([this.user.username], this.StatusFilter, this.AbnormalFilter, this.AbnormalFilterName, moment().add(-2, DateTimeConstants.DAY).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY), null);
    this._dataService.post('/api/abnormalcase/getallabnormalcasebyuser?userID=' + this.userIdLogin
      + '&groupId=' + this._authenService.getLoggedInUser().groupId
      + '&column=' + this.column + '&isDesc=' + this.isDesc
      + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize, JSON.stringify(this.listFilter))
      .subscribe((response: any) => {
        this.abnormalcase = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.pageIndex = response.PageIndex;
        this.loadsuccess = true;
        this.callShowingPage();
      }, error => this._dataService.handleError(error));
  }

  public loadOTRequestByTimeSheet(timesheetID: any) {
    this._dataService.get('/api/otrequest/getOTRequestByTimeSheet?timesheetID=' + timesheetID + '&userId=' + this.userIdLogin)
      .subscribe((response: any) => {
        this.otRequest = response;
        this.OTCheckIn = response.StartTime;
        this.OTCheckOut = response.EndTime;
      }, error => this._dataService.handleError(error));
  }


  public saveExplanation(form: NgForm, timesheetId: number) {
    if (this.chosenGroupLead[0] == null) {
      this._notificationService.printErrorMessage(MessageConstants.CREATED_FAIL_NO_GROUPLEAD_MSG);
      this.entity = [];
      return;
    }
    if (this.OTCheckIn == undefined) {
      this.OTCheckIn = null;
    }
    if (this.OTCheckOut == undefined) {
      this.OTCheckOut = null;
    }
    this.explanationObject.TimeSheetId = timesheetId;
    this.explanationObject.CreatedBy = this.user.id;
    this.explanationObject.ReceiverId = this.chosenGroupLead[0];
    this.explanationObject.StatusRequestId = CommonConstants.PendingCode;
    this.explanationObject.GroupId = this.user.groupId;
    this.entity.toEmail = [];
    if ((this.user.email.localeCompare(this.groupLead[0].Email)) == 0) {
      this.entity.toEmail.push(this.groupLead[0].Email + "," + this.groupLead[0].FullName);
    } else {
      this.entity.toEmail.push(this.user.email + "," + this.user.fullName);
      this.entity.toEmail.push(this.groupLead[0].Email + "," + this.groupLead[0].FullName);
    }
    this.entity.AbnormalDate = this.entity.StartDate = moment(new Date(this.entity.AbnormalDate)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this.AbnormalDate = this.entity.StartDate = moment(new Date(this.entity.AbnormalDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
    this.SendMailModelCreated = new SendMailModelNew(this.entity.GroupName, null, this.entity.DetailReason, this.entity.StartDate, this.entity.EndDate, this.entity.OTDate, null, null, this.entity.StartTime, this.entity.EndTime, this.AbnormalDate, this.entity.ReasonList, this.explanationObject.Actual, this.explanationObject.ReasonDetail, this.explanationObject.Title, this.user.id, this.entity.toEmail, null, MailConstants.Create, MailConstants.ExplanationRequest, MailConstants.ExplanationRequestSubject + this.explanationObject.Title, null, MailConstants.RequestManagement);
    this.explanationObject.DateExplanation = moment(new Date(this.entity.AbnormalDate)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this.explanationObject.ReasonList = this.entity.ReasonList;


    // this.SendMailModel = new SendMailModel(this.explanationObject.Title, this.userIdLogin, this.entity.toEmail, MailConstants.Create, MailConstants.ExplanationRequest, MailConstants.ExplanationRequestSubject, "", MailConstants.RequestManagement);
    this.addsuccess = false;
    this._dataService.post('/api/explanation/add?OTCheckIn=' + this.OTCheckIn + '&OTCheckOut=' + this.OTCheckOut, JSON.stringify(this.explanationObject)).
      subscribe((response: Response) => {
        this.checkDelegate = response;
        this.addsuccess = true;
        this.addEditModal.hide();
        this._notificationService.printSuccessMessage(MessageConstants.ADD_SUCCESS_EXPLANATION_MSG);
        this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModelCreated)).subscribe((response: any) => {
          if (this.checkDelegate.CheckConfigDelegateDefault == true) {
            this.ToEmailSendMail = [];
            this.ToEmailSendMail.push(response.DelegateId);
            this.SendMailModel = new SendMailDelegateModel(this.explanationObject.Title, this.entity.FullName, this.ToEmailSendMail, MailConstants.Delegation, MailConstants.DelegationAssignedList, MailConstants.DelegationExplanationRequest, null, MailConstants.DelegationManagement,
              this.entity.GroupName, this.explanationObject.DateExplanation, this.explanationObject.ReasonList, this.explanationObject.ReasonDetail, this.explanationObject.CreatedBy, this.explanationObject.ReceiverId);
            this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModel)).subscribe((response: any) => { });
          }
          if (this.checkDelegate.CheckGroupDelegateDefault == true) {
            this._dataService.get('/api/group/getgroupbyid?groupId=' + this.explanationObject.GroupId)
              .subscribe((response: any) => {
                this.ToEmailSendMail = [];
                this.ToEmailSendMail.push(response.DelegateId);
                this.SendMailModel = new SendMailDelegateModel(this.explanationObject.Title, this.entity.FullName, this.ToEmailSendMail, MailConstants.Delegation, MailConstants.DelegationAssignedList, MailConstants.DelegationExplanationRequest, null, MailConstants.DelegationManagement,
                  this.entity.GroupName, this.explanationObject.DateExplanation, this.explanationObject.ReasonList, this.explanationObject.ReasonDetail, this.explanationObject.CreatedBy, this.explanationObject.ReceiverId);
                this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModel)).subscribe((response: any) => { });

              }, error => this._dataService.handleError(error));
          }
        });
        this.entity.toEmail = [];
        this.loadData();
        form.resetForm();
      }, error => {
        this.addsuccess = true;
        this._dataService.handleError(error)
      });
  }

  public closePopup(form: NgForm) {
    if (form.dirty) {
      this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_CLOSE_EXPLANATION_MSG, () => {
        this.addEditModal.hide();
        form.reset();
      })
    } else {
      this.addEditModal.hide();
    }

  }

  // get explanation detail
  public showCreateExplanationModal(id: number) {
    this.OTCheckIn = null;
    this.OTCheckOut = null;
    this.getAbnormalById(id);
    this.addEditModal.show();
    this.explanationObject = new ExplanationRequest("", 0, "", "", "", 0, "", "", "");
  }

  private getAbnormalById(id: number) {
    this.timeSheetID = null;
    this._dataService.get('/api/abnormalcase/getabnormalbyid/' + id).subscribe((response: any) => {
      this.entity = response;
      if (this.entity.ReasonList.includes('Unauthorized Leave')) {
        this.isUnauthorizedLeave = true;
      } else {
        this.isUnauthorizedLeave = false;
      }
      this.loadOTRequestByTimeSheet(response.TimeSheetId)
    }, error => this._dataService.handleError(error));
  }

  public getGroupLeadList() {
    this._dataService.get('/api/appUser/getgrouplead?groupId=' + this.user.groupId)
      .subscribe((response: any) => {
        this.groupLead = response;
        this.groupLeadFilterOptions = [];
        this.chosenGroupLead = [];
        for (let userGroup of response) {
          this.chosenGroupLead.push(userGroup.Id);
          this.groupLeadFilterOptions.push({ id: userGroup.Id, name: userGroup.FullName });
        }
      }, error => this._dataService.handleError(error));
  }

  public canCreateExplanation(username: string, abnormalDate: any, status: string): boolean {
    if (this.user.username != username || status != null) {
      return false;
    }
    var dateNow = (new Date(abnormalDate)).getDay();
    let diffInMs: number = (Date.parse(moment().format(DateTimeConstants.FORMAT_DATE_MMDDYYYY)) - Date.parse(abnormalDate));
    if (dateNow == 5 || dateNow == 4) {
      if ((diffInMs / 1000 / 60 / 60) > CommonConstants.DATE_RANGE_CHANGE_STATUS_EXPLANATIONS) {
        return false;
      }
      return true;
    }
    else {
      if ((diffInMs / 1000 / 60 / 60) > CommonConstants.DATE_RANGE_CHANGE_STATUS_EXPLANATION) {
        return false;
      }
      return true;
    }
  }
  public checkCaseOTNotCheckIn(entity: any) {
    for (let item of entity) {
      if (item.ID == CommonConstants.OTNotCheckIn) {
        return true;
      }
      if (item.ID == CommonConstants.OTNotCheck) {
        return true;
      }
    }
    return false;
  }
  public checkCaseOTNotCheckOut(entity: any) {
    for (let item of entity) {
      if (item.ID == CommonConstants.OTNotCheckOut) {
        return true;
      }
      if (item.ID == CommonConstants.OTNotCheck) {
        return true;
      }
    }
    return false;
  }
  pageChanged(event: any): void {
    this.loadsuccess = false;
    this.pageIndex = event.page;
    this.loadData();
  }
  callShowingPage() {
    this.currentMaxEntries = this.pageIndex * this.pageSize;
    if (this.currentMaxEntries >= this.totalRow) {
      this.currentMaxEntries = this.totalRow;
    }
  }
  public picker = {
    opens: CommonConstants.LEFT,
    startDate: moment().subtract(7, DateTimeConstants.DAY),
    endDate: moment(),
    isInvalidDate: function (date) {
      if (date.isSame(Date.toString(), DateTimeConstants.DAY))
        return 'mystyle';
      return false;
    }
  }
  public chosenDate: any = {
    start: '',
    end: '',
  };
  public eventLog = '';
  public selectedDateRangePicker(value: any, dateInput: any) {
    this.value = value;
    dateInput.start = value.start.format();
    dateInput.end = value.end;
  }
  public calendarEventsHandler(e: any) {
    this.eventLog += '\nEvent Fired: ' + e.event.type;
  }
  public toggleDirection(direction: string) {
    this.picker.opens = direction;
  }
  //Load status
  public loadStatus() {
    this._dataService.get('/api/statusrequest/getall')
      .subscribe((response: any) => {
        this.statusFilterOptions = [];
        for (let status of response) {
          this.statusFilterOptions.push({ id: status.Name, name: status.Name });
        }
      }, error => this._dataService.handleError(error));
  }
  // Load abnormalreason
  public loadAbnormalReason() {
    this._dataService.get('/api/abnormalreason/getallabnormalreason')
      .subscribe((response: any) => {
        this.abnormalReasonFilterOptions = [];
        for (let abnormal of response) {
          this.abnormalReasonFilterOptions.push({ id: abnormal.Name, name: abnormal.Name });
        }
      }, error => this._dataService.handleError(error));
  }
  //Load abnormalreasontype
  public loadAbnormalReasonType() {
    this._dataService.get('/api/abnormal-timesheet-type/getall')
      .subscribe((response: any) => {
        this.abnormalReasonTypeFilterOptions = [];
        for (let abnormal of response) {
          if (((abnormal.Name != "DiMuon") && (abnormal.Name != "VeSom"))) {
            this.abnormalReasonTypeFilterOptions.push({ id: abnormal.Name, name: abnormal.Description });
          }
        }
      }, error => this._dataService.handleError(error));
  }
  //Load Full Name
  loadFullName() {
    this._dataService.get('/api/appUser/getuserbygroup')
      .subscribe((response: any[]) => {
        this.FullNameOption = [];
        for (let user of response) {
          this.FullNameOption.push({ id: user.UserName, name: user.FullName + " ( " + user.UserName + " ) " });
        }
      }, error => this._dataService.handleError(error));
  }
  //#endregion
  //#region  fillter
  filterAbnormal() {
    if (this.chosenDate.start != '' && this.chosenDate.end != '') {
      this.startDate = moment(new Date(this.chosenDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      this.endDate = moment(new Date(this.chosenDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    } else {
      this.startDate = null;
      this.endDate = null;
    }
    this.AbnormalFilterName = [];
    for (let abnormalReasonTypeFilter of this.AbnormalReasonTypeFilter) {
      this.AbnormalFilterName.push(this.abnormalReasonTypeFilterOptions.find(x => x.id == abnormalReasonTypeFilter).id);
    }
    this.pageIndex = 1;
    this.listFilter = new FilterAbnormal(this.FullNameFilter, this.StatusFilter, this.AbnormalFilter, this.AbnormalFilterName, this.startDate, this.endDate);
    this.loadData();

  }
  //#endregion
  reset() {
    this.AbnormalFilter = [];
    this.StatusFilter = [];
    this.AbnormalReasonTypeFilter = [];
    this.FullNameFilter = [];
    // this.chosenDate.start = '';
    // this.chosenDate.end = '';
    this.daterangepickerOptions.settings = {
      ranges: DateTimeConstants.Range,
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: true,
      "opens": "left",
      autoUpdateInput: true,
    };
    this.value.start = moment();
    this.value.end = moment();
    this.selectedDateRangePicker(this.value, this.chosenDate);
    this.listFilter = null;
    this.pageIndex = 1;
    this.chosenDate.start = '';
    this.chosenDate.end = '';
    this.loadData();
  }
  //get list after sort
  public sort(property) {
    this.isDesc = !this.isDesc; // change the direction
    this.column = property;
    this.pageIndex = 1;
    this.loadData();
  }
  // check permission of user when load explanation list
  public hasPermission(functionId: string, action: string): boolean {
    return this._authenService.hasPermission(functionId, action);
  }
  clearText(event: any, dropdownSearch: any) {
    dropdownSearch.clearSearch(event);
  };
  Export() {
    this._dataService.post('/api/abnormalcase/exportexcel?userID=' + this.userIdLogin
      + '&groupId=' + this._authenService.getLoggedInUser().groupId
      + '&column=' + this.column + '&isDesc=' + this.isDesc
      + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize, JSON.stringify(this.listFilter))
      .subscribe((response: any[]) => {
        
        if (response != null) {
          window.location.href = SystemConstants.BASE_API + response;
          this.loadData();
          // this._notificationService.printSuccessMessage(MessageConstants.DOWNLOAD_OK_MSG);
        } else {
          this._notificationService.printSuccessMessage(MessageConstants.DOWNLOAD_OK_ERROR);
        }
      }, error => this._dataService.handleError(error));
  }
}
