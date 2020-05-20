import {ModuleWithProviders, NgModule} from '@angular/core';
import {TravelExpenseService} from "./services/travel-expense.service";
import {TravelExpenseResource} from "./resources/travel-expense.resource";
import {FlowStepService} from "./services/flow-step.service";
import {FlowStepResource} from "./resources/flow-step.resource";
import {CommonModule} from "@angular/common";
import {FormsModule} from "@angular/forms";



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
    FlowStepService,


    // Resources
    // DemoResource,
    TravelExpenseResource,
    FlowStepResource,

    // States
    // ProfileState,

    // FullcalendarConfigurator,

    // others
    // {provide: NgbTimeAdapter, useClass: NgbTimeStringAdapter},
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
