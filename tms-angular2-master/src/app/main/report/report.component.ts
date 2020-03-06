import { Component, OnInit, ViewChild } from '@angular/core';
import { PageConstants } from '../../core/common/page.constans';
import { DateTimeConstants } from '../../core/common/datetime.constants';
import { CommonConstants } from '../../core/common/common.constants';
import { AuthenService } from 'app/core/services/authen.service';
import { DaterangepickerConfig } from 'app/core/services/config.service';
import { DataService } from 'app/core/services/data.service';
import { NotificationService } from 'app/core/services/notification.service';
import { UtilityService } from 'app/core/services/utility.service';
import { MessageConstants } from 'app/core/common/message.constants';
import { FilterTimeSheet } from 'app/core/models/filter-time-sheet';
import { SystemConstants } from 'app/core/common/system.constants';
import { IMultiSelectOption, IMultiSelectTexts, IMultiSelectSettings } from 'angular-2-dropdown-multiselect';
import { FilterReport } from '../../core/models/filterReport';
import { setTimeout } from 'timers';
import { ModalDirective } from 'ngx-bootstrap/modal';

@Component({
  selector: 'app-report',
  templateUrl: './report.component.html',
  styleUrls: ['./report.component.css']
})
export class ReportComponent implements OnInit {
  @ViewChild('modalProgressBar') public modalProgressBar: ModalDirective;
  public userID: string;
  public report: any[];
  public value: any = {
    end: '',
    start: ''
  };

  public filter: FilterReport;
  public ListUserID: string[] = [];
  private startDate: string;
  private endDate: string;
  private progressMaxValue: number;
  currentMaxEntries: number;
  loadsuccess: boolean = false;
  public totalRow: number;
  public pageIndex: number = PageConstants.pageIndex;
  public pageSize: number = PageConstants.pageSize;
  public pagingSize: number = PageConstants.pagingSize;
  public progressValueNow = 0;
  public dateOptions: any = {
    locale: { format: 'DD/MM/YYYY' },
    alwaysShowCalendars: false,
    singleDatePicker: true
  };
  public datePickerOptions: any = {
    locale: DateTimeConstants.Locale,
    alwaysShowCalendars: false,
    singleDatePicker: true,
    minDate: moment(),
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
  fullNameDropdowTexts: IMultiSelectTexts = {
    defaultTitle: CommonConstants.FullName,
  }
  public settings;
  public setting1;
  public FullNameOption: IMultiSelectOption[] = [];
  constructor(public _authenService: AuthenService, private _dataService: DataService,
    private daterangepickerOptions: DaterangepickerConfig,
    private _notificationService: NotificationService,
    private _utilityService: UtilityService) {
    if (_authenService.getLoggedInUser().groupId.length == 0) {
      _utilityService.navigateToMain();
    }
    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: true,
      "opens": "right",
      //maxDate: moment().subtract(1, DateTimeConstants.DAY),
      startDate: moment().startOf('month'),
      endDate: moment().subtract(1, DateTimeConstants.DAY),
      ranges: DateTimeConstants.Range,
    };
  }

