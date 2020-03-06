import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DelegationExplanationRequestComponent } from './delegation-explanation-request.component';
import { DelegationExplanationRequestRouter } from './delegation-explanation-request.routes';
import { PaginationModule, ModalModule } from 'ngx-bootstrap';
import { FormsModule } from '@angular/forms';
import { DataService } from './../../core/services/data.service';
import { UtilityService } from './../../core/services/utility.service';
import { UploadService } from './../../core/services/upload.service';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
import { DaterangepickerConfig } from 'app/core/services/config.service';
import { Daterangepicker } from '../../shared/daterangepicker/daterangepicker.module';

@NgModule({
    imports: [
      CommonModule,
      DelegationExplanationRequestRouter,
      FormsModule,
      PaginationModule,
      ModalModule,
      Daterangepicker,
      MultiselectDropdownModule
    ],
     declarations: [DelegationExplanationRequestComponent],
    providers: [DataService, UtilityService, UploadService, DaterangepickerConfig]
  })
  export class DelegationExplanationRequestModule { }