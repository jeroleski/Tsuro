namespace Main;

using Model;
using System.IO;
using System.Text.RegularExpressions;

public class ModelBuilder
{
    public Tsuro LoadGame(string path, int nPlayers)
    {
        //The file containing discriptions for all tiles in a game
        var text = File.ReadAllText(path);



        //Extract the meta data
        var metaRegex = new Regex(@"width\s*=\s*(?<width>\d+),\s*length\s*=\s*(?<length>\d+),\s*tiles\s*=\s*(?<tiles>\d+),\s*endpoints\s*=\s*(?<endpoints>\d+)\s*;");

        var metaMatches = metaRegex.Matches(text);
        if (metaMatches.Count() != 1) throw new ArgumentException("There must be (only) 1 set of meta data");
        var metaGroup = metaMatches[0].Groups;

        int width, length, tiles, endpoints;
        try
        {
            width = Int32.Parse(metaGroup["width"].Value);
            length = Int32.Parse(metaGroup["length"].Value);
            tiles = Int32.Parse(metaGroup["tiles"].Value);
            endpoints = Int32.Parse(metaGroup["endpoints"].Value);
        }
        catch (FormatException)
        {
            throw new ArgumentException("The meta data could not be converted");
        }
        if (endpoints % 2 != 0) throw new ArgumentException("There must be an equal number of endpoints for the tiles");
        var tilePaths = endpoints / 2;


        //Extract the directions used and what indexes have them
        var directionMapRegex = new Regex(@"(?<index>\d+)\s*=\s*\(\s*(?<direction>\w+)\s*,\s*(?<opposite>\d+)\s*\)");

        var directionMapMatches = directionMapRegex.Matches(text);
        if (directionMapMatches.Count() != endpoints) throw new ArgumentException("The matched number of directions (endpoints) is not equal to the amount declared in the meta data");

        var directionMap = new (Direction, int)[endpoints];
        for (var i = 0; i < directionMapMatches.Count; i++)
        {
            var directionMapGroup = directionMapMatches[i].Groups;

            Direction direction;
            int opposite;
            try
            {
                direction = Enum.Parse<Direction>(directionMapGroup["direction"].Value);
                opposite = Int32.Parse(directionMapGroup["opposite"].Value);
            }
            catch (ArgumentException)
            {
                throw new ArgumentException("The direction mapping data could not be converted (Enum)");
            }
            catch (FormatException)
            {
                throw new ArgumentException("The direction mapping data could not be converted (Int)");
            }

            directionMap[i] = (direction, opposite);
        }



        //Extract each individual tile and its paths
        var tileRegex = new Regex(@"(?<tile>(\d+\s+\d+(\s*,\s*)?)+)\s*;");
        var pathRegex = new Regex(@"(?<alfa>\d+)\s+(?<beta>\d+)");

        var tileMatches = tileRegex.Matches(text);
        if (tileMatches.Count() != tiles) throw new ArgumentException("The matched number of tiles is not equal to the amount declared in the meta data");

        var allTilesConnections = new (int, int)[tiles][];
        for (var i = 0; i < tileMatches.Count; i++)
        {
            var tileGroup = tileMatches[i].Groups;
            var tileText = tileGroup["tile"].Value;

            var pathMatches = pathRegex.Matches(tileText);
            if (pathMatches.Count() != tilePaths) throw new ArgumentException("The matched number of paths (half of endpoints) is not equal to the amount declared in the meta data");

            allTilesConnections[i] = new (int, int)[tilePaths];
            for (var j = 0; j < pathMatches.Count; j++)
            {
                var pathGroup = pathMatches[j].Groups;
                int alfa, beta;
                try
                {
                    alfa = Int32.Parse(pathGroup["alfa"].Value);
                    beta = Int32.Parse(pathGroup["beta"].Value);
                }
                catch (FormatException)
                {
                    throw new ArgumentException("The tile path data could not be converted");
                }

                allTilesConnections[i][j] = (alfa, beta);
            }
        }

        return new Tsuro(width, length, directionMap, allTilesConnections, nPlayers);
    }
}
