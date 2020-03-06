import { Component, OnInit, ViewChild, group } from '@angular/core';
import { AuthenService } from 'app/core/services/authen.service';
import { NotificationService } from '../../core/services/notification.service';
import { UtilityService } from 'app/core/services/utility.service';
import { DataService } from '../../core/services/data.service';
import { SystemConstants } from 'app/core/common/system.constants';
import { FilterTimeSheet } from 'app/core/models/filter-time-sheet';
import { DateTimeConstants } from 'app/core/common/datetime.constants';
import { CommonConstants } from 'app/core/common/common.constants';
import { IMultiSelectOption, IMultiSelectTexts, IMultiSelectSettings } from 'angular-2-dropdown-multiselect';
import { DaterangepickerConfig } from 'app/core/services/config.service';
import { FunctionConstants } from '../../core/common/function.constants';
import { DateRangePickerModel } from '../../core/models/date-range-picker';
import { PageConstants } from 'app/core/common/page.constans';
import { ModalDirective } from 'ngx-bootstrap';
import { UploadService } from '../../core/services/upload.service';
import { NgForm } from '@angular/forms';
import { MessageConstants } from '../../core/common/message.constants';
import { DefaultColunmConstants } from 'app/core/common/defaultColunm.constant';
@Component({
  selector: 'app-time-sheet',
  templateUrl: './time-sheet.component.html',
  styleUrls: ['./time-sheet.component.css']
})
export class TimeSheetComponent implements OnInit {
  //#region "Decalre variable, innit some value and so on"
  // Settings configuration
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
  dropdownSettings: IMultiSelectSettings = {
    enableSearch: false,
    autoUnselect: true,
    checkedStyle: 'fontawesome',
    buttonClasses: 'btn btn-default btn-block',
    dynamicTitleMaxItems: 1,
    closeOnClickOutside: true,
    selectionLimit: 1,
    maxHeight:'200px',
  };
  // Text configuration
  textStatusRequest: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectStatusType,
    checked: CommonConstants.CheckStatusType,
    checkedPlural:CommonConstants.CheckStatusType,
    defaultTitle: CommonConstants.chooseStatusExplanation,
  };
  textAbnormalType: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectAbsentType,
    checked: CommonConstants.CheckAbsentType,
    checkedPlural:CommonConstants.CheckAbsentType,
    defaultTitle: CommonConstants.AbnormalTimeSheetType,
  };
  //Date Option for single pick
  public datePickerOptions: any = {
    locale: DateTimeConstants.Locale,
    alwaysShowCalendars: false,
  };
  @ViewChild('modalImport') public modalImport: ModalDirective;
  @ViewChild('file') file;
  public userID: string;
  public baseFolder: string = SystemConstants.BASE_API;
  public totalRow: number;
  pagingSize: number = PageConstants.pagingSize;
  public pageIndex: number = PageConstants.pageIndex;
  public pageSize: number = PageConstants.pageSize;
  public filter: FilterTimeSheet;
  public timeSheet: any[];
  public value: any = {
    start :'',
    end :'',
  };
  column: string = DefaultColunmConstants.DayofCheck;
  isAsc:boolean = false;
  public statusRequestOption: IMultiSelectOption[] = [];
  public AbnormalTypeOption: IMultiSelectOption[] = [];
  public myFilterStatusRequests: string[] = [];
  public myFilterAbnormalTypes: string[] = [];
  public AbnormalTypes: any[];
  public myRequestType: string;
  public myAbnormalType: string;
  public currentMaxEntries: number;
  public fileUpload: string = '';
  public dateRange: string = '';
  private startDate: string;
  private endDate: string;
  loadtimeSheet:boolean = false;
  loadsuccess:boolean = false;
  public fingerTimeSheetError: any[];
  public pageIndexError: number = PageConstants.pageIndex;
  public pageSizeError: number = PageConstants.pageSize;
  public currentMaxEntriesError: number;
  public totalRowError: number;
  public fileName: string = '';
  public isSA: boolean = false;
  public FullNameOption: IMultiSelectOption[] = [];
  public FullNameFilter: string[] = [];
  //#endregion
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
  //Contructor to inject services.
  constructor(public _authenService: AuthenService,
    private _dataService: DataService,
    private _notificationService: NotificationService,
    private _utilityService: UtilityService,
    private _uploadService: UploadService,
    private daterangepickerOptions: DaterangepickerConfig) {
      if(_authenService.getLoggedInUser().groupId.length == 0){
        _utilityService.navigateToMain();
      }
    if (_authenService.checkAccess(FunctionConstants.TimeSheet) === false) {
      _utilityService.navigateToLogin();
    }
    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: true,
      "opens": "left",
      ranges: DateTimeConstants.Range,
    };
  }

  // Innit data or function
  ngOnInit() {
    $('#button').click(function () {
      $("input[type='file']").trigger('click');
    })
    this.initFilterDate();
    this.userID = this._authenService.getLoggedInUser().id;
    this.loadData();
    this.calShowingPage();
    this.getListStatusForDropdown();
    this.getListAbnormaTimeSheetType();
    this.isSA = this.checkRoleSA();
    this.loadFullName();
  }
  //#region "Innit data"
  public initFilterDate() {
    this.startDate = moment().subtract(3, DateTimeConstants.DAY).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this.endDate = moment().format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
  }
  public loadData() {
    this._dataService.post('/api/timesheet/getall?userID=' + this.userID + '&page=' + this.pageIndex + '&pageSize=' + this.pageSize +
    '&colunm=' + this.column + '&isAsc=' + this.isAsc , JSON.stringify(this.filter))
      .subscribe((response: any) => {
        this.timeSheet = response.Items;
        this.totalRow = response.TotalRows;
        this.pageIndex = response.PageIndex;
        this.pageSize = response.PageSize;
        this.loadsuccess = true;
        this.calShowingPage();
      }, error => this._dataService.handleError(error));
  }
  public loadListError() {
    this._dataService.get('/api/timesheet/getlisterror?page=' + this.pageIndexError + '&pageSize=' + this.pageSizeError)
      .subscribe((response: any) => {
        this.fingerTimeSheetError = response.Items;
        this.totalRowError = response.TotalRows;
        this.pageIndexError = response.PageIndex;
        this.pageSizeError = response.PageSize;
        this.calShowingPageError();
      }, error => this._dataService.handleError(error));
  }
  public getListStatusForDropdown() {
    this._dataService.get('/api/statusrequest/getall')
      .subscribe((response: any[]) => {
        this.statusRequestOption = [];
        for (let statusRequest of response) {
          this.statusRequestOption.push({ id: statusRequest.Name, name: statusRequest.Name });
        }
      }, error => this._dataService.handleError(error));
  }
  public getListAbnormaTimeSheetType() {
    this._dataService.get('/api/abnormal-timesheet-type/getall')
      .subscribe((response: any[]) => {
        this.AbnormalTypeOption = [];
        for (let abnormalTimeSheetType of response) {
          this.AbnormalTypeOption.push({ id: abnormalTimeSheetType.Name, name: abnormalTimeSheetType.Description });
        }
      }, error => this._dataService.handleError(error));
  }

   //Load Full Name
   loadFullName() {
    this._dataService.get('/api/appUser/getuserbygroup')
      .subscribe((response: any[]) => {
        this.FullNameOption = [];
        for (let user of response) {
          this.FullNameOption.push({ id: user.Id, name: user.FullName +" ( "+ user.UserName +" ) "   });
        }
      }, error => this._dataService.handleError(error));
  }
  //#endregion

  //#region "CRUD"
  upload(valid: boolean) {
    if (valid) {
      this.loadtimeSheet = true;
      let fileBrowser = this.file.nativeElement;
      if (fileBrowser.files && fileBrowser.files[0]) {
        if(this.file.nativeElement.files[0].size>CommonConstants.MAXLENGTH){
         this._notificationService.printErrorMessage(MessageConstants.FILE_TOO_LARGE);
         return;
        }
        const formData = new FormData();
        formData.append("file", fileBrowser.files[0]);
        this._dataService.postFile('/api/timesheet/import', formData).subscribe(res => {
          this.loadtimeSheet = false;
          if (Array.isArray(res)) {
            this.fingerTimeSheetError = res;
          } else {
            this._notificationService.printSuccessMessage(res);
            this.file.nativeElement.value = "";
            this.fileUpload = '';
            this.fingerTimeSheetError = [];
            this.modalImport.hide();
            this.myFilterStatusRequests = [];
            this.myFilterAbnormalTypes = [];
            this.filter = null;
            this.pageIndex = PageConstants.pageIndex;
            this.loadData();
          }
        }, error => {
          if (error.status == 400) {
            this.pageIndexError = PageConstants.pageIndex;
            this.pageSizeError= PageConstants.pageSize;      
            this.loadListError();
          }
          this.loadtimeSheet = false;
          this._dataService.handleError(error);
        });
      }
      else {
        this.loadtimeSheet = false;
        this._notificationService.printErrorMessage(MessageConstants.SELECT_FILE_UPLOAD);
      }
    }
  }
  //#endregion
  //#region "Business logic"
  filterTimeSheet() {
    this.pageIndex = 1;
    if(this.chosenDate.start != '' && this.chosenDate.end != ''){
      this.startDate = moment(new Date(this.chosenDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      this.endDate = moment(new Date(this.chosenDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
      }else{
        this.startDate = null;
        this.endDate = null;
      }
    this.filter = new FilterTimeSheet(this.myFilterStatusRequests, this.myFilterAbnormalTypes, this.startDate, this.endDate,this.FullNameFilter);
    this.loadData();
  }
  reset() {
    			
    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: true,
      "opens": "left",
      ranges: DateTimeConstants.Range,
      autoUpdateInput: true,
    };
        this.value.start = moment();
        this.value.end = moment();
        this.selectedDateRangePicker(this.value, this.chosenDate);
      
    this.chosenDate.start = '';
    this.chosenDate.end = '';
    // var dateranger = new DateRangePickerModel(moment().subtract(3, DateTimeConstants.DAY), moment(), DateTimeConstants.CustomRange);
    // this.selectedDateRangePicker(dateranger, this.chosenDate);
    this.myFilterStatusRequests = [];
    this.myFilterAbnormalTypes = [];
    this.FullNameFilter=[];
    this.filter = null;
    this.pageIndex = 1;
    this.loadData();
  }
  pageChanged(event: any): void {
    this.loadsuccess = false;
    this.pageIndex = event.page;
    this.loadData();
  }
  pageChangedError(event: any): void {
    this.pageIndexError = event.page;
    this.loadListError();
    this.calShowingPageError();
  }
  //calculate page
  calShowingPage() {
    this.currentMaxEntries = this.pageIndex * this.pageSize;
    if (this.currentMaxEntries >= this.totalRow) {
      this.currentMaxEntries = this.totalRow;
    }
  }
  calShowingPageError() {
    this.currentMaxEntriesError = this.pageIndexError * this.pageSizeError;
    if (this.currentMaxEntriesError >= this.totalRowError) {
      this.currentMaxEntriesError = this.totalRowError;
    }
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
  };
  public chosenDate: any = {
    start: '',
    end: '',
  };
  public selectedDateRangePicker(value: any, dateInput: any) {
    this.value = value;
    dateInput.start = value.start.format();
    dateInput.end = value.end;
  }
  public toggleDirection(direction: string) {
    this.picker.opens = direction;
  }
  public updateSettings() {
    this.daterangepickerOptions.settings.locale = {
      locale: DateTimeConstants.Locale,
      alwaysShowCalendars: false,
      "opens": "left",
    }; 
  }
  //#endregion
  //#region "Poppu or other"
  showUploadModal() {
    this.fileUpload = '';
    this.fingerTimeSheetError = [];
    this.modalImport.show();
  }
  fileClicked(event:any) {
    this.fingerTimeSheetError=[];
    this.fileName='';
  }
  checkName():boolean{
    let fileBrowser = this.file.nativeElement;
    if (fileBrowser.files && fileBrowser.files[0]) {
      this.fileName = fileBrowser.files[0].name;
      return true;
    }
    else{
      return false;
    }
  }
  onChangeSelectFile() {
    let fileBrowser = this.file.nativeElement;
    if (fileBrowser.files && fileBrowser.files[0]) {
      this.fileName = fileBrowser.files[0].name;
    }
  }
  //close want to close
  public closeForm(modal: ModalDirective, form: NgForm) {
    this.file.nativeElement.value = "";
    this.fileUpload = '';
    this.fingerTimeSheetError = [];
    this.fileName = '';
    modal.hide();
    form.resetForm();
  }
  Export() {
    this._dataService.post('/api/timesheet/exportexcel?userID='+this.userID+'&page='+this.pageIndex+'&pageSize='+ this.pageSize +'&colunm=' + this.column +'&isAsc='+this.isAsc,  JSON.stringify(this.filter))
      .subscribe((response: any[]) => {
        this.timeSheet = response;
        if (response != null) {
          window.location.href = SystemConstants.BASE_API + response;
          this.loadData();
          // this._notificationService.printSuccessMessage(MessageConstants.DOWNLOAD_OK_MSG);
        } else {
          this._notificationService.printSuccessMessage(MessageConstants.DOWNLOAD_OK_ERROR);
        }
      }, error => this._dataService.handleError(error));
  }
 
  //
    //sort action
    sort(property) {
      this.isAsc = !this.isAsc; //change the direction    
      this.column = property;
      let direction = this.isAsc ? 1 : -1;
      this.pageIndex = 1;
      this.loadData();
    };
  //#endregion
  Report() {
    this._dataService.post('/api/timesheet/exportexcel?userID='+this.userID, JSON.stringify(this.filter))
    .subscribe((response: any) => {
      this.timeSheet = response;
      if (response != null) {
        window.location.href = SystemConstants.BASE_API + response;
        this.loadData();
        // this._notificationService.printSuccessMessage(MessageConstants.DOWNLOAD_OK_MSG);
      } else {
        this._notificationService.printSuccessMessage(MessageConstants.DOWNLOAD_OK_ERROR);
      }
    }, error => this._dataService.handleError(error));
  }

  checkRoleSA() {
    if (this._authenService.getLoggedInUser().roles === '["SuperAdmin"]') {
      return true;
    }else{
      return false;
    }
  }

  public hasPermission(functionId: string, action: string): boolean {
    return this._authenService.hasPermission(functionId, action);
  }
 
}
