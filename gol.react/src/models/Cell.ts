import { CellState } from "./CellState";

export class Cell {
  public CurrentState: CellState = CellState.Dead;
  public NextState: CellState = CellState.Dead;

  constructor(currenCellState?: CellState) {
    if (currenCellState) {
      this.CurrentState = currenCellState;
    }
  }

  public tick = () => {
    this.CurrentState = this.NextState;
    this.NextState = CellState.Dead;
  };

  public toggle = () => {
    this.CurrentState = this.CurrentState === CellState.Alive ? CellState.Dead : CellState.Alive;
  };
}
