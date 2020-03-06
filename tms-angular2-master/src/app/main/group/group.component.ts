import { Component, OnInit, ViewChild, group,ChangeDetectorRef } from '@angular/core';
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
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { UserGroup } from '../../core/models/usergroup';
import { fail } from 'assert';
import { element } from 'protractor';
import { forEach } from '@angular/router/src/utils/collection';
import { isArray } from 'util';
import { PageConstants } from 'app/core/common/page.constans';
import { FunctionConstants } from 'app/core/common/function.constants';
import { DateTimeConstants } from 'app/core/common/datetime.constants';
import { LoggedInUser } from 'app/core/domain/loggedin.user';
import { SendMailDelegateRequest } from '../../core/models/SendMailDelegateRequest';
import { SendMailModel } from '../../core/models/sendMailModel';
import { MailConstants } from '../../core/common/mail.constants';
import { SendMailDelegateModel } from '../../core/models/SendMailDelegateModel';
import { from } from 'rxjs/observable/from';
import { DaterangepickerConfig } from '../../core/services/config.service';
import * as moment from 'moment';
@Component({
  selector: 'app-group',
  templateUrl: './group.component.html',
  styleUrls: ['./group.component.css']
})
export class GroupComponent implements OnInit {
  @ViewChild('modalAddEdit') public modalAddEdit: ModalDirective;
  @ViewChild('modalDelegate') public modalDelegate: ModalDirective;
  public baseFolder: string = SystemConstants.BASE_API;
  public listGroup: any[];
  public listGroupDelete: any[] = [];
  public userID: string;
  public value: any= {
    end: '',
    start: ''
  };
  public groupID: string;
  public groupItemSelected: UserGroup = new UserGroup();
  public newGroup: UserGroup = new UserGroup();
  public groupLeadIDSelected: any = [];
  public totalRow: number;
  pagingSize: number = PageConstants.pagingSize;
  public pageIndex: number = PageConstants.pageIndex;
  public pageSize: number = PageConstants.pageSize;
  public currentMaxEntries: number;
  isCreate: boolean = false;
  isUpdate: boolean = false;
  loadsuccess: boolean = false;
  count: number = 0;
  public checkButton: boolean = false;
  public usersOption: IMultiSelectOption[] = [];
  public usersOptionCreate: IMultiSelectOption[] = [];
  public usersOptionUpdate: IMultiSelectOption[] = [];
  public roleUser: any[];
  public filterDelegator: IMultiSelectOption[] = [];
  public myFilterDelegators: string[] = [];
  public listGrantUser: any[];
  public groupIDDelegate: string;
  public userLogin: LoggedInUser;
  public requests: any[];
  public listRequestId: string[] = [];
  public filterDelegatorLoadData: any[] = [];
  public fullNameDelegate: string;
  public lstRequestByUser: any[] = [];
  public lstRequestId: string[] = [];
  public lstExplanationRequestByUser: any[] = [];
  listMail: Array<SendMailModel> = [];
  listMailExplanation: Array<SendMailModel> = [];
  public lstExplanationRequestId: string[] = [];
  public groupLeadIDTmp : any = [];
  public checkUpdate : boolean = false;
  public checkGroupName : boolean = false;
  public isDisableResetButton:boolean = false;
  // Setting dropdown with search
  dropdownSearchSettings: IMultiSelectSettings = {
    enableSearch: true,
    autoUnselect: true,
    closeOnSelect: true,
    selectionLimit: 1,
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 10,
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
  // Text configuration Dropdown Group Lead
  GroupLeadDropdownTexts: IMultiSelectTexts = {
    defaultTitle: CommonConstants.GroupLeadDropDownList,
    checkedPlural: CommonConstants.checkedPlural
  };
  constructor(public _authenService: AuthenService,
    public _notificationService: NotificationService,
    private _dataService: DataService,
    private _utilityService: UtilityService,
    private daterangepickerOptions: DaterangepickerConfig,
    private cdRef:ChangeDetectorRef
  ) {
    if (_authenService.getLoggedInUser().groupId.length == 0) {
      _utilityService.navigateToMain();
    }
    if (!_authenService.hasPermission(FunctionConstants.Group, FunctionConstants.Read)) {
      _utilityService.navigateToMain();
    }
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
  ngOnInit() {
    this.loadListGroupLead();
    this.loadData();
    this.calShowingPage();
    this.userLogin = this._authenService.getLoggedInUser();
  }
  //#region "Innit data"
  // Function Load List Group
  public loadData() {
    this.userID = this._authenService.getLoggedInUser().id;
    this.groupID = this._authenService.getLoggedInUser().groupId;
    this.roleUser = JSON.parse(this._authenService.getLoggedInUser().roles)
    this._dataService.get('/api/appUser/getuserbydelegate')
      .subscribe((response: any) => {
        this.filterDelegatorLoadData = [];
        this.filterDelegatorLoadData = response;
        this._dataService.get('/api/group/getallgroup?page=' + this.pageIndex + '&pageSize=' + this.pageSize + '&roleName=' + this.roleUser[0] + '&userID=' + this.userID)
          .subscribe((response: any) => {
            this.listGroup = response.Items;
            this.fullNameDelegate = null;
            if ((this.roleUser[0].localeCompare("GroupLead")) == 0) {
              if (this.filterDelegatorLoadData.find(x => x.Id == this.listGroup[0].DelegateId) != null) {
                this.fullNameDelegate = this.filterDelegatorLoadData.find(x => x.Id == this.listGroup[0].DelegateId).FullName
              }
            }
            this.totalRow = response.TotalRows;
            this.pageIndex = response.PageIndex;
            this.pageSize = response.PageSize;
            this.calShowingPage();
            // this.isCheckAll = false;
            this.loadsuccess = true;
            this.checkCheckBox();


          }, error => this._dataService.handleError(error));
      }, error => this._dataService.handleError(error));

  }
  loadDataDetail(id: string) {
    this._dataService.get('/api/group/detail/' + id).subscribe((response: any) => {
      this.newGroup = response;
    }, error => this._dataService.handleError(error));
  }
  //Create
  loadListGroupLead() {
    this._dataService.get('/api/appUser/getgroupleadtoassign')
      .subscribe((response: any) => {
        this.groupLeadIDSelected = [];
        this.usersOption = [];
        this.usersOptionCreate = [];
        this.usersOptionUpdate =[];
        for (let GroupLead of response) {
          this.usersOptionCreate.push({ id: GroupLead.Id, name: GroupLead.FullName + " ( " + GroupLead.UserName + " ) "});
        }
      }, error => this._notificationService.printErrorMessage(MessageConstants.LoadLeaderFail));
  }
  loadListEditGroupLead(item: UserGroup) {
    this._dataService.get('/api/appUser/getgroupleadtoassign')
      .subscribe((response: any) => {
        this.usersOptionUpdate = [];
        for (let GroupLead of response) {
          this.usersOptionUpdate.push({ id: GroupLead.Id, name: GroupLead.FullName + " ( " + GroupLead.UserName + " ) " });
        }
        this.usersOptionUpdate.push({ id: item.GroupLeadID, name: item.GroupLead + " ( " + item.GroupLeadAccount + " ) "});
      }, error => this._notificationService.printErrorMessage(MessageConstants.LoadLeaderFail));
  }

  //#endregion
  // show modal
  showAddModal() {
    this.modalAddEdit.show();
  }
  showEditModal(item: UserGroup) {
    this.loadListEditGroupLead(item);
    this.groupLeadIDTmp=[];
    this.checkGroupName =false;
    this.checkUpdate = true;
    this.usersOption.push({ id: item.GroupLeadID, name: item.GroupLeadAccount });
    this.usersOptionUpdate.push({ id: item.GroupLeadID, name: item.GroupLead + " ( " + item.GroupLeadAccount + " ) " });
    this.groupLeadIDTmp.push(item.GroupLeadID);
    this.groupLeadIDSelected.push(item.GroupLeadID)
    this.cdRef.detectChanges();
    this.modalAddEdit.show();
  }
  hideModal(form: NgForm) {
    if(this.checkUpdate)
    {
      if((this.groupLeadIDTmp[0].localeCompare(this.groupLeadIDSelected[0]))==0 && this.checkGroupName == false)
      {
        this.modalAddEdit.hide();
        this.checkUpdate = false;
        this.usersOption =[];
        this.usersOptionUpdate =[];
        this.loadListGroupLead();
      }else {
          this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_CLOSE_GROUP_MSG, () => {
            form.reset();
            this.modalAddEdit.hide();
            this.usersOption =[];
            this.loadListGroupLead();
            this.checkGroupName == false;
            this.checkUpdate = false;
          })
      }
    }else
    {
      if (form.dirty) {
        this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_CLOSE_GROUP_MSG, () => {
          form.reset();
          this.modalAddEdit.hide();
        })
      } else {
        this.modalAddEdit.hide();
      }
    }
    
  }

