import { Injectable } from '@angular/core';
import { SudokuCell } from './sudoku-cell';

@Injectable({
  providedIn: 'root'
})
export class SudokuGridService {

  constructor() { }

  private counter: number = 0;

  public getCell(index: number): SudokuCell {
    let c = new SudokuCell();
    c.value = ++this.counter % 9;
    c.excluded = index % 2 == 0;
    c.highlight = index % 3 == 0;
    c.guessed = index % 5 == 0;

    //debugger;

    return c;
  }
}
