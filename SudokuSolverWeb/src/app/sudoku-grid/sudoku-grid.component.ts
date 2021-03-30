import { Component, OnInit } from '@angular/core';
import { SudokuGridService } from '../sudoku-grid.service';
import { Cell } from '../cell';

@Component({
  selector: 'app-sudoku-grid',
  templateUrl: './sudoku-grid.component.html',
  styleUrls: ['./sudoku-grid.component.scss']
})
export class SudokuGridComponent implements OnInit {

  constructor(private grid: SudokuGridService) { }

  ngOnInit(): void {
  }

  public findCell(index: number): Cell {
    return this.grid.board.cellAt(index);
  }

}
