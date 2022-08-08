namespace Model;

class Tsuro
{
    Board board { get; }
    List<Tile> tiles { get; }

    Player[] players { get; }
    Pawn[] pawns { get; }

    public Tsuro(int boardWidth, int boardLength, (Direction, int)[] directionMap, (int, int)[][] allTilesConnections, int nPlayers)
    {
        board = new Board(boardWidth, boardLength);

        Tile.endpointIdToDirection = directionMap;

        tiles = new List<Tile>();
        for (var i = 0; i < allTilesConnections.Length; i++)
        {
            tiles.Add(new Tile(i, allTilesConnections[i]));
        }

        players = new Player[nPlayers];
        pawns = new Pawn[nPlayers];
        for (var i = 0; i < nPlayers; i++)
        {
            pawns[i] = new Pawn(i);
            players[i] = new Player(i, board, pawns[i]);
        }
    }
}