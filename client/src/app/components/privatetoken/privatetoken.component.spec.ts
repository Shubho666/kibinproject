import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrivatetokenComponent } from './privatetoken.component';

describe('PrivatetokenComponent', () => {
  let component: PrivatetokenComponent;
  let fixture: ComponentFixture<PrivatetokenComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrivatetokenComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrivatetokenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
