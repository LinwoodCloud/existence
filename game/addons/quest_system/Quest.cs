using Godot;

namespace ExistenceDot.Quest
{
    [Tool]
    public class Quest : Resource
    {
        // Make sure that every parameter has a default value.
        // Otherwise, there will be problems with creating and editing
        // your resource via the inspector.

        [Export] public string Name { get; set; } = "";

        [Export] public string Description { get; set; } = "";

        [Export] public string Scene { get; set; } = "";
        [Export] public bool MainQuest { get; set; } = true;

        [Export] public string ParentQuest { get; set; } = "";
    }
}