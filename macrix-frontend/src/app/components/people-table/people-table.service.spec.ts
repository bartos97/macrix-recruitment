import { TestBed } from '@angular/core/testing';

import { PeopleTableService } from './people-table.service';

describe('PeopleTableService', () => {
  let service: PeopleTableService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(PeopleTableService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
