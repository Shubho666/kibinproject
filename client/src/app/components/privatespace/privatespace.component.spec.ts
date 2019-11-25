import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { PrivatespaceComponent } from './privatespace.component';

describe('PrivatespaceComponent', () => {
  let component: PrivatespaceComponent;
  let fixture: ComponentFixture<PrivatespaceComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ PrivatespaceComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(PrivatespaceComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