  //close form delegate
  public closeForm(modal: ModalDirective) {
    modal.hide();
  }
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
    if(item.DelegateId != null)
    {
      this.isDisableResetButton= true;
    }
    this.groupItemSelected = item;
    var groupLead: '';
    this.newGroup = item;
    this._dataService.get('/api/appUser/getgrouplead?groupId=' + item.ID)
      .subscribe((response: any) => {
        this.listGrantUser = [];
        for (let userGroup of response) {
          this.listGrantUser.push({ id: userGroup.Id, name: userGroup.FullName });
        }
        groupLead = this.listGrantUser.length > 0 ? this.listGrantUser[0].id : null;
      }, error => this._dataService.handleError(error));
    this._dataService.get('/api/appUser/getuserbygroup')
      .subscribe((response: any) => {
        this.filterDelegator = [];
        this.myFilterDelegators = [];
        for (let userGroup of response) {
          if (this.userLogin.id != userGroup.Id) {
            this.filterDelegator.push({ id: userGroup.Id, name: userGroup.FullName + " ( " + userGroup.UserName + " ) " })
          }
        }
        var index = this.filterDelegator.findIndex(x => x.id == groupLead);
        this.filterDelegator.splice(index, 1);
      }, error => this._dataService.handleError(error));
    this.modalDelegate.show();
  }
  //#region "CRUD"
  createGroup(form: NgForm) {
    this.newGroup = new UserGroup();
    this.isCreate = true;
    this.isUpdate = false;
    form.resetForm();
    //this.loadListGroupLead();
    this.showAddModal();
    this.count = 0;
  }
  editGroup(item: UserGroup, isEdit: boolean) {
    this.count = 0;
    this.groupItemSelected = item;
    this.isUpdate = isEdit;
    this.isCreate = false;
    this.loadDataDetail(item.ID.toString());
    this.newGroup.GroupLead = item.GroupLead;
    this.groupLeadIDSelected = [];
    this.showEditModal(item);
  }
  edit() {
    this.isUpdate = true;
    this.isCreate = false;
  }
  private saveData(form: NgForm) {
    this.newGroup.ID = this.newGroup.ID;
    this.newGroup.Name = this.newGroup.Name != null ? this.newGroup.Name.trim() : CommonConstants.StringEmpty;
    this.newGroup.Description = this.newGroup.Description != null ? this.newGroup.Description.trim() : CommonConstants.StringEmpty;
    if (this.isCreate) {
      if (this.groupLeadIDSelected.length > 0) {
        this.newGroup.GroupLeadID = this.groupLeadIDSelected[0];
      }
      this._dataService.post('/api/group/add', JSON.stringify(this.newGroup))
        .subscribe((response: any) => {
          this.modalAddEdit.hide();
          this.loadData();
          this.loadListGroupLead();
          this._notificationService.printSuccessMessage(MessageConstants.CREATED_OK_MSG);
        }, error => this._dataService.handleError(error));
    } else if (this.isUpdate) {
      if (this.groupLeadIDSelected.length > 0) {
        this.newGroup.GroupLeadID = this.groupLeadIDSelected[0];
      } else {
        this.newGroup.GroupLeadID = this.groupItemSelected.GroupLeadID;
      }
      this._dataService.put('/api/group/update', JSON.stringify(this.newGroup))
        .subscribe((response: any) => {
          this.modalAddEdit.hide();
          this.loadData();
          this.loadListGroupLead();
          this._notificationService.printSuccessMessage(MessageConstants.UPDATED_OK_MSG);
          form.resetForm();
        }, error => this._dataService.handleError(error));
    }
  }
  deleteMulti() {
    // this.listGroupDelete = [];
    // for (let x of this.listGroup) {
    //   if (x.Checked) {
    //     this.listGroupDelete.push(x);
    //   }
    // }
    if (this.listGroupDelete.length == 1) {
      this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_GROUP_DELETE_MSG,
        () => this.deleteMultiItemConfirm(this.listGroupDelete));
    }
    else if (this.listGroupDelete.length > 1) {
      this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_DELETE_MULTI_MSG,
        () => this.deleteMultiItemConfirm(this.listGroupDelete));
    } else {
      this._notificationService.printErrorMessage(MessageConstants.SelectGroupToDelete);
    }
  }
  deleteMultiItemConfirm(listGroupDelete: any) {
    this._dataService.post('/api/group/delete-multi', JSON.stringify(this.listGroupDelete))
      .subscribe((response: any) => {
        this.pageIndex = 1;
        this.loadData();
        this.loadListGroupLead();
        for (let groupDeleted of response) {
          this.listGroupDelete.splice(this.listGroupDelete.indexOf(groupDeleted), 1);
        }
        this.modalAddEdit.hide();
        this._notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG);
      }, error => this._dataService.handleError(error));
  }
  delete(groupId: any) {
    this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_GROUP_DELETE_MSG, () => this.deleteItemConfirm(groupId));
  }
  deleteItemConfirm(id: any) {
    this._dataService.delete('/api/group/delete', 'id', id).subscribe((response: Response) => {
      this.pageIndex = 1;
      this._notificationService.printSuccessMessage(MessageConstants.DELETED_OK_MSG);
      this.loadData();
      this.loadListGroupLead();
    }, error => this._dataService.handleError(error));
  }

  resetDelegate() {
    this._notificationService.printConfirmationDialog(MessageConstants.CONFIRM_DELEGATE_RESET_MSG, () => this.resetDelegateConfirm(this.groupItemSelected.ID));
  }

  resetDelegateConfirm(id: any) {
    this._dataService.delete('/api/group/resetDelegation', 'id', id).subscribe((response: Response) => {
      this.pageIndex = 1;
      this._notificationService.printSuccessMessage(MessageConstants.RESET_OK_MSG);
      this.loadData();
      this.modalDelegate.hide();
      this.isDisableResetButton = false;
    }, error => this._dataService.handleError(error));
  }

  saveDelegate(valid: boolean) {
    this.delegateGroup();
  }
  delegateGroup() {
    this.userLogin = this._authenService.getLoggedInUser();
    this.newGroup.StartDate = moment(new Date(this.chosenRequestDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this.newGroup.EndDate = moment(new Date(this.chosenRequestDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this.newGroup.DelegateId = this.myFilterDelegators[0];
    this._dataService.put('/api/group/setDelegateDefault?userId=' + this.userLogin.id + '&groupId=' + this.userLogin.groupId, JSON.stringify(this.newGroup))
      .subscribe((response: any) => {
        this.value.start = moment();
        this.value.end = moment();
        this.selectedAddRequestDatePicker(this.value,this.chosenRequestDate)
        for (let item of response) {
          if (item.RequestType !== undefined) {
            this.lstRequestId.push(item.ID);
          } else {
            this.lstExplanationRequestId.push(item.ID);
          }
        }
        // this.lstRequest = response;
        this.modalDelegate.hide();
        this.loadData();
        // this.loadListGroupLead();
        this._notificationService.printSuccessMessage(MessageConstants.UPDATED_OK_MSG);

        //send mail delegate default request
        this._dataService.put('/api/request/getallrequestbyuser?userId=' + this.userLogin.id + '&groupId=' + this.userLogin.groupId, JSON.stringify(this.lstRequestId))
          .subscribe((response: any) => {
            this.listMail = [];
            this.lstRequestByUser = response;
            for (let item of this.lstRequestByUser) {
              this.listMail.push(new SendMailDelegateRequest(item.AppUser.Group.Name, item.RequestType.Name, item.DetailReason,
                item.StartDate, item.EndDate, 'Title', item.AppUser.Id, this.myFilterDelegators, MailConstants.Delegation, MailConstants.DelegationAssignedList, MailConstants.RequestSubject, null, MailConstants.DelegationManagement, item.DelegateId, item.toEmail));
            }
            this._dataService.post('/api/System/sendMailMultiFix', JSON.stringify(this.listMail)).subscribe((response: any) => { });

            //send mail explanation
            this._dataService.put('/api/explanation/getlistexplanationdetail', JSON.stringify(this.lstExplanationRequestId))
              .subscribe((responseExplanation: any) => {
                this.listMailExplanation = [];
                this.lstExplanationRequestByUser = responseExplanation;
                for (let item of this.lstExplanationRequestByUser) {
                  this.listMailExplanation.push(new SendMailDelegateModel('Explanation Request: ' + item.Title, item.Receiver.FullName, this.myFilterDelegators, MailConstants.Delegation, MailConstants.DelegationAssignedList, MailConstants.DelegationExplanationRequest, null,
                    MailConstants.DelegationManagement, item.User.Group.Name, item.ExplanationDate, item.ReasonList, item.ReasonDetail, item.User.Id, item.Receiver.Id));
                }
                this._dataService.post('/api/System/sendMailMultiFix', JSON.stringify(this.listMailExplanation)).subscribe((response: any) => { });
              });
          });


      }, error => this._dataService.handleError(error));
  }
  //#endregion

  //#region "Business logic"
  public checkAll(ev) {
    this.listGroup.forEach(x => x.Checked = ev.target.checked);
    if (ev.target.checked) {
      for (let item of this.listGroup) {
        if (!this.IsArrayIncludeItem(this.listGroupDelete, item)) {
          this.listGroupDelete.push(item);
          this.checkButton = true;
        }
      }
    } else {
      for (let item of this.listGroup) {
        if (this.IsArrayIncludeItem(this.listGroupDelete, item)) {
          this.listGroupDelete = this.removeItemArray(item, this.listGroupDelete);
        }
      }
    }
    if (this.listGroupDelete.length > 0) {
      this.checkButton = true;
    } else {
      this.checkButton = false;
    }
  }
  isAllChecked() {
    return this.listGroup.every(_ => _.Checked);
  }
  CheckboxChange(item: UserGroup, event: any) {
    if (event.target.checked) {
      this.listGroupDelete.push(item);
    } else {
      this.listGroupDelete = this.removeItemArray(item, this.listGroupDelete);
    }
    if (this.listGroupDelete.length > 0) {
      this.checkButton = true;
    } else {
      this.checkButton = false;
    }
  }
  resetForm(form: NgForm) {
    if (this.isCreate) {
      form.resetForm();
      this.loadListGroupLead();
      this.groupLeadIDSelected = [];
      this.count = 0;
    }
    if (this.isUpdate) {
      form.resetForm();
      this.loadListEditGroupLead(this.groupItemSelected);
      this.loadDataDetail(this.newGroup.ID.toString());
      this.groupLeadIDSelected = this.groupItemSelected.GroupLeadID;
    }
  }
  onChange() {
    this.count++;
  }
  clearText(event: any, dropdownSearch: any) {
    dropdownSearch.clearSearch(event);
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
    for (let item of this.listGroup) {
      for (let itemDelete of this.listGroupDelete) {
        if (item.ID === itemDelete.ID)
          item.Checked = true;
      }
    }
  }
  IsArrayIncludeItem(array: any, item: any): boolean {
    for (let value of this.listGroupDelete) {
      if (value.ID === item.ID) {
        return true;
      }
    }
    return false;
  }

  public selectedAddRequestDatePicker(value: any, dateInput: any) {
    this.value = value;
    dateInput.start = value.start.format();
    dateInput.end = value.end;
  }


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

  public chosenRequestDate: any = {
    start: moment(),
    end: moment(),

  };

  onChangeGroupName(event: any)
  {
    this.checkGroupName = true;
  }
  //#endregion
}
