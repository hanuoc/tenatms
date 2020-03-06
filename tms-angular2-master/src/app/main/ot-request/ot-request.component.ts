
import { Component, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../core/services/data.service';
import { NotificationService } from '../../core/services/notification.service';
import { UtilityService } from '../../core/services/utility.service';
import { AuthenService } from '../../core/services/authen.service';
import { MessageConstants } from '../../core/common/message.constants';
import { SystemConstants } from '../../core/common/system.constants';
import { DateTimeConstants } from '../../core/common/datetime.constants';
import { DefaultColunmConstants } from '../../core/common/defaultColunm.constant';
import { CommonConstants } from '../../core/common/common.constants';
import { StatusConstants } from '../../core/common/status.constants';
import { FunctionConstants } from '../../core/common/function.constants';
import { UploadService } from '../../core/services/upload.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Router } from '@angular/router';
import { LoggedInUser } from '../../core/domain/loggedin.user';
import { DaterangepickerConfig } from '../../core/services/config.service';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { NgForm } from '@angular/forms';
//import {IMyDpOptions} from 'mydatepicker';
import { FilterOTRequest } from '../../core/models/filterOTRequest';
import { Announcement } from '../../core/models/announcement';
import { filter } from 'rxjs/operator/filter';
import { element } from 'protractor';
import { forEach } from '@angular/router/src/utils/collection';
import { OTRequest } from 'app/core/models/OTRequestModel';
import { DateRangePickerModel } from '../../core/models/date-range-picker';
import { PageConstants } from 'app/core/common/page.constans';
import { SendMailModel } from '../../core/models/sendMailModel';
import { MailConstants } from '../../core/common/mail.constants';
import { error } from 'util';
import { SendMailModelNew } from '../../core/models/SendMailModelNew';
import { SendMailModelTest } from 'app/core/models/SendMailModelTest';
@Component({
  selector: 'app-ot-request',
  templateUrl: './ot-request.component.html',
  styleUrls: ['./ot-request.component.css']
})
export class OTRequestComponent implements OnInit {
  //#region "Decalre variable, innit some value and so on"
  @ViewChild('modalAddEdit') public modalAddEdit: ModalDirective;
  @ViewChild('modalReject') public modalReject: ModalDirective;
  @ViewChild('addEditForm') public addEditForm:NgForm;
  public baseFolder: string = SystemConstants.BASE_API;
  public user: LoggedInUser;
  public group: any = null;
  public value: any = {
    end: '',
    start: '',
  };
  public value1: any = {
    end: '',
    start: '',
  };
  public totalRow: number;
  pagingSize: number = PageConstants.pagingSize;
  public pageIndex: number = PageConstants.pageIndex;
  public pageSize: number = PageConstants.pageSize;
  public otRequest: any[];
  public otRequestGeneralAdmin: any[];
  toEmail: string[];
  public entity: any;
  groupLead: any;
  ListSendMail: Array<SendMailModel> = [];
  SendMailModel: SendMailModel = null;
  SendMailModelCreated: SendMailModelNew = null;
  public currentMaxEntries: number;
  public ListFilter: FilterOTRequest = null;
  isCreate: boolean = false;
  isCancel: boolean = false;
  isDesc: boolean = false;
  isAddFalse: boolean = false;
  checkApprove: boolean = false;
  checkReject: boolean = false;
  loadsuccess: boolean = false;
  column: string = DefaultColunmConstants.OTRequestColunm;
  direction: number;
  listMember: string;
  roleUser: any[];
  //pick time
  public settings;
  public setting1;
  public OTCheckIn: string = '';
  public OTCheckOut: string = '';
  //Declare Announment
  public announcement: Announcement = null;
  //Declare filter
  public listcheck: Array<any> = new Array<any>();
  public listOTRequestID: Array<number> = new Array<number>();
  public listCreateBy: Array<OTRequest> = new Array<OTRequest>();
  public groupLeadOption: IMultiSelectOption[] = [];
  public groupLeadFilter: string[] = [];
  public usersOption: IMultiSelectOption[] = [];
  public allusersOption: IMultiSelectOption[] = [];
  public usersFilter: string;
  public statusRequestOption: IMultiSelectOption[] = [];
  public statusRequestFilter: string[] = [];
  public OTDateTypeOption: IMultiSelectOption[] = [];
  public OTDateTypeFilter: string[] = [];
  public OTDateTypeChoose: string;
  public OTTimeTypeOption: IMultiSelectOption[] = [];
  public OTTimeTypeFilter: string[] = [];
  public OTTimeTypeChoose: string;
  public FullNameOption: IMultiSelectOption[] = [];
  public FullNameOptionCC: IMultiSelectOption[] = [];
  public FullNameGeneralAdmin: IMultiSelectOption[] = [];
  public FullNameFilter: string[] = [];
  public FullNameFilterCC: any[] = [];
  private startDate: string;
  private endDate: string;
  isGreater0: boolean = false;
  //Tunglv Code
  public isEdit: boolean = false;
  public OTDate: string;
  public isUser: boolean = false;
  public listUserByGroup: any[] = [];
  public listToMail: any[] = [];
  public isDisable: boolean = false;
  //Dropdown setting
  // Setting dropdown Filter
  dropdownFilterSettings: IMultiSelectSettings = {
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
  // Setting dropdown 
  dropdownSettings: IMultiSelectSettings = {
    enableSearch: false,
    autoUnselect: true,
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 1,
    closeOnClickOutside: true,
    selectionLimit: 1,
    closeOnSelect: true,
    maxHeight: '200px',
  };
  // Setting dropdown Filter with search
  dropdownFilterSearchSettings: IMultiSelectSettings = {
    enableSearch: true,
    autoUnselect: true,
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 0,
    closeOnClickOutside: true,
    maxHeight: '200px',
  };
  // Setting dropdown with search
  dropdownSearchSettings: IMultiSelectSettings = {
    enableSearch: true,
    autoUnselect: true,
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 3,
    displayAllSelectedText: true,
    showCheckAll: true,
    showUncheckAll: true,
    closeOnClickOutside: true,
    maxHeight: '200px',
  };
  dropdownReceiverSearchSettings: IMultiSelectSettings = {
    enableSearch: true,
    autoUnselect: true,
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 1,
    closeOnClickOutside: true,
    maxHeight: '200px',
  };
  dropdownSearchFilterSettingsCC: IMultiSelectSettings = {
    enableSearch: true,
    autoUnselect: true,
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 0,
    displayAllSelectedText: true,
    showCheckAll: true,
    showUncheckAll: true,
    closeOnClickOutside: true,
    maxHeight: '200px',
  };

  // Text configuration Dropdown Status Request
  StatusDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectStatusType,
    checked: CommonConstants.CheckStatusType,
    checkedPlural: CommonConstants.CheckStatusType,
    defaultTitle: CommonConstants.StatusRequestTitle,
  };
  // Text configuration Dropdown OT Date
  OTDateTypeDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectOTDateType,
    checked: CommonConstants.CheckOTDateType,
    checkedPlural: CommonConstants.CheckOTDateType,
    defaultTitle: CommonConstants.OTDateTypeTitle,
  };
  // Text configuration Dropdown List Member Group
  UserGroupDropdownTexts: IMultiSelectTexts = {
    defaultTitle: CommonConstants.UserGroup,
  };
  // Text configuration Dropdown OT Time 
  OTTimeTypeDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectOTTimeType,
    checked: CommonConstants.CheckOTTimeType,
    checkedPlural: CommonConstants.CheckOTTimeType,
    defaultTitle: CommonConstants.OTTimeTypeTitle,
  };
  // Text configuration Dropdown Full Name
  FullNameDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectFullName,
    checked: CommonConstants.CheckFullName,
    checkedPlural: CommonConstants.CheckFullName,
    defaultTitle: CommonConstants.FullName,
  };

  FullNameDropdownTextsCC: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectFullName,
    checked: CommonConstants.CheckFullName,
    checkedPlural: CommonConstants.CheckFullName,
    defaultTitle: CommonConstants.FullName,
  };
  FullNameDropdownGeneralAdmin: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectFullName,
    checked: CommonConstants.CheckFullName,
    checkedPlural: CommonConstants.CheckFullName,
    defaultTitle: CommonConstants.FullName,
  };

  //Date Option for single pick
  public datePickerOptions: any = {
    locale: DateTimeConstants.Locale,
    alwaysShowCalendars: false,
    "opens": "left",
    singleDatePicker: true,
    minDate: moment(),
    endDate: moment(),
  };
  //#endregion
  //Contructor to inject services.
  constructor(public _authenService: AuthenService,
    private _dataService: DataService,
    private _notificationService: NotificationService,
    private _utilityService: UtilityService,
    private _uploadService: UploadService,
    private daterangepickerOptions: DaterangepickerConfig,
  ) {
    if (_authenService.getLoggedInUser().groupId.length == 0) {
      _utilityService.navigateToMain();
    }
    if (_authenService.checkAccess(FunctionConstants.OTRequest) == false) {
      _utilityService.navigateToLogin();
    }
    this.daterangepickerOptions.settings = {
      singleDatePicker: false,
      locale: DateTimeConstants.Locale,
      autoUpdateInput: true,
      alwaysShowCalendars: true,
      "opens": "left",
      ranges: DateTimeConstants.Range,
      endDate: moment(),
      startDate: moment(),
    };
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
  }
  // Innit data or function           
  ngOnInit() {
    this.roleUser = JSON.parse(this._authenService.getLoggedInUser().roles)
    if ((this.roleUser[0].localeCompare("SuperAdmin")) == 0) {
      this.loadGeneralAdmin();
    } else {
      this.loadData();
    }
    
    this.loadStatus();
    this.loadGroup();
    this.loadListUserByGroup();
    this.loadGroupLead();
    this.loadOTDateType();
    this.loadOTTimeType();
    if (this.checkRole()) {
      this.loadFullName();
    }
    this.loadFullNameCC();
    this.loadFullNameGeneralAdmin();
    // this.loadGeneralAdmin();
    this.loadAllUserName();
    
  }
  //#region "Innit data"
  loadData() {
    this.user = this._authenService.getLoggedInUser();
    this._dataService.post('/api/otrequest/getall?userID=' + this.user.id + '&groupId=' + this.user.groupId + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize
      + '&column=' + this.column + '&isDesc=' + this.isDesc, JSON.stringify(this.ListFilter))
      .subscribe((response: any) => {
        this.otRequest = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.checkCheckBox();
        this.isGreater0 = this.getCheckCondition().length > 0 ? true : false;
        this.loadsuccess = true;
        this.calShowingPage();
      }, error => this._dataService.handleError(error));
  }
  loadGeneralAdmin() {
    this.user = this._authenService.getLoggedInUser();
    this._dataService.post('/api/otrequest/getallGeneralAdmin?userID=' + this.user.id + '&groupId=' + this.user.groupId + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize
      + '&column=' + this.column + '&isDesc=' + this.isDesc, JSON.stringify(this.ListFilter))
      .subscribe((response: any) => {
        this.otRequestGeneralAdmin = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        // this.isGreater0 = this.getCheckCondition().length > 0 ? true : false;
        this.loadsuccess = true;
        this.calShowingPage();
      }, error => this._dataService.handleError(error));
  }
  loadStatus() {
    this._dataService.get('/api/statusrequest/getall')
      .subscribe((response: any) => {
        this.statusRequestOption = [];
        for (let status of response) {
          this.statusRequestOption.push({ id: status.ID, name: status.Name })
        }
      }, error => this._dataService.handleError(error));
  }
  //Load OT Date Type
  loadOTDateType() {
    this._dataService.get('/api/otrequest/getallOTDateType')
      .subscribe((response: any) => {
        this.OTDateTypeOption = [];
        for (let OTDateType of response) {
          this.OTDateTypeOption.push({ id: OTDateType.ID, name: OTDateType.Name })
        }
      }, error => this._dataService.handleError(error));
  }
  //Load OT Time Type
  loadOTTimeType() {
    this._dataService.get('/api/otrequest/getallOTTimeType')
      .subscribe((response: any) => {
        this.OTTimeTypeOption = [];
        for (let OTTimeType of response) {
          this.OTTimeTypeOption.push({ id: OTTimeType.ID, name: OTTimeType.Name })
        }
      }, error => this._dataService.handleError(error));
  }
  // Load Group
  loadGroup() {
    this._dataService.get('/api/group/getgroupbyid?groupID=' + this.user.groupId)
      .subscribe((response: any) => {
        this.group = response;
      }, error => this._dataService.handleError(error));
  }
  // List User in Group
  loadListUserByGroup() {
    this._dataService.get('/api/appUser/getuserbygroup?groupID=' + this.user.groupId)
      .subscribe((response: any) => {
        this.listUserByGroup = response;
        this.usersOption = [];
        for (let userGroup of response) {
          // this.usersOption.push({ id: userGroup.Id, name: userGroup.UserName })
          this.usersOption.push({ id: userGroup.Id, name: userGroup.FullName + " ( " + userGroup.UserName + " ) " })
        }
        this.usersOption = this.usersOption.filter(item => item.id !== this.user.id);
        this.listUserByGroup = response.filter(i => i.Id !== this.user.id);

      }, error => this._dataService.handleError(error));
  }
  loadAllUserName() {
    this._dataService.get('/api/appUser/getuserbygroup')
      .subscribe((response: any) => {
        this.allusersOption = [];
        for (let username of response) {
          // this.allusersOption.push({ id: username.Id, name: username.UserName })
          this.allusersOption.push({ id: username.Id, name: username.FullName + " ( " + username.UserName + " ) " })
        }

      }, error => this._dataService.handleError(error));
  }
  //Load Group Lead
  loadGroupLead() {
    this._dataService.get('/api/appUser/getgrouplead?groupId=' + this.user.groupId)
      .subscribe((response: any) => {
        this.groupLead = response;
        this.groupLeadOption = [];
        for (let userGroup of response) {
          this.groupLeadOption.push({ id: userGroup.Id, name: userGroup.FullName })
          this.groupLeadFilter.push(userGroup.Id);
        }
        if (this.groupLead[0] != null) {
          this.usersOption = this.usersOption.filter(item => item.id != this.groupLead[0].Id);
          this.listUserByGroup = this.listUserByGroup.filter(i => i.Id != this.groupLead[0].Id);
        }
      }, error => this._dataService.handleError(error));
  }
  //LOad OT Request Detail
  loadOTRequestDetail(id: any) {
    this._dataService.get('/api/otrequest/detail/' + id)
      .subscribe((response: any) => {
        this.entity = response;
        var entityCreateBy = this.entity.CreateBy;
        this.loadOTRequestUser(id);
        this.chosenDateOTDate.end = response.OTDate;
        // this.entity.OTDate = moment(new Date(this.chosenDateOTDate.end)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
        this.daterangepickerOptions.settings = {
          locale: DateTimeConstants.Locale,
          minDate: moment(),
          alwaysShowCalendars: true,
          "opens": "right",
          autoUpdateInput: true,
          ranges: DateTimeConstants.Range,
          singleDatePicker: true,
          endDate: moment(new Date(this.chosenDateOTDate.end)),
          startDate: moment(new Date(this.chosenDateOTDate.end)),
        };

        this.value1.end = moment(new Date(this.chosenDateOTDate.end));
        this.value1.start = moment(new Date(this.chosenDateOTDate.end));
        this.selectedDatePicker(this.value1, this.chosenDateOTDate);
      });
  }

  // Load List User in Detail
  loadOTRequestUser(id: any) {
    this._dataService.get('/api/otrequest/getOTRequestUser?requestID=' + id)
      .subscribe((response: any) => {
        this.entity.OTRequestUserID = [];
        for (let userID of response) {
          this.entity.OTRequestUserID.push(userID.UserID);
        }
        if (this.entity.CreatedBy == this.user.id) {
          this.isUser = true
        }
      });
  }
  //Load Full Name to Filter
  loadFullName() {
    this._dataService.get('/api/appUser/getuserbygroup?groupID=' + this.user.groupId)
    .subscribe((response: any[]) => {
      this.FullNameOption = [];
      for (let creator of response) {
        //this.FullNameOption.push({ id: fullname, name: fullname })
        this.FullNameOption.push({ id: creator.Id, name: creator.FullName + " ( " + creator.UserName + " ) " });
      }
    }, error => this._dataService.handleError(error));

    // this._dataService.get('/api/otrequest/GetAllCreateByOTRequest?groupId=' + this.user.groupId)
    //   .subscribe((response: any) => {
    //     this.FullNameOption = [];
    //     for (let fullname of response) {
    //       this.FullNameOption.push({ id: fullname, name: fullname })
    //       // this.FullNameOption.push({ id: fullname.AppUserCreatedBy.Id, name: fullname.AppUserCreatedBy.FullName + " ( " + fullname.AppUserCreatedBy.UserName + " ) "})
    //     }
    //   }, error => this._dataService.handleError(error));

  }
  //get infomation for popup
  getInformationForPopup() {
    this.groupLeadFilter = [];
    this._dataService.get('/api/appUser/getgrouplead?groupId=' + this.user.groupId)
      .subscribe((response: any) => {
        this.groupLeadOption = [];
        for (let userGroup of response) {
          this.groupLeadFilter.push(userGroup.Id);
          this.groupLeadOption.push({ id: userGroup.Id, name: userGroup.FullName });
        }
      }, error => this._dataService.handleError(error));
  }
  //#endregion
  //#region "CRUD"
  private saveData(form: NgForm) {
    this.chosenDateOTDate.end = moment(new Date(this.chosenDateOTDate.end))
    this.toEmail = [];
    this.listMember = "";
    this.entity.ccMail = [];
    this.entity.UserID = this.user.id;
    this.entity.Title = this.entity.Title.trim();
    this.entity.OTDateTypeID = Array.isArray(this.entity.OTDateTypeID) ? this.entity.OTDateTypeID[0] : this.entity.OTDateTypeID;
    this.entity.OTTimeTypeID = Array.isArray(this.entity.OTTimeTypeID) ? this.entity.OTTimeTypeID[0] : this.entity.OTTimeTypeID;
    this.entity.OTDate = this.chosenDateOTDate.end.format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this.OTDate = this.chosenDateOTDate.end.format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
    if (this.groupLead[0] == null) {
      this._notificationService.printErrorMessage(MessageConstants.CREATED_FAIL_NO_GROUPLEAD_MSG);
      this.entity = [];
      return;
    }
    if (!this.isAddFalse) {
      this.entity.OTRequestUserID.push(this.user.id);
    }
    this.toEmail.push(this.user.email + "," + this.user.fullName);
    this.toEmail.push(this.groupLead[0].Email + "," + this.groupLead[0].FullName);
    for (let id of this.entity.OTRequestUserID) {
      let object = this.listUserByGroup.find(x => x.Id == id);

      if (typeof object !== "undefined") {
        this.listToMail.push(object.Email)
        this.toEmail.push(object.Email + "," + object.FullName)
      }
    }
    this.SendMailModelCreated = new SendMailModelNew(this.group.Name, null, this.entity.DetailReason, this.entity.StartDate, this.entity.EndDate, this.OTDate, this.OTDateTypeOption.find(x => x.id == this.entity.OTDateTypeID).name, this.OTTimeTypeOption.find(x => x.id == this.entity.OTTimeTypeID).name, this.entity.StartTime, this.entity.EndTime, null, null, null, null, this.entity.Title, this.user.id, this.toEmail, this.entity.ccMail, MailConstants.Create, MailConstants.OTRequest, MailConstants.OTRequestSubject + this.entity.Title, null, MailConstants.RequestManagement);
    // this.SendMailModel = new SendMailModel(this.entity.Title,this.user.id,this.toEmail,null,MailConstants.Create,MailConstants.OTRequest,MailConstants.OTRequestSubject + this.entity.Title,null,MailConstants.RequestManagement)

    for (let emailcc of this.FullNameFilterCC) {
      this.entity.ccMail.push(emailcc.Email);
    }

    for (let itemToMail of this.listToMail) {
      for (let itemCCMail of this.entity.ccMail) {
        if (itemToMail == itemCCMail) {
          this._notificationService.printErrorMessage("You've selected CC mail address same as To mail address. Please select again!");
          this.entity.OTRequestUserID = [];
          this.FullNameFilterCC = [];
          this.listToMail = [];
          this.entity.ccMail = [];
          return;
        }
      }
    }
    this.isDisable = true;
    // this.SendMailModel = new SendMailModelTest(this.entity.Title, this.user.id, this.entity.toEmail,this.entity.ccMail, MailConstants.Create, MailConstants.Request, MailConstants.RequestSubject + this.entity.Title, null, MailConstants.RequestManagement);
    // this.SendMailModel = new SendMailModelTest(this.entity.Title,this.user.id,this.toEmail,null,this.entity.ccMail,MailConstants.Create,MailConstants.OTRequest,MailConstants.OTRequestSubject + this.entity.Title,null,MailConstants.RequestManagement)
    
    if (this.entity.ID == null) {
      if (this.checkCreate()) {
        this._dataService.post('/api/otrequest/add?userID=' + this.user.id + "&groupId=" + this.user.groupId, JSON.stringify(this.entity))
          .subscribe((response: any) => {
            this.daterangepickerOptions.settings = {
              locale: DateTimeConstants.Locale,
              autoUpdateInput: true,
              alwaysShowCalendars: true,
              singleDatePicker: false,
              "opens": "left",
              ranges: DateTimeConstants.Range,
              endDate: moment()
            };
            this.isDisable = false;
            this.loadData();
            this.modalAddEdit.hide();
            form.resetForm();
            this._notificationService.printSuccessMessage(MessageConstants.CREATED_OK_MSG);
            this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModelCreated)).subscribe((response: any) => { })
            this.toEmail = [];
            this.removeItemArray2(this.user.id,this.entity.OTRequestUserID);
            this.isDisable = true;
          }, error => {
            this.isDisable = false;
            this.removeItemArray2(this.user.id,this.entity.OTRequestUserID);
            this._dataService.handleError(error);
            this.isAddFalse = true;
          });
      } else {
        this.isDisable = false;
        this._notificationService.printErrorMessage(MessageConstants.CREATED_FAIL_MSG);
        this.entity = [];
        this.entity.OTRequestUserID.remove(this.user.id);
        form.resetForm();
      
      }
    }
    else {
      this._dataService.put('/api/otrequest/updateotrequest', JSON.stringify(this.entity))
        .subscribe((response: any) => {
          this.daterangepickerOptions.settings = {
            locale: DateTimeConstants.Locale,
            autoUpdateInput: true,
            alwaysShowCalendars: true,
            singleDatePicker: false,
            "opens": "left",
            ranges: DateTimeConstants.Range,
            endDate: moment()
          };
          this.isDisable = false;
          this.loadData();
          this.modalAddEdit.hide();
          form.resetForm();
          this._notificationService.printSuccessMessage(MessageConstants.UPDATED_OK_MSG);
          this.toEmail = [];
          this.isDisable = true;
          this.removeItemArray2(this.user.id,this.entity.OTRequestUserID);          
        }, error => {
          this.isDisable = false;
          this.removeItemArray2(this.user.id,this.entity.OTRequestUserID);
          this._dataService.handleError(error);
          this.isAddFalse = true;
        });
    }


    //     if (this.checkCreate()) {
    //         this._dataService.post('/api/otrequest/add?userID=' + this.user.id + "&groupId=" + this.user.groupId, JSON.stringify(this.entity))
    //           .subscribe((response: any) => {
    //             this.loadData();
    //             this.modalAddEdit.hide();
    //             form.resetForm();
    //             this._notificationService.printSuccessMessage(MessageConstants.CREATED_OK_MSG);
    //             this._dataService.post('/api/System/sendMail',JSON.stringify(this.SendMailModelCreated)).subscribe((response: any) => {})
    //             this.toEmail = [];
    //           }, error => {
    //             this._dataService.handleError(error);
    //             this.isAddFalse = true;
    //         } );
    //       } else {
    //         this._notificationService.printErrorMessage(MessageConstants.CREATED_FAIL_MSG);
    //         this.entity = [];
    //         form.resetForm();
    //       }
  }
  //#endregion
  //#region "Poppu or other"
  showAddModal() {
    this.isDisable = false;
    this.isEdit = false;
    this.isCreate = true;
    this.isCancel = false;
    this.chosenDateOTDate.end = moment();
    this.chosenDateOTDate.start = moment();
    this.groupLeadFilter = [];
    this.entity = {};
    this.FullNameFilterCC = [];
    this.modalAddEdit.show();
    // this.loadGroupLead();
    this.getInformationForPopup();
    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      singleDatePicker: true,
      minDate: moment(),
      alwaysShowCalendars: true,
      "opens": "right",
      autoUpdateInput: true,
      endDate: moment(),
      ranges: DateTimeConstants.Range,
    };
    this.value1.end = moment(new Date(this.chosenDateOTDate.end));
    this.value1.start = moment(new Date(this.chosenDateOTDate.end));
    this.selectedDatePicker(this.value1, this.chosenDateOTDate);
  }
  showEditModal(id: any) {
    this.isEdit = false;
    this.isCreate = false;
    this.isCancel = true;
    this.groupLeadFilter = [];
    this.loadOTRequestDetail(id);
    //this.loadGroupLead();
    this.modalAddEdit.show();
  }
  showUpdateModal(id: any) {
    //this.loadGroupLead();
    this.entity = {};
    this.isEdit = true;
    this.isCreate = false;
    this.isCancel = false;
    this.loadOTRequestDetail(id);
    this.modalAddEdit.show();
    // this.chosenDateOTDate.start = response.OTDate;
    this.entity.OTDate = moment(new Date(this.chosenDateOTDate.end)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
  }
  showEditOTRequest(id: any) {
    this.isEdit = true;
    this.isCreate = false;
    this.isCancel = false;
    this.groupLeadFilter = [];
    this.modalAddEdit.show();
  }
  // showResetModal(id: any) {
  //   this.loadOTRequestDetail(id);

  showResetModal(id: any) {
    // this.loadOTRequestDetail(id);
    this._dataService.get('/api/otrequest/detail/' + id)
      .subscribe((response: any) => {
        this.entity = response;
        this.loadOTRequestUser(id);
        this.chosenDateOTDate.end = response.OTDate;
        this.chosenDateOTDate.start = response.OTDate;
        this.entity.OTDate = moment(new Date(this.chosenDateOTDate.end)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
        this.daterangepickerOptions.settings = {
          locale: DateTimeConstants.Locale,
          "opens": "left",
          alwaysShowCalendars: false,
          singleDatePicker: true,
          minDate: moment(),
          autoUpdateInput: true,
        };
        this.value1.end = moment(new Date(this.chosenDateOTDate.end));
        this.value1.start = moment(new Date(this.chosenDateOTDate.start));
        this.selectedDatePicker(this.value1, this.chosenDateOTDate);
      });
  }

  hideModal(form: NgForm) {
    if (form.dirty) {
      this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_CLOSE_OTREQUEST_MSG, () => {
        form.reset();
        this.modalAddEdit.hide();
        this.daterangepickerOptions.settings = {
          locale: DateTimeConstants.Locale,
          autoUpdateInput: true,
          alwaysShowCalendars: true,
          singleDatePicker: false,
          "opens": "left",
          ranges: DateTimeConstants.Range,
          endDate: moment()
        };
      });
    } else {
      this.modalAddEdit.hide();
    }
  }

  hideModalAction() {
    this.modalAddEdit.hide();
    this.chosenDateOTDate.end = moment();
    this.entity = [];
    this.FullNameFilterCC = [];
    // this.FullNameFilterCC =[];

  }
  //#endregion
  //#region "Business logic"
  //Check box
  getCheckCondition(): any {
    if (this.otRequest != null) {
      var list = this.otRequest.filter(function (data) {
        var currentDate = moment();
        var otDate = moment(new Date(data.OTDate)).add(1, DateTimeConstants.DAY);
        if (data.StatusRequest.Name != CommonConstants.Cancelled) {
          if (otDate.diff(currentDate) > 0) {
            return data;
          }
        }
      });
      return list;
    }
  }
  //check checkAll checked or not
  // checkAllchecked() {
  //   return this.getCheckCondition().length == 0;
  // }
  //change checkAll on click
  public checkAll(ev, checked: any) {
    this.getCheckCondition().forEach(x => x.Checked = ev.target.checked)
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
  }
  //condition of check all
  isAllChecked() {
    return this.getCheckCondition().every(_ => _.Checked);
  }


  //reset Filter
  reset() {
    this.statusRequestFilter = [];
    this.OTDateTypeFilter = [];
    this.OTTimeTypeFilter = [];
    this.FullNameFilter = [];

    this.listcheck = [];
    this.checkApprove = false;
    this.checkReject = false;
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
    // var dateranger = new DateRangePickerModel('','',DateTimeConstants.CustomRange);
    // this.selectedDateRangePicker(dateranger,this.chosenDate);
    this.ListFilter = null;
    this.pageIndex = 1;
    if ((this.roleUser[0].localeCompare("SuperAdmin")) == 0) {
      this.loadGeneralAdmin();
    } else {
      this.loadData();
    }
    this.chosenDate.start = '';
    this.chosenDate.end = '';
    // this.loadData();
  }
  //reset in create
  resetForm(form: NgForm) {
    form.resetForm();
    // this.datePickerOptions = {
    //   autoUpdateInput: true,
    // };
    this.value1.start = moment();
    this.value1.end = moment();
    this.selectedDatePicker(this.value1, this.chosenDateOTDate);
    this.showAddModal();
  }
  //filter action
  filterOTRequest() {
    if (this.chosenDate.start != '' && this.chosenDate.end != '') {
      this.startDate = moment(new Date(this.chosenDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      this.endDate = moment(new Date(this.chosenDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    } else {
      this.startDate = null;
      this.endDate = null;
    }
    this.listcheck = [];
    this.checkApprove = false;
    this.checkReject = false;
    this.ListFilter = new FilterOTRequest(this.statusRequestFilter, this.OTTimeTypeFilter, this.OTDateTypeFilter, this.FullNameFilter, this.startDate, this.endDate);
    this.pageIndex = 1;
    if ((this.roleUser[0].localeCompare("SuperAdmin")) == 0) {
      this.loadGeneralAdmin();
    } else {
      this.loadData();
    }
  }
  //page change action
  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.loadsuccess = false;
    // this.loadData();
    this.calShowingPage();
    this.checkApprovedAll();
    this.checkRejectedAll();
    // this.loadGeneralAdmin();
    if ((this.roleUser[0].localeCompare("SuperAdmin")) == 0) {
      this.loadGeneralAdmin();
    } else {
      this.loadData();
    }
  }
  //sort action
  sort(property) {
    this.isDesc = !this.isDesc; //change the direction    
    this.column = property;
    let direction = this.isDesc ? 1 : -1;
    this.pageIndex = 1;
    // this.loadData();
    // this.loadGeneralAdmin();
    if ((this.roleUser[0].localeCompare("SuperAdmin")) == 0) {
      this.loadGeneralAdmin();
    } else {
      this.loadData();
    }
  };
  //calculate page
  calShowingPage() {
    this.currentMaxEntries = this.pageIndex * this.pageSize;
    if (this.currentMaxEntries >= this.totalRow) {
      this.currentMaxEntries = this.totalRow;
    }
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
  public chosenDate: any = {
    start: '',
    end: '',
  };
  public chosenDateOTDate: any = {
    start: moment().subtract(3, DateTimeConstants.DAY),
    end: moment(),
  };
  public selectedDatePicker(value: any, dateInput: any) {
    this.value1 = value;
    dateInput.start = value.start.format();
    dateInput.end = value.end.format();
  }
  public selectedDateRangePicker(value: any, dateInput: any) {
    this.value = value;
    dateInput.start = value.start.format();
    dateInput.end = value.end.format();
  }
  //Action cancel
  cancelOTRequest(id: any) {
    this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_CANCEL_OTREQUEST_MSG, () => this.cancelOTRequestConfirm(id));
  }
  //Cancel confirm
  cancelOTRequestConfirm(id: any) {
    this._dataService.post('/api/otrequest/changeStatus?requestID=' + id + '&action=' + StatusConstants.Cancelled).
      subscribe((response: Response) => {
        this.listcheck = [];
        this._notificationService.printSuccessMessage(MessageConstants.CANCEL_OK_MSG);
        this.loadData();
        this.hideModalAction();
      });
  }
  //Approve OT Request
  approvedOTRequest(id: any, title: string, email: string) {
    this.toEmail = [];
    // this.toEmail.push(email);
    this.loadOTRequestDetail(id);
    // this.SendMailModel = new SendMailModel(title,this.user.id,this.toEmail,null,MailConstants.Approve,MailConstants.OTRequest,MailConstants.OTRequestSubject + title,null,MailConstants.RequestManagement)
    this._dataService.post('/api/otrequest/changeStatus?requestID=' + id + '&action=' + StatusConstants.Approved).
      subscribe((response: Response) => {
        this.listcheck = [];
        this.entity.OTDate = moment(new Date(this.chosenDateOTDate.end)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
        this.toEmail.push(email + "," + this.entity.AppUserCreatedBy.FullName);
        this.SendMailModelCreated = new SendMailModelNew(this.group.Name, null, this.entity.DetailReason, this.entity.StartDate, this.entity.EndDate, this.entity.OTDate, this.OTDateTypeOption.find(x => x.id == this.entity.OTDateTypeID).name, this.OTTimeTypeOption.find(x => x.id == this.entity.OTTimeTypeID).name, this.entity.StartTime, this.entity.EndTime, null, null, null, null, this.entity.Title, this.user.id, this.toEmail, null, MailConstants.Approve, MailConstants.OTRequest, MailConstants.OTRequestSubject + title, null, MailConstants.RequestManagement);
        this._notificationService.printSuccessMessage(MessageConstants.APPROVED_SUCCES_MSG);
        this.loadData();
        this.checkApprove = false;
        this.hideModalAction();
        this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModelCreated)).subscribe((response: any) => { });
        this.resetEmail();
      }, error => this._dataService.handleError(error));
  }
  //Reject OT Request
  rejectedOTRequest(id: any, title: string, userID: string, email: string) {
    this._notificationService.printPromptDialog("", MessageConstants.TEXTAREA_COMMENT_OTREQUEST_MSG, (value: string) => this.rejectedOTRequestConfirm(value, id, title, userID, email));
  }
  rejectedOTRequestConfirm(value: any, id: any, title: any, userID: string, email: string) {
    if (value.trim() != '') {
      this.toEmail = [];
      // this.toEmail.push(email);
      this.loadOTRequestDetail(id);
      // this.SendMailModel = new SendMailModel(title,this.user.id,this.toEmail,null,MailConstants.Reject,MailConstants.OTRequest,MailConstants.OTRequestSubject + title,value,MailConstants.RequestManagement)
      this._dataService.post('/api/otrequest/changeStatus?requestID=' + id + '&action=' + StatusConstants.Rejected).
        subscribe((response: Response) => {
          this.entity.OTDate = moment(new Date(this.chosenDateOTDate.end)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
          this.toEmail.push(email + "," + this.entity.AppUserCreatedBy.FullName);
          this.SendMailModelCreated = new SendMailModelNew(this.group.Name, null, this.entity.DetailReason, this.entity.StartDate, this.entity.EndDate, this.entity.OTDate, this.OTDateTypeOption.find(x => x.id == this.entity.OTDateTypeID).name, this.OTTimeTypeOption.find(x => x.id == this.entity.OTTimeTypeID).name, this.entity.StartTime, this.entity.EndTime, null, null, null, null, title, this.user.id, this.toEmail, null, MailConstants.Reject, MailConstants.OTRequest, MailConstants.OTRequestSubject + title, value, MailConstants.RequestManagement);
          this.listcheck = [];
          this._notificationService.printSuccessMessage(MessageConstants.Rejected_SUCCES_MSG);
          this.loadData();
          this.checkReject = false;
          this.hideModalAction();
          this._dataService.post('/api/System/sendMail', JSON.stringify(this.SendMailModelCreated)).subscribe((response: any) => { });
          this.resetEmail();
        }, error => this._dataService.handleError(error));
    } else {
      this._notificationService.printErrorMessage(MessageConstants.VALID_COMMENT_REJECT_MSG);
    }
  }
  //check Create condition
  checkCreate() {
    if (moment().locale(DateTimeConstants.FORMAT_local_EN).format(DateTimeConstants.FORMAT_DAYOFWEEK_DDDD) == DateTimeConstants.DAYOFWEEKFRIDAY) {
      return moment().format(DateTimeConstants.FORMAT_HOUR_HH) <= 17;
    }
    if ((moment().locale(DateTimeConstants.FORMAT_local_EN).format(DateTimeConstants.FORMAT_DAYOFWEEK_DDDD) != DateTimeConstants.WEEKENDSAT) &&
      (moment().locale(DateTimeConstants.FORMAT_local_EN).format(DateTimeConstants.FORMAT_DAYOFWEEK_DDDD) != DateTimeConstants.WEEKENDSUN)) {
      return moment().format(DateTimeConstants.FORMAT_HOUR_HH) <= 24;
    }
    return true;
  }
  checkRole() {
    return this.user.roles.indexOf(CommonConstants.GroupLead) > 1;
  }
  //Check Cancel if approve or reject can not be cancel and only cancel of yourself
  checkCancel(userID: string, status: string, date: any) {
    var currentDate = moment();
    var otDate = moment(new Date(date)).add(1, DateTimeConstants.DAY);

    return (((status !== StatusConstants.Approved) && (status !== StatusConstants.Rejected)) &&
      (status !== StatusConstants.Cancelled) && this.user.id == userID && otDate.diff(currentDate) > 0)
  }
  //check Approved condition
  checkApproved(status: string, date: any) {
    var currentDate = moment();
    var otDate = moment(new Date(date)).add(1, DateTimeConstants.DAY);
    return ((status !== StatusConstants.Approved) && (status !== StatusConstants.Cancelled) && otDate.diff(currentDate) > 0)
  }
  //check Approved condition
  checkRejected(status: string, date: any) {
    var currentDate = moment();
    var otDate = moment(new Date(date)).add(1, DateTimeConstants.DAY);
    return ((status !== StatusConstants.Rejected) && (status !== StatusConstants.Cancelled) && otDate.diff(currentDate) > 0);
  }
  //Action approve all
  approvedAll() {
    this.listOTRequestID = [];
    this.ListSendMail = [];
    for (let item of this.listcheck) {
      item.OTDate = moment(new Date(item.OTDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
      this.toEmail = [];
      this.listOTRequestID.push(item.ID);
      this.toEmail.push(item.AppUserCreatedBy.Email + "," + item.AppUserCreatedBy.FullName);
      this.ListSendMail.push(new SendMailModelNew(this.group.Name, null, null, null, null, item.OTDate, item.OTDateType.Name, item.OTTimeType.Name, item.StartTime, item.EndTime, null, null, null, null, item.Title, this.user.id, this.toEmail, null, MailConstants.Approve, MailConstants.OTRequest, MailConstants.OTRequestSubject, null, MailConstants.RequestManagement));
      // this.toEmail.push(item.AppUserCreatedBy.Email);
      // this.ListSendMail.push(new SendMailModel(item.Title, this.user.id, this.toEmail,null, MailConstants.Approve, MailConstants.OTRequest, MailConstants.OTRequestSubject, null, MailConstants.RequestManagement));
    }
    this._dataService.post('/api/otrequest/changeStatusMulti?action=' + StatusConstants.Approved, JSON.stringify(this.listOTRequestID)).
      subscribe((response: Response) => {
        this.listcheck = [];
        this._notificationService.printSuccessMessage(MessageConstants.APPROVED_SUCCES_MSG);
        // this._dataService.post('/api/System/sendMailMulti', JSON.stringify(this.ListSendMail)).subscribe((response: any) => { });
        this._dataService.post('/api/System/sendMailMultiFix', JSON.stringify(this.ListSendMail)).subscribe((response: any) => { });
        this.resetEmail();
        this.checkApprove = false;
        this.loadData();
      }, error => this._dataService.handleError(error));
  }
  //click reject button
  rejectedAll() {
    this._notificationService.printPromptDialog("", MessageConstants.TEXTAREA_COMMENT_OTREQUEST_MSG, (value: string) => this.rejectedAllOTRequestConfirm(value));
  }
  //reject confirm
  rejectedAllOTRequestConfirm(value: any) {
    this.listOTRequestID = [];
    this.listCreateBy = [];
    this.ListSendMail = [];
    if (value.trim() != '') {
      for (let item of this.listcheck) {
        item.OTDate = moment(new Date(item.OTDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
        this.toEmail = [];
        this.listOTRequestID.push(item.ID);
        this.toEmail.push(item.AppUserCreatedBy.Email + "," + item.AppUserCreatedBy.FullName);
        var otrequests = new OTRequest(item.Title, item.CreateBy);
        this.listCreateBy.push(otrequests);
        this.ListSendMail.push(new SendMailModelNew(this.group.Name, null, null, null, null, item.OTDate, item.OTDateType.Name, item.OTTimeType.Name, item.StartTime, item.EndTime, null, null, null, null, item.Title, this.user.id, this.toEmail, null, MailConstants.Reject, MailConstants.OTRequest, MailConstants.OTRequestSubject, value, MailConstants.RequestManagement));
        // this.ListSendMail.push(new SendMailModel(item.Title, this.user.id, this.toEmail,null, MailConstants.Reject, MailConstants.Request, MailConstants.OTRequestSubject, value, MailConstants.RequestManagement));
      }//push list OT Request ID to change status and list title createby to send notification
      if (value.trim() != '') {
        this._dataService.post('/api/otrequest/changeStatusMulti?action=' + StatusConstants.Rejected, JSON.stringify(this.listOTRequestID)).
          subscribe((response: Response) => {
            this.listcheck = [];
            this._notificationService.printSuccessMessage(MessageConstants.Rejected_SUCCES_MSG);
            this._dataService.post('/api/System/sendMailMultiFix', JSON.stringify(this.ListSendMail)).subscribe((response: any) => { });
            // this._dataService.post('/api/System/sendMailMulti', JSON.stringify(this.ListSendMail)).subscribe((response: any) => { });
            this.resetEmail();
            this.checkReject = false;
            this.loadData();
          }, error => this._dataService.handleError(error));//change status
      } else {
        this._notificationService.printErrorMessage(MessageConstants.VALID_COMMENT_REJECT_MSG);
      }
    }
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
  }
  //Check ApproveAll condition
  checkApprovedAll() {
    if (this.listcheck != null) {
      var list = [];
      var list = this.listcheck.filter(function (data) {
        var currentDate = moment();
        var otDate = moment(new Date(data.OTDate)).add(1, DateTimeConstants.DAY);
        if (data.StatusRequest.Name !== CommonConstants.Cancelled && data.StatusRequest.Name !== CommonConstants.Approved) {
          if (otDate.diff(currentDate) > 0) {
            return data;
          }
        }
      });
      if (list.length == this.listcheck.length) {
        this.checkApprove = true;
      }
      if (list.length != this.listcheck.length) {
        this.checkApprove = false;
      }
      if (list.length == 0) {
        this.checkApprove = false;
      }
    }
  }
  //Check Reject All condition
  checkRejectedAll() {
    if (this.listcheck != null) {
      var list = [];
      var list = this.listcheck.filter(function (data) {
        var currentDate = moment();
        var otDate = moment(new Date(data.OTDate)).add(1, DateTimeConstants.DAY);
        if (data.StatusRequest.Name != CommonConstants.Cancelled && data.StatusRequest.Name != CommonConstants.Rejected) {
          if (otDate.diff(currentDate) > 0) {
            return data;
          }
        }
      });
      if (list.length == this.listcheck.length) {
        this.checkReject = true;
      }
      if (list.length != this.listcheck.length) {
        this.checkReject = false;
      }
      if (list.length == 0) {
        this.checkReject = false;
      }
    }
  }
  clearText(event: any, dropdownSearch: any) {
    dropdownSearch.clearSearch(event);
  };
  // method reset email
  resetEmail() {
    this.toEmail = [];
    this.ListSendMail = [];
  }
  //remove item in array
  removeItemArray(item: any, array: any): any {
    //var index = array.indexOf(item);
    var index = this.findWithAttr(array, "ID", item.ID);
    if (index > -1) {
      array.splice(index, 1);
    }
    return array;
  }
  removeItemArray2(item: any, array: any): any {
    var index = array.indexOf(item);
    //var index = this.findWithAttr(array, "ID", item.ID);
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
    for (let item of this.otRequest) {
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

  loadFullNameCC() {
    this._dataService.get('/api/appUser/getallappuser?userlogin=' + this.user.id + '&groupId=' + this.user.groupId)
      .subscribe((response: any[]) => {
        this.FullNameOptionCC = [];
        for (let user of response) {
          // this.FullNameOptionCC.push({ id: user, name: user.FullName });
          this.FullNameOptionCC.push({ id: user, name: user.FullName + " ( " + user.UserName + " ) "});
        }
      }, error => this._dataService.handleError(error));
  }

  public checkEditPendding(entity: any): boolean {
    let j = (moment().startOf('day'));
    let i = moment(new Date(entity.CreatedDate)).startOf('day').add(2, DateTimeConstants.DAY)
    // let diffInMs: number = (Date.parse(moment()) - Date.parse(entity.CreatedDate));
    // let date :number = (diffInMs / 1000 / 60 / 60);
    // let abDate = moment(new Date(abnormaldate)).add(DateTimeConstants.LimitTimeApproveRejectRequest, DateTimeConstants.HOURS);
    if ((entity.StatusRequest.Name == CommonConstants.Approved || entity.StatusRequest.Name == CommonConstants.Rejected || entity.StatusRequest.Name == CommonConstants.Cancelled) || j > i) {
      return true;
    }
    else {
      return false;
    }
  }
  //#endregion
  // loadFullNameGeneralAdmin() {
  //   this._dataService.get('/api/appUser/getAll')
  //     .subscribe((response: any[]) => {
  //       this.FullNameGeneralAdmin = [];
  //       for (let user of response) {
  //         if (this.user.fullName != user)
  //           this.FullNameGeneralAdmin.push({ id: user, name: user });
  //       }
  //     }, error => this._dataService.handleError(error));
  // }

  loadFullNameGeneralAdmin() {
    this._dataService.get('/api/appUser/getuserbygroup')
      .subscribe((response: any[]) => {
        this.FullNameGeneralAdmin = [];
        for (let creator of response) {
          if (this.user.fullName != creator)
          this.FullNameGeneralAdmin.push({ id: creator.Id, name: creator.FullName + " ( " + creator.UserName + " ) " });
        }
      }, error => this._dataService.handleError(error));
  }

}
