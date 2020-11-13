import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { CreateTravelExpensePage } from './create-travel-expense.page';

describe('CreateTravelExpensePage', () => {
  let component: CreateTravelExpensePage;
  let fixture: ComponentFixture<CreateTravelExpensePage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CreateTravelExpensePage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(CreateTravelExpensePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
