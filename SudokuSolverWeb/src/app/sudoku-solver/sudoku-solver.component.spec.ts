import { ComponentFixture, TestBed, waitForAsync } from '@angular/core/testing';

import { SudokuSolverComponent } from './sudoku-solver.component';

describe('SudokuSolverComponent', () => {
  let component: SudokuSolverComponent;
  let fixture: ComponentFixture<SudokuSolverComponent>;

  beforeEach(waitForAsync(() => {
    TestBed.configureTestingModule({
      declarations: [ SudokuSolverComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SudokuSolverComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
