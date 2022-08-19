namespace Model;

public class Player
{
    public readonly int id;

    Board board;
    public Pawn pawn {get;}

    public List<Tile> tiles {get;}

    public Player(int _id, Board _board, Pawn _pawn)
    {
        id = _id;
        board = _board;
        pawn = _pawn;
        tiles = new List<Tile>();
    }

    public void PlaceTile(int tileId, Direction direction, int boardX, int boardY)
    {
        var tile = tiles.Find(tile => tile.id == tileId);
        if (tile == null) throw new ArgumentException("The specified tile id is not in the players hand");
        board.PlaceTile(tile, direction, boardX, boardY);
        tiles.Remove(tile);
    }
}