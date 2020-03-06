import { Routes } from '@angular/router';
import { MainComponent } from './main.component';
import { UserDetailComponent } from 'app/main/user/user-detail/user-detail.component';

export const mainRoutes: Routes = [
    {
        //localhost:4200/main
        path: '', component: MainComponent, children: [
            //localhost:4200/main
            { path: '', redirectTo: 'home', pathMatch: 'full' },
            //localhost:4200/main/home
            { path: 'home', loadChildren: './home/home.module#HomeModule' },
            //localhost:4200/main/user
            { path: 'user', loadChildren: './user/user.module#UserModule' },
            //localhost:4200/main/user/detail
            { path: 'user/detail', component: UserDetailComponent },

            { path: 'role', loadChildren: './role/role.module#RoleModule' },

        //    { path: 'function', loadChildren: './function/function.module#FunctionModule' },

           //{ path: 'announcement', loadChildren: './announcement/announcement.module#AnnouncementModule' },

            { path: 'ot-request', loadChildren: './ot-request/ot-request.module#OTRequestModule' },

            { path: 'abnormalcase', loadChildren: './abnormalcase/abnormalcase.module#AbnormalcaseModule'},
            
            { path: 'request', loadChildren: './request/request.module#RequestModule' },
            { path: 'entitle-day', loadChildren: './entitle-day/entitle-day.module#EntitleDayModule' },
            { path: 'ot-list', loadChildren: './ot-list/ot-list.module#OTListModule' },
           { path: 'delegation-request', loadChildren: './delegation-request/delegation-request.module#DelegationRequestModule' },
            { path: 'delegation-explanation-request', loadChildren: './delegation-explanation-request/delegation-explanation-request.module#DelegationExplanationRequestModule' },
            { path: 'time-sheet', loadChildren: './time-sheet/time-sheet.module#TimeSheetModule' },
            { path: 'explanation', loadChildren: './explanation-request/explanation-request.module#ExplanationRequestModule' },
            { path: 'group', loadChildren: './group/group.module#GroupModule' },

            { path: 'management-delegation-request', loadChildren: './management-delegation-request/management-delegation-request.module#ManagementDelegationRequestModule' },
            { path: 'management-delegation-explanation-request', loadChildren: './management-delegation-explanation-request/management-delegation-explanation-request.module#ManagementDelegationExpnalationRequestModule' },
            { path: 'entitle-day-management', loadChildren: './entitle-day-management/entitle-day-management.module#EntitleDayManagementModule' },

            { path: 'timeday', loadChildren: './timeday/timeday.module#TimedayModule'},
            { path: 'holiday', loadChildren: './holiday/holiday.module#HolidayModule'},

            { path: 'sendmail', loadChildren: './sendmail/sendmail.module#SendMailModule'},
            
            {path : 'report', loadChildren: './report/report.module#ReportModule'},

            { path: 'management-config-delegation', loadChildren: './management-config-delegation/management-config-delegation.module#ManagementConfigDelegationModule' },
        ]
    }

]