using Godot;

public class Dialogue : Object
{
    public int Id { get; set; }
    public Line[] Lines { get; set; }
    public int Priority { get; set; }
}