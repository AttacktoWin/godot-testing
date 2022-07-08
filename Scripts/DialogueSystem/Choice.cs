public class Choice : DialoguePiece
{
    public string ChoiceId { get; set; }
    public string[] Options { get; set; }
    public override string PieceType { get; } = "Choice";
}