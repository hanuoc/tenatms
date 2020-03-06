import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthenService } from 'app/core/services/authen.service';
import { DataService } from 'app/core/services/data.service';
import { ModalDirective } from 'ngx-bootstrap';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { DaterangepickerConfig } from '../../core/services/config.service';
import { DateTimeConstants } from '../../core/common/datetime.constants';
import { CommonConstants } from '../../core/common/common.constants';
import { ExplanationFilter } from 'app/core/models/explanation-filter';
import { StatusConstants } from 'app/core/common/status.constants';
import { MessageConstants } from '../../core/common/message.constants';
import { NotificationService } from '../../core/services/notification.service';
import { DateRangePickerModel } from '../../core/models/date-range-picker';
import { PageConstants } from 'app/core/common/page.constans';
import { Announcement } from 'app/core/models/announcement';
import { UtilityService } from '../../core/services/utility.service';
import { SendMailModel } from 'app/core/models/sendMailModel';
import { MailConstants } from '../../core/common/mail.constants';
import { Hero } from '../../core/models/dashboardModel';
import { PassDataService } from '../../core/services/passData.service';

import { SendMailDelegateModel } from '../../core/models/SendMailDelegateModel';

import { SendMailModelNew } from 'app/core/models/SendMailModelNew';
@Component({
  selector: 'app-explanation-request',
  templateUrl: './explanation-request.component.html',
  styleUrls: ['./explanation-request.component.css']
})
export class ExplanationRequestComponent implements OnInit {

  //#region "Declare variable, innit some value and so on"
  @ViewChild('addEditModal') public addEditModal: ModalDirective;
  @ViewChild('modalDelegate') public modalDelegate: ModalDirective;
  public entity: any;
  public cvcv: any;
  public explanations: any[];
  public statusRequest: any[];
  public user: any;
  public value: any = {
    end : '',
    start : ''
  };
  public Test: any;
  public toEmail: string[];
  public isDesc: boolean = false;
  public column: string = '';
  pagingSize: number = PageConstants.pagingSize;
  public pageIndex: number = PageConstants.pageIndex;
  public pageSize: number = PageConstants.pageSize;
  public totalRow: number;
  public chosenCreatorFilter: string[] = [];  
  public chosenStatusFilter: string[] = [];
  public chosenReasonFilter: string[] = [];
  public explanationFilter: ExplanationFilter = null;
  private startDate: string;
  private endDate: string;
  public totalEntries: number;
  public creatorFilterOptions: IMultiSelectOption[] = [];
  public statusFilterOptions: IMultiSelectOption[] = [];
  public reasonFilterOptions: IMultiSelectOption[] = [];
  public checkboxChecked: boolean = false;
  loadsuccess: boolean = false;
  public announcement: Announcement;
  public delegateFilterOptions: IMultiSelectOption[] = [];
  public chosenDelegate: string[] = [];
  public explanationId: number;
  public explanationObject: any;
  SendMailModel: SendMailModel = null;
  SendMailModelCreated: SendMailModel = null;
  listMail: Array<SendMailModel> = [];
  DashBoard: Hero;
  public isUnauthorizedLeave: boolean = false;
  public listExplanationChecked: any[] = [];
  public approveChecked: boolean = false;
  public rejectChecked: boolean = false;
  public delegateChecked: boolean = false;
  public listExplanationIdSelected: string[] = [];
  public delegateId: number;
  isGreater0: boolean = false;
  public explanationsSuperAdmin: any[];
  public roleUser :any[];
  public creatorFilterOptionsSuperAdmin: IMultiSelectOption[] = [];
  public chosenCreatorFilterSuperAdmin: string[] = [];


  // Declare settings for Dropdown
  dropdownSettings: IMultiSelectSettings = {
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 0,
    displayAllSelectedText: true,
    showCheckAll: true,
    showUncheckAll: true,
    closeOnClickOutside: true,
    maxHeight: '200px',
  };
  dropdownSearchSettings: IMultiSelectSettings = {
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
  SearchSettings: IMultiSelectSettings = {
    checkedStyle: 'glyphicon',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 1,
    autoUnselect: true,
    closeOnSelect: true,
    selectionLimit: 1,
    enableSearch: true,
    maxHeight: '200px',
  };
  // Declare text for Status Dropdown
  statusDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectStatusType,
    checked: CommonConstants.CheckStatusType,
    checkedPlural: CommonConstants.CheckStatusType,
    defaultTitle: CommonConstants.StatusRequestTitle,
  };

