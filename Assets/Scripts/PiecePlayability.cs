using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PiecePlayability
{
    protected readonly Piece Piece;
    protected readonly Board Board;
    protected readonly GameHandler GameHandler;
    
    public PiecePlayability(Piece piece, GameHandler gameHandler)
    {
        Piece = piece;
        Board = gameHandler.Board;
        GameHandler = gameHandler;
    }
    

    public abstract void ChangeHasToTakePlayability();
    public abstract bool GetCalculatedNormalPlayability();
    public abstract void ChangeNormalPlayability();
    public abstract void FindPiecesToEliminate();

}