import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagementDelegationRequestComponent } from './management-delegation-request.component';

describe('ManagementDelegationRequestComponent', () => {
  let component: ManagementDelegationRequestComponent;
  let fixture: ComponentFixture<ManagementDelegationRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManagementDelegationRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManagementDelegationRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
