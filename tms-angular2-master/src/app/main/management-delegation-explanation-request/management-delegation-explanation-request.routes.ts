import { ManagementDelegationExplanationRequestComponent} from './management-delegation-explanation-request.component';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: ManagementDelegationExplanationRequestComponent },
];
export const ManagementDelegationExpnalationRequestRouter = RouterModule.forChild(routes);