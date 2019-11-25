import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { IdeazoneComponent } from './ideazone.component';

describe('IdeazoneComponent', () => {
  let component: IdeazoneComponent;
  let fixture: ComponentFixture<IdeazoneComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ IdeazoneComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(IdeazoneComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
