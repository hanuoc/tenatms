
import { CommonModule } from '@angular/common';
import { NgModule } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
import { ModalModule, PaginationModule } from 'ngx-bootstrap';
import { Daterangepicker } from '../../shared/daterangepicker/daterangepicker.module';
import { DataService } from './../../core/services/data.service';
import { UploadService } from './../../core/services/upload.service';
import { UtilityService } from './../../core/services/utility.service';
import { HolidayComponent } from './holiday.component';
import { HolidayRouter } from './holiday.routes';


@NgModule({
  imports: [
    CommonModule,
    HolidayRouter,
    FormsModule,
    PaginationModule,
    ModalModule,
    Daterangepicker,
    MultiselectDropdownModule,
  ],
  declarations: [HolidayComponent],
  providers: [DataService, UtilityService, UploadService]
})
export class HolidayModule { }
