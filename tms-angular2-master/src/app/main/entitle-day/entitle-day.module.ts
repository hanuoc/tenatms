import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { EntitleDayComponent } from './entitle-day.component';
import { EntitleDayRouter } from './entitle-day.routes';
import { PaginationModule, ModalModule } from 'ngx-bootstrap';
import { FormsModule } from '@angular/forms';
import { DataService } from './../../core/services/data.service';
import { UtilityService } from './../../core/services/utility.service';
import { UploadService } from './../../core/services/upload.service';
import { Daterangepicker } from 'ng2-daterangepicker';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';

@NgModule({
    imports:[
        CommonModule,
        FormsModule,
        EntitleDayRouter,
        PaginationModule,
        ModalModule,
        Daterangepicker,
        MultiselectDropdownModule,
    ],
    declarations:[EntitleDayComponent],
    providers: [DataService, UtilityService, UploadService]
})
export class EntitleDayModule { }