namespace Extentions;

using Model;
using static Model.Direction;

static class Extentions
{
    public static Direction Opposite(this Direction direction)
    {
        switch (direction)
        {
            case UP:
                return DOWN;
            case RIGHT:
                return LEFT;
            case DOWN:
                return UP;
            case LEFT:
                return RIGHT;
            default:
                throw new KeyNotFoundException("The given Direction value did not match any cases");
        }
    }

    public static Direction Turn(this Direction original, Direction turn)
    {
        switch (original)
        {
            case UP:
                return turn;
            case RIGHT:
                switch (turn)
                {
                    case UP:
                        return DOWN;
                    case RIGHT:
                        return LEFT;
                    case DOWN:
                        return UP;
                    case LEFT:
                        return RIGHT;
                    default:
                        throw new KeyNotFoundException("The given Direction value did not match any cases");
                }
            case DOWN:
                return turn.Opposite();
            case LEFT:
                switch (turn)
                {
                    case UP:
                        return DOWN;
                    case RIGHT:
                        return LEFT;
                    case DOWN:
                        return UP;
                    case LEFT:
                        return RIGHT;
                    default:
                        throw new KeyNotFoundException("The given Direction value did not match any cases");
                }
            default:
                        throw new KeyNotFoundException("The given Direction value did not match any cases");
        }
    }

    
}