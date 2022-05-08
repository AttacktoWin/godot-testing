using Godot;

public class DialogueViewedResults : Object
{
    public string NPCId { get; set; }
    public NPCDialogue[] UnlockedDialogue { get; set; }
    public NPCDialogue[] RemovedDialogue { get; set; }
}