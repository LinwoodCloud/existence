using Godot;

namespace ExistenceDot.Quest
{
    public class Quest : Resource
    {
        // Make sure that every parameter has a default value.
        // Otherwise, there will be problems with creating and editing
        // your resource via the inspector.

        [Export] public string Name { get; set; } = "";

        [Export] public string Description { get; set; } = "";

        [Export] public Quest[] NextQuests { get; set; } = new Quest[0];

        [Export] public string Scene { get; set; } = "";
        [Export] public bool MainQuest { get; set; } = true;
        [Export] public Quest ParentQuest { get; set; }
    }
}