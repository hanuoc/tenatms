import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { RevenueComponent } from './revenue/revenue.component';
import { VisitorComponent } from './visitor/visitor.component';
import { ReportRouter } from './report.routes';
import { ChartsModule } from 'ng2-charts';
import { ReportComponent } from './report.component';
import { Daterangepicker } from '../../shared/daterangepicker/daterangepicker.module';
import { PaginationModule } from 'ngx-bootstrap';
import { DataService } from 'app/core/services/data.service';
import { UtilityService } from 'app/core/services/utility.service';
import { UploadService } from 'app/core/services/upload.service';
import { FormsModule, NgModel } from '@angular/forms';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
import { ModalModule } from 'ngx-bootstrap/modal';
@NgModule({
  imports: [
    CommonModule,
    ReportRouter,
    PaginationModule,
    FormsModule,
    ChartsModule,
    Daterangepicker,
    MultiselectDropdownModule,
    ModalModule.forRoot(),
  ],
  declarations: [ReportComponent],
  providers: [DataService, UtilityService, UploadService]
})
export class ReportModule { }
