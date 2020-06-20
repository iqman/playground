import { BrowserModule } from '@angular/platform-browser';
import { NgModule } from '@angular/core';

import { AppRoutingModule } from './app-routing.module';
import { AppComponent } from './app.component';
import { SudokuSolverComponent } from './sudoku-solver/sudoku-solver.component';
import { SudokuCellComponent } from './sudoku-cell/sudoku-cell.component';
import { SudokuGridComponent } from './sudoku-grid/sudoku-grid.component';

@NgModule({
  declarations: [
    AppComponent,
    SudokuSolverComponent,
    SudokuCellComponent,
    SudokuGridComponent
  ],
  imports: [
    BrowserModule,
    AppRoutingModule
  ],
  providers: [],
  bootstrap: [AppComponent]
})
export class AppModule { }
