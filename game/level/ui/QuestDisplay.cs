using Godot;

namespace ExistenceDot.Level.UI
{
    public class QuestDisplay : Control
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        private Label _title, _description;
        private AnimationPlayer _animationPlayer;
        
        public override void _Ready()
        {
            _title = GetNode<Label>("VBoxContainer/Title");
            _description = GetNode<Label>("VBoxContainer/Description");
            _animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        }


        public void Show(Quest.Quest quest)
        {
            _title.Text = quest.Name;
            _description.Text = quest.Description;
            _animationPlayer.Play("DisplayQuest");
        }


        public void CloseDisplay()
        {
            QueueFree();
        }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    }
}
