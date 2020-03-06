import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EntitleDayComponent } from './entitle-day.component';

describe('EntitleDayComponent', () => {
  let component: EntitleDayComponent;
  let fixture: ComponentFixture<EntitleDayComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EntitleDayComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EntitleDayComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
