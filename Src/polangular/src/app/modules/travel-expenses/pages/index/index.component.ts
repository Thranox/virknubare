import { Component, OnInit } from '@angular/core';
import {TravelExpense} from '../../../../shared/model/travel-expense.model';
import {from, Observable} from 'rxjs';
import {MockTravelExpenseService} from '../../../../shared/mocks/mock-travel-expense.service';
import {TravelExpenseService} from '../../../../shared/services/travel-expense.service';
import {ColumnMode} from '@swimlane/ngx-datatable';

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit {
    travelExpenses$: Observable<TravelExpense[]>;
    ngxDatatableColumnMode = ColumnMode;

    travelExpenses: TravelExpense[] = [];
    travelExpensesRows: TravelExpense[] = [];

  constructor(private travelExpenseService: TravelExpenseService) {}

  ngOnInit(): void {
      this.travelExpenses$ = this.travelExpenseService.getTravelExpenses();

      this.travelExpenses$.subscribe((travelExpenses) => {
          this.travelExpenses = travelExpenses;
          this.travelExpensesRows = travelExpenses;
      });
  }



    updateFilter(event) {
        const val = event.target.value.toLowerCase();

        console.log('Filtering on', val);

        if (val === '') {
            this.travelExpensesRows = this.travelExpenses;
        }

        // filter our data
        this.travelExpensesRows = this.travelExpensesRows.filter((d) => {
            return d.description.toLowerCase().indexOf(val) !== -1 || !val;
        });

        // Whenever the filter changes, always go back to the first page
        // this.table.offset = 0;
    }

}
