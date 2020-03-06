import { Component, OnInit, ViewChild } from '@angular/core';
import { DataService } from '../../core/services/data.service';
import { NotificationService } from '../../core/services/notification.service';
import { UtilityService } from '../../core/services/utility.service';
import { AuthenService } from '../../core/services/authen.service';
import { DateTimeConstants } from '../../core/common/datetime.constants';
import { MessageConstants } from '../../core/common/message.constants';
import { SystemConstants } from '../../core/common/system.constants';
import { UploadService } from '../../core/services/upload.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { Router } from '@angular/router';
import { DaterangepickerConfig } from '../../core/services/config.service';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { CommonConstants } from 'app/core/common/common.constants';
import { ListOTFilter } from 'app/core/models/list-OT-filter';
import { FunctionConstants } from '../../core/common/function.constants';
import { DateRangePickerModel } from '../../core/models/date-range-picker';
import { LoggedInUser } from 'app/core/domain/loggedin.user';
import { PageConstants } from 'app/core/common/page.constans';
import { Message } from '@angular/compiler/src/i18n/i18n_ast';
import { DefaultColunmConstants } from 'app/core/common/defaultColunm.constant';
@Component({
  selector: 'app-ot-list',
  templateUrl: './ot-list.component.html',
  styleUrls: ['./ot-list.component.css']
})
export class OtListComponent implements OnInit {
  public baseFolder: string = SystemConstants.BASE_API;
  public user: LoggedInUser;
  public FullNameOption: IMultiSelectOption[] = [];
  public FullNameFilter: string[] = [];
  public OTDateTypeOption: IMultiSelectOption[] = [];
  public OTDateTypeFilter: string[] = [];
  public OTTimeTypeOption: IMultiSelectOption[] = [];
  public OTTimeTypeFilter: string[] = [];
  public value: any;
  public otRequest: any[];
  public userID: string;
  public groupID: string;
  public totalRow: number;
  pagingSize: number = PageConstants.pagingSize;
  public pageIndex: number = PageConstants.pageIndex;
  public pageSize: number = PageConstants.pageSize;
  public pageDisplay: number = 10;
  public ListFilter: ListOTFilter = null;
  loadsuccess:boolean = false;
  public startDate: string;
  public endDate: string;
  

  //Export
  public FullName: string;
  public Group: string;
  public OTDate: string;
  public OTDateType: string;
  public OTTimeType: string;
  public CheckIn: Date;
  public CheckOut: Date;
  public WorkingTime: string;
  public Approver: string;
  public Status: string;


  public statusRequestFilter: string[] = [];
  public currentMaxEntries: number;
  isDesc: boolean = true;
  column: string = DefaultColunmConstants.UserNameColunm;
  direction: number;

