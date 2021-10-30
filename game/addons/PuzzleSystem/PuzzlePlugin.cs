using Godot;

#if TOOLS
namespace ExistenceDot.PuzzleSystem
{
    [Tool]
    public class PuzzlePlugin : EditorPlugin
    {
        public override void _EnterTree()
        {
            AddCustomType("Lever", "Spatial", GD.Load<Script>("res://addons/PuzzleSystem/Lever.cs"), null);
        }

        public override void _ExitTree()
        {
            // Clean-up of the plugin goes here.
            // Always remember to remove it from the engine when deactivated.
            RemoveCustomType("Lever");
        }
    }
}
#endif