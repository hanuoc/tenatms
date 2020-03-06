import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OTRequestComponent } from './ot-request.component';

describe('OTRequestComponent', () => {
  let component: OTRequestComponent;
  let fixture: ComponentFixture<OTRequestComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OTRequestComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OTRequestComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
