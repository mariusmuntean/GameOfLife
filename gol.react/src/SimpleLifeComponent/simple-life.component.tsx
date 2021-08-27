import React, { FC, useCallback } from "react";
import { useState } from "react";
import { Layer, Stage, Rect } from "react-konva";
import { Cell } from "../models/Cell";
import { CellState } from "../models/CellState";
import { Life } from "../models/Life";
import { InitialCellsType } from "../models/InitialCellsType";
import { EmptyCellsType } from "../models/EmptyCellsType";
import { download, pickFile } from "./../utils/download";

interface Props {
  width: number;
  height: number;

  rows: number;
  columns: number;
}

function getInitialLife(columns: number, rows: number, onNewGeneration: (newCells: ReadonlyArray<ReadonlyArray<Cell>>) => void): Life | (() => Life) {
  return () => {
    const initialLife = new Life({ columns, rows } as EmptyCellsType, onNewGeneration);

    // Glider
    initialLife.toggle(2, 2);
    initialLife.toggle(3, 2);
    initialLife.toggle(4, 2);
    initialLife.toggle(4, 1);
    initialLife.toggle(3, 0);

    return initialLife;
  };
}

export const SimpleLife: FC<Props> = ({ width, height, rows, columns }) => {
  const onNewGeneration = (newCells: ReadonlyArray<ReadonlyArray<Cell>>) => {
    // console.dir(newCells);
  };

  const [life, setLife] = useState<Life>(getInitialLife(columns, rows, onNewGeneration));
  const [, updateState] = useState({});
  const forceUpdate = useCallback(() => updateState({}), []);

  const onCellClicked = (row: number, column: number) => {
    life.toggle(row, column);
    forceUpdate();
  };

  const onTick = () => {
    life.tick();
    forceUpdate();
  };

  const onClear = () => {
    setLife(getInitialLife(columns, rows, onNewGeneration));
  };

  const onSave = () => {
    download(`game state ${new Date().toTimeString()}.json`, JSON.stringify(life.cells));
  };

  const onLoad = () => {
    pickFile((fileContent) => {
      const reloadedCellData = JSON.parse(fileContent);
      if (!reloadedCellData) {
        return;
      }

      const reloadedCellsMissingPrototypeChain = reloadedCellData as Array<Array<Cell>>;
      if (!reloadedCellsMissingPrototypeChain) {
        return;
      }

      const reconstructed: Cell[][] = [];
      const rows = reloadedCellsMissingPrototypeChain.length;
      const cols = reloadedCellsMissingPrototypeChain[0]?.length;

      for (let row: number = 0; row < rows; row++) {
        reconstructed[row] = reconstructed[row] ?? [];
        for (let col: number = 0; col < cols; col++) {
          reconstructed[row][col] = new Cell(reloadedCellsMissingPrototypeChain[row][col].CurrentState);
        }
      }

      const initialCell: InitialCellsType = new InitialCellsType();
      initialCell.initialCells = reconstructed;
      setLife(new Life(initialCell));
    });
  };

  const cellEdgeAndSpacingLength = Math.min(width / columns, (height - 30) / rows);
  const cellEdgeLength = 0.9 * cellEdgeAndSpacingLength;

  const canvasWidth = cellEdgeAndSpacingLength * columns;
  const canvasHeight = cellEdgeAndSpacingLength * rows;

  return (
    <>
      <Stage width={canvasWidth} height={canvasHeight}>
        <Layer>
          {life &&
            life.cells.map((cellRow, rowIndex) => {
              return cellRow.map((cell, columnIndex) => {
                return (
                  <Rect
                    key={(rowIndex + 1) * (columnIndex + 1)}
                    x={columnIndex * cellEdgeAndSpacingLength}
                    y={rowIndex * cellEdgeAndSpacingLength}
                    width={cellEdgeLength}
                    height={cellEdgeLength}
                    fill={cell.CurrentState === CellState.Alive ? "red" : "black"}
                    onClick={(e) => onCellClicked(rowIndex, columnIndex)}
                  ></Rect>
                );
              });
            })}
        </Layer>
      </Stage>
      <button onClick={() => onClear()}>Clear</button>
      <button onClick={() => onTick()}>Tick</button>
      <button onClick={() => onSave()}>Save</button>
      <button onClick={() => onLoad()}>Load</button>
    </>
  );
};
