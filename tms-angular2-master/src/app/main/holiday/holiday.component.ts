import { Component, OnInit, ViewChild } from '@angular/core';
import { NgForm } from '@angular/forms';
import { DateTimeConstants } from 'app/core/common/datetime.constants';
import { MessageConstants } from 'app/core/common/message.constants';
import { PageConstants } from 'app/core/common/page.constans';
import { AuthenService } from 'app/core/services/authen.service';
import { DataService } from 'app/core/services/data.service';
import { UtilityService } from 'app/core/services/utility.service';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { DaterangepickerConfig } from '../../core/services/config.service';
import { NotificationService } from '../../core/services/notification.service';
import { HolidayModel } from './holiday.model';

@Component({
  selector: 'app-holiday',
  templateUrl: './holiday.component.html',
  styleUrls: ['./holiday.component.css']
})

export class HolidayComponent implements OnInit {
  @ViewChild('modalAddEdit') modalAddEdit: ModalDirective;
  @ViewChild('addEditForm') addEditForm: NgForm;
  holidaylist: any[];
  entity: any = {};
  isCreate = false;
  stateCreate = false;
  isUpdate = false;
  isDelete = false;
  isViewDetail = false;
  totalRow: number;
  pageIndex: number = PageConstants.pageIndex;
  pageSize: number = PageConstants.pageSize;
  pagingSize: number = PageConstants.pagingSize;
  currentMaxEntries: number;
  loadsuccess = false;
  isDesc = false;
  column: string = 'CreatedDate';
  value: any;
  public daterangeOptions: any = {
    locale: DateTimeConstants.Locale,
    autoUpdateInput: true,
    alwaysShowCalendars: false,
    singleDatePicker: true,
    showDropdowns: true,
    endDate: null,
    startDate: moment(),
    minDate: moment(),
  };
  public daterangeOptions1: any = {
    locale: DateTimeConstants.Locale,
    autoUpdateInput: true,
    alwaysShowCalendars: false,
    singleDatePicker: true,
    showDropdowns: true,
    endDate: null,
    startDate: moment(),
    minDate: moment(),
  };
  detailentity: any;
  ischangedData = false;
  constructor(public _authenService: AuthenService,
    private _dataService: DataService,
    private _notificationService: NotificationService,
    public daterangepickerOptions: DaterangepickerConfig,
    private _utilityService: UtilityService) {
    if (_authenService.getLoggedInUser().groupId.length === 0 || !_authenService.hasPermission('HOLIDAY', 'read')) {
      _utilityService.navigateToMain();
    } else {
      this.isCreate = _authenService.hasPermission('HOLIDAY', 'create');
      this.isUpdate = _authenService.hasPermission('HOLIDAY', 'update');
      this.isDelete = _authenService.hasPermission('HOLIDAY', 'delete');
      this.isViewDetail = this.isUpdate || this.isDelete;
    }
    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      autoUpdateInput: true,
      alwaysShowCalendars: false,
      singleDatePicker: true,
      showDropdowns: true,
      minDate: moment(),
    };
  }
  ngOnInit() {
    this.loadData();
  }
  callShowingPage() {
    this.currentMaxEntries = this.pageIndex * this.pageSize;
    if (this.currentMaxEntries >= this.totalRow) {
      this.currentMaxEntries = this.totalRow;
    }
  }
  pageChanged(event: any): void {
    this.loadsuccess = false;
    this.pageIndex = event.page;
    this.loadData();
  }
  public selectedDatePicker($event: any) {

    var timestamp = Date.parse($event.end);
    if (isNaN(timestamp) == false) {
      this.ischangedData = true;
      this.entity.Workingday = moment($event.end);
    }
  };

  public selectedDatePickerDate($event: any) {
    var timestamp = Date.parse($event.end);
    if (isNaN(timestamp) == false) {
      this.ischangedData = true;
      this.entity.Date = moment($event.end);
    }
  }

  public loadData() {
    this._dataService.get('/api/holiday/list?column=' +this.column+'&isDesc=' + this.isDesc
      + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize)
      .subscribe((response: any) => {
        this.holidaylist = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.loadsuccess = true;
        this.callShowingPage();
      }, error => this._dataService.handleError(error));

  }
  //#region Show modal
  showAddModal(form: NgForm) {
    form.reset();
    this.stateCreate = true;
    this.entity = new HolidayModel();
    this.modalAddEdit.show();
    this.daterangeOptions = {
      locale: DateTimeConstants.Locale,
      autoUpdateInput: true,
      alwaysShowCalendars: false,
      singleDatePicker: true,
      showDropdowns: true,
      endDate: null,
      startDate: moment(),
      minDate: moment(),
    };
    this.daterangeOptions1 = {
      locale: DateTimeConstants.Locale,
      autoUpdateInput: true,
      alwaysShowCalendars: false,
      singleDatePicker: true,
      showDropdowns: true,
      endDate: null,
      startDate: moment(),
      minDate: moment(),
    };
  }
  showEditModal(form: NgForm, id: any) {
    form.reset();
    const detail = this.holidaylist.find(x => x.ID === id);
    this.daterangeOptions = {
      locale: DateTimeConstants.Locale,
      autoUpdateInput: true,
      alwaysShowCalendars: false,
      singleDatePicker: true,
      showDropdowns: true,
      endDate: null,
      startDate: detail.Date ? moment(detail.Date) : moment(),
      minDate: moment(),
    };
    this.daterangeOptions1 = {
      locale: DateTimeConstants.Locale,
      autoUpdateInput: true,
      alwaysShowCalendars: false,
      singleDatePicker: true,
      showDropdowns: true,
      endDate: null,
      startDate: detail.Workingday ? moment(detail.Workingday) : moment(),
      minDate: moment(),
    };
    setTimeout(() => {
      this.entity = new HolidayModel();
      this.entity.ID = detail.ID;
      this.entity.Date = detail.Date;
      this.entity.Workingday = detail.Workingday ? detail.Workingday : null;
      this.entity.Note = detail.Note;
    });
    this.stateCreate = false;
    this.modalAddEdit.show();
  }

  hideModal(form: NgForm) {
    if (form.dirty || this.ischangedData) {
      this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_CLOSE_REQUEST_MSG, () => {
        this.modalAddEdit.hide();
        this.ischangedData = false;
        form.reset();
      })
    } else {
      this.modalAddEdit.hide();
      this.ischangedData = false;
      form.reset();
    }
  }

  //#endregion
  // Action delete
  public deleteConfirm(id: any): void {
    this._dataService.delete('/api/holiday/delete', 'id', id).subscribe((response: Response) => {
      this._notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG);
      this.loadData();
    }, error => this._dataService.handleError(error));
  }
  // Click button delete turn on confirm
  public delete(id: number) {
    this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_DELETE_TIMEDAY_MSG, () => this.deleteConfirm(id));
  }
  // Create && Update
  private saveData(form: NgForm) {
    if (this.entity.ID == undefined) {
      this._dataService.post('/api/holiday/create', JSON.stringify(this.entity))
        .subscribe((response: any) => {
          this.loadData();
          this.modalAddEdit.hide();
          this.entity = response;
          this.ischangedData = false;
          this._notificationService.printSuccessMessage(MessageConstants.CREATED_OK_MSG);
        }, error => this._dataService.handleError(error));
    } else {
      this._dataService.put('/api/holiday/update', JSON.stringify(this.entity))
        .subscribe((response: any) => {
          this.modalAddEdit.hide();
          this.ischangedData = false;
          this._notificationService.printSuccessMessage(MessageConstants.UPDATED_OK_MSG);
          this.loadData();

        }, error => this._dataService.handleError(error));
    }

  }
  /// Reset form
  resetForm(form: NgForm) {
    if (this.stateCreate === true) {
      form.reset();
      this.entity = new HolidayModel();
      this.daterangeOptions = {
        locale: DateTimeConstants.Locale,
        autoUpdateInput: true,
        alwaysShowCalendars: false,
        singleDatePicker: true,
        showDropdowns: true,
        endDate: null,
        startDate: moment(),
        minDate: moment(),
      };
      this.daterangeOptions1 = {
        locale: DateTimeConstants.Locale,
        autoUpdateInput: true,
        alwaysShowCalendars: false,
        singleDatePicker: true,
        showDropdowns: true,
        endDate: null,
        startDate: moment(),
        minDate: moment(),
      };
    } else {
      form.reset(this.entity);
      const detail = this.holidaylist.find(x => x.ID === this.entity.ID);
      this.entity = new HolidayModel();
      this.entity.ID = detail.ID;
      this.entity.Date = detail.Date;
      this.entity.Workingday = detail.Workingday ? detail.Workingday : null;
      this.entity.Note = detail.Note;
      this.daterangeOptions = {
        locale: DateTimeConstants.Locale,
        autoUpdateInput: true,
        alwaysShowCalendars: false,
        singleDatePicker: true,
        showDropdowns: true,
        endDate: null,
        startDate: detail.Date ? moment(detail.Date) : moment(),
        minDate: moment(),
      };
      this.daterangeOptions1 = {
        locale: DateTimeConstants.Locale,
        autoUpdateInput: true,
        alwaysShowCalendars: false,
        singleDatePicker: true,
        showDropdowns: true,
        endDate: null,
        startDate: detail.Workingday ? moment(detail.Workingday) : moment(),
        minDate: moment(),
      };
    }

  }
}
