import { TestBed } from '@angular/core/testing';

import { StartingPositionsService } from './starting-positions.service';

describe('StartingPositionsService', () => {
  let service: StartingPositionsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(StartingPositionsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
