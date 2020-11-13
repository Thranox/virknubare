import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import {IndexComponent} from './pages/index/index.component';
import {DetailsComponent} from './pages/details/details.component';
import {CreateComponent} from './pages/create/create.component';


const routes: Routes = [
  {path: '', component: IndexComponent},
  {path: 'create', component: CreateComponent},
  {path: ':id', component: DetailsComponent},

 

];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class TravelExpensesRoutingModule { }
