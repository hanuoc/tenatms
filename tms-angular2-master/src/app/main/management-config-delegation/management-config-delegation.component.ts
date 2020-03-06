import { Component, OnInit, ViewChild } from '@angular/core';
import { IMultiSelectTexts, IMultiSelectOption, IMultiSelectSettings } from 'angular-2-dropdown-multiselect';
import { CommonConstants } from 'app/core/common/common.constants';
import { AuthenService } from '../../core/services/authen.service';
import { MessageConstants } from '../../core/common/message.constants';
import { DataService } from '../../core/services/data.service';
import { NotificationService } from '../../core/services/notification.service';
import { UtilityService } from '../../core/services/utility.service';
import { LoggedInUser } from 'app/core/domain/loggedin.user';
import { DefaultColunmConstants } from '../../core/common/defaultColunm.constant';
import { PageConstants } from 'app/core/common/page.constans';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DateTimeConstants } from 'app/core/common/datetime.constants';
import { ConfigDelegation } from '../../core/models/delegationModel';
import { SendMailModel } from '../../core/models/sendMailModel';
import { SendMailDelegateRequest } from '../../core/models/SendMailDelegateRequest';
import { MailConstants } from '../../core/common/mail.constants';
import { SendMailDelegateModel } from '../../core/models/SendMailDelegateModel';
import { DaterangepickerConfig } from 'app/core/services/config.service';
@Component({
  selector: 'app-management-config-delegation',
  templateUrl: './management-config-delegation.component.html',
  styleUrls: ['./management-config-delegation.component.css']
})
export class ManagementConfigDelegationComponent implements OnInit {
  //#region "Decalre variable, innit some value and so on"
  @ViewChild('modalDelegate') public modalDelegate: ModalDirective;
  public FullNameOption: IMultiSelectOption[] = [];
  public userLogin: LoggedInUser;
  public pageIndex: number = PageConstants.pageIndex;
  public pageSize: number = PageConstants.pageSize;
  public totalRow: number;
  public listFullName: string[];
  public listUser: any[];
  public filterDelegator: IMultiSelectOption[] = [];
  public myFilterDelegators: string[] = [];
  public currentMaxEntries: number;
  public FullNameFilter: string[] = [];
  pagingSize: number = PageConstants.pagingSize;
  loadsuccess: boolean = false;
  public delegation: ConfigDelegation = new ConfigDelegation();
  listMail: Array<SendMailModel> = [];
  listMailExplanation: Array<SendMailModel> = [];
  public lstRequestByUser: any[] = [];
  public lstExplanationRequestByUser: any[] = [];
  public lstExplanationRequestId: any[] = [];
  public isDisableResetButton: boolean = false;
  public value: any= {
    end: '',
    start: ''
  };
  FullNameDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectFullName,
    checked: CommonConstants.CheckFullName,
    checkedPlural: CommonConstants.CheckFullName,
    defaultTitle: CommonConstants.FullName,
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

