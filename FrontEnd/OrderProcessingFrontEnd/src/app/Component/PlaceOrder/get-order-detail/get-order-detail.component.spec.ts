import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetOrderDetailComponent } from './get-order-detail.component';

describe('GetOrderDetailComponent', () => {
  let component: GetOrderDetailComponent;
  let fixture: ComponentFixture<GetOrderDetailComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetOrderDetailComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetOrderDetailComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
