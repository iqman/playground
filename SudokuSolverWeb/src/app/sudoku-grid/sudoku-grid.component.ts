import { Component, OnInit } from '@angular/core';
import { SudokuGridService } from '../sudoku-grid.service';
import { SudokuCell } from '../sudoku-cell';

@Component({
  selector: 'app-sudoku-grid',
  templateUrl: './sudoku-grid.component.html',
  styleUrls: ['./sudoku-grid.component.scss']
})
export class SudokuGridComponent implements OnInit {

  constructor(private grid: SudokuGridService) { }

  ngOnInit(): void {
  }

  public findCell(index: number): SudokuCell {
    return this.grid.getCell(index);
  }

}
