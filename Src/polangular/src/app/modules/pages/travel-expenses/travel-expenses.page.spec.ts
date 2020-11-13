import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { TravelExpensesPage } from './travel-expenses.page';

describe('TravelExpensesPage', () => {
  let component: TravelExpensesPage;
  let fixture: ComponentFixture<TravelExpensesPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TravelExpensesPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(TravelExpensesPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
