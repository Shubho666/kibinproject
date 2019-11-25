import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ChatsystemComponent } from './chatsystem.component';

describe('ChatsystemComponent', () => {
  let component: ChatsystemComponent;
  let fixture: ComponentFixture<ChatsystemComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ChatsystemComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ChatsystemComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
