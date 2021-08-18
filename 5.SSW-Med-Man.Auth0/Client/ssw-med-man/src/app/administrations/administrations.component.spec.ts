import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AdministrationsComponent } from './administrations.component';

describe('AdministrationsComponent', () => {
  let component: AdministrationsComponent;
  let fixture: ComponentFixture<AdministrationsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AdministrationsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AdministrationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
