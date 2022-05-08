using Godot;
using System;
using System.Collections.Generic;

public class DialogueSystem : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    private Dictionary<int, Dictionary<int, CharacterDialogue[]>> unlockTable = new Dictionary<int, Dictionary<int, CharacterDialogue[]>>();
    private List<Character> characters = new List<Character>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach (Node child in this.GetChildren())
        {
            if (child is Character)
            {
                characters.Add(child as Character);
            }
        }

        this.Connect(nameof(CharacterInstance.ViewedDialogue), this, nameof(DialogueViewed));
        this.Connect(nameof(CharacterInstance.GetDialogue), this, nameof(GetTopDialogue));
    }

    public void DialogueViewed(CharacterDialogue dialogue)
    {
        var unlockedDialogues = unlockTable[dialogue.CharacterId][dialogue.DialogueId];
        foreach (CharacterDialogue unlockedDialogue in unlockedDialogues)
        {
            EmitSignal(nameof(unlockDialogue), unlockedDialogue);
        }
    }

    public Dialogue GetTopDialogue(int characterId)
    {
        var character = characters.Find(c => c.Id == characterId);
        if (character is not null)
        {
            return character.GetTopDialogue();
        }
        return null;
    }

    [Signal]
    public delegate void unlockDialogue(CharacterDialogue dialogue);
}
