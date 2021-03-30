import { Cell } from '../cell';
import { Component, OnInit, Input } from '@angular/core';

@Component({
  selector: 'app-sudoku-cell',
  templateUrl: './sudoku-cell.component.html',
  styleUrls: ['./sudoku-cell.component.scss']
})
export class SudokuCellComponent implements OnInit {

  constructor() { }

  @Input()
  public cell: Cell;

  ngOnInit(): void {
  }

}
