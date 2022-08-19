using Model;

class StateContainor
{
    Tsuro? tsuro;

    public Tsuro property
    {
        get => tsuro ?? throw new ArgumentNullException("The saved value of the game has not jet been set");
        set 
        {
            tsuro = value;
        }
    }
}