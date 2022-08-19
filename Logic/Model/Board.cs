namespace Model;

using static Model.Direction;

public class Board
{
    public Tile[][] fields { get; }

    public Board(int width, int length)
    {
        fields = new Tile[width][];
        for (var i = 0; i < length; i++)
        {
            fields[i] = new Tile[length];
        }
    }

    public void PlaceTile(Tile tile, Direction direction, int x, int y)
    {
        if (fields[x][y] != null) throw new ArgumentException("The specified field on the board is already occupied");
        fields[x][y] = tile;
        tile.Place(direction);

        if (x - 1 >= 0) fields[x - 1][y].ConnectTo(tile, UP);
        if (x + 1 < fields.Length) fields[x + 1][y].ConnectTo(tile, DOWN);
        if (y - 1 >= 0) fields[x][y - 1].ConnectTo(tile, LEFT);
        if (y + 1 < fields[x].Length) fields[x][y + 1].ConnectTo(tile, RIGHT);

        CheckMove(x, y);
    }

    public void CheckMove(int x, int y)
    {
        fields[x][y].MovePawns();
        if (x - 1 >= 0) fields[x - 1][y].MovePawns();
        if (x + 1 < fields.Length) fields[x + 1][y].MovePawns();
        if (y - 1 >= 0) fields[x][y - 1].MovePawns();
        if (y + 1 < fields[x].Length) fields[x][y + 1].MovePawns();
    }
}
