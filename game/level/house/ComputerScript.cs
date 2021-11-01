using Godot;

namespace ExistenceDot.Level
{
    public class ComputerScript : Node
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";
        private AnimatedPanel _computerScreen;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            var computerArea = GetNode<Area>("Area");
            computerArea.Connect("body_entered", this, nameof(EnteredComputerArea));
            computerArea.Connect("body_exited", this, nameof(ExitedComputerArea));
        }

        public void EnteredComputerArea(Node otherNode)
        {
            if (!(otherNode is PlayerScript) || _computerScreen != null)
                return;
            _computerScreen = ResourceLoader.Load<PackedScene>("res://level/house/quest_selector.tscn").Instance<AnimatedPanel>();
            AddChild(_computerScreen);
            _computerScreen.ShowPanel();
        }

        public void ExitedComputerArea(Node otherNode)
        {
            _computerScreen?.HidePanel();
            _computerScreen = null;
        }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    }
}