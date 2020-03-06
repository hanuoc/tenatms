import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ManagementDelegationExplanationRequestComponent } from './management-delegation-explanation-request.component';
import { ManagementDelegationExpnalationRequestRouter } from './management-delegation-explanation-request.routes';
import { PaginationModule, ModalModule } from 'ngx-bootstrap';
import { FormsModule } from '@angular/forms';
import { DataService } from './../../core/services/data.service';
import { UtilityService } from './../../core/services/utility.service';
import { UploadService } from './../../core/services/upload.service';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';

@NgModule({
    imports: [
      CommonModule,
      ManagementDelegationExpnalationRequestRouter,
      FormsModule,
      PaginationModule,
      ModalModule,
      MultiselectDropdownModule
    ],
     declarations: [ManagementDelegationExplanationRequestComponent],
    providers: [DataService, UtilityService, UploadService]
  })
  export class ManagementDelegationExpnalationRequestModule { }