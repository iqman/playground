import { StatusType } from './../status-type.enum';
import { StartingPosition } from './../starting-position';
import { Component, OnInit } from '@angular/core';
import { StartingPositionsService } from '../starting-positions.service';
import { SudokuGridService } from '../sudoku-grid.service';
import { SolverService } from '../solver.service';
import { Board } from '../board';

@Component({
  selector: 'app-sudoku-solver',
  templateUrl: './sudoku-solver.component.html',
  styleUrls: ['./sudoku-solver.component.scss']
})
export class SudokuSolverComponent implements OnInit {

  private _selectedStartingPosition: StartingPosition;
  public numberToSolve: number = 1;
  public logEntries: string[] = [];

  constructor(private startingPositionService: StartingPositionsService,
    private grid: SudokuGridService,
    private solver: SolverService) {}

  ngOnInit(): void {

    this.solver.statusUpdate = (type: StatusType, msg: string) => {
      this.logEntries.push(msg)
    };
  }

  get selectedStartingPositionId():string {
    return this._selectedStartingPosition?.id;
  }
  set selectedStartingPositionId(startingPositionId:string) {

    this._selectedStartingPosition = this.startingPositionService.getStartingPositions()
        .find(s => s.id === startingPositionId);
    
    this.grid.reset(this._selectedStartingPosition);
  }

  get startingPositions():StartingPosition[] {
    return this.startingPositionService.getStartingPositions();
  }

  solveStep() {
    this.solver.solveStep(this.numberToSolve);
    this.numberToSolve = (this.numberToSolve++ % Board.BoardSize+1);
  }

}
