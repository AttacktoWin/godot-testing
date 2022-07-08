using Godot;
using System.Collections.Generic;

public class DialogueQueue : Object
{
    private List<Dialogue> dialogueHeap { get; set; }
    public int Size { get; set; }

    public DialogueQueue()
    {
        dialogueHeap = new List<Dialogue>();
        Size = 0;
    }

    public DialogueQueue(IEnumerable<Dialogue> items)
    {
        dialogueHeap = new List<Dialogue>(items);
    }

    private void Swap(int lhs, int rhs)
    {
        Dialogue temp = dialogueHeap[lhs];
        dialogueHeap[lhs] = dialogueHeap[rhs];
        dialogueHeap[rhs] = temp;
    }

    private int Parent(int key)
    {
        return (key - 1) / 2;
    }

    private int Left(int key)
    {
        return 2 * key + 1;
    }

    private int Right(int key)
    {
        return 2 * key + 2;
    }

    private void decreaseKey(int key, int new_priority)
    {
        dialogueHeap[key].Priority = new_priority;

        while (key != 0 && dialogueHeap[key].Priority < dialogueHeap[Parent(key)].Priority)
        {
            Swap(key, Parent(key));
            key = Parent(key);
        }
    }

    private void MinHeapify(int key)
    {
        int l = Left(key);
        int r = Right(key);

        int smallest = key;
        if (l < Size && dialogueHeap[l].Priority < dialogueHeap[smallest].Priority)
        {
            smallest = l;
        }
        if (r < Size && dialogueHeap[r].Priority < dialogueHeap[smallest].Priority)
        {
            smallest = r;
        }

        if (smallest != key)
        {
            Swap(key, smallest);
            MinHeapify(smallest);
        }
    }

    public void Enqueue(Dialogue dialogue)
    {
        int i = Size;
        dialogueHeap.Add(dialogue);
        Size++;

        while (i != 0 && dialogueHeap[i].Priority < dialogueHeap[Parent(i)].Priority)
        {
            Swap(i, Parent(i));
            i = Parent(i);
        }
    }

    public Dialogue Peek()
    {
        return dialogueHeap[0];
    }

    public Dialogue Dequeue()
    {
        if (Size <= 0)
        {
            return null;
        }

        if (Size == 1)
        {
            Dialogue min = dialogueHeap[0];
            dialogueHeap.Clear();
            Size = 0;
            return min;
        }

        Dialogue root = dialogueHeap[0];

        dialogueHeap[0] = dialogueHeap[Size - 1];
        Size--;
        MinHeapify(0);

        return root;
    }

    public void Remove(string itemId)
    {
        var key = dialogueHeap.FindIndex(e => e.Id == itemId);
        if (key >= 0)
        {
            decreaseKey(key, int.MinValue);
            Dequeue();
        }
    }

    public string[] GetContents()
    {
        string[] contents = new string[Size];
        int i = 0;
        foreach(Dialogue dialogue in dialogueHeap)
        {
            if (dialogue != null)
            {
                contents[i] = dialogue.Id;
                i++;
            }
        }
        return contents;
    }
}