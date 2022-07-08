using Godot;

public class Dialogue : Object
{
    public string Id { get; set; }
    public DialoguePiece[] Pieces { get; set; }
    public int Priority { get; set; }
}