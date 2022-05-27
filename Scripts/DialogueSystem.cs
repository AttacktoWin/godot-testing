using Godot;
using System;
using System.Collections.Generic;

public class DialogueSystem : Node2D
{
    private Dictionary<string, Dictionary<int, DialogueViewedResults>> unlockTable = new Dictionary<string, Dictionary<int, DialogueViewedResults>>();
    private List<NPC> npcs = new List<NPC>();

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        foreach (Node child in this.GetChildren())
        {
            if (child is NPC)
            {
                npcs.Add(child as NPC);
            }
        }

        // this.Connect(nameof(NPCInstance.ViewedDialogue), this, nameof(DialogueViewed));
        // this.Connect(nameof(NPCInstance.GetDialogue), this, nameof(GetTopDialogue));
    }

    private void DialogueViewed(NPCDialogue dialogue)
    {
        var results = unlockTable[dialogue.NPCId][dialogue.DialogueId];
        foreach (NPCDialogue unlockedDialogue in results.UnlockedDialogue)
        {
            var npc = npcs.Find(c => c.Id == unlockedDialogue.NPCId);
            if (npc != null)
            {
                npc.UnlockDialogue(unlockedDialogue.DialogueId);
            }
        }
        foreach (NPCDialogue removedDialogue in results.RemovedDialogue)
        {
            var npc = npcs.Find(c => c.Id == removedDialogue.NPCId);
            if (npc != null)
            {
                npc.RemoveDialogue(removedDialogue.DialogueId);
            }
        }
    }

    private void GetTopDialogue(string npcId)
    {
        var npc = npcs.Find(c => c.Id == npcId);
        if (npc != null)
        {
            GD.Print(npc.GetTopDialogue());
        }
        GD.Print("Nothing found.");
    }

    private void SaveState()
    {
        // Save entire system state to json
        var npcStringList = new List<string>();
        foreach (var npc in npcs)
        {
            string ids = String.Join(",", npc.SaveQueue());
            npcStringList.Add($"\"{npc.Id}\": [{ids}]");
        }
        var jsonString = '{' + String.Join(",", npcStringList) + '}';
        // TODO: Write this to a file
    }
}
