import { ManagementConfigDelegationComponent } from './management-config-delegation.component';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: ManagementConfigDelegationComponent },
];
export const ManagementConfigDelegationRouter = RouterModule.forChild(routes);