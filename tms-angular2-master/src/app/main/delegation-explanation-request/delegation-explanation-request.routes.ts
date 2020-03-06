import { Routes, RouterModule } from '@angular/router';
import { DelegationExplanationRequestComponent} from './delegation-explanation-request.component'

const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: DelegationExplanationRequestComponent }
];
export const DelegationExplanationRequestRouter = RouterModule.forChild(routes);