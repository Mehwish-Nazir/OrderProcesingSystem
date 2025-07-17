import { TestBed } from '@angular/core/testing';

import { CustomersService } from '../Customer/customers.service';

describe('CustomersService', () => {
  let service: CustomersService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CustomersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
