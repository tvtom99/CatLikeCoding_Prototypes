
/*
    CellState
    ---------
    Cellstate is an enum that is used to track the state of a mine in the game. Mines can be in 1 of several states; displaying the number of mines around it, flagged by the user as representing something, be a mine, unrevealed, etc
 */

[System.Flags]
public enum CellState
{
    Zero,
    One,
    Two,
    Three,
    Four,
    Five,
    Six,
    Mine = 1 << 3,   //This is because numbers up to 6 only need 3 bits. The 4th bit is used to denote if there is a mine or not. 000 001 010 011 100 101 = 1 2 3 4 5 6
    MarkedSure = 1 << 4,
    MarkedUnsure = 1 << 5,  //These follow the same idea but are for the user flags that can be put onto mines
    Revealed = 1 << 6,
    Marked = MarkedSure | MarkedUnsure,
    MarkedOrRevealed = Marked | Revealed,   //These are combined masks that make checking cell states easier
    MarkedSureOrMine = MarkedSure | Mine
}


//The following class is used to abstract functions when checking the state of a mine
public static class CellStateExtensionMethods
{
    public static bool Is(this CellState state, CellState mask) => (state & mask) != 0; //Check if the state of a cell is equal to a given mask
    public static bool IsNot(this CellState state, CellState mask) => (mask & state) == 0;  //Check if the state of a cell is NOT equal to a given mask
    public static CellState With(this CellState state, CellState mask) => state | mask; //do some googling to work out what these are doing. I thought | was or but IDK how this works then?
    public static CellState Without(this CellState state, CellState mask) => state & ~mask;
}