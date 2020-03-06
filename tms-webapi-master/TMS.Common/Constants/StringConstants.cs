using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TMS.Common.Constants
{
    public class StringConstants
    {
        //string constant
        public const string TimeSheetAbsentMorning = "VS";
        public const string TimeSheetAbsentAfternoon = "VC";
        public const string TimeSheetAbsent = "V";
        public const string AbsentMorning = "Morning Leave";
        public const string AbsentAfternoon = "Afternoon Leave";
        public const string Absent = "Full-time Leave";
        public const string FilterTimeSheetComeLate = "DiMuon";
        public const string FilterTimeSheetComeBackSoon = "VeSom";
        public const string StringEmpty = "";
        public const string FormatTime12 = "hh:mm";
        public const string FormatTimeSA = "SA";
        public const string FormatTimeCH = "CH";
        public const string TimeAutoGetTimeSheet = "TimeAutoGetTimeSheet";
        public const string TimeAutoGetTimeSheetNew = "TimeAutoGetTimeSheetNew";
        public const char SplitTime = ':';
        public const string TimeAutoReset = "TimeAutoReset";
        public const string UnauthorizedLateComing = "Unauthorized Late-Coming";
        public const string UnauthorizedEarlyLeaving = "Unauthorized Early-Leaving";
        public const string UnusedAuthorizedLeave="Unused Authorized Leave";
        public const string UnauthorizedLeave="Unauthorized Leave";
        public const string Leave="Leave";
        public const string ForgetToCheck="ForgetToCheck";
        public const string MinusAllowance_40Percent = "40%";
        //Entitle Day
        public const string TimeResetEntitleDay = "TimeResetEntitleDay";
        public const string TimeResetEntitleDayNew = "TimeResetEntitleDayNew";
        //Request
        public const string TimeAutoRequest = "TimeAutoRequest";
        public const string TimeAutoRequestNew = "TimeAutoRequestNew";
        public const string LogErrorSaveDataBase="Save to database:";
        public const string LogErrorSaveChangeDataBase = "TimeSheetService cannot save change to database:";
        // Job Name
        public const string JobTimeSheet = "Job Import Time Sheet";
        public const string JobChangeStatus = "Job Change Request Status";
        public const string JobUpdateEntitleDay = "Job Update Entitle Day";
        public const string JobResetEntitleDay = "Job Reset Entitle Day";
    }
}
