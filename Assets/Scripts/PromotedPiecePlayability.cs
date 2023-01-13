public class PromotedPiecePlayability : PiecePlayability
{
    private RedPiecePlayability _redPiecePlayability;
    private WhitePiecePlayability _whitePiecePlayability;
    
    public PromotedPiecePlayability(Piece piece, GameHandler gameHandler) : base(piece, gameHandler)
    {
        _redPiecePlayability = new RedPiecePlayability(piece, gameHandler);
        _whitePiecePlayability = new WhitePiecePlayability(piece, gameHandler);
    }

    public override void ChangeHasToTakePlayability()
    {
        _redPiecePlayability.ChangeHasToTakePlayability();
        _whitePiecePlayability.ChangeHasToTakePlayability();
    }

    public override bool GetCalculatedNormalPlayability()
    {
        return _redPiecePlayability.GetCalculatedNormalPlayability() ||
               _whitePiecePlayability.GetCalculatedNormalPlayability();
    }

    public override void ChangeNormalPlayability()
    {
        _redPiecePlayability.ChangeNormalPlayability();
        _whitePiecePlayability.ChangeNormalPlayability();
    }

    public override void FindPiecesToEliminate()
    {
        _redPiecePlayability.FindPiecesToEliminate();
        _whitePiecePlayability.FindPiecesToEliminate();
    }

    public override void TryPromote()
    {
    }
}