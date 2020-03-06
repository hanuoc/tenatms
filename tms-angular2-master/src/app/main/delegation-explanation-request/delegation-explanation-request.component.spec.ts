import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DelegationExplanationRequestComponent } from './delegation-explanation-request.component';

describe('DelegationExplanationRequestComponent', () => {
  let component: DelegationExplanationRequestComponent;
  let fixture: ComponentFixture<DelegationExplanationRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DelegationExplanationRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DelegationExplanationRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
