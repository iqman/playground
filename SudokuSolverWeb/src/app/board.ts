import { Cell } from './cell';

export class Board {

  private _cells: Cell[];

  public static readonly BoardSize: number = 9;

  public constructor()
  {
    this._cells = new Array<Cell>(Board.BoardSize*Board.BoardSize);

      for (var i = 0; i < Board.BoardSize*Board.BoardSize; i++)
      {
        this._cells[i] = new Cell();
      }
  }

  public clone() : Board
  {
      var b = new Board();
      for (var i = 0; i < this._cells.length; i++)
      {
          b._cells[i] = this._cells[i].clone();
      }

      return b;
  }

  public restore(b: Board)
  {
      for (var i = 0; i < this._cells.length; i++)
      {
          this._cells[i] = b._cells[i];
      }
  }

  public init(startValues: number[])
  {
      for (var i = 0; i < Board.BoardSize * Board.BoardSize; i++)
      {
          this._cells[i] = new Cell();
          this._cells[i].value =startValues[i];
      }
  }

  public cellAt(index: number): Cell
  {
      return this._cells[index];
  }


  public getCell(x: number, y: number): Cell
  {
      return this._cells[x+y* Board.BoardSize];
  }

  public getGroup(groupNumber: number): Cell[]
  {
      var indices = this.indicesForGroup(groupNumber);
      var res: Cell[] = [];
      for (var i = 0; i < this._cells.length; i++)
      {
          if (indices.indexOf(i) != -1)
          {
              res.push(this._cells[i]);
          }
      }
      return res;
  }

  public getRow(rowNumber: number): Cell[]
  {
      var indices = this.indicesForRow(rowNumber);
      var res: Cell[] = [];
      for (var i = 0; i < this._cells.length; i++)
      {
        if (indices.indexOf(i) != -1)
          {
              res.push(this._cells[i]);
          }
      }
      return res;
  }

  public getColumn(columnNumber: number): Cell[]
  {
      var indices = this.indicesForColumn(columnNumber);
      var res: Cell[] = [];
      for (var i = 0; i < this._cells.length; i++)
      {
        if (indices.indexOf(i) != -1)
          {
              res.push(this._cells[i]);
          }
      }
      return res;
  }

  public indicesForRow(rowNumber: number): number[]
  {
      var indices: number[] = [];
      for (var i = 0; i < this._cells.length; i++)
      {
          if (i >= Board.BoardSize * rowNumber && i < Board.BoardSize * (rowNumber + 1))
          {
              indices.push(i);
          }
      }
      return indices;
  }

  public indicesForColumn(columnNumber: number): number[]
  {
    var indices: number[] = [];
      for (var i = 0; i < this._cells.length; i++)
      {
          if (i % Board.BoardSize == columnNumber)
          {
              indices.push(i);
          }
      }
      return indices;
  }

  public indicesForGroup(groupNumber: number): number[]
  {
      if (groupNumber == 3 || groupNumber == 4 || groupNumber == 5)
      {
          groupNumber += 6;
      }

      if (groupNumber == 6 || groupNumber == 7 || groupNumber == 8)
      {
          groupNumber += 12;
      }

      var indices: number[] = [];
      for (var i = 0; i < this._cells.length; i++)
      {
          if (i >= (0 + groupNumber) * 3 && i < (0 + groupNumber) * 3 + 3 ||
              i >= (3 + groupNumber) * 3 && i < (3 + groupNumber) * 3 + 3 ||
              i >= (6 + groupNumber) * 3 && i < (6 + groupNumber) * 3 + 3)
          {
              indices.push(i);
          }
      }
      return indices;
  }

  public clearExclusions()
  {
      for (var i = 0; i < this._cells.length; i++)
      {
          this._cells[i].excluded = false;
      }
  }

  public clearFiftyFifties()
  {
      for (var i = 0; i < this._cells.length; i++)
      {
          this._cells[i].fiftyFifties = new Set<number>();
      }
  }

  public clearFiftyFiftiesForNumber(number: number)
  {
      for (var i = 0; i < this._cells.length; i++)
      {
        this._cells[i].fiftyFifties.delete(number);
      }
  }
}
