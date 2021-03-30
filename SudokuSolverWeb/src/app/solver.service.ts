import { SudokuGridService } from './sudoku-grid.service';
import { Injectable } from '@angular/core';
import { Board } from './board';
import { Cell } from './cell';
import { StatusType } from './status-type.enum';
import { SnapShotContainer } from './snap-shot-container';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class SolverService {

  private readonly boardSnapshots: SnapShotContainer[];
  private stepsSinceLastProgress: number;




  public statusUpdate: (type: StatusType, status: string) => void;
  protected onStatusUpdate(type: StatusType, status: string)
  {
    let s = this.statusUpdate;
    if (s) s(type, status)
  }

  constructor(private grid: SudokuGridService) { }

  solveStep(number: number) {

    var b = this.grid.board;

    b.clearExclusions();

    if (this.isDone(b))
      {
          return;
      }

      this.excludeCells(b, number);

      var cell = this.findCertainPlacementCell(b);

      if (cell)
      {
        cell.highlight = true;
        cell.value = number;

        b.clearFiftyFiftiesForNumber(number);
        this.excludeCells(b, number);

        this.onStatusUpdate(StatusType.Info, `${number}: Found`);

        this.registerProgress();
      }
      else
      {
        this.onStatusUpdate(StatusType.Info, `${number}: Not found`);
      }

      this.findFiftyFifties(b, number);
  }

  private findFiftyFifties(b: Board, number: number)
  {
    var freeRowCells = this.findNFreeCells(2, b.getRow.bind(b));

    if (freeRowCells.length > 0)
    {
      if (freeRowCells.length != freeRowCells.filter(c => c.fiftyFifties.has(number)).length)
      {
        for (var i = 0;i<freeRowCells.length;i++) {
          freeRowCells[i].fiftyFifties = freeRowCells[i].fiftyFifties.add(number);
        }

        this.onStatusUpdate(StatusType.Info, `${number}: Found row 50/50`);

        this.registerProgress();
      }
    }

    var freeColumnCells = this.findNFreeCells(2, b.getColumn.bind(b));

    if (freeColumnCells.length > 0)
    {
      if (freeColumnCells.length != freeColumnCells.filter(c => c.fiftyFifties.has(number)).length)
      {
        for (var i = 0;i<freeColumnCells.length;i++) {
          freeColumnCells[i].fiftyFifties.add(number);
        }

        this.onStatusUpdate(StatusType.Info, `${number}: Found column 50/50`);

        this.registerProgress();
      }
    }
  }

  private findCertainPlacementCell(solverBoard: Board) : Cell
  {
    var cell = this.findSingleFreeCell(solverBoard.getRow.bind(solverBoard));
    if (!cell) {
      cell = this.findSingleFreeCell(solverBoard.getColumn.bind(solverBoard));
    }
    if (!cell) {
      cell = this.findSingleFreeCell(solverBoard.getGroup.bind(solverBoard));
    }

    return cell;
  }

  private findSingleFreeCell(func: (n: number) => Cell[]): Cell
  {
    var freeCells = this.findNFreeCells(1, func);
    if (freeCells.length > 0) {
      return freeCells[0];
    }
    
    return undefined;
  }

  private findNFreeCells(cellsToBeFree: number, func: (n: number) => Cell[]): Cell[]
  {
    var freeCells: Cell[] = [];
    for (var i = 0; i < Board.BoardSize; i++)
    {
      var set = func(i);

      var nonExcluded = set.filter(c => !c.excluded);
      if (nonExcluded.length == cellsToBeFree)
      {
        freeCells = freeCells.concat(nonExcluded);
      }
    }

    return freeCells;
  }

  public isDone(board: Board): boolean
  {
    for (var i = 0; i< Board.BoardSize * Board.BoardSize;i++) {
      if (board.cellAt(i).value === Cell.EmptyValue) {
        return false;
      }
    }

    var valid = this.isBoardValid(board);
    
    if (valid) {
      this.onStatusUpdate(StatusType.Completion, "Sudoku has been solved");
    }

    return valid;
  }

  public isBoardValid(board: Board): boolean
  {
    board.clearExclusions();

    var allValid: boolean = true;

    for (var number = 1; number <= Board.BoardSize; number++)
    {
      this.excludeCells(board, number);

      if (!this.allValid(number, board.getRow.bind(board)))
      {
        this.onStatusUpdate(StatusType.Validity, `${number}: Found invalid row`);
        allValid = false;
      }
      if (!this.allValid(number, board.getColumn.bind(board)))
      {
        this.onStatusUpdate(StatusType.Validity, `${number}: Found invalid column`);
        allValid = false;
      }

      if (!this.allValid(number, board.getGroup.bind(board)))
      {
        this.onStatusUpdate(StatusType.Validity, `${number}: Found invalid group`);
        allValid = false;
      }

      board.clearExclusions();
    }

    if (allValid)
    {
      this.onStatusUpdate(StatusType.Validity, "Board is valid");
    }
    else if (this.boardSnapshots.length == 0)
    {
      this.onStatusUpdate(StatusType.Validity, "Invalid starting position, or invalid placement of number");
    }

    return allValid;
  }

  private allValid(number: number,func: (n: number) => Cell[]): boolean
  {
      for (var i = 0; i < Board.BoardSize; i++)
      {
          var set = func(i);

          if (set.filter(c => c.value === number).length == 1)
          {
              continue;
          }

          if (set.filter(c => c.value === number).length > 1 ||
              set.filter(c => c.excluded == false).filter(c => c.value == Cell.EmptyValue).length == 0)
          {
              return false;
          }
      }

      return true;
  }

  private excludeCells(b: Board, number: number)
  {
      for (var i = 0; i < Board.BoardSize*Board.BoardSize; i++)
      {
        var cell = b.cellAt(i);

        if (cell.value != Cell.EmptyValue)
        {
          cell.excluded = true;
        }
      }

      var indicesToExclude: number[] = [];

      for (var i = 0; i < Board.BoardSize; i++)
      {
          var column = b.getColumn(i);
          if (column.map(c => c.value).indexOf(number)!= -1)
          {
            indicesToExclude = Array.from(new Set([...indicesToExclude, ...b.indicesForColumn(i)]))
          }

          var row = b.getRow(i);
          if (row.map(c => c.value).indexOf(number)!= -1)
          {
            indicesToExclude = Array.from(new Set([...indicesToExclude, ...b.indicesForRow(i)]))
          }

          var group = b.getGroup(i);
          if (group.map(c => c.value).indexOf(number)!= -1)
          {
            indicesToExclude = Array.from(new Set([...indicesToExclude, ...b.indicesForGroup(i)]))
          }
      }

      indicesToExclude.map(i => {b.cellAt(i).excluded = true})
  }

  private registerProgress()
  {
    this.stepsSinceLastProgress = 0;
  }
}
