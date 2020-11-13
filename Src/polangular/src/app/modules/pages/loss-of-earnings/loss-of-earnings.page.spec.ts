import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { LossOfEarningsPage } from './loss-of-earnings.page';

describe('LossOfEarningsPage', () => {
  let component: LossOfEarningsPage;
  let fixture: ComponentFixture<LossOfEarningsPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ LossOfEarningsPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(LossOfEarningsPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
