import { ComponentFixture, TestBed } from '@angular/core/testing';

import { GetCustomersComponent } from './get-customers.component';

describe('GetCustomersComponent', () => {
  let component: GetCustomersComponent;
  let fixture: ComponentFixture<GetCustomersComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [GetCustomersComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(GetCustomersComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
