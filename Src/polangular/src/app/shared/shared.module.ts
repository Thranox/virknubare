import {ModuleWithProviders, NgModule} from '@angular/core';

//import {TravelExpenseResource} from "./resources/travel-expense.resource";
//import {FlowStepService} from "./services/flow-step.service";
//import {FlowStepResource} from "./resources/flow-step.resource";



//import {UserInfoResource} from "./resources/user-info.resource";
import {UserInfoService} from "./services/user-info.service";



const DECLARATIONS = [
    // Components
    // PostViewComponent,


    // Pipes
    // AssetPipe,

];

const PROVIDERS = [
    //FlowStepService,
    UserInfoService,

    //TravelExpenseResource,
    //FlowStepResource,
    //UserInfoResource,



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
    public static forRoot(): ModuleWithProviders<SharedModule> {
        return {
            ngModule: SharedModule,
            providers: PROVIDERS,
        };
    }
}
