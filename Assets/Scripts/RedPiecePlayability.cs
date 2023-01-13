public class RedPiecePlayability : PiecePlayability
{
    public RedPiecePlayability(Piece piece, GameHandler gameHandler) : base(piece, gameHandler)
    {
               
    }
    public override void ChangeHasToTakePlayability()
    {
        var Coord = Piece.Coord;
        if (Board.Matrix[Coord.x + 1, Coord.y - 1].Piece != null &&
            Board.Matrix[Coord.x + 1, Coord.y - 1].Piece.Side != Piece.Side &&
            !Board.Matrix[Coord.x + 1, Coord.y - 1].IsWood)
        {
            if (Board.Matrix[Coord.x + 2, Coord.y - 2].Piece == null &&
                !Board.Matrix[Coord.x + 2, Coord.y - 2].IsWood)
            {
                GameHandler.ChangeTilePlayability(1,Coord.x + 2, Coord.y - 2);
            }
        }
        if (Board.Matrix[Coord.x - 1, Coord.y - 1].Piece != null &&
            Board.Matrix[Coord.x - 1, Coord.y - 1].Piece.Side != Piece.Side &&
            !Board.Matrix[Coord.x - 1, Coord.y - 1].IsWood)
        {
            if (Board.Matrix[Coord.x - 2, Coord.y - 2].Piece == null &&
                !Board.Matrix[Coord.x - 2, Coord.y - 2].IsWood)
            {
                GameHandler.ChangeTilePlayability(1,Coord.x - 2, Coord.y - 2);
            }
        }
    }

    public override bool GetCalculatedNormalPlayability()
    {
        var Coord = Piece.Coord;
        return (Board.Matrix[Coord.x + 1, Coord.y - 1].Piece == null &&
                           !Board.Matrix[Coord.x + 1, Coord.y - 1].IsWood) ||
                          (Board.Matrix[Coord.x - 1, Coord.y - 1].Piece == null && 
                           !Board.Matrix[Coord.x - 1, Coord.y - 1].IsWood);
    }

    public override void ChangeNormalPlayability()
    {
        var Coord = Piece.Coord;
        if (Board.Matrix[Coord.x + 1, Coord.y - 1].Piece == null &&
            !Board.Matrix[Coord.x + 1, Coord.y - 1].IsWood)
        {
            GameHandler.ChangeTilePlayability(1, Coord.x + 1, Coord.y - 1);
        }

        if (Board.Matrix[Coord.x - 1, Coord.y - 1].Piece == null &&
            !Board.Matrix[Coord.x - 1, Coord.y - 1].IsWood)
        {
            GameHandler.ChangeTilePlayability(1, Coord.x - 1, Coord.y - 1);
        }
    }

    public override void FindPiecesToEliminate()
    {
        var Coord = Piece.Coord;
        if (Board.Matrix[Coord.x + 1, Coord.y - 1].Piece != null &&
            Board.Matrix[Coord.x + 1, Coord.y - 1].Piece.Side != Piece.Side &&
            !Board.Matrix[Coord.x + 1, Coord.y - 1].IsWood)
        {
            if (Board.Matrix[Coord.x + 2, Coord.y - 2].Piece == null &&
                !Board.Matrix[Coord.x + 2, Coord.y - 2].IsWood)
            {
                Piece.PiecesToEliminate.Add(Board.Matrix[Coord.x + 1, Coord.y - 1].Piece);
            }
        }

        if (Board.Matrix[Coord.x - 1, Coord.y - 1].Piece != null &&
            Board.Matrix[Coord.x - 1, Coord.y - 1].Piece.Side != Piece.Side &&
            !Board.Matrix[Coord.x - 1, Coord.y - 1].IsWood)
        {
            if (Board.Matrix[Coord.x - 2, Coord.y - 2].Piece == null &&
                !Board.Matrix[Coord.x - 2, Coord.y - 2].IsWood)
            {
                Piece.PiecesToEliminate.Add(Board.Matrix[Coord.x - 1, Coord.y - 1].Piece);
            }
        }
    }

    public override void TryPromote()
    {
        if (Piece.Coord.y == 1)
        {
            Piece.Promote();
        }
    }
}