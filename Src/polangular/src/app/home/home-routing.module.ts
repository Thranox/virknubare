import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';

import { HomePage } from './home.page';
import { TravelExpensesPage } from '../modules/pages/travel-expenses/travel-expenses.page';
import { PageNotFoundComponent } from '../components/page-not-found/page-not-found.component';
import { ReportsPage } from '../modules/pages/reports/reports.page';
import { ArchivesPage } from '../modules/pages/archives/archives.page';
import { HelpPage } from '../modules/pages/help/help.page';

const routes: Routes = [
    {
        path: 'menu',
        component: HomePage,
        children: [
            {
                path: 'travel-expenses',
                children: [
                    {
                        path: '',
                        component: TravelExpensesPage,
                        loadChildren: () => import('../modules/pages/travel-expenses/travel-expenses.module').then(m => m.TravelExpensesPageModule)
                    },
                    
                ],
            },
            {
                path: 'reports',
                component: ReportsPage,
                loadChildren: () => import('../modules/pages/reports/reports.module').then(m => m.ReportsPageModule)

            },
            {
                path: 'archives',
                component: ArchivesPage,
                loadChildren: () => import('../modules/pages/archives/archives.module').then(m => m.ArchivesPageModule)

            },
            {
                path: 'help',
                component: HelpPage,
                loadChildren: () => import('../modules/pages/help/help.module').then(m => m.HelpPageModule)

            },
        ]
    },
    {
        path: '',
        redirectTo: 'menu',
        pathMatch: 'full'
    },
    
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule],
})
export class HomePageRoutingModule {}
