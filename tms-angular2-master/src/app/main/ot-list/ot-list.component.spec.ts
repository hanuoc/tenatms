import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { OtListComponent } from './ot-list.component';

describe('OtListComponent', () => {
  let component: OtListComponent;
  let fixture: ComponentFixture<OtListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ OtListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(OtListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
