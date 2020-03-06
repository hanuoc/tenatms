import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManagementDelegationRequestComponent } from './management-delegation-request.component';
import { ManagementDelegationRequestRouter } from './management-delegation-request.routes';
import { PaginationModule, ModalModule } from 'ngx-bootstrap';
import { FormsModule } from '@angular/forms';
import { DataService } from './../../core/services/data.service';
import { UtilityService } from './../../core/services/utility.service';
import { UploadService } from './../../core/services/upload.service';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';

@NgModule({
    imports: [
      CommonModule,
      ManagementDelegationRequestRouter,
      FormsModule,
      PaginationModule,
      ModalModule,
      MultiselectDropdownModule
    ],
     declarations: [ManagementDelegationRequestComponent],
    providers: [DataService, UtilityService, UploadService]
  })
  export class ManagementDelegationRequestModule { }