using Godot;

public class NPCInstance : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    private string Id = "";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
       EmitSignal(nameof(GetDialogue), Id);
    }


    [Signal]
    public delegate void ViewedDialogue(NPCDialogue dialogue);

    [Signal]
    public delegate void GetDialogue(string npcId);
}
