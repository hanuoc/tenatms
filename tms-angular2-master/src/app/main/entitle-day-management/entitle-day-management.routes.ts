import { EntitleDayManagementComponent } from './entitle-day-management.component';
import { Routes, RouterModule } from '@angular/router';
const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: EntitleDayManagementComponent }
];
export const EntitleDayManagementRouter = RouterModule.forChild(routes);