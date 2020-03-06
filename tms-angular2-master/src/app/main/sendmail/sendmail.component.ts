import { Component, OnInit, ViewChild } from '@angular/core';
import { AuthenService } from 'app/core/services/authen.service';
import { DataService } from 'app/core/services/data.service';
import { UtilityService } from 'app/core/services/utility.service';
import { Response } from '@angular/http/src/static_response';
import { NotificationService } from 'app/core/services/notification.service';
import { MessageConstants } from 'app/core/common/message.constants';
import { NgForm } from '@angular/forms';
import { SendMail } from '../../core/models/sendMail';
import { ModalDirective } from 'ngx-bootstrap/modal';
import { SystemConstants } from '../../core/common/system.constants';
import { FilterGroup } from '../../core/models/filterGroup'
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { CommonConstants } from 'app/core/common/common.constants';
import { HttpErrorResponse } from '@angular/common/http/src/response';
import { HttpClientModule, HttpClient } from '@angular/common/http';
import { PageConstants } from 'app/core/common/page.constans';
import { UploadService } from 'app/core/services/upload.service';
import { constructDependencies } from '@angular/core/src/di/reflective_provider';
import { DefaultColunmConstants } from 'app/core/common/defaultColunm.constant';
import { FunctionConstants } from '../../core/common/function.constants';

@Component({
  selector: 'app-sendmail',
  templateUrl: './sendmail.component.html',
  styleUrls: ['./sendmail.component.css']
})
export class SendmailComponent implements OnInit {

  filesToUpload: Array<File>;
  selectedFileNames: string[] = [];
  public isLoadingData: Boolean = false;
  public baseFolder: string = SystemConstants.BASE_API;
  @ViewChild('file') file: any;
  @ViewChild('listMember') public listMember: ModalDirective;
  @ViewChild("imagePath") imagePath;
  public entity: SendMail = new SendMail();
  public checkChecked: any[];
  public checkbox: string[];
  pagingSize: number = PageConstants.pagingSize;
  public pageIndex: number = PageConstants.pageIndex;
  public pageSize: number = PageConstants.pageSize;
  public totalRow: number;
  public currentMaxEntries: number;
  public filterGroup: FilterGroup = null;
  public search: string = '';
  public groupFilter: string[];
  public groupOption: IMultiSelectOption[] = [];
  public users: any[];
  isDesc: boolean = false;
  listEmail: any;
  column: string = DefaultColunmConstants.UserNameColunm;

  //group filter setting
  GroupFilterSettings: IMultiSelectSettings = {
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 1,
    autoUnselect: true,
    closeOnSelect: true,
    selectionLimit: 1,
    maxHeight: '200px',
  };

