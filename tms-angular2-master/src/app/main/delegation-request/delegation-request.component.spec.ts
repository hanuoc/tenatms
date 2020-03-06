import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DelegationRequestComponent } from './delegation-request.component';

describe('DelegationRequestComponent', () => {
  let component: DelegationRequestComponent;
  let fixture: ComponentFixture<DelegationRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DelegationRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DelegationRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
