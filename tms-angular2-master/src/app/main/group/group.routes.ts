import { GroupComponent } from './group.component';
import { Routes, RouterModule } from '@angular/router';
const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: GroupComponent }
];
export const GroupRouter = RouterModule.forChild(routes);