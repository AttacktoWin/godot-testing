using Godot;
using JsonSubTypes;
using Newtonsoft.Json;

[JsonConverter(typeof(JsonSubtypes), "PieceType")]
[JsonSubtypes.KnownSubType(typeof(Line), "Line")]
[JsonSubtypes.KnownSubType(typeof(Choice), "Choice")]
public class DialoguePiece : Object
{
    public string DisplayName { get; set; }
    public string Contents { get; set; }
    public virtual string PieceType { get; }
}