import { ComponentFixture, TestBed } from '@angular/core/testing';

import { CustomerWithUserDetailComponent } from './customer-with-user-detail.component';

describe('CustomerWithUserDetailComponent', () => {
  let component: CustomerWithUserDetailComponent;
  let fixture: ComponentFixture<CustomerWithUserDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [CustomerWithUserDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(CustomerWithUserDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
