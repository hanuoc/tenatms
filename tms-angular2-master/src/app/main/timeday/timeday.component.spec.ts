import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TimedayComponent } from './timeday.component';

describe('TimedayComponent', () => {
  let component: TimedayComponent;
  let fixture: ComponentFixture<TimedayComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimedayComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimedayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
