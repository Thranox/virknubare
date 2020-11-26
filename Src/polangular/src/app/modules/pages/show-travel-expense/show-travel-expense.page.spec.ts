import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { ShowTravelExpensePage } from './show-travel-expense.page';

describe('ShowTravelExpensePage', () => {
  let component: ShowTravelExpensePage;
  let fixture: ComponentFixture<ShowTravelExpensePage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ShowTravelExpensePage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(ShowTravelExpensePage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