  textDelegatorRequest: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectDelegate,
    checked: CommonConstants.CheckDelegateType,
    checkedPlural: CommonConstants.CheckDelegateType,
    defaultTitle: CommonConstants.DelegatorTitle,
  };
  //#endregion

  //Contructor to inject services.
  constructor(public _authenService: AuthenService,
    private _dataService: DataService,
    private _notificationService: NotificationService,
    private _utilityService: UtilityService,
    private daterangepickerOptions: DaterangepickerConfig
  ) { 
    this.daterangepickerOptions.settings = {
      opens: CommonConstants.LEFT,
      minDate: moment(),
      // startRequestDate: moment().subtract(3, DateTimeConstants.DAY),
      // endRequestDate: moment(),
      isInvalidDate: function (date) {
        if (date.isSame(Date.toString(), DateTimeConstants.DAY))
          return 'mystyle';
        return false;
      },
      locale: DateTimeConstants.Locale,
      autoUpdateInput: true,
      alwaysShowCalendars: true,
      showDropdowns: true,
      maxDate: null,
      endDate: moment(),
      startDate: moment(),
    };
  }
  //#endregion

  // Innit data or function   
  ngOnInit() {
    this.userLogin = this._authenService.getLoggedInUser();
    this.loadData();
    this.loadFullName()
  }
  //#endregion "Innit data"

  //Load data
  public loadData() {
    this._dataService.post('/api/configdelegation/getalluserconfigdelegation?&page=' + this.pageIndex + '&pageSize=' + this.pageSize + '&groupId=' + this.userLogin.groupId, JSON.stringify(this.FullNameFilter))
      .subscribe((response: any) => {
        this.listUser = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.loadsuccess = true;
        this.calShowingPage();
      }, error => this._dataService.handleError(error));
  }

  loadFullName() {
    this._dataService.get('/api/appUser/getuserbygroup?groupID=' + this.userLogin.groupId)
      .subscribe((response: any[]) => {
        this.FullNameOption = [];
        for (let user of response) {
          if ((this.userLogin.id.localeCompare(user.Id)) != 0) {
            this.FullNameOption.push({ id: user.Id, name: user.FullName + " ( " + user.UserName + " ) "  });
          }
        }
      }, error => this._dataService.handleError(error));
  }
  //#endregion "Load data"

  //#region "Popup or other"
  //Show delegate
  showDelegate(item: any) {
    this.daterangepickerOptions.settings = {
      opens: CommonConstants.LEFT,
      minDate: moment(),
      // startRequestDate: moment().subtract(3, DateTimeConstants.DAY),
      // endRequestDate: moment(),
      isInvalidDate: function (date) {
        if (date.isSame(Date.toString(), DateTimeConstants.DAY))
          return 'mystyle';
        return false;
      },
      locale: DateTimeConstants.Locale,
      autoUpdateInput: true,
      alwaysShowCalendars: true,
      showDropdowns: true,
      maxDate: null,
      endDate: moment(),
      startDate: moment(),
    };
    this.value.start = moment();
    this.value.end = moment();
    this.selectedAddRequestDatePicker(this.value,this.chosenRequestDate)
    if(item.AssignTo != null)
    {
      this.isDisableResetButton = true
    }
    this.delegation = item;
    this._dataService.get('/api/appUser/getuserbygroup')
      .subscribe((response: any) => {
        this.filterDelegator = [];
        this.myFilterDelegators = [];
        for (let userGroup of response) {
          if (this.userLogin.id != userGroup.Id) {
            this.filterDelegator.push({ id: userGroup.Id, name:  userGroup.FullName + " ( " + userGroup.UserName + " ) " })
          }
        }
      }, error => this._dataService.handleError(error));
    this.modalDelegate.show();
  }

  //close form delegate
  public closeForm(modal: ModalDirective) {
    modal.hide();
  }
  //#endregion "Load data"

  //#region "CRUD"
  saveDelegate(valid: boolean) {
    this.delegateGroup();
  }
  delegateGroup() {
    this.delegation.StartDate = moment(new Date(this.chosenRequestDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this.delegation.EndDate = moment(new Date(this.chosenRequestDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this.delegation.AssignTo = this.myFilterDelegators[0];
    this._dataService.put('/api/configdelegation/addconfigdelegation?userId=' + this.delegation.UserId + '&groupId=' + this.userLogin.groupId, JSON.stringify(this.delegation))
      .subscribe((response: any) => {
        this.value.start = moment();
        this.value.end = moment();
        this.selectedAddRequestDatePicker(this.value,this.chosenRequestDate)
        this.modalDelegate.hide();
        this.loadData();
        this._notificationService.printSuccessMessage(MessageConstants.UPDATED_OK_MSG);

        for (let item of response) {
          if (item.RequestType !== undefined) {
            this.lstRequestByUser.push(item);
          } else {
            this.lstExplanationRequestId.push(item.ID);
          }
        }

        //send mail delegate default request
        this.listMail = [];
        for (let item of this.lstRequestByUser) {
          this.listMail.push(new SendMailDelegateRequest(item.AppUser.Group.Name, item.RequestType.Name, item.DetailReason,
            item.StartDate, item.EndDate, 'Title', item.AppUser.Id, this.myFilterDelegators, MailConstants.Delegation, MailConstants.DelegationAssignedList, MailConstants.RequestSubject, null, MailConstants.DelegationManagement, item.DelegateId, item.toEmail));
        }
        this._dataService.post('/api/System/sendMailMultiFix', JSON.stringify(this.listMail)).subscribe((response: any) => {

          //send mail explanation
          this._dataService.put('/api/explanation/getlistexplanationdetail', JSON.stringify(this.lstExplanationRequestId))
            .subscribe((responseExplanation: any) => {
              this.lstExplanationRequestByUser = responseExplanation;
              this.listMailExplanation = [];
              for (let item of this.lstExplanationRequestByUser) {
                this.listMailExplanation.push(new SendMailDelegateModel('Explanation Request: ' + item.Title, item.Receiver.FullName, this.myFilterDelegators, MailConstants.Delegation, MailConstants.DelegationAssignedList, MailConstants.DelegationExplanationRequest, null,
                  MailConstants.DelegationManagement, item.User.Group.Name, item.ExplanationDate, item.ReasonList, item.ReasonDetail, item.User.Id, item.Receiver.Id));
              }
              this._dataService.post('/api/System/sendMailMultiFix', JSON.stringify(this.listMailExplanation)).subscribe((response: any) => { });
            });

        });




      }, error => this._dataService.handleError(error));
  }

  delete() {
    this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_DELEGATION_DELETE_MSG, () => this.deleteItemConfirm(this.delegation.ID));
  }

  deleteItemConfirm(id: any) {
    this._dataService.delete('/api/configdelegation/deleteDelegation', 'id', id).subscribe((response: Response) => {
      this.pageIndex = 1;
      this._notificationService.printSuccessMessage(MessageConstants.RESET_OK_MSG);
      this.loadData();
      this.modalDelegate.hide();
      this.isDisableResetButton = false;
    }, error => this._dataService.handleError(error));
  }
  //#endregion
  //#region "Business logic"
  public pickerRequest = {
    opens: CommonConstants.RIGHT,
    minDate: moment(),
    // startRequestDate: moment().subtract(3, DateTimeConstants.DAY),
    // endRequestDate: moment(),
    autoUnselect: true,
    alwaysShowCalendars: true,
    ranges: false,
    isInvalidDate: function (date) {
      if (date.isSame(Date.toString(), DateTimeConstants.DAY))
        return 'mystyle';
      return false;
    }
  }

  public selectedAddRequestDatePicker(value: any, dateInput: any) {
    this.value = value;
    dateInput.start = value.start.format();
    dateInput.end = value.end;
  }

  public chosenRequestDate: any = {
    start: moment(),
    end: moment(),

  };

  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.loadsuccess = false;
    this.loadData();
    this.calShowingPage();
  }
  //calculate page
  calShowingPage() {
    this.currentMaxEntries = this.pageIndex * this.pageSize;
    if (this.currentMaxEntries >= this.totalRow) {
      this.currentMaxEntries = this.totalRow;
    }
  }

  public reset() {
    this.FullNameFilter = [];
    this.pageIndex = 1;
    this.loadData();
  }

  public filter() {
    this.pageIndex = 1;
    this.loadData();
  }
  //#endregion "Business logic"
}
