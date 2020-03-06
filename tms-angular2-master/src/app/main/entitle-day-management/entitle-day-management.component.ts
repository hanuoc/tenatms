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
import { IMultiSelectTexts, IMultiSelectOption } from 'angular-2-dropdown-multiselect';
import { from } from 'rxjs/observable/from';
import { PageConstants } from 'app/core/common/page.constans';
import { FunctionConstants } from 'app/core/common/function.constants';

@Component({
  selector: 'app-entitle-day-management',
  templateUrl: './entitle-day-management.component.html',
  styleUrls: ['./entitle-day-management.component.css']
})
export class EntitleDayManagementComponent implements OnInit {
  @ViewChild('modalAddEdit') public modalAddEdit: ModalDirective;
  @ViewChild('addEditForm') public addEditForm: NgForm;
  public baseFolder: string = SystemConstants.BASE_API;
  public entitledaymanagement: any[];
  public userID: string;
  public groupID: string;
  public myRoles: string[] = [];
  column: string = '';
  pagingSize: number = PageConstants.pagingSize;
  public pageIndex: number = PageConstants.pageIndex;
  public pageSize: number = PageConstants.pageSize;
  public pageDisplay: number = 10;
  public totalRow: number;
  isDesc: boolean = true;
  public entity: any;
  isCreate: boolean = false;
  isCancel: boolean = false;
  constructor(
    public _authenService: AuthenService,
    private _dataService: DataService,
    private _notificationService: NotificationService,
    private _utilityService: UtilityService
  ) {
    if (_authenService.getLoggedInUser().groupId.length == 0) {
      _utilityService.navigateToMain();
    }
    if (!_authenService.hasPermission(FunctionConstants.EntitleDayAdmin, FunctionConstants.Read)) {
      _utilityService.navigateToMain();
    }
  }

  ngOnInit() {
    this.loadData();

  }
  // Function Load list view Entitle Day Management
  public loadData() {
    this.userID = this._authenService.getLoggedInUser().id;
    this.groupID = this._authenService.getLoggedInUser().groupId;
    this._dataService.get('/api/entitle-day-management/getallentitledaymanagement')
      .subscribe((response: any) => {
        this.entitledaymanagement = response;
      }, error => this._dataService.handleError(error));
  }
  //Show Add Entitle Day Management
  showAddModal(form: NgForm) {
    this.isCreate = true;
    this.entity = {};
    this.modalAddEdit.show();
  }
  //Show Edit Entitle Day Management
  showEditModal(id: any) {
    this.isCreate = false;
    this.loadDetail(id);
    this.modalAddEdit.show();
  }
  //Hide popup Entitle Day Management
  hideModal(form: NgForm) {
    if (form.dirty) {
      this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_CLOSE_ENTITLE_DAY_ADMIN_MSG, () => {
        this.modalAddEdit.hide();
        this.entity = [];
        form.reset();
      })
    } else {
      this.modalAddEdit.hide();
      this.entity = [];
    }

  }
  // Reset Form Add and Creat
  resetForm(form: NgForm) {
    if (this.isCreate == true) {
      form.resetForm();
    }
    else {
      form.resetForm();
      this.loadDetail(this.entity.ID.toString());
    }
  }
  // Function Save Data
  private saveData(form: NgForm) {
    this.entity.HolidayType = this.entity.HolidayType.trim();
    // this.entity.UnitType = this.entity.UnitType.trim();
    if (this.entity.Description != null) {
      this.entity.Description = this.entity.Description.trim();
    }
    if (this.entity.ID == undefined) {
      this._dataService.post('/api/entitle-day-management/add', JSON.stringify(this.entity))
        .subscribe((response: any) => {
          this.loadData();
          this.modalAddEdit.hide();
          form.resetForm();
          this._notificationService.printSuccessMessage(MessageConstants.CREATED_OK_MSG);
        }, error => this._dataService.handleError(error));
    }
    else {
      this._dataService.put('/api/entitle-day-management/update', JSON.stringify(this.entity))
        .subscribe((response: any) => {
          this.loadData();
          this.modalAddEdit.hide();
          form.resetForm();
          this._notificationService.printSuccessMessage(MessageConstants.UPDATED_OK_MSG);
        }, error => this._dataService.handleError(error));
    }
  }
  // Show popup delete
  deleteItem(id: any) {
    this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_DELETE_ENTITLE_MSG, () => this.deleteItemConfirm(id));
  }
  // Function Delete data
  deleteItemConfirm(id: any) {
    this._dataService.delete('/api/entitle-day-management/delete', 'id', id)
      .subscribe((response: Response) => {
        this._notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG);
        this.loadData();
      }, error => this._dataService.handleError(error));;
  }
  // Function Detail
  loadDetail(id: any) {
    this._dataService.get('/api/entitle-day-management/detail/' + id)
      .subscribe((response: any) => {
        this.entity = response;
      });
  }
}
