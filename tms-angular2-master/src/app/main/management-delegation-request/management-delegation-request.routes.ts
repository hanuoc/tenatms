import { ManagementDelegationRequestComponent} from './management-delegation-request.component';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: ManagementDelegationRequestComponent },
];
export const ManagementDelegationRequestRouter = RouterModule.forChild(routes);