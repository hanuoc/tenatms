import { TimeSheetComponent } from './time-sheet.component';
import { Routes, RouterModule } from '@angular/router';
const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: TimeSheetComponent }
];
export const TimeSheetRouter = RouterModule.forChild(routes);