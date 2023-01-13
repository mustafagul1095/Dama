using UnityEngine;

public class TurnHandler
{
    private Side _turn = Side.White;
    public Side Turn => _turn;

    public void PassTurn()
    {
        
        if (_turn == Side.Red)
        {
            _turn = Side.White;
        }
        else 
        {
            _turn = Side.Red;
        }
    }
}