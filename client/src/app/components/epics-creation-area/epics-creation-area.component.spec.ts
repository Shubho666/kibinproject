import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EpicsCreationAreaComponent } from './epics-creation-area.component';

describe('EpicsCreationAreaComponent', () => {
  let component: EpicsCreationAreaComponent;
  let fixture: ComponentFixture<EpicsCreationAreaComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EpicsCreationAreaComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EpicsCreationAreaComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
