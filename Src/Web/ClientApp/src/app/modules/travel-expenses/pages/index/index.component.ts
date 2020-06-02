import { Component, OnInit } from '@angular/core';
import {TravelExpense} from '../../../../shared/model/travel-expense.model';
import {from, Observable} from 'rxjs';
import {TravelExpenseService} from '../../../../shared/services/travel-expense.service';
import {MockTravelExpenseService} from "../../../../shared/mocks/mock-travel-expense.service";

@Component({
  selector: 'app-index',
  templateUrl: './index.component.html',
  styleUrls: ['./index.component.scss']
})
export class IndexComponent implements OnInit {
    travelExpenses$: Observable<TravelExpense[]>;

  constructor(private travelExpenseService: TravelExpenseService) {}

  ngOnInit(): void {
      this.travelExpenses$ = this.travelExpenseService.getTravelExpenses();
  }

}
