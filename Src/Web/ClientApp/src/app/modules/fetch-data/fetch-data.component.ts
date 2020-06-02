import {Component, Inject} from '@angular/core';
import {HttpClient} from '@angular/common/http';
import {TravelExpense} from "../../shared/model/travel-expense.model";
import {FlowStep} from "../../shared/model/flow-step.model";
import {AuthService} from "../../core/services/auth.service";
import {TravelExpenseService} from "../../shared/services/travel-expense.service";
import {FlowStepService} from "../../shared/services/flow-step.service";

@Component({
  selector: 'app-fetch-data',
  templateUrl: './fetch-data.component.html'
})
export class FetchDataComponent {
  public travelExpenses: TravelExpense[] = [];
  public flowSteps: FlowStep[] = [];

  public selectedTravelExpense: TravelExpense = null;
  public nextFlowStep: FlowStep = null;

  public descriptionInput = '';

  constructor(
    http: HttpClient,
    authService: AuthService,
    private _travelExpenseService: TravelExpenseService,
    private _flowStepService: FlowStepService,
  ) {

    this.getAllTravelExpenses();
    this.getAllFlowSteps();

  }

  getAllTravelExpenses() {
    this._travelExpenseService.getTravelExpenses().subscribe(result => {
        console.log('Got travel expenses', result);
        this.travelExpenses = result;
      },
      error => console.error(error)
    );
  }

  getAllFlowSteps() {
    this._flowStepService.getFlowsteps().subscribe(result => {
      this.flowSteps = result;
    })
  }

  getById(id: string) {
    // @ts-ignore
    this.selectedTravelExpense = 'Loading...';

    this._travelExpenseService.getTravelExpenseById(id).subscribe(result => {
      console.log('The id got', result);
      this.selectedTravelExpense = result;
      this.descriptionInput = this.selectedTravelExpense.description;

      this.getNextFlowStep();
    })
  }

  getNextFlowStep() {
    this._flowStepService.getFlowsteps().subscribe(result => {
      result.forEach((flowStep => {
        if (flowStep.fromStageId === this.selectedTravelExpense.stageId) {
          console.log('Next step for this is', flowStep);
          this.nextFlowStep = flowStep;
        }
      }));
    });
  }

  createTravelExpense() {
    const newTravelExpense = new TravelExpense();
    newTravelExpense.description = 'New travel expense 1';
    this._travelExpenseService.createTravelExpense(newTravelExpense).subscribe(response => {
      this.getAllTravelExpenses();
    });
  }

  updateTravelExpense() {
    const updatedTravelExpense = JSON.parse(JSON.stringify(this.selectedTravelExpense));
    updatedTravelExpense.description = this.descriptionInput;
    this._travelExpenseService.updateTravelExpense(updatedTravelExpense).subscribe(response => {
      console.log('Response from updatefunction', response);
      this.getAllTravelExpenses();
    });
  }

  goBack() {
    this.selectedTravelExpense = null;
  }

  moveToNextFlowStep() {
    console.log('Move travelexpense to', this.nextFlowStep.key);

    this._flowStepService.processStep(this.selectedTravelExpense, this.nextFlowStep).subscribe(response => {
      console.log('Got response from processStep:', response);
    })
  }
}
