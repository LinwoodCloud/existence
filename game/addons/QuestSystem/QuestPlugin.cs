using Godot;

#if TOOLS
namespace ExistenceDot.QuestSystem
{
    [Tool]
    public class QuestSystem : EditorPlugin
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        public override void _EnterTree()
        {
            AddCustomType("Quest", "Resource", GD.Load<Script>("res://addons/QuestSystem/Quest.cs"), null);
            AddCustomType("QuestInteract", "Node", GD.Load<Script>("res://addons/QuestSystem/QuestInteract.cs"), null);
        }

        public override void _ExitTree()
        {
            // Clean-up of the plugin goes here.
            // Always remember to remove it from the engine when deactivated.
            RemoveCustomType("Quest");
            RemoveCustomType("QuestInteract");
        }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    }
}
#endif