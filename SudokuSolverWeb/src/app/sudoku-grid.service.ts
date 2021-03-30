import { Board } from './board';
import { StartingPosition } from './starting-position';
import { Injectable } from '@angular/core';
import { Cell } from './cell';
import { StartingPositionsService } from './starting-positions.service';

@Injectable({
  providedIn: 'root'
})
export class SudokuGridService {

  _board: Board;

  constructor(private startingPositionService: StartingPositionsService) {
    this.reset(this.startingPositionService.getEmptyStartingPositions);
  }

  get board(): Board {
    return this._board;
  }

  public reset(startingPosition: StartingPosition) {
    
    this._board = new Board();

    this._board.init(startingPosition.values);
  }
}
