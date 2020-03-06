
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { AbnormalcaseComponent} from './abnormalcase.component';
import { AbnormalcaseRouter } from './abnormalcase.routes';
import { PaginationModule, ModalModule } from 'ngx-bootstrap';
import { FormsModule } from '@angular/forms';
import { DataService } from './../../core/services/data.service';
import { UtilityService } from './../../core/services/utility.service';
import { UploadService } from './../../core/services/upload.service';
import { Daterangepicker } from '../../shared/daterangepicker/daterangepicker.module';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
import { AmazingTimePickerModule } from 'amazing-time-picker';
//import { SimpleTinyComponent } from '../../shared/simple-tiny/simple-tiny.component';

@NgModule({
  imports: [
    CommonModule,
    AbnormalcaseRouter,
    FormsModule,
    PaginationModule,
    ModalModule,
    Daterangepicker,
    MultiselectDropdownModule,
    AmazingTimePickerModule
  ],
   declarations: [AbnormalcaseComponent],
  providers: [DataService, UtilityService, UploadService]
})
export class AbnormalcaseModule { }
