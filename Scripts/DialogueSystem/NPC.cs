using Godot;
using System;
using System.Collections.Generic;
using Newtonsoft.Json;

public class NPC : Node2D
{
    [Export]
    public string Id = "";
    [Export(PropertyHint.File, "*.json")]
    private string DialogueFilePath = "";

    private Dictionary<string, Dialogue> DialogueAvailable;
    private DialogueQueue DialogueQueue;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        DialogueQueue = new DialogueQueue();
        // Get dialogues from file
        var dialogueFile = new File();
        if (dialogueFile.FileExists(DialogueFilePath))
        {
            dialogueFile.Open(DialogueFilePath, File.ModeFlags.Read);
            var text = dialogueFile.GetAsText();
            dialogueFile.Close();
            if (!text.Empty())
            {
                DialogueAvailable = JsonConvert.DeserializeObject<Dictionary<string, Dialogue>>(text, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.All });
            }
            else
            {
                DialogueAvailable = new Dictionary<string, Dialogue>();
            }
        } 
    }

    public void Init()
    {
        if (!DialogueAvailable.ContainsKey("default"))
        {
            throw new Exception($"No default dialogue provided for NPC \"{Id}\"");
        }

        DialogueQueue.Enqueue(DialogueAvailable["default"]);
    }

    public void Init(string[] queuedIds)
    {
        foreach (var id in queuedIds)
        {
            if (DialogueAvailable.ContainsKey(id))
            {
                DialogueQueue.Enqueue(DialogueAvailable[id]);
            }
        }
    }

    public string[] Save()
    {
        return DialogueQueue.GetContents(); 
    }

    public Dialogue GetTopDialogue()
    {
        return DialogueQueue.Dequeue();
    }

    public void UnlockDialogue(string DialogueId)
    {
        if (!DialogueAvailable.ContainsKey(DialogueId))
        {
            throw new Exception($"Unknown Dialogue Id \"{DialogueId}\" on NPC \"{this.Id}\"");
        }

        this.DialogueQueue.Enqueue(DialogueAvailable[DialogueId]);
    }
}
