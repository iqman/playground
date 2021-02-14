import { SudokuGridService } from './sudoku-grid.service';
import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class SolverService {

  constructor(private grid: SudokuGridService) { }

  solveStep(n: number) {
    this.grid.exclude(n);
  }
}
