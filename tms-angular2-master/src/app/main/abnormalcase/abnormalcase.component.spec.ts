import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AbnormalcaseComponent } from './abnormalcase.component';

describe('AbnormalcaseComponent', () => {
  let component: AbnormalcaseComponent;
  let fixture: ComponentFixture<AbnormalcaseComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AbnormalcaseComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AbnormalcaseComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
