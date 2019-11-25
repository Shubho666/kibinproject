import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CardsRequestsComponent } from './cards-requests.component';

describe('CardsRequestsComponent', () => {
  let component: CardsRequestsComponent;
  let fixture: ComponentFixture<CardsRequestsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CardsRequestsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CardsRequestsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
