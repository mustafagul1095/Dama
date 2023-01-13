using UnityEngine;

public class TurnHandler
{
    private Side _turn = Side.Black;
    public Side Turn => _turn;

    public void PassTurn()
    {
        
        if (_turn == Side.Red)
        {
            _turn = Side.Black;
        }
        else 
        {
            _turn = Side.Red;
        }
    }
}