  // Declare text for Reasons Dropdown
  reasonDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectReasonType,
    checked: CommonConstants.CheckReasonType,
    checkedPlural: CommonConstants.CheckReasonType,
    defaultTitle: CommonConstants.ReasonRequestTitle
  };

  creatorDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectFullName,
    checked: CommonConstants.CheckFullName,
    checkedPlural: CommonConstants.CheckFullName,
    defaultTitle: CommonConstants.FullName,
  };

  delegateDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectDelegate,
    checked: CommonConstants.CheckDelegateType,
    checkedPlural: CommonConstants.CheckDelegateType,
    defaultTitle: CommonConstants.DelegatorTitle,
  };
  //#endregion

  //Contructor to inject services
  constructor(public _authenService: AuthenService,
    private _dataService: DataService,
    private _notificationService: NotificationService,
    private daterangepickerOptions: DaterangepickerConfig,
    private _passDataService: PassDataService,
    private _utilityService: UtilityService) {
    if (_authenService.getLoggedInUser().groupId.length == 0) {
      _utilityService.navigateToMain();
    }
    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: true,
      "opens": "left",
      ranges: DateTimeConstants.Range,
    };
  }

  // Init data or function
  ngOnInit() {
    this.DashBoard = this._passDataService.dashboard;
    this.user = this._authenService.getLoggedInUser();
    this.roleUser =  JSON.parse(this._authenService.getLoggedInUser().roles)
    if (this.DashBoard == undefined) {
      if((this.roleUser[0].localeCompare("SuperAdmin")) == 0 || (this.roleUser[0].localeCompare("Admin")) == 0)
      {
        this.loadDataSuperAdmin();
        this.loadCreatorFilterSuperAdmin();
      }else
      {
        this.loadData();
      }
    }
    if (this.DashBoard != undefined) {
      if (this.DashBoard.clickFromDashBoard == false) {
        if((this.roleUser[0].localeCompare("SuperAdmin")) == 0 || (this.roleUser[0].localeCompare("Admin")) == 0)
        {
          this.loadDataSuperAdmin();
          this.loadCreatorFilterSuperAdmin();
        }else
        {
          this.loadData();
        }
      }
      if (this.DashBoard.clickFromDashBoard == true) {
        if((this.roleUser[0].localeCompare("SuperAdmin")) == 0 || (this.roleUser[0].localeCompare("Admin")) == 0)
        {
          this.loadDataSuperAdmin();
          this.loadCreatorFilterSuperAdmin();
        }else
        {
          this.loadPersonalData();
          this.DashBoard.clickFromDashBoard = false;
        }
        
      }
    }
    this.loadCreatorFilter();
    this.loadStatusFilter();
    this.loadReasonFilter();
    this.monitorPageChange();
    this.toEmail = [];
  }

  //#region "Init data"
  public loadData() {
    this._dataService.post('/api/explanation/getall?userId=' + this.user.id + '&groupId='
      + this.user.groupId + '&column=' + this.column + '&isDesc=' + this.isDesc + '&page='
      + this.pageIndex + '&pageSize=' + this.pageSize, JSON.stringify(this.explanationFilter))
      .subscribe((response: any) => {
        this.explanations = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.checkCheckBox();
        this.isGreater0 = this.getListAfterCondition().length > 0 ? true : false;
        this.loadsuccess = true;
        this.monitorPageChange();
      }, error => this._dataService.handleError(error));

  }
  public loadDataSuperAdmin() {
    this._dataService.post('/api/explanation/getallexplanation?column=' + this.column + '&isDesc=' + this.isDesc + '&page='
      + this.pageIndex + '&pageSize=' + this.pageSize, JSON.stringify(this.explanationFilter))
      .subscribe((response: any) => {
        this.explanationsSuperAdmin = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.loadsuccess = true;
        this.monitorPageChange();
      }, error => this._dataService.handleError(error));

  }
  public loadPersonalData() {
    this.explanationFilter = new ExplanationFilter([this.user.id], this.chosenStatusFilter, this.chosenReasonFilter, this.startDate, this.endDate,this.chosenCreatorFilterSuperAdmin);
    this.chosenCreatorFilter = [this.user.id]
    this._dataService.post('/api/explanation/getall?userId=' + this.user.id + '&groupId='
      + this.user.groupId + '&column=' + this.column + '&isDesc=' + this.isDesc + '&page='
      + this.pageIndex + '&pageSize=' + this.pageSize, JSON.stringify(this.explanationFilter))
      .subscribe((response: any) => {
        this.explanations = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.loadsuccess = true;
        this.monitorPageChange();
      }, error => this._dataService.handleError(error));
  }

  // Get list explanation status to filter
  public loadCreatorFilter() {
    this._dataService.get('/api/appUser/getuserbygroup?groupID=' + this.user.groupId)
      .subscribe((response: any) => {
        this.creatorFilterOptions = [];
        for (let creator of response) {
          this.creatorFilterOptions.push({ id: creator.Id, name: creator.FullName + " ( " + creator.UserName + " ) " });
        }
      }, error => this._dataService.handleError(error));
  }
  
    //Get list full name  explanation status to filter for super ad min
  public loadCreatorFilterSuperAdmin() {
    this._dataService.get('/api/appUser/getuserbygroup')
      .subscribe((response: any) => {
        this.creatorFilterOptionsSuperAdmin = [];
        for (let creator of response) {
          if((this.user.id.localeCompare(creator.Id)) != 0 )
          {
            this.creatorFilterOptionsSuperAdmin.push({ id: creator.Id, name: creator.FullName + " ( " + creator.UserName + " ) " });
          }
        }
      }, error => this._dataService.handleError(error));
  }


  // Get list explanation status to filter
  public loadStatusFilter() {
    this._dataService.get('/api/statusrequest/getall')
      .subscribe((response: any) => {
        this.statusFilterOptions = [];
        for (let status of response) {
          this.statusFilterOptions.push({ id: status.ID, name: status.Name });
        }
      }, error => this._dataService.handleError(error));
  }

  // Get list explanation reason to filter
  public loadReasonFilter() {
    this._dataService.get('/api/abnormalreason/getallabnormalreason')
      .subscribe((response: any) => {
        this.reasonFilterOptions = [];
        for (let status of response) {
          this.reasonFilterOptions.push({ id: status.ID, name: status.Name });
        }
      }, error => this._dataService.handleError(error));
  }
  //#endregion

  //#region "Business logic"

  // check permission of user when load explanation list
  public checkPermisson(entity: any): boolean {
    return (entity.User.Id == this.user.id && (entity.StatusRequest.Name == StatusConstants.Pending || entity.StatusRequest.Name == StatusConstants.Delegation));
  }

  // check permission of user when load explanation list
  public hasPermission(functionId: string, action: string): boolean {
    return this._authenService.hasPermission(functionId, action);
  }

  public checkCanCel(entity: any) {
    return (this.checkPermisson(entity) && !entity.IsExpiredDate);
  }
  //Check User login can Approve request
  public checkApprove(entity: any): boolean {
    return ((entity.StatusRequest.Name == CommonConstants.Pending || entity.StatusRequest.Name == CommonConstants.Rejected) && !entity.IsExpiredDate);
  }

  //Check User login can Reject request
  public checkReject(entity: any): boolean {
    return ((entity.StatusRequest.Name == CommonConstants.Pending || entity.StatusRequest.Name == CommonConstants.Approved) && !entity.IsExpiredDate);
  }

  //Check User login can Delegate request
  public checkDelegate(entity: any): boolean {
    return ((entity.StatusRequest.Name == CommonConstants.Pending) && !entity.IsExpiredDate);
  }

  public getAllowRangeDate(date: any): boolean {
    let diffInMs: number = (Date.parse(moment().format(DateTimeConstants.FORMAT_DATE_MMDDYYYY)) - Date.parse(date));
    return (diffInMs / 1000 / 60 / 60) <= CommonConstants.DATE_RANGE_CHANGE_STATUS_EXPLANATION;
  }

  //change status request
  cancelExplanation(id: any, Title: string) {
    this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_CANCEL_EXPLANATION_MSG, () => this.cancelExplanationConfirm(id, Title));
  }
  //comfirm change status request
  cancelExplanationConfirm(id: any, Title: string) {
    // this.toEmail = this.user.email;
    // this.SendMailModel = new SendMailModel(Title,this.user.id,this.toEmail,MailConstants.Cancel,MailConstants.ExplanationRequest,MailConstants.ExplanationRequestSubject,null,MailConstants.ExplanationRequestManagement);
    this._dataService.post('/api/explanation/changestatus?explanationId=' + id + '&statusName=' + CommonConstants.Cancelled + '&delegateId=' + null).subscribe((response: Response) => {
      this._notificationService.printSuccessMessage(MessageConstants.CANCEL_OK_MSG);
      // this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModel)).subscribe((response: any) => {});
      // this.resetEmail();
      this.setDefaultValues();
      this.listExplanationChecked = [];
    });
  }

  //approve explanation
  approveExplanation(id: any, Title: string, User: any) {
    this.entity={};
    // this.toEmail.push(User.Email);
    this.showDetailData(id);
    // this.SendMailModel = new SendMailModel(Title, this.user.id, this.toEmail, MailConstants.Approve, MailConstants.ExplanationRequest, MailConstants.ExplanationRequestSubject, null, MailConstants.ExplanationRequestManagement);
    this._dataService.post('/api/explanation/changestatus?explanationId=' + id + '&statusName=' + CommonConstants.Approved + '&delegateId=' + this.user.id).subscribe((response: Response) => {
      this._notificationService.printSuccessMessage(MessageConstants.APPROVED_SUCCESS_EXPLANATION_MSG);
      this.toEmail.push(User.Email + "," + User.FullName);
      // this.entity.ExplanationDate = moment(new Date(this.entity.ExplanationDate)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY)
      this.SendMailModelCreated = new SendMailModelNew(this.entity.GroupName, null, this.entity.DetailReason, this.entity.StartDate, this.entity.EndDate, null, null, null, null, null, this.entity.ExplanationDate, this.entity.ReasonList, this.entity.Actual, this.entity.ReasonDetail, Title, this.user.id, this.toEmail, null, MailConstants.Approve, MailConstants.ExplanationRequest, MailConstants.ExplanationRequestSubject, null, MailConstants.ExplanationRequestManagement);
      this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModelCreated)).subscribe((response: any) => { });
      this.resetEmail();
      this.setDefaultValues();
      this.listExplanationChecked = [];
    }, error => this._dataService.handleError(error));
  }

  //comfirm change status request
  rejectExplanation(entity: any, value: any) {
    this._notificationService.printPromptDialog("", MessageConstants.TEXTAREA_COMMENT_REJECT_EXPLANATION_MSG, (value: string) => this.rejectExplanationConfirm(entity, value));
  }

  // reject explanation
  rejectExplanationConfirm(entity: any, value: any) {
    if (value.trim() != '') {
      // this.toEmail.push(entity.User.Email);
      this.showDetailData(entity.ID);
      // this.SendMailModel = new SendMailModel(entity.Title, this.user.id, this.toEmail, MailConstants.Reject, MailConstants.ExplanationRequest, MailConstants.ExplanationRequestSubject, value, MailConstants.ExplanationRequestManagement);
      this._dataService.post('/api/explanation/changeStatus?explanationId=' + entity.ID + '&statusName=' + CommonConstants.Rejected + '&delegateId=' + this.user.id).
        subscribe((response: Response) => {
          this.toEmail.push(entity.User.Email + "," + entity.User.FullName);
          this.SendMailModelCreated = new SendMailModelNew(this.entity.GroupName, null, this.entity.DetailReason, this.entity.StartDate, this.entity.EndDate, null, null, null, null, null, this.entity.ExplanationDate, this.entity.ReasonList, this.entity.Actual, this.entity.ReasonDetail, entity.Title, this.user.id, this.toEmail, null, MailConstants.Reject, MailConstants.ExplanationRequest, MailConstants.ExplanationRequestSubject, value, MailConstants.ExplanationRequestManagement);
          this.setDefaultValues();
          this.listExplanationChecked = [];
          this._notificationService.printSuccessMessage(MessageConstants.REJECTED_SUCCESS_EXPLANATION_MSG);
          this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModelCreated)).subscribe((response: any) => { });
          this.resetEmail();
        }, error => this._dataService.handleError(error));
    } else {
      this._notificationService.printErrorMessage(MessageConstants.VALID_COMMENT_REJECT_MSG);
    }
  }

  // show dialog to choose member to delegate
  showDelegateConfirm(entity: any, delegateId: number) {
    this._dataService.get('/api/appUser/getuserbygroup')
      .subscribe((response: any) => {
        this.explanationObject = entity;
        this.delegateFilterOptions = [];
        this.delegateId = delegateId;
        this.chosenDelegate = [];
        for (let userGroup of response) {
          if (this.user.id != userGroup.Id) {
            this.delegateFilterOptions.push({ id: userGroup.Id, name: userGroup.FullName + " ( " + userGroup.UserName + " ) " });
          }
        }
      }, error => this._dataService.handleError(error));
    this.modalDelegate.show();
  }

  public closeDelegateDialog() {
    this.modalDelegate.hide();
    this.explanationId = 0;
    this.chosenDelegate = [];
  }
  public Loadetailexplan(explanID: string) {
    this._dataService.get('/api/explanation/detail/' + explanID).subscribe((response: any) => {
      if (response.ReasonList.includes('Unauthorized Leave')) {
        this.isUnauthorizedLeave = true;
      } else {
        this.isUnauthorizedLeave = false;
      }
      this.entity = response;
    }, error => this._dataService.handleError(error)
    );
  }
  // delegate a explanation or explanation selected
  public delegateExplanation(valid: boolean, entity: any, delegateId: number) {
    if (this.delegateChecked && delegateId == CommonConstants.delegateId) {
      this.listExplanationIdSelected = [];
      this.listMail = [];
      for (let item of this.listExplanationChecked) {
        this.listExplanationIdSelected.push(item.ID);
        this._dataService.get('/api/explanation/detail/' + item.ID).subscribe((response: any) => {
          if (response.ReasonList.includes('Unauthorized Leave')) {
            this.isUnauthorizedLeave = true;
          } else {
            this.isUnauthorizedLeave = false;
          }

          this.listMail.push(new SendMailDelegateModel('Explanation Request: ' + item.Title, response.Receiver.FullName, this.chosenDelegate, MailConstants.Delegation, MailConstants.DelegationAssignedList, MailConstants.DelegationExplanationRequest, null,
            MailConstants.DelegationManagement, item.User.Group.Name, item.ExplanationDate, response.ReasonList, item.ReasonDetail, item.User.Id, response.Receiver.Id));
        }, error => this._dataService.handleError(error)
        );
      }

      this._dataService.post('/api/explanation/changestatusmulti?statusName=' + CommonConstants.Delegated + '&delegateId=' + this.chosenDelegate[0], JSON.stringify(this.listExplanationIdSelected)).
        subscribe((response: Response) => {
          this.setDefaultValues();
          this.listExplanationChecked = [];
          this._notificationService.printSuccessMessage(MessageConstants.DELEGATED_SUCCESS_EXPLANATION_MSG);
          this._dataService.post('/api/System/sendMailMultiFix', JSON.stringify(this.listMail)).subscribe((response: any) => { });
          this.resetEmail();
        }, error => this._dataService.handleError(error));
    } else {
      this._dataService.get('/api/explanation/detail/' + entity.ID).subscribe((response: any) => {
        if (response.ReasonList.includes('Unauthorized Leave')) {
          this.isUnauthorizedLeave = true;
        } else {
          this.isUnauthorizedLeave = false;
        }
        this.cvcv = response;
        this.SendMailModel = new SendMailDelegateModel(entity.Title, entity.User.FullName, this.chosenDelegate, MailConstants.Delegation, MailConstants.DelegationAssignedList, MailConstants.DelegationExplanationRequest, null, MailConstants.DelegationManagement,
          entity.User.Group.Name, entity.ExplanationDate, response.ReasonList, entity.ReasonDetail, entity.User.Id, entity.Receiver.Id);

        this._dataService.post('/api/explanation/changeStatus?explanationId=' + entity.ID + '&statusName=' + CommonConstants.Delegated + '&delegateId=' + this.chosenDelegate[0]).
          subscribe((response: Response) => {
            this.setDefaultValues();
            this.listExplanationChecked = [];
            this.explanationId = 0;
            this._notificationService.printSuccessMessage(MessageConstants.DELEGATED_SUCCESS_EXPLANATION_MSG);
            this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModel)).subscribe((response: any) => { });
            this.resetEmail();

          }, error => this._dataService.handleError(error));
      }, error => this._dataService.handleError(error)

      );
    }
  }

  // set value into default value
  private setDefaultValues() {
    this.addEditModal.hide();
    this.modalDelegate.hide();
    this.approveChecked = false;
    this.rejectChecked = false;
    this.delegateChecked = false;
    if((this.roleUser[0].localeCompare("SuperAdmin")) == 0 || (this.roleUser[0].localeCompare("Admin")) == 0 )
    {
      this.loadDataSuperAdmin();
    }
    else
    {
      this.loadData();
    }
  }

  // get list explanation after conditions
  public getListAfterCondition() {
    return this.explanations.filter(function (data) {
      if (data.StatusRequest.Name != CommonConstants.Cancelled && data.StatusRequest.Name != CommonConstants.Delegated) {
        let diffInMs: number = (Date.parse(moment().format(DateTimeConstants.FORMAT_DATE_MMDDYYYY)) - Date.parse(data.CreatedDate));
        if ((diffInMs / 1000 / 60 / 60) <= CommonConstants.DATE_RANGE_CHANGE_STATUS_EXPLANATION) {
          return data;
        }
      }
    })
  }

  // check when explanations list not enough condition to change status
  // public checkAllchecked() {
  //   return this.getListAfterCondition().length == 0;
  // }

  // check all checkbox
  public checkAll(ev: any) {
    this.getListAfterCondition().forEach(x => x.Checked = ev.target.checked)
    if (ev.target.checked) {
      for (let item of this.getListAfterCondition()) {
        if (!this.IsArrayIncludeItem(this.listExplanationChecked, item)) {
          this._dataService.get('/api/explanation/detail/' + item.ID).subscribe((response: any) => {
            item.ReasonList = response.ReasonList;
            item.Actual = response.Actual;
          }, error => this._dataService.handleError(error));
          this.listExplanationChecked.push(item);
        }
      }
    } else {
      for (let item of this.getListAfterCondition()) {
        if (this.IsArrayIncludeItem(this.listExplanationChecked, item)) {
          this.listExplanationChecked = this.removeItemArray(item, this.listExplanationChecked);
        }
      }
    }
    this.checkApproveSelected();
    this.checkRejecteSelected();
    this.checkDelegateSelected();
    // this.checkboxChange();
  }

  // check when all checkbox is checked
  public isAllChecked() {
    return this.getListAfterCondition().every(_ => _.Checked);
  }

  //Change on click checkbox
  checkBoxList(event: any, item: any) {
    if (event.target.checked) {
      if (!this.IsArrayIncludeItem(this.listExplanationChecked, item)) {
        this._dataService.get('/api/explanation/detail/' + item.ID).subscribe((response: any) => {
          item.ReasonList = response.ReasonList;
          item.Actual = response.Actual;
        }, error => this._dataService.handleError(error));
        this.listExplanationChecked.push(item);
      }
    } else {
      if (this.IsArrayIncludeItem(this.listExplanationChecked, item)) {
        this.removeItemArray(item, this.listExplanationChecked);
      }
    }
    this.checkApproveSelected();
    this.checkRejecteSelected();
    this.checkDelegateSelected();
  }

  // check enable approved selected button if enough condition
  checkApproveSelected() {
    if (this.listExplanationChecked != null) {
      var list = [];
      list = this.listExplanationChecked.filter(function (data) {
        if (data.StatusRequest.Name == CommonConstants.Pending || data.StatusRequest.Name == CommonConstants.Rejected) {
          let diffInMs: number = (Date.parse(moment().format(DateTimeConstants.FORMAT_DATE_MMDDYYYY)) - Date.parse(data.CreatedDate));
          if ((diffInMs / 1000 / 60 / 60) <= CommonConstants.DATE_RANGE_CHANGE_STATUS_EXPLANATION) {
            return data;
          }
        }
      });
      if (list.length == this.listExplanationChecked.length) {
        this.approveChecked = true;
      }
      if (list.length != this.listExplanationChecked.length) {
        this.approveChecked = false;
      }
      if (this.listExplanationChecked.length == 0) {
        this.approveChecked = false;
      }
    }
  }

  // check enable reject selected button if enough condition
  checkRejecteSelected() {
    if (this.listExplanationChecked != null) {
      var list = [];
      list = this.listExplanationChecked.filter(function (data) {
        if (data.StatusRequest.Name === CommonConstants.Pending || data.StatusRequest.Name == CommonConstants.Approved) {
          let diffInMs: number = (Date.parse(moment().format(DateTimeConstants.FORMAT_DATE_MMDDYYYY)) - Date.parse(data.CreatedDate));
          if ((diffInMs / 1000 / 60 / 60) <= CommonConstants.DATE_RANGE_CHANGE_STATUS_EXPLANATION) {
            return data;
          }
        }
      });
      if (list.length == this.listExplanationChecked.length) {
        this.rejectChecked = true;
      }
      if (list.length != this.listExplanationChecked.length) {
        this.rejectChecked = false;
      }
      if (this.listExplanationChecked.length == 0) {
        this.rejectChecked = false;
      }
    }
  }

  // check enable delegate selected button if enough condition
  checkDelegateSelected() {
    if (this.listExplanationChecked != null) {
      var list = [];
      list = this.listExplanationChecked.filter(function (data) {
        if (data.StatusRequest.Name == CommonConstants.Pending) {
          let diffInMs: number = (Date.parse(moment().format(DateTimeConstants.FORMAT_DATE_MMDDYYYY)) - Date.parse(data.CreatedDate));
          if ((diffInMs / 1000 / 60 / 60) <= CommonConstants.DATE_RANGE_CHANGE_STATUS_EXPLANATION) {
            return data;
          }
        }
      });
      if (list.length == this.listExplanationChecked.length) {
        this.delegateChecked = true;
      }
      if (list.length != this.listExplanationChecked.length) {
        this.delegateChecked = false;
      }
      if (this.listExplanationChecked.length == 0) {
        this.delegateChecked = false;
      }
    }
  }

  // approve selected explanations
  public approveSelectedExplanation() {
    this.listExplanationIdSelected = [];
    this.listMail = [];
    for (let item of this.listExplanationChecked) {
      this.toEmail = [];
      item.ExplanationDate = moment(new Date(item.ExplanationDate)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      this.listExplanationIdSelected.push(item.ID);
      this.toEmail.push(item.User.Email + "," + item.User.FullName);
      this.listMail.push(new SendMailModelNew(item.User.Group.Name, null, null, null, null, null, null, null, null, null, item.ExplanationDate, item.ReasonList, item.Actual, item.ReasonDetail, 'Explanation request:' + item.Title, this.user.id, this.toEmail, null, MailConstants.Approve, MailConstants.ExplanationRequest, MailConstants.ExplanationRequestSubject, null, MailConstants.ExplanationRequestManagement));
      // this.listMail.push(new SendMailModel('Explanation request:' + item.Title, this.user.id, this.toEmail, MailConstants.Approve, MailConstants.ExplanationRequest, MailConstants.ExplanationRequestSubject, null, MailConstants.ExplanationRequestManagement));
    }
    this._dataService.post('/api/explanation/changestatusmulti?statusName=' + CommonConstants.Approved + '&delegateId=' + this.user.id, JSON.stringify(this.listExplanationIdSelected)).
      subscribe((response: Response) => {
        this.setDefaultValues();
        this.listExplanationChecked = [];
        this._notificationService.printSuccessMessage(MessageConstants.APPROVED_SUCCESS_EXPLANATION_MSG);
        this._dataService.post('/api/System/sendMailMultiFix', JSON.stringify(this.listMail)).subscribe((response: any) => { });
        // this._dataService.post('/api/System/sendMailMulti', JSON.stringify(this.listMail)).subscribe((response: any) => { });
        this.resetEmail();
      }, error => this._dataService.handleError(error));
  }

  // show dialog to comment before reject selected explanations
  public rejectSelectedExplanationConfirm() {
    this._notificationService.printPromptDialog("", MessageConstants.TEXTAREA_COMMENT_REJECT_EXPLANATION_MSG,
      (value: string) => {
        this.rejectSelectedExplanation(value);
      }
    );
  }

  // reject selected explanations
  public rejectSelectedExplanation(value: any) {
      this.listExplanationIdSelected = [];
      this.listMail = [];
      for (let item of this.listExplanationChecked) {
        this.toEmail = [];
        item.ExplanationDate = moment(new Date(item.ExplanationDate)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
        this.listExplanationIdSelected.push(item.ID);
        this.toEmail.push(item.User.Email + "," + item.User.FullName);
        this.listMail.push(new SendMailModelNew(item.User.Group.Name, null, null, null, null, null, null, null, null, null, item.ExplanationDate, item.ReasonList, item.Actual, item.ReasonDetail, 'Explanation request:' + item.Title, this.user.id, this.toEmail, null, MailConstants.Reject, MailConstants.ExplanationRequest, MailConstants.ExplanationRequestSubject, value, MailConstants.ExplanationRequestManagement));
        // this.listMail.push(new SendMailModel(item.Title, this.user.id, this.toEmail, MailConstants.Reject, MailConstants.ExplanationRequest, MailConstants.ExplanationRequestSubject, value, MailConstants.ExplanationRequestManagement));
      }
      if (value.trim() != '') {
        this._dataService.post('/api/explanation/changestatusmulti?statusName=' + CommonConstants.Rejected + '&delegateId=' + this.user.id, JSON.stringify(this.listExplanationIdSelected)).
          subscribe((response: Response) => {
            this.setDefaultValues();
            this.listExplanationChecked = [];
            this._notificationService.printSuccessMessage(MessageConstants.REJECTED_SUCCESS_EXPLANATION_MSG);
            this._dataService.post('/api/System/sendMailMultiFix', JSON.stringify(this.listMail)).subscribe((response: any) => { });
            // this._dataService.post('/api/System/sendMailMulti', JSON.stringify(this.listMail)).subscribe((response: any) => { });
            this.resetEmail();
          }, error => this._dataService.handleError(error));
        this.listExplanationChecked = [];
      } else {
        this._notificationService.printErrorMessage(MessageConstants.VALID_COMMENT_REJECT_MSG);
      }
    }
  
  // delegate selected explanations
  public delegateSelectedExplanation(valid: boolean, id: number) {
    this.listExplanationIdSelected = [];
    this.listMail = [];
    for (let item of this.listExplanationChecked) {
      this.listExplanationIdSelected.push(item.ID);
      this.listMail.push(new SendMailModel('Explanation request: ' + item.Title, this.user.id, this.chosenDelegate, MailConstants.Delegation, MailConstants.DelegationAssignedList, MailConstants.DelegationExplanationRequest, null, MailConstants.DelegationManagement));
    }
    this._dataService.post('/api/explanation/changestatusmulti?statusName=' + CommonConstants.Delegated + '&delegateId=' + this.chosenDelegate[0], JSON.stringify(this.listExplanationIdSelected)).
      subscribe((response: Response) => {
        this.setDefaultValues();
        this.listExplanationChecked = [];
        this.explanationId = 0;
        this.modalDelegate.hide();
        this._notificationService.printSuccessMessage(MessageConstants.DELEGATED_SUCCESS_EXPLANATION_MSG);
        this._dataService.post('/api/System/sendMailMulti', JSON.stringify(this.listMail)).subscribe((response: any) => { });
        this.resetEmail();
      });
  }

  // show/hide approve or reject selected button when checkbox change
  public checkboxChange() {
    let i = 0;
    this.explanations.forEach(x => {
      if (!x.Checked) {
        i++;
      }
    });
    if (i == this.explanations.length) {
      this.checkboxChecked = false;
    } else {
      this.checkboxChecked = true;
    }
  }

  // get explanation detail
  public showDetail(id: number) {
    this._dataService.get('/api/explanation/detail/' + id).subscribe((response: any) => {
      if (response.ReasonList.includes('Unauthorized Leave')) {
        this.isUnauthorizedLeave = true;
      } else {
        this.isUnauthorizedLeave = false;
      }
      this.entity = response;
      this.cvcv = response;
      this.entity.ExplanationDate = moment(new Date(this.entity.ExplanationDate)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY)
      this.addEditModal.show();
    }, error => this._dataService.handleError(error));
  }

  // get explanation detail data for send mail
  public showDetailData(id: number) {
    this._dataService.get('/api/explanation/detail/' + id).subscribe((response: any) => {
      this.entity = response;
      this.entity.ExplanationDate = moment(new Date(this.entity.ExplanationDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY)
    }, error => this._dataService.handleError(error));
  }

  //get list after sort
  public sort(property) {
    this.pageIndex = CommonConstants.pageIndex;
    this.isDesc = !this.isDesc; //change the direction    
    this.column = property;
    this.setDefaultValues();
  }

  // get list filter
  filter() {
    if (this.chosenDate.start != '' && this.chosenDate.end != '') {
      this.startDate = moment(new Date(this.chosenDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      this.endDate = moment(new Date(this.chosenDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    } else {
      this.startDate = null;
      this.endDate = null;
    }
    this.explanationFilter = new ExplanationFilter(this.chosenCreatorFilter, this.chosenStatusFilter, this.chosenReasonFilter, this.startDate, this.endDate, this.chosenCreatorFilterSuperAdmin);
    this.pageIndex = CommonConstants.pageIndex;
    this.setDefaultValues();
    // this.loadData();
  }

  // reset all chonsen filter and get list origin
  public reset() {
    this.daterangepickerOptions.settings = {
      alwaysShowCalendars: true,
      "opens": "left",
      locale: DateTimeConstants.Locale,
      ranges: DateTimeConstants.Range,
      autoUpdateInput: true,
    };
        this.value.start = moment();
        this.value.end = moment();
        this.selectedDateRangePicker(this.value, this.chosenDate);
    this.chosenDate.start = '';
    this.chosenDate.end = '';

    // var dateranger = new DateRangePickerModel(moment().subtract(3, DateTimeConstants.DAY), moment(), DateTimeConstants.CustomRange);
    // this.selectedDateRangePicker(dateranger, this.chosenDate);
    this.explanationFilter = null;
    this.chosenStatusFilter = [];
    this.chosenReasonFilter = [];
    this.chosenCreatorFilter = [];
    this.chosenCreatorFilterSuperAdmin=[];
    this.pageIndex = CommonConstants.pageIndex;
    this.setDefaultValues();
  }

  // call when next/previous page
  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.loadsuccess = false;
    this.setDefaultValues();
    //this.loadData();
    this.monitorPageChange();
    this.checkApproveSelected();
    this.checkRejecteSelected();
    this.checkDelegateSelected();
  }

  // calculate total showing entries when page change
  public monitorPageChange() {
    this.totalEntries = this.pageIndex * this.pageSize;
    if (this.totalEntries >= this.totalRow) {
      this.totalEntries = this.totalRow;
    }
  }

  // default date picker
  public picker = {
    opens: CommonConstants.LEFT,
    startDate: moment(),
    endDate: moment(),
    isInvalidDate: function (date) {
      if (date.isSame(Date.toString(), DateTimeConstants.DAY))
        return 'mystyle';
      return false;
    }
  }
  // get value when chosen date
  public chosenDate: any = {
    start: '',
    end: '',
  };

  // Call event when chosen date
  public selectedDateRangePicker(value: any, dateInput: any) {
    this.value = value;
    dateInput.start = value.start.format();
    dateInput.end = value.end;
  }
  // reset to email
  resetEmail() {
    this.toEmail = [];
    this.listMail = [];
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
    let x = this.explanations;
    for (let item of this.explanations) {
      for (let itemCheck of this.listExplanationChecked) {
        if (item.ID === itemCheck.ID)
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
}
