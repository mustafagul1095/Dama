public class PromotedPiecePlayability : PiecePlayability
{
    private RedPiecePlayability _redPiecePlayability;
    private BlackPiecePlayability _blackPiecePlayability;
    
    public PromotedPiecePlayability(Piece piece, GameHandler gameHandler) : base(piece, gameHandler)
    {
        _redPiecePlayability = new RedPiecePlayability(piece, gameHandler);
        _blackPiecePlayability = new BlackPiecePlayability(piece, gameHandler);
    }

    public override void ChangeHasToTakePlayability()
    {
        _redPiecePlayability.ChangeHasToTakePlayability();
        _blackPiecePlayability.ChangeHasToTakePlayability();
    }

    public override bool GetCalculatedNormalPlayability()
    {
        return _redPiecePlayability.GetCalculatedNormalPlayability() ||
               _blackPiecePlayability.GetCalculatedNormalPlayability();
    }

    public override void ChangeNormalPlayability()
    {
        _redPiecePlayability.ChangeNormalPlayability();
        _blackPiecePlayability.ChangeNormalPlayability();
    }

    public override void FindPiecesToEliminate()
    {
        _redPiecePlayability.FindPiecesToEliminate();
        _blackPiecePlayability.FindPiecesToEliminate();
    }

    public override void TryPromote()
    {
    }
}