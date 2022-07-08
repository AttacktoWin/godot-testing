using Godot;

public class Line : DialoguePiece
{
    public string PortraitName { get; set; }
    public string SoundEffectName { get; set; }
    public override string PieceType { get; } = "Line";
}