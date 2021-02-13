import { StartingPosition } from './../starting-position';
import { Component, OnInit } from '@angular/core';
import { StartingPositionsService } from '../starting-positions.service';

@Component({
  selector: 'app-sudoku-solver',
  templateUrl: './sudoku-solver.component.html',
  styleUrls: ['./sudoku-solver.component.scss']
})
export class SudokuSolverComponent implements OnInit {

  constructor(private startingPositionService: StartingPositionsService) { }

  ngOnInit(): void {
  }

  public startingPositions(): StartingPosition[] {
    return this.startingPositionService.getStartingPositions();
  }

}
