using Godot;
using System;
using System.Collections.Generic;

public class Character : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export(PropertyHint.Range, "0,99,1")]
    public int Id = 0;
    private List<Dialogue> dialogueList = new List<Dialogue>();
    private PriorityQueue<Dialogue, int> dialogueQueue = new PriorityQueue<Dialogue, int>();
    [Export(PropertyHint.File, "*.json")]
    private string dialogueFile;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Get dialogues from file
        if (dialogueFile.IsAbsPath())
        {

        }

        this.Connect(nameof(DialogueSystem.unlockDialogue), this, nameof(unlockDialogue));
    }

    private void unlockDialogue(CharacterDialogue dialogue)
    {
        if (dialogue.CharacterId == this.Id)
        {
            var unlocked = dialogueList.Find(d => d.Id == dialogue.DialogueId);
            if (unlocked is not null)
            {
                dialogueQueue.Enqueue(unlocked, unlocked.Priority);
            }
        }
    }

    public Dialogue GetTopDialogue()
    {
        return dialogueQueue.Dequeue();
    }
}
