import { Cell } from "./Cell";
import { CellState } from "./CellState";
import { EmptyCellsType } from "./EmptyCellsType";
import { InitialCellsType } from "./InitialCellsType";

export class Life {
  readonly columns: number;
  readonly rows: number;
  readonly onNewGeneration?: (newCells: ReadonlyArray<ReadonlyArray<Cell>>) => void;

  private _cells: Cell[][];

  public get cells(): Cell[][] {
    return this._cells;
  }

  constructor(input: InitialCellsType | EmptyCellsType, onNewGeneration?: (newCells: ReadonlyArray<ReadonlyArray<Cell>>) => void) {
    if (input instanceof InitialCellsType) {
      this._cells = input.initialCells;
      this.rows = this._cells.length;
      this.columns = this._cells[0].length;
    } else {
      this.columns = input.columns;
      this.rows = input.rows;
      if (this.columns <= 0 || this.rows <= 0) {
        throw new Error("Width and height must be greater than 0");
      }
      this._cells = [];
      for (let row: number = 0; row < this.rows; row++) {
        for (let col: number = 0; col < this.columns; col++) {
          this._cells[row] = this._cells[row] ?? [];
          this._cells[row][col] = new Cell(CellState.Dead);
        }
      }
    }

    this.onNewGeneration = onNewGeneration;
  }

  public tick = () => {
    // Compute the next state for each cell
    for (let row: number = 0; row < this.rows; row++) {
      for (let col: number = 0; col < this.columns; col++) {
        const currentCell = this._cells[row][col];
        const cellNeighbors = this.getNeighbors(row, col);
        const liveNeighbors = cellNeighbors.filter((neighbor) => neighbor.CurrentState === CellState.Alive).length;

        // Rule source - https://en.wikipedia.org/wiki/Conway%27s_Game_of_Life#Rules
        if (currentCell.CurrentState === CellState.Alive && (liveNeighbors === 2 || liveNeighbors === 3)) {
          currentCell.NextState = CellState.Alive;
        } else if (currentCell.CurrentState === CellState.Dead && liveNeighbors === 3) {
          currentCell.NextState = CellState.Alive;
        } else {
          currentCell.NextState = CellState.Dead;
        }
      }
    }

    // Switch each cell to its next state
    for (let row: number = 0; row < this.rows; row++) {
      for (let col: number = 0; col < this.columns; col++) {
        const currentCell = this._cells[row][col];
        currentCell.tick();
      }
    }

    this.onNewGeneration?.(this.cells);
  };

  public toggle = (row: number, col: number) => {
    if (row < 0 || row >= this.rows) {
      throw new Error(`Row '${row}' is out of range [${0} - ${this.rows}]`);
    }

    if (col < 0 || col >= this.columns) {
      throw new Error(`Col '${col}' is out of range [${0} - ${this.columns}]`);
    }

    const cellToToggle = this.cells[row][col];
    cellToToggle.toggle();
  };

  private getNeighbors = (row: number, col: number): Cell[] => {
    const neighbors: Cell[] = [];

    for (let colOffset: number = -1; colOffset <= 1; colOffset++) {
      for (let rowOffset: number = -1; rowOffset <= 1; rowOffset++) {
        if (colOffset === 0 && rowOffset === 0) {
          // skip self
          continue;
        }

        const neighborRow = row + rowOffset;
        const neighborCol = col + colOffset;
        if (neighborRow >= 0 && neighborRow < this.rows) {
          if (neighborCol >= 0 && neighborCol < this.columns) {
            neighbors.push(this._cells[neighborRow][neighborCol]);
          }
        }
      }
    }

    return neighbors;
  };
}
