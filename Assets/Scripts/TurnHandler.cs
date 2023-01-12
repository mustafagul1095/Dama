using UnityEngine;

public class TurnHandler
{
    private Turn _turn = Turn.White;
    public Turn Turn => _turn;

    public void PassTurn()
    {
        
        if (_turn == Turn.Red)
        {
            _turn = Turn.White;
        }
        else 
        {
            _turn = Turn.Red;
        }
    }
}