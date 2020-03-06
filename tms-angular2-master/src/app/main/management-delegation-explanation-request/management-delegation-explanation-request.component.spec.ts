import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagementDelegationExplanationRequestComponent } from './management-delegation-explanation-request.component';

describe('ManagementDelegationExplanationRequestComponent', () => {
  let component: ManagementDelegationExplanationRequestComponent;
  let fixture: ComponentFixture<ManagementDelegationExplanationRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManagementDelegationExplanationRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManagementDelegationExplanationRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
