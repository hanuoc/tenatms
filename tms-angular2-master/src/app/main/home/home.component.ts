import { Component, OnDestroy, OnInit } from '@angular/core';
import { IMultiSelectOption, IMultiSelectSettings, IMultiSelectTexts } from 'angular-2-dropdown-multiselect';
import { LoggedInUser } from 'app/core/domain/loggedin.user';
import { AuthenService } from 'app/core/services/authen.service';
import { DataService } from 'app/core/services/data.service';
import { UtilityService } from 'app/core/services/utility.service';
import { CommonConstants } from '../../core/common/common.constants';
import { DateTimeConstants } from '../../core/common/datetime.constants';
import { Hero } from '../../core/models/dashboardModel';
import { DaterangepickerConfig } from '../../core/services/config.service';
import { PassDataService } from '../../core/services/passData.service';
@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css'],
})
export class HomeComponent implements OnInit, OnDestroy {
  //Admin-HR view

  //data chart
  abnormalChart: any;
  lateComing = false;
  earlyLeaving = false;
  leave = false;
  unusedEarlyLeaving = false;
  unusedLateComing = false;
  unusedLeave = false;
  withoutCheckIn = false;
  withoutCheckOut = false;
  withoutCheckInOut = false;
  //end data chart
  abnormalChartPercent: any;
  requestchart: any;
  isReset: boolean;
  otrequestchart: any;
  exrequestchart: any;
  value: any= {
    start : '',
    end: ''
    };
  valueDate: any = {
  start : '',
  end: ''
  };
  dateInput: any;
  groupIdRequest: any = 1;
  groupIdOTRequest: any = 1;
  groupIdExRequest: any = 1;
  groupRequestName: any;
  groupOTRequestName: any;
  groupExRequestName: any;
  //Member - GroupLead view
  totalRequest: number = 0;
  totalOTRequest: number = 0;
  totalExplanation: number = 0;
  totalAbnormal: number = 0;
  requestchartMemberGroup: any;
  otrequestchartMemberGroup: any;
  exrequestchartMemberGroup: any;
  private startDate: string;
  private endDate: string;
  isdisable: boolean = true;
  a: any;
  group: any;
  public settings;
  public setting1;
  totalUser: any;
  user: LoggedInUser;
  role: any;
  numberSlide: number = 0;
  DashBoard: Hero = {
    clickFromDashBoard: false
  };
  public groupOption: IMultiSelectOption[] = [];
  public GroupFilter: string[] = [];
  public chosenDate: any = {
    start: moment().subtract(8, DateTimeConstants.DAY).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY),
    end: moment().subtract(1, DateTimeConstants.DAY).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY),
  };
  public chosenDate1: any = {
    start: moment().subtract(22, DateTimeConstants.DAY).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY),
    end: moment().subtract(22, DateTimeConstants.DAY).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY),
  };
  // whether user have group or not
  isGroup: boolean = true;

  // Text configuration Dropdown group 
  GroupDropDownTexts: IMultiSelectTexts = {
    allSelected: CommonConstants.AllSelectGroupType,
    checked: CommonConstants.CheckGroupType,
    checkedPlural: CommonConstants.CheckGroupType,
    defaultTitle: CommonConstants.GroupTitle,
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
  constructor(
    private daterangepickerOptions: DaterangepickerConfig,
    private _authenService: AuthenService,
    private _dataService: DataService,
    private _utilityService: UtilityService,
    private _passDataService: PassDataService) {

  }
  ngOnInit() {
    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      opens: CommonConstants.LEFT,
      minDate: moment().subtract(1, DateTimeConstants.MONTH).subtract(1, DateTimeConstants.DAY),
      maxDate: moment().subtract(1, DateTimeConstants.DAY),
      startDate: moment().subtract(8, DateTimeConstants.DAY),
      endDate: moment().subtract(1, DateTimeConstants.DAY),
      alwaysShowCalendars: true,
      autoUpdateInput: true,
      ranges: true,
      isInvalidDate: function (date) {
        if (date.isSame(Date.toString(), DateTimeConstants.DAY))
          return 'mystyle';
        return false;
      }
    };
    //var dateranger = new DateRangePickerModel(moment().subtract(3, DateTimeConstants.DAY), moment(), "Custom Range");
    //this.selectedDateRangePicker(dateranger, this.chosenRequestDate);
    this.user = this._authenService.getLoggedInUser();
    this.isGroup = this.user.groupId.length != 0;
    this.role = this.user.roles.replace("[\"", "").replace("\"]", "");
    if (this.role == "Admin" || this.role == "HR" || this.role == "SuperAdmin") {
      this.loadTotalUser();
      this.loadAbnormalChart();
      this.loadAbnormalChartPercent();
      this.loadRequestChart();
      this.loadGroup();
      this.loadOTRequestChart();
      this.loadExRequestChart();
    }
    if (this.role == "Member") {
      this.loadTotalRequestMember();
      this.loadTotalOTRequestMember();
      this.loadTotalExRequestMember();
      this.loadTotalAbnormalMember();
      this.loadRequestChartMember();
      this.loadOTRequestChartMember();
      this.loadEXRequestChartMember();
    }
    if (this.role == "GroupLead") {
      this.loadTotalRequestMember();
      this.loadTotalExRequestMember();
      this.loadTotalAbnormalMember();
      this.loadTotalAbnormalGroup();
      this.loadRequestChartGroup();
      this.loadOTRequestChartGroup();
      this.loadEXRequestChartGroup();
    }
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
    
    this.loadGroupsFilter();
  }
  ngOnDestroy() {
    this._passDataService.dashboard = this.DashBoard;
  }
  loadTotalUser() {
    this._dataService.get('/api/appuser/gettotaluser')
      .subscribe((response: any) => {
        this.totalUser = response;
      }, error => this._dataService.handleError(error));
  }

  loadAbnormalChart() {
    this._dataService.post('/api/abnormalcase/abnormalchart?DateStart=' + this.chosenDate.start + '&DateEnd=' + this.chosenDate.end , JSON.stringify(this.GroupFilter))
      .subscribe((response: any) => {
        this.abnormalChart = [];
        this.abnormalChart = response;
      }, error => this._dataService.handleError(error));
  }
  loadAbnormalChartPercent() {
    this._dataService.get('/api/abnormalcase/abnormalchartpercent')
      .subscribe((response: any) => {
        this.abnormalChartPercent = [];
        this.abnormalChartPercent = response;
      }, error => this._dataService.handleError(error));
  }
  loadRequestChart() {
    this._dataService.get('/api/request/requestchart?groupId=' + this.groupIdRequest)
      .subscribe((response: any) => {
        this.requestchart = [];
        this.requestchart = response;
      }, error => this._dataService.handleError(error));
  }
  loadOTRequestChart() {
    this._dataService.get('/api/otrequest/otrequestchart?groupId=' + this.groupIdOTRequest)
      .subscribe((response: any) => {
        this.otrequestchart = [];
        this.otrequestchart = response;
      }, error => this._dataService.handleError(error));
  }
  loadExRequestChart() {
    this._dataService.get('/api/explanation/exrequestchart?groupId=' + this.groupIdExRequest)
      .subscribe((response: any) => {
        this.exrequestchart = [];
        this.exrequestchart = response;
      }, error => this._dataService.handleError(error));
  }
  loadGroup() {
    this._dataService.get('/api/group/getallgroup')
      .subscribe((response: any) => {
        this.group = [];
        this.group = response;
        this.groupRequestName = this.group.filter(data => { if (data.ID == this.groupIdRequest) return data; })[0].Name;
        this.groupExRequestName = this.group.filter(data => { if (data.ID == this.groupIdExRequest) return data; })[0].Name;
        this.groupOTRequestName = this.group.filter(data => { if (data.ID == this.groupIdOTRequest) return data; })[0].Name;
      }, error => this._dataService.handleError(error));
  }
  loadTotalRequestMember() {
    this._dataService.get('/api/request/requestbyuser?userId=' + this.user.id)
      .subscribe((response: number) => {
        this.totalRequest = typeof (response) === 'object' ? 0 : response;
      }, error => this._dataService.handleError(error));
  }
  loadTotalOTRequestMember() {
    this._dataService.get('/api/otrequest/otrequestbyuser?userId=' + this.user.id)
      .subscribe((response: number) => {
        this.totalOTRequest = typeof (response) === 'object' ? 0 : response;
      }, error => this._dataService.handleError(error));
  }
  loadTotalExRequestMember() {
    this._dataService.get('/api/explanation/exrequestbyuser?userId=' + this.user.id)
      .subscribe((response) => {
        this.totalExplanation = typeof (response) === 'object' ? 0 : response;
      }, error => this._dataService.handleError(error));
  }
  loadTotalAbnormalMember() {
    this._dataService.get('/api/abnormalcase/abnormalbyuser?userId=' + this.user.id)
      .subscribe((response) => {
        this.totalAbnormal = typeof (response) === 'object' ? 0 : response;
      }, error => this._dataService.handleError(error));
  }
  loadRequestChartMember() {
    this._dataService.get('/api/request/requestchartbyUser?userID=' + this.user.id)
      .subscribe((response: any) => {
        this.requestchartMemberGroup = [];
        this.requestchartMemberGroup = response;
      }, error => this._dataService.handleError(error));
  }
  loadOTRequestChartMember() {
    this._dataService.get('/api/otrequest/otrequestchartbyUser?userID=' + this.user.id)
      .subscribe((response: any) => {
        this.otrequestchartMemberGroup = [];
        this.otrequestchartMemberGroup = response;
      }, error => this._dataService.handleError(error));
  }
  loadEXRequestChartMember() {
    this._dataService.get('/api/explanation/exrequestchartbyUser?userID=' + this.user.id)
      .subscribe((response: any) => {
        this.exrequestchartMemberGroup = [];
        this.exrequestchartMemberGroup = response;
      }, error => this._dataService.handleError(error));
  }
  //  
  loadTotalAbnormalGroup() {
    this._dataService.get('/api/abnormalcase/abnormalbyuser?userId=' + this.user.id)
      .subscribe((response) => {
        this.totalAbnormal = typeof (response) === 'object' ? 0 : response;
      }, error => this._dataService.handleError(error));
  }
  loadRequestChartGroup() {
    this._dataService.get('/api/request/requestchart?groupId=' + this.user.groupId)
      .subscribe((response: any) => {
        this.requestchartMemberGroup = [];
        this.requestchartMemberGroup = response;
      }, error => this._dataService.handleError(error));
  }
  loadOTRequestChartGroup() {
    this._dataService.get('/api/otrequest/otrequestchart?groupId=' + this.user.groupId)
      .subscribe((response: any) => {
        this.otrequestchartMemberGroup = [];
        this.otrequestchartMemberGroup = response;
      }, error => this._dataService.handleError(error));
  }
  loadEXRequestChartGroup() {
    this._dataService.get('/api/explanation/exrequestchart?groupId=' + this.user.groupId)
      .subscribe((response: any) => {
        this.exrequestchartMemberGroup = [];
        this.exrequestchartMemberGroup = response;
      }, error => this._dataService.handleError(error));
  }
  loadGroupsFilter() {
    this._dataService.get('/api/group/getallgroup')
      .subscribe((response: any) => {
        this.GroupFilter=[];
        this.groupOption = [];
        for (let group of response) {
          this.groupOption.push({ id: group.ID, name: group.Name });
        }
      });
  }
  //Bussiness
  chooseGroup(groupID: any) {
    this.groupIdRequest = groupID;
    this.groupRequestName = this.group.filter(data => { if (data.ID == this.groupIdRequest) return data; })[0].Name;
    this.loadRequestChart();
  }
  chooseGroupOTRequest(groupID: any) {
    this.groupIdOTRequest = groupID;
    this.groupOTRequestName = this.group.filter(data => { if (data.ID == this.groupIdOTRequest) return data; })[0].Name;
    this.loadOTRequestChart();
  }
  chooseGroupExRequest(groupID: any) {
    this.groupIdExRequest = groupID;
    this.groupExRequestName = this.group.filter(data => { if (data.ID == this.groupIdExRequest) return data; })[0].Name;
    this.loadExRequestChart();
  }
  ClickAbnormalFromDashBoard(event) {
    this.DashBoard.clickFromDashBoard = true;
    this._utilityService.navigate("/main/abnormalcase/index");
  }
  ClickRequestFromDashBoard(event) {
    this.DashBoard.clickFromDashBoard = true;
    this._utilityService.navigate("/main/request/index");
  }
  ClickExplanationFromDashBoard(event) {
    this.DashBoard.clickFromDashBoard = true;
    this._utilityService.navigate("/main/explanation/index");
  }
  //Tùng Code
  ClickUserFromDashBoard($event) {
    this.DashBoard.clickFromDashBoard = false;
    this._utilityService.navigate("/main/user/index");
  }
  ClickActiveUserFromDashBoard(event) {
    this.DashBoard.clickFromDashBoard = "active";
    this._utilityService.navigate("/main/user/index");
  }
  ClickInActiveUserFromDashBoard(event) {
    this.DashBoard.clickFromDashBoard = "inactive";
    this._utilityService.navigate("/main/user/index");
  }
  ClickOnsiteUserFromDashBoard(event) {
    this.DashBoard.clickFromDashBoard = "onsite";
    this._utilityService.navigate("/main/user/index");
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

  public eventLog = '';
  public selectedDateRangePicker(valueDate: any, dateInput: any) {
    this.valueDate = valueDate;
    this.dateInput = dateInput;
    this.chosenDate.start = valueDate.start.format();
    this.chosenDate.end = valueDate.end.format();
  }
  public applyDatepicker(value: any, dateInput: any) {
    this.isdisable = false;
    this.value = value;
    this.dateInput = dateInput;
    this.startDate = moment(new Date(dateInput.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this.endDate = moment(new Date(dateInput.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
  }
  filterRequestIsAssign() {
    this.isReset = true;
    this.startDate = moment(new Date(this.chosenDate.start)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this.endDate = moment(new Date(this.chosenDate.end)).format(DateTimeConstants.FORMAT_DATE_MMDDYYYY);
    this._dataService.post('/api/abnormalcase/abnormalchart?DateStart=' + this.startDate + '&DateEnd=' + this.endDate , JSON.stringify(this.GroupFilter))
      .subscribe((response: any) => {
        this.abnormalChart = response;
      }, error => this._dataService.handleError(error));
  }
  public calendarEventsHandler(e: any) {
    this.eventLog += '\nEvent Fired: ' + e.event.type;
  }
  public toggleDirection(direction: string) {
    this.picker.opens = direction;
  }
  public reset() {
    this.daterangepickerOptions.settings = {
      locale: DateTimeConstants.Locale,
      opens: CommonConstants.LEFT,
      minDate: moment().subtract(1, DateTimeConstants.MONTH).subtract(1, DateTimeConstants.DAY),
      maxDate: moment().subtract(1, DateTimeConstants.DAY),
      startDate: moment().subtract(8, DateTimeConstants.DAY),
      endDate: moment().subtract(1, DateTimeConstants.DAY),
      alwaysShowCalendars: true,
      autoUpdateInput: true,
      ranges: true,
      isInvalidDate: function (date) {
        if (date.isSame(Date.toString(), DateTimeConstants.DAY))
          return 'mystyle';
        return false;
      }
    };
    this.valueDate.start = moment().subtract(8, DateTimeConstants.DAY);
    this.valueDate.end = moment().subtract(1, DateTimeConstants.DAY);
    this.selectedDateRangePicker(this.valueDate, this.chosenDate);
    this.GroupFilter=[];
    this.filterRequestIsAssign();
    this.isdisable = true;

  }

  ClickAbnormalChart(number: any) {
    $(".children" + number).slideToggle();
  }
}
