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
import { NgForm } from '@angular/forms';
import { CommonConstants } from 'app/core/common/common.constants';
import { IMultiSelectTexts, IMultiSelectOption, IMultiSelectSettings } from 'angular-2-dropdown-multiselect';
import { PageConstants } from 'app/core/common/page.constans';
import { DateRangePickerModel } from 'app/core/models/date-range-picker';
import { ListEntitleDayFilter } from 'app/core/models/filter-entitleday';
import { DefaultColunmConstants } from '../../core/common/defaultColunm.constant';

@Component({
  selector: 'app-entitle-day',
  templateUrl: './entitle-day.component.html',
  styleUrls: ['./entitle-day.component.css']
})
export class EntitleDayComponent implements OnInit {
  @ViewChild('modalAddEdit') public modalAddEdit: ModalDirective;
  @ViewChild('addEditForm') public addEditForm: NgForm;
  public baseFolder: string = SystemConstants.BASE_API;
  public entitleday: any[];
  public FullNameOption: IMultiSelectOption[] = [];
  public userID: string;
  public groupID: string;
  public totalRow: number;
  pagingSize: number = PageConstants.pagingSize;
  public pageIndex: number = PageConstants.pageIndex;
  public pageSize: number = PageConstants.pageSize;
  public pageDisplay: number = 10;
  public ListFilter: ListEntitleDayFilter = null;
  public FullNameFilter: string[] = [];
  loadsuccess: boolean = false;
  isCreate: boolean = false;
  isCancel: boolean = false;
  public entity: any;

  public DayOffTypeOption: IMultiSelectOption[] = [];
  public UserTypeOption: IMultiSelectOption[] = [];
  public chosenType: string[] = [];
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

  OTDateTypeDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectDayOffType,
    checked: CommonConstants.CheckDayOffType,
    checkedPlural: CommonConstants.CheckDayOffType,
    defaultTitle: CommonConstants.DayOffType,
  };
  FullNameDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectFullName,
    checked: CommonConstants.CheckFullName,
    checkedPlural: CommonConstants.CheckFullName,
    defaultTitle: CommonConstants.FullName,
  };
  constructor(public _authenService: AuthenService,
    private _dataService: DataService,
    private _notificationService: NotificationService,
    private _utilityService: UtilityService
  ) {
    if (_authenService.getLoggedInUser().groupId.length == 0) {
      _utilityService.navigateToMain();
    }
  }

  ngOnInit() {
    this.loadData();
    this.calShowingPage();
    this.loadType();
    this.loadFullName();
  }
  // Function Load List Entitle Day
  public loadData() {
    this.userID = this._authenService.getLoggedInUser().id;
    this.groupID = this._authenService.getLoggedInUser().groupId;
    this._dataService.post('/api/entitleday/getallentitle?userID=' + this.userID + '&groupId=' + this.groupID + '&column=' + this.column + '&isDesc=' + this.isDesc + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize, JSON.stringify(this.ListFilter))
      .subscribe((response: any) => {
        this.entitleday = response;
        this.entitleday = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.loadsuccess = true;
        this.calShowingPage();
      }, error => this._dataService.handleError(error));
  }
  loadFullName() {
    this._dataService.get('/api/appUser/getuserbygroup')
      .subscribe((response: any[]) => {
        this.FullNameOption = [];
        for (let user of response) {
          this.FullNameOption.push({ id: user.Id, name: user.FullName + " ( " + user.UserName + " ) " });
        }
      }, error => this._dataService.handleError(error));
  }

  public loadType() {
    this.userID = this._authenService.getLoggedInUser().id;
    this._dataService.get('/api/entitleday/getalltypefilter?userID=' + this.userID)
      .subscribe((response: any) => {
        this.DayOffTypeOption = [];
        for (let entitle of response) {
          this.DayOffTypeOption.push({ id: entitle.ID, name: entitle.HolidayType });
        }
      }, error => this._dataService.handleError(error));
  }
  // Function Paging
  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.loadsuccess = false;
    this.loadData();
  }
  calShowingPage() {
    this.currentMaxEntries = this.pageIndex * this.pageSize;
    if (this.currentMaxEntries >= this.totalRow) {
      this.currentMaxEntries = this.totalRow;
    }
  }

  // Function Paging Sort
  sort(property) {
    if (property === 'FullName' && !this._authenService.hasPermission('ENTITLEDAY_LIST', 'readAllAdmin')) {
      return;
    }
    this.handleSort(property);
  }

  public handleSort(property) {
    this.isDesc = !this.isDesc; // change the direction
    this.column = property;
    this.pageIndex = 1;
    this.loadData();
  }
  public filter() {
    this.ListFilter = new ListEntitleDayFilter(this.FullNameFilter, this.chosenType);
    this.pageIndex = 1;
    this.loadData();
  }
  public reset() {
    this.FullNameFilter = [];
    this.chosenType = [];
    this.ListFilter = null;
    this.pageIndex = 1;
    this.loadData();
  }
  Export() {
    this._dataService.post('/api/entitleday/exportexcel?userID=' + this.userID + '&groupId=' + this.groupID + '&column=' + this.column
      + '&isDesc=' + this.isDesc + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize, JSON.stringify(this.ListFilter))
      .subscribe((response: any[]) => {
        // this.entitleday = response;
        if (response != null) {
          window.location.href = SystemConstants.BASE_API + response;
          this.loadData();
        } else {
          this._notificationService.printSuccessMessage(MessageConstants.DOWNLOAD_OK_ERROR);
        }
      }, error => this._dataService.handleError(error));
  }

  //Show Edit Entitle Day Management
  showEditModal(id: any) {
    this.isCreate = false;
    this.loadDetail(id);
    this.modalAddEdit.show();
  }
  // Function Detail
  loadDetail(id: any) {
    this._dataService.get('/api/entitledayappuser/detail/' + id)
      .subscribe((response: any) => {
        this.entity = response[0];
      });
  }
  //Hide popup Entitle Day Management
  hideModal(form: NgForm) {
    if (form.dirty) {
      this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_CLOSE_ENTITLE_DAY_MSG, () => {
        this.modalAddEdit.hide();
        this.entity = [];
        form.reset();
      })

    } else {
      this.modalAddEdit.hide();
    }

  }

  resetForm(form: NgForm) {
    if (this.isCreate == true) {
      form.resetForm();
    }
    else {
      form.resetForm();
      this.loadDetail(this.entity.EntitleDayAppUserId.toString());
    }
  }
  private saveData(form: NgForm) {
    if (this.entity.Description != null) {
      this.entity.Description = this.entity.Description.trim();
    }
    this._dataService.put('/api/entitledayappuser/update', JSON.stringify(this.entity))
      .subscribe((response: any) => {
        this.loadData();
        this.modalAddEdit.hide();
        form.resetForm();
        this._notificationService.printSuccessMessage(MessageConstants.UPDATED_OK_MSG);
      }, error => this._dataService.handleError(error));
  }
}