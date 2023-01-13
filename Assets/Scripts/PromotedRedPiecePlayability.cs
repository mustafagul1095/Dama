public class PromotedRedPiecePlayability : PiecePlayability
{
    public PromotedRedPiecePlayability(Piece piece, GameHandler gameHandler) : base(piece, gameHandler)
    {
    }

    public override void ChangeHasToTakePlayability()
    {
        throw new System.NotImplementedException();
    }

    public override bool GetCalculatedNormalPlayability()
    {
        throw new System.NotImplementedException();
    }

    public override void ChangeNormalPlayability()
    {
        throw new System.NotImplementedException();
    }

    public override void FindPiecesToEliminate()
    {
        throw new System.NotImplementedException();
    }
}