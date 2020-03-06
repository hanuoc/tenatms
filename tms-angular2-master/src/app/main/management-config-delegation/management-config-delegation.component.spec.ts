import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ManagementConfigDelegationComponent } from './management-config-delegation.component';

describe('ManagementConfigDelegationComponent', () => {
  let component: ManagementConfigDelegationComponent;
  let fixture: ComponentFixture<ManagementConfigDelegationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ManagementConfigDelegationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ManagementConfigDelegationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
