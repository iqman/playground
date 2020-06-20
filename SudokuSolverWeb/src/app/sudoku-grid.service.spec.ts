import { TestBed } from '@angular/core/testing';

import { SudokuGridService } from './sudoku-grid.service';

describe('SudokuGridService', () => {
  let service: SudokuGridService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SudokuGridService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