  ngOnInit() {
    this.userID = this._authenService.getLoggedInUser().id;
    this.InnitFillter();
    this.loadData();
    this.loadFullName();
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
  InnitFillter() {
    this.startDate = moment(new Date(this.chosenDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this.endDate = moment(new Date(this.chosenDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this.filter = new FilterReport(this.ListUserID, this.startDate, this.endDate);
  }
  Report() {
    if (this.chosenDate.start != '' && this.chosenDate.end != '') {
      this.startDate = moment(new Date(this.chosenDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      this.endDate = moment(new Date(this.chosenDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    } else {
      this._notificationService.printErrorMessage("Please Choose Start Date and End Date");
      this.startDate = null;
      this.endDate = null;
      return;
    }
    // this.pageIndex = 1;
    this.filter = new FilterReport(this.ListUserID, this.startDate, this.endDate);
    this.loadsuccess = false;
    this._dataService.post('/api/timesheet/exportexcel?userID=' + this.userID, JSON.stringify(this.filter))
      .subscribe((response: any) => {
        this.loadsuccess = true;
        if (response != null) {
          window.location.href = SystemConstants.BASE_API + response;
        } else {
          this._notificationService.printSuccessMessage(MessageConstants.DOWNLOAD_OK_ERROR);
        }
      }, error => {
        this.loadsuccess = true;
        this._dataService.handleError(error);
      });
  }
  ProgressBar() {
    this.progressValueNow = 0;
    this.progressMaxValue = 0;
    this.CountUserReportEx();
    this.modalProgressBar.show();
    let interval = setInterval(() => {
      this.GetProgressBarValue();
      if (this.progressValueNow >= this.progressMaxValue) {
        this.RemoveProgressBarValue();
        this.modalProgressBar.hide();
        clearInterval(interval);
      }
    }, 1000);
  }
  Round(value: any) {
    return Math.round(value);
  }
  GetProgressBarValue() {
    //this.progressValueNow ++;
    this._dataService.get('/api/timesheet/get-progress-value?userID=' + this.userID)
      .subscribe((response: any) => {
        this.progressValueNow = response > this.totalRow ? this.totalRow : response;
      }, error => {
        this._dataService.handleError(error);
      });
  }
  CountUserReportEx() {
    //this.progressValueNow ++;
    this._dataService.get('/api/timesheet/count-user-reportex?userID=' + this.userID)
      .subscribe((response: any) => {
        this.progressMaxValue = response;
      }, error => {
        this._dataService.handleError(error);
      });
  }
  RemoveProgressBarValue() {
    this._dataService.get('/api/timesheet/remove-progress-value?userID=' + this.userID)
      .subscribe((response: any) => {
        this.progressValueNow = 0;
      }, error => {
        this._dataService.handleError(error);
      });
  }
  ReportEx() {
    if (this.chosenDate.start != '' && this.chosenDate.end != '') {
      this.startDate = moment(new Date(this.chosenDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      this.endDate = moment(new Date(this.chosenDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    } else {
      this._notificationService.printErrorMessage("Please Choose Start Date and End Date");
      this.startDate = null;
      this.endDate = null;
      return;
    }
    // this.pageIndex = 1;
    this.filter = new FilterReport(this.ListUserID, this.startDate, this.endDate);
    this.loadsuccess = false;
    this._dataService.post('/api/timesheet/exportexcelex?userID=' + this.userID, JSON.stringify(this.filter))
      .subscribe((response: any) => {
        this.loadsuccess = true;
        if (response != null) {
          window.location.href = SystemConstants.BASE_API + response;
        } else {
          this._notificationService.printSuccessMessage(MessageConstants.DOWNLOAD_OK_ERROR);
        }
      }, error => {
        this.loadsuccess = true;
        this._dataService.handleError(error);
      });
    this.ProgressBar();
  }
  public loadData() {
    this.loadsuccess = false;
    this._dataService.post('/api/timesheet/getallreport?userID=' + this.userID + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize
      , JSON.stringify(this.filter))
      .subscribe((response: any) => {
        this.report = response.Items;
        this.totalRow = response.TotalRows;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.loadsuccess = true;
        this.callShowingPage();
      }, error => {
        this.loadsuccess = true;
        this._dataService.handleError(error);
      });
  }
  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.loadsuccess = false;
    this.loadData();
    this.callShowingPage();
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
    start: moment().startOf('month'),
    end: moment().subtract(1, DateTimeConstants.DAY),
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
  loadFullName() {
    this._dataService.post('/api/appUser/GetUserByAll')
      .subscribe((response: any[]) => {
        this.FullNameOption = [];
        for (let user of response) {
          this.FullNameOption.push({ id: user.Id, name: user.FullName });
        }
      }, error => this._dataService.handleError(error));
  }
  clearText(event: any, dropdownSearch: any) {
    dropdownSearch.clearSearch(event);
  };
  filterReport() {
    if (this.chosenDate.start != '' && this.chosenDate.end != '') {
      this.startDate = moment(new Date(this.chosenDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      this.endDate = moment(new Date(this.chosenDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    } else {
      this.startDate = null;
      this.endDate = null;
    }
    this.pageIndex = 1;
    this.filter = new FilterReport(this.ListUserID, this.startDate, this.endDate);
    this.loadData();
  }
  reset() {
    this.ListUserID = [];

    this.daterangepickerOptions.settings = {
      alwaysShowCalendars: true,
      locale: DateTimeConstants.Locale,
      ranges: DateTimeConstants.Range,
      autoUpdateInput: true,
      "opens": "right",
      minDate : null,
      startDate : null,
    };
    this.value.start = moment().startOf('month');
    this.value.end = moment().subtract(1, DateTimeConstants.DAY);
    this.selectedDateRangePicker(this.value, this.chosenDate);
    this.InnitFillter();
    this.pageIndex = 1;
    this.loadData();
  }
}
