import { SendmailComponent} from './sendmail.component';
import { Routes, RouterModule } from '@angular/router';

const routes: Routes = [
    { path: '', redirectTo: 'index', pathMatch: 'full' },
    { path: 'index', component: SendmailComponent },
];
export const SendMailRouter = RouterModule.forChild(routes);