import { Component, OnInit, ViewChild, Pipe, group } from '@angular/core';
import { NgForm } from '@angular/forms';
import { DataService } from '../../core/services/data.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { NotificationService } from '../../core/services/notification.service';
import { UploadService } from '../../core/services/upload.service';
import { AuthenService } from '../../core/services/authen.service';
import { UtilityService } from '../../core/services/utility.service';
import { MessageConstants } from '../../core/common/message.constants';
import { CommonConstants } from '../../core/common/common.constants';
import { SystemConstants } from '../../core/common/system.constants';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { DaterangepickerConfig } from '../../core/services/config.service';
import { PageConstants } from 'app/core/common/page.constans';
import { DefaultColunmConstants } from 'app/core/common/defaultColunm.constant';
import { FilterUser } from 'app/core/models/filterUser';
import { LoggedInUser } from 'app/core/domain/loggedin.user';
import { FunctionConstants } from '../../core/common/function.constants';
import { UserModel } from '../../core/models/userModel';
import { error, isNullOrUndefined } from 'util';
import { ChildcareLeave } from 'app/core/models/ChildcareLeave';
import { SendMailModel } from 'app/core/models/sendMailModel';
import { MailConstants } from '../../core/common/mail.constants';
import { DateTimeConstants } from 'app/core/common/datetime.constants';
import { DatePickerComponent } from 'ngx-bootstrap/datepicker/datepicker.component';
import { Hero } from 'app/core/models/dashboardModel';
import { PassDataService } from 'app/core/services/passData.service';
import { UserOnsiteInfo } from '../../core/models/UserOnsiteInfo';

@Component({
  selector: 'app-user',
  templateUrl: './user.component.html',
  styleUrls: ['./user.component.css']
})
export class UserComponent implements OnInit {
  //#region "Decalre variable, innit some value and so on"
  @ViewChild('modalAddEdit') public modalAddEdit: ModalDirective;
  @ViewChild('modalChildcareLeave') public modalChildcareLeave: ModalDirective;
  @ViewChild('modalManageOnsite') public modalManageOnsite: ModalDirective;
  @ViewChild('modalResign') public modalResign: ModalDirective;
  @ViewChild('avatar') avatar;
  @ViewChild('modalAddConfirm') public modalAddConfirm: ModalDirective;
  @ViewChild('addEditForm') public addEditForm: NgForm;
  @ViewChild('ManageOnsiteForm') public ManageOnsiteForm: NgForm;
  @ViewChild('ResignForm') public ResignForm: NgForm;
  public myRoles: string[] = [];
  public pickerChild: DatePickerComponent;
  public GroupModel: string[] = [];
  public GenderModel: string[] = [];
  pagingSize: number = PageConstants.pagingSize;
  public pageIndex: number = PageConstants.pageIndex;
  public pageSize: number = PageConstants.pageSize;
  public totalRow: number;
  public pageIndexOnsite: number = PageConstants.pageIndex;
  public pageSizeOnsite: number = PageConstants.pagingSizeOnsite;
  public totalRowOnsite: number;
  public currentMaxEntries: number;
  public currentMaxEntriesOnsite: number;
  public filter: string = '';
  public users: any[];
  public currentUser: LoggedInUser;
  public entity: UserModel;
  public ChildcareLeave: ChildcareLeave;
  public userSelectedToUpdate: any;
  public resignDate: any;
  fingerUserId: any;
  public IsChildcareLeave: string;
  public ChildcareLeaveTime: string;
  public baseFolder: string = SystemConstants.BASE_API;
  public allRoles: IMultiSelectOption[] = [];
  public groupOption: IMultiSelectOption[] = [];
  public StatusOption: IMultiSelectOption[] = [];
  public genderOption: IMultiSelectOption[] = [];
  public roles: any[];
  public UserOnsiteInfo: UserOnsiteInfo = new UserOnsiteInfo();
  public listUserOnsiteInfo: UserOnsiteInfo[];
  public UserOnsiteInfoInTime: UserOnsiteInfo = new UserOnsiteInfo();
  isDesc: boolean = true;
  isEdit: boolean = false;
  isDetail: boolean = false;
  isCreate: boolean = false;
  isSA: boolean = false;
  isDisable: boolean = false;
  isAddOnsite = true;
  loadsuccess: boolean = false;
  column: string = DefaultColunmConstants.UserNameColunm;
  direction: number;
  public radioValue: any;
  //Tùng Code
  DashBoard: Hero;
  public value: any = {
    end: '',
    start: '',
  };
  public value2: any = {
    end: '',
    start: '',
  };
  public valueDate: any = {
    end: '',
    start: '',
  };
  public valueStartdate: any = {
    end: '',
    start: '',
  };

  public chosenOnsiteDate: any = {
    start: moment(),
    end: moment(),
  };
  public listcheck: Array<any> = new Array<any>();
  public checkButton: boolean = false;
  public listUserID: Array<any> = new Array<any>();
  //send mail
  public toEmail: any = [];
  public SendMailModel: SendMailModel;
  //Filter opption
  public FullNameOption: IMultiSelectOption[] = [];
  public FullNameFilter: string[] = [];
  public GroupOption: IMultiSelectOption[] = [];
  public GroupFilter: string[] = [];
  public ActiveFilter: boolean[] = [];
  public ListFilter: FilterUser = null;
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

  FullNameDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectFullName,
    checked: CommonConstants.CheckFullName,
    checkedPlural: CommonConstants.CheckFullName,
    defaultTitle: CommonConstants.FullName,
  };
  // Text configuration Dropdown group 
  GroupDropDownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectGroupType,
    checked: CommonConstants.CheckGroupType,
    checkedPlural: CommonConstants.CheckGroupType,
    defaultTitle: CommonConstants.GroupTitle,
  };
  ActiveDropDownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectActionType,
    checked: CommonConstants.CheckActionType,
    checkedPlural: CommonConstants.CheckActionType,
    defaultTitle: CommonConstants.ActionTitle,
  };
  GroupDropDownSettings: IMultiSelectSettings = {
    enableSearch: true,
    autoUnselect: true,
    closeOnSelect: true,
    selectionLimit: 1,
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 1,
    closeOnClickOutside: true,
    maxHeight: '200px',
  };
  RoleDropDownTexts: IMultiSelectTexts = {
    defaultTitle: CommonConstants.RoleTitle,
  };
  RoleDropDownSettings: IMultiSelectSettings = {
    autoUnselect: true,
    closeOnSelect: true,
    selectionLimit: 1,
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 1,
    closeOnClickOutside: true,
    maxHeight: '200px',
  };
  GenderDropDownTexts: IMultiSelectTexts = {
    defaultTitle: CommonConstants.GenderTitle,
  };
  GenderDropDownSettings: IMultiSelectSettings = {
    autoUnselect: true,
    closeOnSelect: true,
    selectionLimit: 1,
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 1,
    closeOnClickOutside: true,
    maxHeight: '200px',
  };
  public daterangeOptions: any = {
    locale: DateTimeConstants.Locale,
    autoUpdateInput: true,
    alwaysShowCalendars: false,
    singleDatePicker: true,
    showDropdowns: true,
    endDate: null,
    startDate: moment(),
    minDate: null,
    maxDate: null,
  };
  public daterangeOptionsBirthday: any = {
    locale: DateTimeConstants.Locale,
    autoUpdateInput: true,
    alwaysShowCalendars: false,
    singleDatePicker: true,
    showDropdowns: true,
    maxDate: moment(),
    endDate: moment(),
    startDate: moment(),
  };
  public datePickerChildOptions: any = {
    locale: DateTimeConstants.Locale,
    autoUpdateInput: true,
    alwaysShowCalendars: false,
    "opens": "right",
    ranges: DateTimeConstants.RangeChildcareLeave,
    singleDatePicker: false,
    minDate: moment(),
    maxDate: null,
  };
  public daterangepickerOptionsChildcareLeave: any = {
    autoUpdateInput: true,
    locale: DateTimeConstants.Locale,
    alwaysShowCalendars: false,
    singleDatePicker: false,
    showDropdowns: true,
    maxDate: null,
    minDate: moment(),
    endDate: moment().subtract(-365, DateTimeConstants.DAY),
    startDate: moment(),
  };
  public daterangepicker = this.daterangepickerOptions;
  dataOld: UserModel;
  //#endregion

  //Contructor to inject services.
  constructor(private _dataService: DataService,
    private _notificationService: NotificationService,
    private _utilityService: UtilityService,
    private _uploadService: UploadService,
    public _authenService: AuthenService,
    private daterangepickerOptions: DaterangepickerConfig,
    private _passDataService: PassDataService) {
    if (_authenService.getLoggedInUser().groupId.length == 0) {
      _utilityService.navigateToMain();
    }
    this.currentUser = this._authenService.getLoggedInUser();
    if (_authenService.checkAccess(CommonConstants.USER) == false) {
      _utilityService.navigateToLogin();
    }
    if (!_authenService.hasPermission(FunctionConstants.User, FunctionConstants.Read)) {
      _utilityService.navigateToMain();
    }
    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      autoUpdateInput: true,
      alwaysShowCalendars: false,
      singleDatePicker: true,
      showDropdowns: true,
      maxDate: moment(),
      endDate: moment(),
      startDate: moment(),
    };

  }

  // Innit data or function
  ngOnInit() {
    this.DashBoard = this._passDataService.dashboard;
    if (this.DashBoard == undefined || this.DashBoard.clickFromDashBoard == false) {
      this.loadData();
    }
    // if(this.DashBoard != undefined)
    else {
      if (this.DashBoard.clickFromDashBoard == "active") {
        this.loadDataActive();
        this.DashBoard.clickFromDashBoard = false;
      }
      if (this.DashBoard.clickFromDashBoard == "inactive") {
        this.loadDataInActive();
        this.DashBoard.clickFromDashBoard = false;
      }
      if (this.DashBoard.clickFromDashBoard == "onsite") {
        this.loadDataOnSite();
        this.DashBoard.clickFromDashBoard = false;
      }
    }

    this.radioValue = CommonConstants.StringEmpty;
    this.isSA = this.checkRoleSA();
    this.InitGender();
    this.loadRoles();
    // this.loadData();
    this.loadGroup();
    this.loadFullName();
    this.loadGroups();
    this.loadStatus();
  }

  //#region "Innit data"
  InitGender() {
    this.genderOption = [];
    this.genderOption.push({ id: CommonConstants.StringTrue, name: CommonConstants.Male });
    this.genderOption.push({ id: CommonConstants.StringFalse, name: CommonConstants.Female });
  }
  InitDefaultValue() {
    this.myRoles = [];
    this.GenderModel = [];
    this.GroupModel = [];
  }

  loadData() {
    this._dataService.post('/api/appUser/getlistpaging?page=' + this.pageIndex + '&pageSize=' + this.pageSize +
      '&column=' + this.column + '&isDesc=' + this.isDesc, JSON.stringify(this.ListFilter))
      .subscribe((response: any) => {
        this.users = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.checkCheckBox();
        this.loadsuccess = true;
        this.calShowingPage();
      });
  }

  loadDataActive() {
    this.ListFilter = new FilterUser(this.FullNameFilter, this.GroupFilter, this.ActiveFilter = [true]);
    this._dataService.post('/api/appUser/getuseractive?page=' + this.pageIndex + '&pageSize=' + this.pageSize +
      '&column=' + this.column + '&isDesc=' + this.isDesc, JSON.stringify(this.ListFilter))
      .subscribe((response: any) => {
        this.users = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.checkCheckBox();
        this.loadsuccess = true;
        this.calShowingPage();
      });
  }
  loadDataInActive() {
    this.ListFilter = new FilterUser(this.FullNameFilter, this.GroupFilter, this.ActiveFilter = [false]);
    this._dataService.post('/api/appUser/getuserinactive?page=' + this.pageIndex + '&pageSize=' + this.pageSize +
      '&column=' + this.column + '&isDesc=' + this.isDesc, JSON.stringify(this.ListFilter))
      .subscribe((response: any) => {
        this.users = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.checkCheckBox();
        this.loadsuccess = true;
        this.calShowingPage();
      });
  }
  loadDataOnSite() {
    this._dataService.post('/api/appUser/getuseronsite?page=' + this.pageIndex + '&pageSize=' + this.pageSize +
      '&column=' + this.column + '&isDesc=' + this.isDesc, JSON.stringify(this.ListFilter))
      .subscribe((response: any) => {
        this.users = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.checkCheckBox();
        this.loadsuccess = true;
        this.calShowingPage();
      });
  }

  loadUserOnsiteInfo(id: any) {
    this.loadsuccess = false;
    this._dataService.get('/api/user-onsite/getinfo?userID=' + id + '&page='
      + this.pageIndexOnsite + '&pageSize=' + this.pageSizeOnsite)
      .subscribe((response: any) => {
        this.listUserOnsiteInfo = response.Items;
        this.pageIndexOnsite = response.PageIndex;
        this.pageSizeOnsite = response.PageSize;
        this.totalRowOnsite = response.TotalRows;
        this.loadsuccess = true;
        this.calShowingPageOnsite();
      }, error => {
        this.loadsuccess = true;
        this._dataService.handleError(error);
      });
  }

  loadDetailUserOnsiteInfoInTime(id: any) {
    this._dataService.get('/api/user-onsite/getinfointime?userID=' + id)
      .subscribe((response: any) => {
        this.UserOnsiteInfoInTime = response;
      }, error => {
        this._dataService.handleError(error);
      });
  }


  loadGroups() {
    this._dataService.get('/api/group/getallgroup')
      .subscribe((response: any) => {
        this.groupOption = [];
        for (let group of response) {
          this.groupOption.push({ id: group.ID, name: group.Name });
        }
      });
  }
  loadStatus() {
    this._dataService.get('/api/appUser/getallstatus')
      .subscribe((response: any) => {
        this.StatusOption = [];
        for (let status of response) {
          this.StatusOption.push({ id: status.Status, name: status.Status });
          this.StatusOption = [
            { id: true, name: 'Active' },
            { id: false, name: 'InActive' }
          ]
        }
      });
  }

  loadRoles() {
    this._dataService.get('/api/appRole/getlistall').subscribe((response: any[]) => {
      this.allRoles = [];
      for (let role of response) {
        this.allRoles.push({ id: role.Name, name: role.Description });
      }
    }, error => this._dataService.handleError(error));
  }

  loadUserDetail(id: any) {
    this._dataService.get('/api/appUser/detail/' + id)
      .subscribe((response: any) => {
        this.entity = response;
        console.log(this.entity);
        
        this.IsChildcareLeave = response.ChildcareLeaveID == null ? CommonConstants.No : CommonConstants.Yes;
        this.chosenDate.start = response.StartWorkingDay;
        this.chosenbirthday.start = response.BirthDay;
        this.myRoles = [];
        for (let role of this.entity.Roles) {
          this.myRoles.push(role);
        }
        this.entity.BirthDay = moment(new Date(this.entity.BirthDay)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
        this.loadFingerUserID(response.Id);
        this.daterangeOptionsBirthday = {
          locale: DateTimeConstants.Locale,
          alwaysShowCalendars: false,
          singleDatePicker: true,
          showDropdowns: true,
          autoUpdateInput: true,
          maxDate: moment(),
          endDate: moment(this.chosenbirthday.start),
          startDate: moment(this.chosenbirthday.start),
        };
        this.daterangeOptions = {
          locale: DateTimeConstants.Locale,
          alwaysShowCalendars: false,
          singleDatePicker: true,
          showDropdowns: true,
          autoUpdateInput: true,
          maxDate: null,
          endDate: moment(this.chosenDate.start),
          startDate: moment(this.chosenDate.start),
        };
      });
  }
  loadChildcareLeaveDetail(id: any) {
    this._dataService.get('/api/childcare-leave/detail/' + id)
      .subscribe((response: any) => {
        this.ChildcareLeave = response;
        this.radioValue = this.ChildcareLeave.IsLateComing == true ? CommonConstants.LateComing : CommonConstants.EarlyLeaving;
        this.ChildcareLeaveTime = this.ChildcareLeave.Time;
        this.chosenDateChildcareLeave.start = moment(this.ChildcareLeave.StartDate);
        this.chosenDateChildcareLeave.end = moment(this.ChildcareLeave.EndDate);
        this.IsChildcareLeave = "Yes";
        this.daterangepickerOptionsChildcareLeave = {
          autoUpdateInput: true,
          locale: DateTimeConstants.Locale,
          alwaysShowCalendars: false,
          singleDatePicker: false,
          showDropdowns: true,
          maxDate: null,
          minDate: moment(),
          endDate: moment(this.ChildcareLeave.EndDate),
          startDate: moment(this.ChildcareLeave.StartDate),
        };
      });
  }
  loadGroup() {
    this._dataService.get('/api/group/getallgroup').subscribe((response: any[]) => {
      this.GroupOption = [];
      for (let group of response) {
        this.GroupOption.push({ id: group.ID, name: group.Name });
      }
    }, error => this._dataService.handleError(error));
  }
  loadFullName() {
    this._dataService.get('/api/appUser/getuserbyroleadmin')
      .subscribe((response: any[]) => {
        this.FullNameOption = [];
        for (let user of response) {
          this.FullNameOption.push({ id: user.Id, name: user.FullName + " ( " + user.UserName + " ) " });
        }
      }, error => this._dataService.handleError(error));
  }
  loadFingerUserID(userID: any) {
    this.fingerUserId = null;
    this._dataService.get('/api/appUser/getfingeruseridbyuserid?userId=' + userID).subscribe((response: any) => {
      this.fingerUserId = response.ID;
    });
  }
  //#endregion

  //#region "CRUD"
  saveChange(form: NgForm) {
    if (form.valid) {
      this.entity.Gender = this.GenderModel[0];
      this.entity.GroupId = this.GroupModel[0];
      this.entity.Roles = this.myRoles;
      this.entity.StartWorkingDay = this.chosenDate.start;
      this.entity.BirthDay = this.chosenbirthday.start;
      this.entity.FullName = this.entity.FullName.trim();
      this.entity.Email = this.entity.Email.trim();
      this.entity.UserName = this.entity.UserName.trim();
      this.saveData(form);
    }
  }
  saveChangeOnsite(form: NgForm) {
    this.UserOnsiteInfo.StartDate = moment(new Date(this.chosenOnsiteDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this.UserOnsiteInfo.EndDate = moment(new Date(this.chosenOnsiteDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this.UserOnsiteInfo.UserID = this.entity.Id;
    if (this.isAddOnsite) {
      this._dataService.post('/api/user-onsite/add', JSON.stringify(this.UserOnsiteInfo))
        .subscribe((response: any) => {
          this.isAddOnsite = true;
          form.resetForm();
          // this.selectedOnsiteDatePicker(this.valueDate,this.chosenOnsiteDate)
          this.modalManageOnsite.hide();
          this.pageIndexOnsite = 1;
          this._notificationService.printSuccessMessage(MessageConstants.CREATED_OK_MSG);
        }, error => this._dataService.handleError(error));
    }
    else {
      this._dataService.put('/api/user-onsite/update', JSON.stringify(this.UserOnsiteInfo))
        .subscribe((response: any) => {
          form.resetForm();
          this.modalManageOnsite.hide();
          this._notificationService.printSuccessMessage(MessageConstants.UPDATED_OK_MSG);
          this.isAddOnsite = true;
          this.pageIndexOnsite = 1;
        }, error => this._dataService.handleError(error));
    }
  }
  saveChangeChildcareLeave(form: NgForm) {
    if (this.entity.ChildcareLeaveID == undefined) {
      this.chosenDateChildcareLeave.end = moment(new Date(this.chosenDateChildcareLeave.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      this.chosenDateChildcareLeave.start = moment(new Date(this.chosenDateChildcareLeave.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      this.ChildcareLeave = new ChildcareLeave(0, this.chosenDateChildcareLeave.start, this.chosenDateChildcareLeave.end, false, false, this.ChildcareLeaveTime);
      this.ChildcareLeave.IsLateComing = this.radioValue == CommonConstants.LateComing ? true : false;
      this.ChildcareLeave.IsEarlyLeaving = this.radioValue == CommonConstants.EarlyLeaving ? true : false;
      this._dataService.post('/api/childcare-leave/add?UserID=' + this.entity.Id, JSON.stringify(this.ChildcareLeave))
        .subscribe((response: any) => {
          this.loadUserDetail(this.entity.Id);
          this.ChildcareLeave = response;
          this.chosenDateChildcareLeave.start = moment(new Date(this.ChildcareLeave.StartDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
          this.chosenDateChildcareLeave.end = moment(new Date(this.ChildcareLeave.EndDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
          this.modalChildcareLeave.hide();
          form.resetForm();
          this._notificationService.printSuccessMessage(MessageConstants.CREATED_OK_MSG);
        }, error => this._dataService.handleError(error));
    }
    else {
      this.chosenDateChildcareLeave.end = moment(new Date(this.chosenDateChildcareLeave.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      this.chosenDateChildcareLeave.start = moment(new Date(this.chosenDateChildcareLeave.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      this.ChildcareLeave = new ChildcareLeave(0, this.chosenDateChildcareLeave.start, this.chosenDateChildcareLeave.end, false, false, this.ChildcareLeaveTime);
      this.ChildcareLeave.IsLateComing = this.radioValue == CommonConstants.LateComing ? true : false;
      this.ChildcareLeave.IsEarlyLeaving = this.radioValue == CommonConstants.EarlyLeaving ? true : false;
      this._dataService.put('/api/childcare-leave/update?UserID=' + this.entity.Id, JSON.stringify(this.ChildcareLeave))
        .subscribe((response: any) => {
          this.ChildcareLeave = response;
          this.chosenDateChildcareLeave.start = moment(new Date(this.ChildcareLeave.StartDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
          this.chosenDateChildcareLeave.end = moment(new Date(this.ChildcareLeave.EndDate)).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
          this.loadUserDetail(this.entity.Id);
          this.modalChildcareLeave.hide();
          form.resetForm();
          this._notificationService.printSuccessMessage(MessageConstants.UPDATED_OK_MSG);
        }, error => this._dataService.handleError(error));
    }
  }
  private saveData(form: NgForm) {
    this.isDisable = true;
    this.toEmail.push(this.entity.Email);
    this.SendMailModel = new SendMailModel(MailConstants.UpdateUserTitle, this._authenService.getLoggedInUser().id, this.toEmail, MailConstants.Update, MailConstants.UpdateUser, MailConstants.UpdateUserSubject, null, null);
    if (this.entity.Id == undefined) {
      this.dataOld = this.entity;
      this._dataService.post('/api/appUser/add', JSON.stringify(this.entity))
        .subscribe((response: any) => {
          if (response === 'Inaction') {
            this.modalAddConfirm.show();
            this.isDisable = false;
            return;
          }
          this.modalAddEdit.hide();
          this.loadData();
          form.resetForm();
          this._notificationService.printSuccessMessage(MessageConstants.CREATED_OK_MSG);
          this.loadFullName();
          this.isDisable = false;
        }, error => {
          this.chosenbirthday.start = this.entity.BirthDay;
          this.isDisable = false;
          this._dataService.handleError(error)
        });
    } else {
      this.entity.FullName = this.entity.FullName.trim();
      this.entity.Email = this.entity.Email.trim();
      this.entity.StartWorkingDay = this.chosenDate.start;
      this._dataService.put('/api/appUser/update', JSON.stringify(this.entity))
        .subscribe((response: any) => {
          this.SendMail(this.SendMailModel);
          this.toEmail = [];
          this.loadData();
          this.modalAddEdit.hide();
          form.resetForm();
          this.InitDefaultValue();
          this.updateDateTime();
          this.isDisable = false;
          this._notificationService.printSuccessMessage(MessageConstants.UPDATED_OK_MSG);
        }, error => this._dataService.handleError(error));
    }
  }
  deleteItem(id: any) {
    this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_DELETE_MSG, () => this.deleteItemConfirm(id));
  }
  deleteItemConfirm(id: any) {
    this._dataService.delete('/api/appUser/delete', 'id', id).subscribe((response: Response) => {
      this._notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG);
      this.loadData();
    });
  }
  DeleteOnsiteInfo(ID: any, UserID: any) {
    this._dataService.delete('/api/user-onsite/delete', 'id', ID).subscribe((response: Response) => {
      this._notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG);
      this.loadUserOnsiteInfo(UserID);
    });
  }
  SaveResign(form: NgForm) {
    this._dataService.get('/api/appUser/resign?userId=' + this.entity.Id + '&resignationDate=' + moment(new Date(this.chosenResign.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY))
      .subscribe((response: any) => {
        this.loadData();
        this.modalResign.hide();
        form.reset();
        this.InitDefaultValue();
        this.InitGender();
        this._notificationService.printSuccessMessage(MessageConstants.Resign_Success_MSG);
      }, error => this._dataService.handleError(error));
  }
  //#endregion

  //#region "Poppu or other"
  showAddModal() {
    this.InitDefaultValue();
    this.isCreate = true;
    this.isDetail = false;
    this.entity = new UserModel();
    this.daterangeOptionsBirthday = {
      locale: DateTimeConstants.Locale,
      autoUpdateInput: true,
      alwaysShowCalendars: false,
      singleDatePicker: true,
      showDropdowns: true,
      maxDate: moment(),
      endDate: moment(),
      startDate: moment(),
    };
    this.daterangeOptions = {
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: false,
      singleDatePicker: true,
      showDropdowns: true,
      autoUpdateInput: true,
      maxDate: null,
      endDate: moment(),
      startDate: moment(),
    };
    this.chosenDate.start = '';
    this.chosenbirthday.start = '';
    this.fingerUserId = '';
    //this.InitGender();
    //this.loadGroups();
    this.modalAddEdit.show();
  }
  showEditModal(id: any, user: any, status: boolean) {
    this.loadDetailUserOnsiteInfoInTime(id);
    this.userSelectedToUpdate = user;
    //this.InitGender();
    this.isDetail = status;
    this.isEdit = !status;
    this.isCreate = false;
    this.GroupModel = [];
    this.GenderModel = [];

    //this.loadGroups();
    if (user.GroupId != null) {
      this.GroupModel[0] = user.GroupId;
    } else {
      this.GroupModel = [];
    }
    this.GenderModel[0] = user.Gender;
    this.loadUserDetail(id);
    this.modalAddEdit.show();
  }
  EditOnsiteInfo(onsiteInfo: any, form: any) {
    this.UserOnsiteInfo = onsiteInfo;
    this.isAddOnsite = false;
    this.daterangepickerOption = {
      autoUpdateInput: true,
      "opens": "left",
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: false,
      singleDatePicker: false,
      showDropdowns: true,
      startDate: new Date(this.UserOnsiteInfo.StartDate),
      endDate: new Date(this.UserOnsiteInfo.EndDate),
      maxDate: null,
    }
    this.chosenOnsiteDate.start = new Date(this.UserOnsiteInfo.StartDate);
    this.chosenOnsiteDate.end = new Date(this.UserOnsiteInfo.EndDate);
    this.valueDate.start = new Date(this.UserOnsiteInfo.StartDate);
    this.valueDate.end = new Date(this.UserOnsiteInfo.EndDate);
    this.selectedOnsiteDatePicker(this.valueDate, this.chosenOnsiteDate)
  }
  BackToAdd() {
    this.UserOnsiteInfo = new UserOnsiteInfo();
    this.isAddOnsite = true;
  }
  public daterangepickerOption: any = {
    autoUpdateInput: true,
    "opens": "left",
    locale: DateTimeConstants.Locale,
    alwaysShowCalendars: false,
    singleDatePicker: false,
    showDropdowns: true,
    startDate: moment(),
    endDate: moment(),
    maxDate: null,
  };
  showManageOnsiteModal(id: any) {
    this.loadUserDetail(id);
    this.loadUserOnsiteInfo(id);
    this.daterangepickerOption = {
      autoUpdateInput: true,
      "opens": "left",
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: false,
      singleDatePicker: false,
      showDropdowns: true,
      startDate: moment(),
      endDate: moment(),
      maxDate: null,
    }
    // this.chosenOnsiteDate.start = moment();
    // this.chosenOnsiteDate.end = moment();
    this.valueDate.start = moment();
    this.valueDate.end = moment();
    this.selectedOnsiteDatePicker(this.valueDate, this.chosenOnsiteDate)
    this.modalManageOnsite.show();
  }
  public daterangepickerOptionResign: any = {
    autoUpdateInput: true,
    locale: DateTimeConstants.Locale,
    alwaysShowCalendars: false,
    singleDatePicker: true,
    showDropdowns: true,
    startDate: moment(),
    endDate: '',
    maxDate: null,
    minDate: null
  };
  showResignModal(userID: any) {
    this.chosenResign.start = moment();
    this.daterangepickerOptionResign = {
      autoUpdateInput: true,
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: false,
      singleDatePicker: true,
      showDropdowns: true,
      maxDate: null,
      minDate: null,
      startDate: moment(),
      endDate: '',
    };
    this.loadUserDetail(userID)
    this.value2.start = moment();

    this.selectedDatePickerResign(this.value2, this.chosenResign)
    this.modalResign.show();
  }
  hideModal() {
    this.modalAddEdit.hide();
    this.entity = null;
  }
  hideModalCreate(form: NgForm) {
    this.isDisable = false;
    if (form.dirty) {
      this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_CLOSE_USER_MSG, () => {
        this.modalAddEdit.hide();
        this.entity = null;
        form.reset();
      })
    } else {
      this.modalAddEdit.hide();
      this.entity = null;
    }
  }
  hideModalManageOnsite(form: NgForm) {
    if (form.dirty) {
      this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_CLOSE_USER_MSG, () => {
        form.reset();
        this.modalManageOnsite.hide();
        this.isAddOnsite = true;
      })
    } else {
      this.modalManageOnsite.hide();
      this.isAddOnsite = true;
    }
  }
  hideModalResign(form: NgForm) {
    if (form.dirty) {
      this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_CLOSE_USER_MSG, () => {
        form.reset();
        this.modalResign.hide();
      })
    } else {
      this.modalResign.hide();
    }
  }

  hideModalAddConfirm(form: NgForm) {
    this.modalAddConfirm.hide();
  }
  //#endregion

  //#region "Business logic"
  resetAdd() {

    this.entity = new UserModel;
    this.GenderModel = [];
    this.myRoles = [];
    this.GroupModel = [];
    this.fingerUserId = null;
    this.chosenbirthday.start = null;
    this.chosenbirthday.end = '';
    this.chosenDate.start = '';
    this.chosenDate.end = '';
    this.daterangeOptions = {
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: false,
      singleDatePicker: true,
      showDropdowns: true,
      autoUpdateInput: true,
      maxDate: null,
      startDate: moment(),
      endDate: null,
    };
    this.daterangeOptionsBirthday = {
      locale: DateTimeConstants.Locale,
      autoUpdateInput: true,
      alwaysShowCalendars: false,
      singleDatePicker: true,
      showDropdowns: true,
      maxDate: moment(),
      startDate: moment(),
      endDate: null,
    };
    // this.value.start = moment();
    // this.value.end = moment();
    // this.selectedDatePicker(this.value, this.chosenbirthday);
    // this.valueStartdate.start = moment();
    // this.valueStartdate.end = moment();
    // this.selectedDatePickerStartdate(this.valueStartdate,this.chosenDate)

  }
  resetChildCare(entity: any) {
    if (this.entity.ChildcareLeaveID != null) {
      this.loadChildcareLeaveDetail(entity.ChildcareLeaveID);
    } else {
      this.InitChildcareLeaveValues();
    }
    
  }

  reset(id: any) {
    this.loadUserDetail(id);
    this.GenderModel[0] = this.entity.Gender;
    if (this.userSelectedToUpdate.GroupId != null) {
      this.GroupModel[0] = this.userSelectedToUpdate.GroupId;
    } else {
      this.GroupModel = [];
    }
  }

  sort(property) {
    this.isDesc = !this.isDesc; //change the direction    
    this.column = property;
    let direction = this.isDesc ? 1 : -1;
    this.pageIndex = 1;
    this.loadData();
  };

  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.loadsuccess = false;
    this.loadData();
  }
  pageChangedOnsite(event: any): void {
    this.pageIndexOnsite = event.page;
    this.loadsuccess = false;
    this.loadUserOnsiteInfo(this.entity.Id);
  }
  public selectGender(event) {
    this.entity.Gender = event.target.value
  }

  public selectedDate(value: any) {
    this.entity.BirthDay = moment(value.end._d).format(DateTimeConstants.FORMAT_DATE_DDMMYYYY);
  }

  public checkAll(ev, checked: any) {
    // this.getCheckCondition().forEach(x => x.Checked = ev.target.checked)
    // if (!checked) {
    //   this.listcheck = [];
    //   for (let item of this.getCheckCondition()) {
    //     this.listcheck.push(item);
    //   }
    // }
    // if (checked) {
    //   this.listcheck = [];
    // }
    this.users.forEach(x => x.Checked = ev.target.checked);
    if (ev.target.checked) {
      for (let item of this.users) {
        if (!this.IsArrayIncludeItem(this.listcheck, item)) {
          this.listcheck.push(item);
        }
      }
    } else {
      for (let item of this.users) {
        if (this.IsArrayIncludeItem(this.listcheck, item)) {
          this.listcheck = this.removeItemArray(item, this.listcheck);
        }
      }
    }
    this.checkChangeStatusCondition();
  }
  public isAllChecked() {
    return this.users.every(_ => _.Checked);
  }
  //reset Filter
  resetFilter() {
    this.GroupFilter = [];
    this.FullNameFilter = [];
    this.ActiveFilter = [];
    this.ListFilter = null;
    this.pageIndex = 1;
    this.listcheck = [];
    this.checkButton = false;
    this.loadData();
  }
  //reset in create
  resetForm(form: NgForm) {
    form.resetForm();
  }
  //filter action
  filterUser() {
    this.ListFilter = new FilterUser(this.FullNameFilter, this.GroupFilter, this.ActiveFilter);
    this.pageIndex = 1;
    this.listcheck = [];
    this.checkButton = false;
    this.loadData();
  }
  getCheckCondition(): any {
    if (this.users != null) {
      var list = this.users.filter(item => item.Id !== this.currentUser.id);
      return list;
    }
  }
  checkChangeStatus(userId: any) {
    return userId == this.currentUser.id;
  }
  checkChangeStatusCondition() {
    if (this.listcheck != null) {
      var list = [];
      list = this.listcheck.filter(item => item.Id !== this.currentUser.id);
      if (list.length == this.listcheck.length) {
        this.checkButton = true;
      }
      if (list.length != this.listcheck.length) {
        this.checkButton = false;
      }
      if (list.length == 0) {
        this.checkButton = false;
      }
    }
  }
  //
  checkOnChange(value: any, checked: any) {
    if (this.listcheck != null) {
      if (checked == true) {
        this.listcheck.push(value);
      }
      if (checked == false) {
        this.listcheck = this.removeItemArray(value, this.listcheck);
      }
      this.checkChangeStatusCondition();
    }
  }
  changeStatusMulti() {
    this.listUserID = [];
    for (let item of this.listcheck) {
      this.listUserID.push(item.Id);
    }
    this._dataService.post('/api/appUser/changestatusMulti', JSON.stringify(this.listUserID)).subscribe((response: Response) => {
      this._notificationService.printSuccessMessage(MessageConstants.CHANGESTATUS_OK_MSG);
      this.checkButton = false;
      this.listcheck = [];
      this.loadData();
    });
  }
  changeStatus(userId: any) {
    this._dataService.post('/api/appUser/changestatus?userId=' + userId).subscribe((response: Response) => {
      this._notificationService.printSuccessMessage(MessageConstants.CHANGESTATUS_OK_MSG);
      this.loadData();
      this.hideModal();
      this.listcheck = [];
    });
  }
  editForm() {
    this.isDetail = false;
    this.isEdit = true;
  }
  closeEditForm() {
    this.isDetail = true;
  }
  //calculate page
  calShowingPage() {
    this.currentMaxEntries = this.pageIndex * this.pageSize;
    if (this.currentMaxEntries >= this.totalRow) {
      this.currentMaxEntries = this.totalRow;
    }
  }
  calShowingPageOnsite() {
    this.currentMaxEntriesOnsite = this.pageIndexOnsite * this.pageSizeOnsite;
    if (this.currentMaxEntriesOnsite >= this.totalRowOnsite) {
      this.currentMaxEntriesOnsite = this.totalRowOnsite;
    }
  }
  ShowPopupChildcareLeave(entity: any) {
    if (this.entity.ChildcareLeaveID != null) {
      this.loadChildcareLeaveDetail(entity.ChildcareLeaveID);
    } else {
      this.InitChildcareLeaveValues();
      this.daterangepickerOptionsChildcareLeave = {
        autoUpdateInput: true,
        locale: DateTimeConstants.Locale,
        alwaysShowCalendars: false,
        singleDatePicker: false,
        showDropdowns: true,
        maxDate: null,
        minDate: moment(),
        endDate: moment().subtract(-365, DateTimeConstants.DAY),
        startDate: moment(),
      };
    }
    this.modalChildcareLeave.show();
  }
  deleteChildcareLeave(user: UserModel) {
    this._dataService.delete('/api/childcare-leave/delete', 'id', user.ChildcareLeaveID).subscribe((response: Response) => {
      this.modalChildcareLeave.hide();
      this.loadUserDetail(user.Id);
      this._notificationService.printSuccessMessage(MessageConstants.REMOVE_OK_MSG);
    });
  }
  SendMail(mailModel: SendMailModel) {
    this._dataService.post('/api/System/sendMail', JSON.stringify(mailModel)).subscribe((response: any) => { })
  }
  InitChildcareLeaveValues() {
    this.ChildcareLeaveTime = "";
    this.radioValue = "";
    this.chosenDateChildcareLeave.end = moment().subtract(-365, DateTimeConstants.DAY);
    this.chosenDateChildcareLeave.start = moment().subtract(0, DateTimeConstants.DAY);
    this.daterangepickerOptionsChildcareLeave = {
      autoUpdateInput: true,
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: false,
      singleDatePicker: false,
      showDropdowns: true,
      maxDate: null,
      minDate: moment(),
      endDate: moment().subtract(-365, DateTimeConstants.DAY),
      startDate: moment(),
    };
  }
  clearText(event: any, dropdownSearch: any) {
    dropdownSearch.clearSearch(event);
  };
  //Date Picker
  public chosenDate: any = {
    start: moment().subtract(3, DateTimeConstants.DAY),
    end: moment(),
  };
  public chosenDateChildcareLeave: any = {
    start: moment(),
    end: moment().subtract(-365, DateTimeConstants.DAY),
  };
  public chosenbirthday: any = {
    start: moment(),
    end: '',
  }
  public chosenResign: any = {
    start: moment(),
    end: '',
  }
  public selectedDatePicker(value: any) {
    this.chosenbirthday.start = value.end;
  }
  public selectedDatePickerResign(value: any, dateInput: any) {
    this.value2 = value;
    dateInput.start = value.start.format();
    dateInput.end = value.end;
  }
  public selectedDatePickerStartdate(value: any, dateInput: any) {
    dateInput.start = value.start.format();
    dateInput.end = value.end;
  }
  public selectedDateRangePicker(value: any, dateInput: any) {
    this.value = value;
    dateInput.start = value.start.format();
    dateInput.end = value.end;
  }
  public selectedDateChildcareLeave(value: any) {
    this.chosenDateChildcareLeave.start = value.start.format();
    this.chosenDateChildcareLeave.end = value.end;
  }
  public selectedOnsiteDatePicker(value: any, dateInput: any) {
    this.valueDate = value;
    dateInput.start = value.start.format();
    dateInput.end = value.end.format();
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
  public hiddenModalChild() {
    this.modalChildcareLeave.hide()
    this.updateDateTime();
  }
  public updateDateTime() {
    this.daterangeOptions.settings = {
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: false,
      singleDatePicker: true,
      showDropdowns: true,
      autoUpdateInput: true,
      maxDate: null,
      endDate: moment(this.chosenDate.start),
      startDate: null,
    };

    this.valueStartdate.start = moment(this.chosenDate.start);
    this.valueStartdate.end = moment(this.chosenDate.start);
    this.selectedDatePicker(this.valueStartdate); // da sưa
    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: false,
      singleDatePicker: true,
      showDropdowns: true,
      autoUpdateInput: true,
      maxDate: moment(),
      endDate: moment(this.chosenbirthday.start),
      startDate: moment(this.chosenbirthday.start),
    };
    this.value.start = moment(this.chosenbirthday.start);
    this.value.end = moment(this.chosenbirthday.start);
    this.selectedDatePickerStartdate(this.value, this.chosenbirthday);
  }
  //remove item in array
  removeItemArray(item: any, array: any): any {
    //var index = array.indexOf(item);
    var index = this.findWithAttr(array, "Id", item.Id);
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
    for (let item of this.users) {
      for (let itemCheck of this.listcheck) {
        if (item.Id === itemCheck.Id)
          item.Checked = true;
      }
    }
  }
  IsArrayIncludeItem(array: any, item: any): boolean {
    for (let value of this.listcheck) {
      if (value.Id === item.Id) {
        return true;
      }
    }
    return false;
  }
  checkRoleSA() {
    if (this._authenService.getLoggedInUser().roles === '["SuperAdmin"]') {
      return true;
    } else {
      return false;
    }
  }
  SaveAddConfirm(form: NgForm) {
    this.isDisable = false;
    this.modalAddConfirm.hide();
    this._dataService.delete('/api/appUser/updateuserold', 'id', this.dataOld.Email)
      .subscribe((response: any) => {
        this._dataService.post('/api/appUser/add', JSON.stringify(this.dataOld))
          .subscribe((response: any) => {
            this.updateDateTime()
            this.loadData();
            this.modalAddEdit.hide();
            this.chosenDate.start = moment();
            this.chosenbirthday.start = moment();
            this._notificationService.printSuccessMessage(MessageConstants.CREATED_OK_MSG);
            this.loadFullName();
            this.isDisable = false;
            form.resetForm();
          }, error => {
            this.chosenbirthday.start = this.entity.BirthDay;
            this.isDisable = false;
            this._dataService.handleError(error)
          });
      }, error => this._dataService.handleError(error));
  }
  //#endregion
}
