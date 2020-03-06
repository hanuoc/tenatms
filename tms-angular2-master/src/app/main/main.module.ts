import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MainComponent } from './main.component';
import { mainRoutes } from './main.routes';
import { RouterModule, Routes } from '@angular/router';
import { UserModule } from './user/user.module';
import { HomeModule } from './home/home.module';
import { UtilityService } from '../core/services/utility.service';
import { AuthenService } from '../core/services/authen.service';
import { SignalrService } from '../core/services/signalr.service';
import { SidebarMenuComponent } from '../shared/sidebar-menu/sidebar-menu.component';
import { TopMenuComponent } from '../shared/top-menu/top-menu.component';
import { PaginationModule, ModalModule } from 'ngx-bootstrap';
import { Daterangepicker } from 'ng2-daterangepicker';
import { MultiselectDropdownModule } from 'angular-2-dropdown-multiselect';
import { FormsModule } from '@angular/forms';
import { PassDataService } from '../core/services/passData.service';
@NgModule({
  imports: [
    CommonModule,
    UserModule,
    HomeModule,
    FormsModule,
    RouterModule.forChild(mainRoutes),
    PaginationModule,
    ModalModule,
    Daterangepicker,
    MultiselectDropdownModule,
  ],
  declarations: [MainComponent, SidebarMenuComponent, TopMenuComponent],
  providers: [UtilityService, AuthenService, SignalrService,PassDataService]
})
export class MainModule { }
