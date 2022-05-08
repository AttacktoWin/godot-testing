using Godot;
using System;

public class Dialogue
{
    public int Id { get; set; }
    public Line[] Lines { get; set; }
    public int Priority { get; set; }
}