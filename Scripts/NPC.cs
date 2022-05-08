using Godot;
using System;
using System.Collections.Generic;

public class NPC : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";
    [Export]
    public string Id = "";
    private List<Dialogue> dialogueList = new List<Dialogue>();
    private DialogueQueue dialogueQueue = new DialogueQueue();
    [Export(PropertyHint.File, "*.json")]
    private string dialogueFile = "";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Get dialogues from file
        if (dialogueFile.IsAbsPath())
        {

        }
    }

    public void UnlockDialogue(int dialogueId)
    {
        var unlocked = dialogueList.Find(d => d.Id == dialogueId);
        if (unlocked != null)
        {
            dialogueQueue.Enqueue(unlocked);
        }
    }

    public void RemoveDialogue(int dialogueId)
    {
        dialogueQueue.Remove(dialogueId);
        
    }

    public Dialogue GetTopDialogue()
    {
        return dialogueQueue.Dequeue();
    }

    public int[] SaveQueue() 
    {
        return dialogueQueue.GetContents();
    }

    public void LoadFromQueue(int[] ids)
    {
        foreach (var id in ids)
        {
            var dialogue = dialogueList.Find(d => d.Id == id);
            if (dialogue != null)
            {
                dialogueQueue.Enqueue(dialogue);
            }
        }
    }
}
