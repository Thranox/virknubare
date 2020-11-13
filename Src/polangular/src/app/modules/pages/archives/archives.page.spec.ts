import { async, ComponentFixture, TestBed } from '@angular/core/testing';
import { IonicModule } from '@ionic/angular';

import { ArchivesPage } from './archives.page';

describe('ArchivesPage', () => {
  let component: ArchivesPage;
  let fixture: ComponentFixture<ArchivesPage>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ArchivesPage ],
      imports: [IonicModule.forRoot()]
    }).compileComponents();

    fixture = TestBed.createComponent(ArchivesPage);
    component = fixture.componentInstance;
    fixture.detectChanges();
  }));

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
