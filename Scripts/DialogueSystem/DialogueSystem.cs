using Godot;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

public class DialogueSystem : Node2D
{
    [Export(PropertyHint.File, "*.json")]
    private string DialogueUnlockTablePath = "";
    [Export]
    private string SaveFileName = "dialogueSystemSave.save";

    private List<NPC> NPCs = new List<NPC>();
    private Dictionary<string, DialogueNPCIds[]> UnlockTable;

    public override void _Ready()
    {
        foreach (var child in this.GetChildren())
        {
            if (child is NPC)
            {
                NPCs.Add(child as NPC);
            }
        }
        var fileManager = new File();
        if (fileManager.FileExists($"user://{SaveFileName}"))
        {
            fileManager.Open($"user://{SaveFileName}", File.ModeFlags.Read);
            var saveText = fileManager.GetAsText();
            fileManager.Close();
            Dictionary<string, string[]> saveDict = JsonConvert.DeserializeObject<Dictionary<string, string[]>>(saveText);
            foreach (var npc in NPCs)
            {
                if (saveDict.ContainsKey(npc.Id))
                {
                    npc.Init(saveDict[npc.Id]);
                }
                else
                {
                    npc.Init();
                }
            }
        }
        else
        {
            foreach (var npc in NPCs)
            {
                npc.Init();
            }
        }

        if (fileManager.FileExists(DialogueUnlockTablePath))
        {
            fileManager.Open(DialogueUnlockTablePath, File.ModeFlags.Read);
            var text = fileManager.GetAsText();
            fileManager.Close();

            UnlockTable = JsonConvert.DeserializeObject<Dictionary<string, DialogueNPCIds[]>>(text);
        }
        else
        {
            UnlockTable = new Dictionary<string, DialogueNPCIds[]>();
        }
    }

    public Dialogue GetTopDialogue(string NPCId)
    {
        NPC npc = NPCs.Find(n => n.Id == NPCId);

        if (npc == null)
        {
            throw new Exception($"No NPC with id \"{NPCId}\" exists");
        }

        return npc.GetTopDialogue();
    }

    private void UnlockDialogues(DialogueNPCIds[] unlockedDialogue)
    {
        foreach (var ids in unlockedDialogue)
        {
            var npc = NPCs.Find(n => n.Id == ids.NPCId);
            if (npc != null)
            {
                npc.UnlockDialogue(ids.DialogueId);
            }
        }
    }

    public void DialogueViewed(string NPCId, string DialogueId)
    {
        string key = $"{NPCId}-{DialogueId}";
        if (!UnlockTable.ContainsKey(key))
        {
            return;
        }
        var unlockedDialouge = UnlockTable[key];
        
        UnlockDialogues(unlockedDialouge);
        // TODO: Add functionality with unlocking stuff in the world
    }

    public void ChoiceSelected(string ChoiceId, int OptionSelected)
    {
        string key = $"choice-{ChoiceId}-{OptionSelected}";
        if (!UnlockTable.ContainsKey(key))
        {
            return;
        }
        var unlockedDialouge = UnlockTable[key];

        UnlockDialogues(unlockedDialouge);
        // TODO: Add functionality with unlocking stuff in the world
    }

    public void EventViewed(string EventTag)
    {
        string key = $"world-{EventTag}";
        if (!UnlockTable.ContainsKey(key))
        {
            return;
        }
        var unlockedDialogue = UnlockTable[key];
        UnlockDialogues(unlockedDialogue);
    }

    public void DisplayDialogue(Dialogue dialogue)
    {
        // TODO: Implement a textbox
        GD.Print("Dialogue: {");
        GD.Print($"    Id: {dialogue.Id},");
        GD.Print($"    Pieces: [");
        foreach(var piece in dialogue.Pieces)
        {
            GD.Print("    {");
            GD.Print($"        DisplayName: {piece.DisplayName},");
            GD.Print($"        Contents: {piece.Contents}");
            if (piece is Line)
            {
                GD.Print($"        PortraitName: {(piece as Line).PortraitName}");
                GD.Print($"        SoundEffectName: {(piece as Line).SoundEffectName}");
            }
            else if (piece is Choice)
            {
                GD.Print($"        ChoiceId: {(piece as Choice).ChoiceId}");
                GD.Print($"        Options: [{String.Join(", ", (piece as Choice).Options)}]");
            }
            GD.Print("    }");
        }
        GD.Print("]");
        GD.Print($"Priority: {dialogue.Priority}");
    }

    public void Save()
    {
        var saveText = "{";
        string[] npcSaves = new string[NPCs.Count];
        for(var i = 0; i < NPCs.Count; i++)
        {
            var ids = NPCs[i].Save();
            npcSaves[i] = $"\"{NPCs[i].Id}\": [\"{String.Join("\",", ids)}\"]";
        }
        saveText += String.Join(",", npcSaves);
        saveText += "}";

        var saveFile = new File();
        saveFile.Open($"user://{SaveFileName}", File.ModeFlags.Write);
        saveFile.StoreLine(saveText);
        saveFile.Close();
    }
}
