namespace Model;

public class Tile
{
    public static (Direction, int)[] endpointIdToDirection { get; set; } = new (Direction, int)[0];

    public readonly int id;

    public Path[] paths { get; }
    public Direction? facing;

    public Tile(int _id, (int, int)[] connections)
    {
        id = _id;

        //check for unique connections
        var entries = new int[connections.Length * 2];
        for (var i = 0; i < connections.Length; i++)
        {
            var (alfa, beta) = connections[i];
            entries[2 * i] = alfa;
            entries[2 * i + 1] = beta;
        }
        if (entries.Distinct().Count() != entries.Length) throw new ArgumentException("Each connection can only uccur once");

        paths = new Path[2 * connections.Length];
        for (var i = 0; i < connections.Length; i++)
        {
            var path = new Path();
            var (alfa, beta) = connections[i];
            paths[alfa] = path;
            paths[beta] = path;
        }
    }

    static List<int> EndpointsInDirection(Direction direction)
    {
        var idsInDirection = new List<int>();
        foreach (var (d, i) in endpointIdToDirection)
        {
            if (d == direction) idsInDirection.Add(i);
        }
        return idsInDirection;
    }

    public void Place(Direction direction)
    {
        facing = direction;
    }

    public void ConnectTo(Tile tile, Direction direction)
    {
        var endpointsToConnect = EndpointsInDirection(direction);

        foreach (var endpointId in endpointsToConnect)
        {
            var (_, oppositeEndpoint) = endpointIdToDirection[endpointId];
            paths[endpointId].ConnectTo(tile.paths[oppositeEndpoint]);
        }
    }

    public void MovePawns()
    {
        foreach (var path in paths)
        {
            path.MovePawns();
        }
    }
}

public class Path
{
    //TODO: make sure that alfa and beta consistently is the high or low id

    public Path? endpointAlfa { get; set; }
    public Path? endpointBeta { get; set; }

    public Pawn? edgeAlfa { get; set; }
    public Pawn? edgeBeta { get; set; }

    public void ConnectTo(Path path, bool firstConnection = true)
    {
        if (endpointAlfa == null)
        {
            endpointAlfa = path;
            if (firstConnection) path.ConnectTo(this, false);
        }
        else if (endpointBeta == null)
        {
            endpointBeta = path;
            if (firstConnection) path.ConnectTo(this, false);
        }
        else
        {
            throw new InvalidOperationException("There should only be 2 paths connected to any other path, but a 3rd has been tried to be connected");
        }
    }

    public void MovePawns()
    {
        if (edgeAlfa != null && endpointAlfa != null)
        {
            endpointAlfa.edgeAlfa = edgeAlfa;
            edgeAlfa = null;
            endpointAlfa.SwapEdges();
        }

        if (edgeBeta != null && endpointBeta != null)
        {
            endpointBeta.edgeBeta = edgeBeta;
            edgeBeta = null;
            endpointBeta.SwapEdges();
        }
    }

    public void SwapEdges()
    {
        if (edgeAlfa != null && edgeBeta != null)
        {
            edgeAlfa.Kill();
            edgeBeta.Kill();
        }
        else
        {
            var temp = edgeAlfa;
            edgeAlfa = edgeBeta;
            edgeBeta = temp;
        }
    }
}
