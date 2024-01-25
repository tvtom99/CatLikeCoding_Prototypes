using Unity.Collections;
using UnityEngine;

public struct Grid
{
    /*The following was copied from the tutorial but I will explain what I understand it to do*/
    //Basically, 'get; private set;' is dictating how other things can interact with this variable. So, Grid.Row is fine to be called anywhere, but Grid.Row = 5 can only be done within this struct.
    public int Rows { get; private set; }
    public int Columns { get; private set; }
    public int CellCount => states.Length;

    NativeArray<CellState> states;  //I assume the cells are being stored in a NativeArray so it is easier for us to use the GPU to implement them in the future.

    public void Initialise(int rows, int columns)
    {
        Rows = rows; 
        Columns = columns;
        states = new NativeArray<CellState>(Rows * Columns, Allocator.Persistent);
    }

    public void Dispose()
    {
        states.Dispose();
    }

    public CellState this[int i]
    {
        get => states[i];
        set => states[i] = value;
    }
}