  //text configuration dropdown group
  GroupFilterDropdownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectGroupType,
    checked: CommonConstants.CheckGroupType,
    checkedPlural:CommonConstants.CheckGroupType,
    defaultTitle: CommonConstants.chooseGroup
  };

  // Text configuration Dropdown List Member Group
  UserGroupDropdownTexts: IMultiSelectTexts = {
    defaultTitle: CommonConstants.SearchUsername,
  };

  // Setting dropdown with search
  dropdownSearchSettings: IMultiSelectSettings = {
    enableSearch: true,
    autoUnselect: true,
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 1,
    closeOnClickOutside: true,
    maxHeight: '200px',
  };

  constructor(
    private _dataService: DataService,
    private _notificationService: NotificationService,
    private _authenService: AuthenService,
    private _utilityService: UtilityService,
    private _uploadService: UploadService) {
    if (_authenService.getLoggedInUser().groupId.length == 0) {
      _utilityService.navigateToMain();
    }
    if (!_authenService.hasPermission(FunctionConstants.SendMail, FunctionConstants.Read)) {
      _utilityService.navigateToMain();
    }
    this.filesToUpload = [];
    this.selectedFileNames = [];
    this.checkChecked = [];
  }

  ngOnInit() {
    this.GetListMember();
    this.loadGroup();
    this.entity = new SendMail();
    this.entity.toEmail = [];
  }

  //#region "Innit data"
  public GetListMember() {
    this._dataService.post('/api/human/allmember?page=' + this.pageIndex + '&pageSize=' + this.pageSize + '&column=' + this.column + '&search=' + this.search + '&isDesc=' + this.isDesc, JSON.stringify(this.filterGroup))
      .subscribe((response: any) => {
        this.users = response.Items;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.totalRow = response.TotalRows;
        this.checkCheckBox();
        this.calShowingPage();
      }, error => this._dataService.handleError(error));
  }
  //event of tinymce
  public keyupHandlerContentFunction(e: any) {
    this.entity.Content = e;
  }

  //method reset filter after
  public reset() {
    this.groupFilter = [];
    this.filterGroup = null;
    this.search = '';
    this.pageIndex = PageConstants.pageIndex;
    this.GetListMember();
  }

  //method filter request assign
  public filterByGroup() {
    this.pageIndex = PageConstants.pageIndex;
    this.filterGroup = new FilterGroup(this.groupFilter[0]);
    this.GetListMember();
  }

  multiFileEvent(fileInput: any) {
    //Clear Uploaded Files result message
    this.selectedFileNames = [];
    this.filesToUpload = <Array<File>>fileInput.target.files;
    for (let i = 0; i < this.filesToUpload.length; i++) {
      this.selectedFileNames.push(this.filesToUpload[i].name);
    }
  }

  //method send mail to
  private SendMail(form: NgForm) {
    if (this.entity.toEmail.length == 0) {
      this._notificationService.printErrorMessage(MessageConstants.SEND_MAIL_NO_CHOSEN_MEMBER_MSG);
      if (this.entity.Content == undefined || this.entity.Subject == undefined)
        this._notificationService.printErrorMessage(MessageConstants.SEND_MAIL_NO_INPUT_MSG);
    }
    else {
      let MaxContentLength = DefaultColunmConstants.Size;
      for (let items of this.filesToUpload) {
        if (items.size > MaxContentLength) {
          this._notificationService.printErrorMessage(MessageConstants.FILE_MAX_LENG);
          return;
        }
      }
      if (this.filesToUpload.length > 0) {
        this._uploadService.postWithFile('/api/upload/saveImage?type=sendmail', null, this.filesToUpload).then((filesToUpload: string) => {
          this.entity.attackFile = this.selectedFileNames;
          this.isLoadingData = true;
          this._dataService.post('/api/human/sendemail', JSON.stringify(this.entity))
            .subscribe((response: Response) => {
              this._notificationService.printSuccessMessage(MessageConstants.SEND_MAIL_OK_MSG);
              this.isLoadingData = false;
              this.resetSendMail();
              this.listEmail = '';
            }, error => {
              if (error.status == 400) {
                this._notificationService.printErrorMessage(MessageConstants.SEND_MAIL_NO_INPUT_MSG);
                this.isLoadingData = false;
              }
              else if (error.status == 404) {
                this._notificationService.printErrorMessage(MessageConstants.SEND_MAIL_ERROR_MSG);
                this.isLoadingData = false;
              }
            });
        });
      }
      else {
        this.isLoadingData = true;
        this._dataService.post('/api/human/sendemail', JSON.stringify(this.entity))
          .subscribe((response: Response) => {
            this._notificationService.printSuccessMessage(MessageConstants.SEND_MAIL_OK_MSG);
            this.isLoadingData = false;
            this.resetSendMail();
            this.listEmail = '';
          }, error => {
            if (error.status == 400) {
              this._notificationService.printErrorMessage(MessageConstants.SEND_MAIL_NO_INPUT_MSG);
              this.isLoadingData = false;
            }
            else if (error.status == 404) {
              this._notificationService.printErrorMessage(MessageConstants.SEND_MAIL_ERROR_MSG);
              this.isLoadingData = false;
            }
          });
      }
    }
  }

  //#endregion "Innit data"

  //change on click textbox
  checkBoxList(value: any, checked: any) {
    if (checked == true)
      this.entity.toEmail.push(value.Email);
    if (checked == false)
      this.entity.toEmail = this.entity.toEmail.filter(item => item !== value.Email);
  }

  //action check checked
  checkCheckBox() {
    for (let items of this.users) {
      var filterChecked = this.entity.toEmail.filter(x => x == items.Email);
      if (filterChecked.length > 0)
        items.Checked = true;
    }
  }

  //action load group
  loadGroup() {
    this._dataService.get('/api/group/getallgroup')
      .subscribe((response: any) => {
        this.groupOption = [];
        for (let status of response) {
          this.groupOption.push({ id: status.ID, name: status.Name })
        }
      }, error => this._dataService.handleError(error));
  }

  //action show list member using modal
  showListMemberModal() {
    this.listMember.show();
    this.reset();
    this.pageIndex = PageConstants.pageIndex;
  }

  getCheckCondition(): any {
    if (this.users != null) {
      var list = this.users.filter(function (data) {
        return data;
      });
      return list;
    }
  }

  //check checkAll checked or not
  checkAllchecked() {
    var result = this.getCheckCondition();
    return this.getCheckCondition().length == 0;
  }

  //change checkAll on click
  public checkAll(ev, checked: any) {
    this.getCheckCondition().forEach(x => x.Checked = ev.target.checked)
    if (!checked) {
      this.entity.toEmail = [];
      for (let item of this.getCheckCondition()) {
        this.entity.toEmail.push(item.Email);
      }
    }
    if (checked) {
      this.entity.toEmail = [];
    }
  }

  //condition of check all
  isAllChecked() {
    if (!this.checkAllchecked()) {
      return this.getCheckCondition().every(_ => _.Checked);
    }
  }

  //close want to close
  public closeForm(modal: ModalDirective) {
    this.listMember.hide();
    if (this.listEmail == undefined) {
      this.entity.toEmail = [];
    }
    else {
      for (let item of this.users) {
        var checked = this.listEmail.filter(a => a == item.Email);
        if (checked == item.email)
          item.Checked = true;
        else{
          item.Checked = false;
          this.entity.toEmail = this.listEmail;
        }
      }
    }
  }

  //action hide modal but still keep checked
  hideModal(form: NgForm) {
    this.listMember.hide();
    this.listEmail = this.entity.toEmail.filter((v, i, a) => a.indexOf(v) === i);
  }

  //action reset field of form send mail
  resetSendMail() {
    var my_editor_id = 'my-editor-id';
    //set the content empty
    tinymce.get(my_editor_id).setContent('');
    this.file.nativeElement.value = "";
    this.selectedFileNames = [];
    this.filesToUpload = [];
    this.entity.toEmail = [];
    this.entity.Subject = '';
    this.entity.Content = '';
    this.entity.attackFile = [];
  }

  //action change page
  pageChanged(event: any): void {
    this.pageIndex = event.page;
    this.GetListMember();
    this.calShowingPage();
  }

  //action show current page
  calShowingPage() {
    this.currentMaxEntries = this.pageIndex * this.pageSize;
    if (this.currentMaxEntries >= this.totalRow) {
      this.currentMaxEntries = this.totalRow;
    }
  }

}
