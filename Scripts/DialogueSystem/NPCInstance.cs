using Godot;

public class NPCInstance : Interactible
{
    [Export]
    private string NPCId = "";

    private bool Interacted = false;

    public override void _Ready()
    {
        // Just for testing
        Interact();
        GD.Print("Interacting again...");
        Interacted = false;
        Interact();
        GD.Print("Interacting again...");
        Interacted = false;
        Interact();
    }

    public override void Interact()
    {
        if (Interacted)
        {
            return;
        }

        DialogueSystem system = GetNode<DialogueSystem>("/root/DialogueSystem");
        if (system == null)
        {
            throw new System.Exception("No instance of Dialogue System found");
        }

        Dialogue topDialogue = system.GetTopDialogue(NPCId);
        system.DisplayDialogue(topDialogue);
        system.DialogueViewed(NPCId, topDialogue.Id);
    }
}
