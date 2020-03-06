
export class DateTimeConstants {
    public static FORMAT_DATE_DDMMYYYY='DD/MM/YYYY';
    public static FORMAT_DATE_MMDDYYYY='MM/DD/YYYY';
    public static FORMAT_DAYOFWEEK_DDDD="dddd";
    public static FORMAT_HOUR_HH= "HH";
    public static FORMAT_local_EN =  "en-gb";
    public static DAYOFWEEKFRIDAY = "Friday";
    public static WEEKENDSAT = "Saturday";
    public static WEEKENDSUN = "Sunday";
    public static MONTH='month';
    public static DAY='day';
    public static HOURS='hours';
    public static DATE_RANGE_PICKER = 'daterangepicker';
    public static APPLY_DATE_RANGE_PICKER = 'apply.daterangepicker';
    public static CANCEL_DATE_RANGE_PICKER = 'cancel.daterangepicker';
    public static HIDECALENDAR_DATE_RANGE_PICKER = 'hideCalendar.daterangepicker';
    public static SHOWCALENDAR_DATE_RANGE_PICKER = 'showCalendar.daterangepicker';
    public static HIDE_DATE_RANGE_PICKER = 'hide.daterangepicker';
    public static SHOW_DATE_RANGE_PICKER = 'show.daterangepicker';
    public static OPTIOINS = 'options';
    public static SETTINGS = 'settings';
    public static CustomRange= 'Custom Range';
    public static LimitTimeApproveRejectRequest = 23;
    public static LimitTimeCreateMorningLeave = 12;
    public static LimitTimeCreateAfternoonLeave = 18;
    public static Locale = {
        "format": 'DD/MM/YYYY',
        "separator": " - ",
    };
    public static Range = {
        'Today': [moment(), moment()],
        'Yesterday': [moment().subtract(1, 'days'), moment().subtract(1, 'days')],
        'Last 7 Days': [moment().subtract(6, 'days'), moment()],
        'Last 30 Days': [moment().subtract(29, 'days'), moment()],
        'This Month': [moment().startOf('month'), moment().endOf('month')],
        'Last Month': [moment().subtract(1, 'month').startOf('month'), moment().subtract(1, 'month').endOf('month')]
        // '1 Week ago': [moment().subtract(7, DateTimeConstants.DAY), moment()],
        // '2 Week ago': [moment().subtract(14, DateTimeConstants.DAY), moment()],
        // '3 Week ago': [moment().subtract(21, DateTimeConstants.DAY), moment()],
        // '1 Month ago': [moment().subtract(1, DateTimeConstants.MONTH), moment()],
        // '3 Month ago': [moment().subtract(3, DateTimeConstants.MONTH), moment()],
        // '6 Month ago': [moment().subtract(6, DateTimeConstants.MONTH), moment()],
        // '7 months ago': [moment().subtract(12, 'month'), moment()],
      };
    public static RangeChildcareLeave = {
        '3 Months': [moment(), moment().subtract(-90, 'days')],
        '6 Months': [moment(), moment().subtract(-180, 'days')],
        '1 Year': [moment(), moment().subtract(-365, 'days')]
      };
}
