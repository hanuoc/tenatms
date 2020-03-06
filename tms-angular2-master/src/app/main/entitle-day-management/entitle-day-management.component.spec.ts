import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EntitleDayManagementComponent } from './entitle-day-management.component';

describe('EntitleDayManagementComponent', () => {
  let component: EntitleDayManagementComponent;
  let fixture: ComponentFixture<EntitleDayManagementComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EntitleDayManagementComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EntitleDayManagementComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
