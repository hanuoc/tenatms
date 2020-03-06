import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthenService } from 'app/core/services/authen.service';
import { DataService } from 'app/core/services/data.service';
import { NotificationService } from '../../core/services/notification.service';
import { UtilityService } from 'app/core/services/utility.service';
import { NgForm } from '@angular/forms';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { MessageConstants } from 'app/core/common/message.constants';
import { AnimationKeyframesSequenceMetadata } from '@angular/core/src/animation/dsl';

@Component({
  selector: 'app-timeday',
  templateUrl: './timeday.component.html',
  styleUrls: ['./timeday.component.css']
})

export class TimedayComponent implements OnInit {
  @ViewChild('modalAddEdit') public modalAddEdit: ModalDirective;
  @ViewChild('addEditForm') public addEditForm: NgForm;
  public timedaylist: any[];
  public entity: any;
  isCreate: boolean = false;
  isSA: boolean = false;
  checkInTime: string;
  checkOutTime: string;
  constructor(public _authenService: AuthenService,
    private _dataService: DataService,
    private _notificationService: NotificationService,
    private _utilityService: UtilityService) { 
      if(_authenService.getLoggedInUser().groupId.length == 0){
        _utilityService.navigateToMain();
      }
    }

  ngOnInit() {
    this.loadData();
    this.isSA = this.checkRoleSA();
  }
  public loadData() {
    this._dataService.post('/api/timeday/getall').subscribe((response: any) => {
      this.timedaylist = response;
    }, error => this._dataService.handleError(error));
  }
  //#region Show modal
  showAddModal() {
    this.isCreate = true;
    this.entity = {};
    this.modalAddEdit.show();
  }
  showEditModal(id: any) {
    this.isCreate = false;
    this.entity = {}
    this.showEdit(id);
    this.modalAddEdit.show();
  }
  hideModal(form: NgForm) {
    if(form.dirty)
    {
      this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_CLOSE_TIMEDAY_MSG, () => {
        form.reset();
        this.modalAddEdit.hide();
      })
    }else
    {
      form.reset();
      this.modalAddEdit.hide();
    }
    
  }

  //#endregion
  //Action delete
  public deleteConfirm(id: any): void {
    this._dataService.delete('/api/timeday/delete', 'id', id).subscribe((response: Response) => {
      this._notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG);
      this.loadData();
    }, error => this._dataService.handleError(error));
  }
  //Click button delete turn on confirm
  public delete(id: number) {
    this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_DELETE_TIMEDAY_MSG, () => this.deleteConfirm(id));
  }
  /// Create && Update
  private saveData(form: NgForm) {
    // if(this.entity.CheckOut>this.entity.CheckIn){
      this.entity.Workingday = this.entity.Workingday.trim();
      if (this.entity.ID == undefined) {
        this._dataService.post('/api/timeday/add', JSON.stringify(this.entity))
          .subscribe((response: any) => {
            this.loadData();
            this.modalAddEdit.hide();
            form.resetForm();
            this._notificationService.printSuccessMessage(MessageConstants.CREATED_OK_MSG);
          }, error => this._dataService.handleError(error));
      }
      else {
        this._dataService.put('/api/timeday/update', JSON.stringify(this.entity))
          .subscribe((response: any) => {
            this.loadData();
            this.modalAddEdit.hide();
            form.resetForm();
            this._notificationService.printSuccessMessage(MessageConstants.UPDATED_OK_MSG);
          }, error => this._dataService.handleError(error));
      }
    // }
    // else{
    //   this._notificationService.printErrorMessage(MessageConstants.DateTime_ERROR_MSG);
    // }
   
  }
  /// Reset form
  resetForm(form: NgForm) {
    if (this.isCreate == true) {
      form.resetForm();
      this.checkInTime = null;
      this.checkOutTime =null;
    }
    else {
      form.resetForm();
      this.showEdit(this.entity.ID);
    }
  }
  /// Show edit form
  public showEdit(id: any) {
    this._dataService.get('/api/timeday/detail/' + id).subscribe((response: any) => {
      this.entity = response;
      this.modalAddEdit.show();
    }, error => this._dataService.handleError(error));
  }
  
  checkRoleSA() {
    if (this._authenService.getLoggedInUser().roles === '["SuperAdmin"]') {
      return true;
    }else{
      return false;
    }
  }
}
