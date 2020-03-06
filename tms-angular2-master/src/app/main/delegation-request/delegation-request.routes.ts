import { DelegationRequestComponent} from './delegation-request.component';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: DelegationRequestComponent },
];
export const DelegationRequestRouter = RouterModule.forChild(routes);