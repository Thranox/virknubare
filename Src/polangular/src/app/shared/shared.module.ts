import {ModuleWithProviders, NgModule} from '@angular/core';
import {TravelExpenseService} from "./services/travel-expense.service";
import {TravelExpenseResource} from "./resources/travel-expense.resource";
import {FlowStepService} from "./services/flow-step.service";
import {FlowStepResource} from "./resources/flow-step.resource";
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";
import {NgbDateAdapter, NgbDateParserFormatter, NgbTimeAdapter} from "@ng-bootstrap/ng-bootstrap";
import {NgbDateDayjsAdapter} from "./others/datepicker-adapter";
import {DanishDateParserFormatter} from "./others/datepicker-custom-formatter";
import {NgbTimeStringAdapter} from "./others/timepicker-adapter";
import {MockTravelExpenseService} from "./mocks/mock-travel-expense.service";



const DECLARATIONS = [
    // Components
    // PostViewComponent,


    // Pipes
    // AssetPipe,

];

const PROVIDERS = [
    // Services
    // DemoService,
    TravelExpenseService,
    MockTravelExpenseService,
    FlowStepService,


    // Resources
    // DemoResource,
    TravelExpenseResource,
    FlowStepResource,

    // States
    // ProfileState,

    // FullcalendarConfigurator,

    // others
    {provide: NgbTimeAdapter, useClass: NgbTimeStringAdapter},
    {provide: NgbDateAdapter, useClass: NgbDateDayjsAdapter},
    {provide: NgbDateParserFormatter, useClass: DanishDateParserFormatter}
];

@NgModule({
    imports: [
    ],
    declarations: DECLARATIONS,
    exports: DECLARATIONS,
    entryComponents: [
        // FileImageModalComponent,
    ]
})
export class SharedModule {
    public static forRoot(): ModuleWithProviders {
        return {
            ngModule: SharedModule,
            providers: PROVIDERS,
        };
    }
}
