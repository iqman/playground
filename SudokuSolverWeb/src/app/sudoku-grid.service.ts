import { StartingPosition } from './starting-position';
import { Injectable } from '@angular/core';
import { SudokuCell } from './sudoku-cell';
import { StartingPositionsService } from './starting-positions.service';

@Injectable({
  providedIn: 'root'
})
export class SudokuGridService {

  private _grid: SudokuCell[];

  constructor(private startingPositionService: StartingPositionsService) {
    this.reset(this.startingPositionService.getEmptyStartingPositions);
  }

  private counter: number = 0;

  get grid(): SudokuCell[] {
    return this._grid;
  }
  public getCell(index: number): SudokuCell {
    return this.grid[index];
  }

  public exclude(index: number) {
    this._grid[index].excluded = true;
  }

  public reset(startingPosition: StartingPosition) {
    
    this._grid = [];

    startingPosition.values.map(v => this._grid.push(new SudokuCell(v, false, false, false, false, [])));
    
  }
}