  dropdownSettings: IMultiSelectSettings = {
    enableSearch: false,
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 0,
    displayAllSelectedText: true,
    showCheckAll: true,
    showUncheckAll: true,
    closeOnClickOutside: true,
    maxHeight:'200px',
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
    maxHeight:'200px',
  };

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
    maxHeight:'200px',
  };
  OTDateTypeDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectOTDateType,
    checked: CommonConstants.CheckOTDateType,
    checkedPlural:CommonConstants.CheckOTDateType,
    defaultTitle: CommonConstants.OTDateTypeTitle,
  };
    // Text configuration Dropdown OT Time 
    OTTimeTypeDropdownTexts: IMultiSelectTexts = {
      allSelected: CommonConstants.AllSelectOTTimeType,
      checked: CommonConstants.CheckOTTimeType,
      checkedPlural:CommonConstants.CheckOTTimeType,
      defaultTitle: CommonConstants.OTTimeTypeTitle,
    };
  FullNameDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectFullName,
    checked: CommonConstants.CheckFullName,
    checkedPlural:CommonConstants.CheckFullName,
    defaultTitle: CommonConstants.FullName,
  };
  public datePickerOptions: any = {
    locale: DateTimeConstants.Locale,
    alwaysShowCalendars: false,
    singleDatePicker: true,
  };
  constructor(
    public _authenService: AuthenService,
    private _dataService: DataService,
    private _utilityService: UtilityService,
    private daterangepickerOptions: DaterangepickerConfig,
    private _notificationService: NotificationService,
  ) {
    if(_authenService.getLoggedInUser().groupId.length == 0){
      _utilityService.navigateToMain();
    }
    // if (_authenService.checkAccess(FunctionConstants.OTRequest) == false) {
    //   _utilityService.navigateToLogin();
    // }
    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      startDate: moment().startOf('month'),
      endDate: moment(),
      alwaysShowCalendars: true,
      "opens": "left",
      ranges: DateTimeConstants.Range,
    };
  }

  ngOnInit() {
    this.loadData();
    this.loadOTTimeType();
    this.loadStatus();
    this.loadFullName();
  }
  //Load view list OT

  public loadData() {
    this.groupID = this._authenService.getLoggedInUser().groupId;
    this.userID = this._authenService.getLoggedInUser().id;
    this._dataService.post('/api/ot-list/getallotlist?userID=' + this.userID + '&groupId=' + this.groupID + '&column=' + this.column
      + '&isDesc=' + this.isDesc + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize, JSON.stringify(this.ListFilter))
      .subscribe((response: any) => {
        this.otRequest = response;
        this.otRequest = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.loadsuccess = true;
        this.calShowingPage();
      }, error => this._dataService.handleError(error));
  }
  //Function Load by Filter
  public loadStatus() {
    this._dataService.get('/api/otrequest/getallOTDateType')
      .subscribe((response: any) => {
        this.OTDateTypeOption = [];
        for (let OTDateType of response) {
          this.OTDateTypeOption.push({ id: OTDateType.Name, name: OTDateType.Name })
        }
      }, error => this._dataService.handleError(error));
  }
    //Load OT Time Type
    loadOTTimeType() {
      this._dataService.get('/api/otrequest/getallOTTimeType')
        .subscribe((response: any) => {
          this.OTTimeTypeOption = [];
          for (let OTTimeType of response) {
            this.OTTimeTypeOption.push({ id: OTTimeType.Name, name: OTTimeType.Name })
          }
        }, error => this._dataService.handleError(error));
    }

  loadFullName() {
    this._dataService.get('/api/appUser/getuserbygroup')
    .subscribe((response: any[]) => {
      this.FullNameOption = [];
      for (let user of response) {
        this.FullNameOption.push({ id: user.UserName, name: user.FullName +" ( "+ user.UserName +" ) "});
      }
    }, error => this._dataService.handleError(error));
  }
  Export(){
    this._dataService.post('/api/ot-list/exportexcel?userID=' + this.userID + '&groupId=' + this.groupID + '&column=' + this.column
      + '&isDesc=' + this.isDesc + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize, JSON.stringify(this.ListFilter))
    .subscribe((response: any[]) => {
      this.otRequest = response;
      if (response != null) {
        window.location.href = SystemConstants.BASE_API + response;
        this.loadData();
      } else {
        this._notificationService.printSuccessMessage(MessageConstants.DOWNLOAD_OK_ERROR);
      }
    }, error => this._dataService.handleError(error));
  }

  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.loadsuccess = false;
    this.loadData();
    this.calShowingPage();
  }
  calShowingPage() {
    this.currentMaxEntries = this.pageIndex * this.pageSize;
    if (this.currentMaxEntries >= this.totalRow) {
      this.currentMaxEntries = this.totalRow;
    }
  }
  //Function filter
  public filter() {
    if(this.chosenDate.start != '' && this.chosenDate.end != ''){
      this.startDate = moment(new Date(this.chosenDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      this.endDate = moment(new Date(this.chosenDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    // this.ListFilter = new ListOTFilter(this.FullNameFilter, this.OTDateTypeFilter, this.OTTimeTypeFilter, this.startDate, this.endDate);
      
      }
      else{
        this.startDate = null;
        this.endDate = null;
        this.ListFilter = null;  
      }
    this.ListFilter = new ListOTFilter(this.FullNameFilter, this.OTDateTypeFilter, this.OTTimeTypeFilter, this.startDate, this.endDate);
    this.pageIndex = 1;
    this.loadData();
  }
  //Function Sort
  sort(property) {
    this.isDesc = !this.isDesc; //change the direction    
    this.column = property;
    let direction = this.isDesc ? 1 : -1;
    this.pageIndex = 1;
    this.loadData();
  }
  //Function Reset Filter
  public reset() {
    if(this.chosenDate.start!==''){
      this.daterangepickerOptions.settings = {
        locale: DateTimeConstants.Locale,
        ranges: DateTimeConstants.Range,
        autoUpdateInput: true,
        alwaysShowCalendars: true,
        "opens": "left",
      };
        this.value.start = moment().startOf('month');
        this.value.end = moment();
        this.selectedDateRangePicker(this.value, this.chosenDate)
    };
    this.chosenDate.start = '';
    this.chosenDate.end = '';
    this.OTDateTypeFilter = [];
    this.OTTimeTypeFilter=[];
    this.FullNameFilter = [];
    this.ListFilter = null;
    this.pageIndex = 1;
    this.loadData();
}

  //Filter by date month year
  public picker = {
    opens: CommonConstants.RIGHT,
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
  public eventLog = '';
  public selectedDateRangePicker(value: any, dateInput: any) {
    this.value = value;
    dateInput.start = value.start.format();
    dateInput.end = value.end.format();
  }

  public calendarEventsHandler(e: any) {
    this.eventLog += '\nEvent Fired: ' + e.event.type;
  }

  public toggleDirection(direction: string) {
    this.picker.opens = direction;
  }
  clearText(event:any,dropdownSearch:any){
    dropdownSearch.clearSearch(event);
  };
}
