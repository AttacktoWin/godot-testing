using Godot;
using System;

public class CharacterInstance : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export(PropertyHint.Range, "0,9,1")]
    private int Id = 0;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        
    }


    [Signal]
    public delegate void ViewedDialogue(CharacterDialogue dialogue);

    [Signal]
    public delegate Dialogue GetDialogue(int characterId);
}
