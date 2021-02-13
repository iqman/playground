import { Injectable } from '@angular/core';
import { StartingPosition } from './starting-position';

@Injectable({
  providedIn: 'root'
})
export class StartingPositionsService {

  private startingPositions: StartingPosition[] = [
    new StartingPosition("1", "RRf6bgb9GG",

        [6, 0, 9, 1, 0, 2, 0, 8, 0,
        0, 0, 0, 0, 0, 0, 4, 0, 0,
        5, 0, 2, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 2, 0, 3, 0, 4,
        1, 0, 0, 0, 0, 5, 0, 0, 0,
        0, 2, 0, 0, 0, 0, 5, 0, 6,
        0, 0, 0, 8, 0, 1, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 9,
        8, 0, 5, 9, 0, 7, 0, 4, 0])
  ];

  constructor() {

    // https://cracking-the-cryptic.web.app/sudoku/RRf6bgb9GG
    //this.startingPositions.push(
     // );
/*
    // https://sudoku.game/ easy
    private static readonly int[] Easy =
    {
        0, 0, 0, 0, 0, 0, 4, 0, 2,
        0, 9, 4, 5, 0, 2, 0, 0, 0,
        2, 0, 8, 0, 0, 0, 0, 1, 0,
        0, 5, 3, 0, 4, 0, 0, 0, 7,
        0, 8, 0, 1, 2, 5, 0, 6, 0,
        1, 0, 0, 0, 6, 0, 9, 5, 0,
        0, 7, 0, 0, 0, 0, 2, 0, 1,
        0, 0, 0, 4, 0, 6, 8, 3, 0,
        4, 0, 9, 0, 0, 0, 0, 0, 0
    };

    // https://sudoku.game/ medium
    private static readonly int[] Medium =
    {
        0, 0, 0, 2, 0, 0, 0, 6, 4,
        0, 6, 5, 0, 0, 3, 2, 0, 0,
        7, 1, 2, 4, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 5, 1,
        0, 8, 4, 0, 0, 0, 6, 7, 0,
        2, 5, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 7, 3, 4, 9,
        0, 0, 1, 9, 0, 0, 7, 8, 0,
        5, 9, 0, 0, 0, 4, 0, 0, 0
    };

    // https://sudoku.game/ Hard
    private static readonly int[] Hard =
    {
        0, 0, 5, 0, 6, 0, 0, 8, 0,
        1, 4, 0, 0, 0, 0, 0, 0, 0,
        0, 6, 9, 1, 0, 0, 0, 2, 0,
        9, 0, 0, 0, 4, 0, 6, 0, 0,
        0, 3, 0, 2, 8, 0, 0, 5, 0,
        4, 0, 0, 0, 3, 6, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 7, 0, 0, 5, 0, 2,
        2, 9, 0, 0, 0, 0, 3, 6, 0
    };

    // https://sudoku.game/ Very Hard
    private static readonly int[] VeryHard =
    {
        0, 5, 0, 0, 3, 2, 6, 0, 8,
        0, 0, 0, 1, 0, 5, 7, 3, 0,
        0, 0, 0, 0, 0, 6, 0, 0, 2,
        0, 0, 0, 0, 5, 0, 0, 8, 7,
        0, 0, 7, 0, 1, 0, 2, 0, 0,
        3, 6, 0, 0, 8, 0, 0, 0, 0,
        4, 0, 0, 7, 0, 0, 0, 0, 0,
        0, 7, 6, 3, 0, 4, 0, 0, 0,
        8, 0, 9, 5, 2, 0, 0, 7, 0
    };

    private static readonly int[] UH2020_27_SS =
    {
        0, 0, 0, 6, 9, 0, 0, 1, 8,
        0, 0, 9, 1, 0, 0, 0, 0, 0,
        0, 1, 6, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 1, 2, 0,
        0, 0, 2, 3, 0, 4, 5, 0, 0,
        0, 6, 7, 0, 0, 0, 0, 0, 0,
        0, 0, 0, 0, 0, 0, 7, 3, 0,
        0, 0, 0, 0, 0, 5, 8, 0, 0,
        4, 8, 0, 0, 7, 9, 0, 0, 0
    };
    */
  }

  public getStartingPositions() : StartingPosition[] {
    return this.startingPositions;
  } 
}
