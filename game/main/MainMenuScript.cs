using ExistenceDot.Loading;
using Godot;

namespace ExistenceDot
{
    public class MainMenuScript : AnimatedPanel
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        private OptionsScript _options;
        private AnimatedPanel _credits;

        public override void _Ready()
        {
            base._Ready();
            GetNode<Button>("VBoxContainer/StartButton").Connect("pressed", this, nameof(StartGame));
            GetNode<Button>("VBoxContainer/OptionsButton").Connect("pressed", this, nameof(OpenOptions));
            GetNode<Button>("VBoxContainer/WebsiteButton").Connect("pressed", this, nameof(OpenWebsite));
            GetNode<Button>("VBoxContainer/CommunityButton").Connect("pressed", this, nameof(OpenCommunity));
            GetNode<Button>("VBoxContainer/CreditsButton").Connect("pressed", this, nameof(OpenCredits));
            GetNode<Button>("VBoxContainer/QuitButton").Connect("pressed", this, nameof(QuitGame));
            _options = GetNode<OptionsScript>("../Options");
            _credits = GetNode<AnimatedPanel>("../Credits");
        }

        public void ConfirmQuit()
        {
            (GetNode("ConfirmQuit") as ConfirmationDialog)?.Show();
        }

        public void CancelQuit()
        {
            (GetNode("ConfirmQuit") as ConfirmationDialog)?.Hide();
        }

        public void QuitGame()
        {
            GetTree().Quit();
        }

        public void OpenWebsite()
        {
            OS.ShellOpen("https://existence.linwood.dev");
        }

        public void StartGame()
        {
            var loading = GetNode<LoadingScript>("/root/LoadingScreen/Loading");
            loading.LoadScene("res://level/LevelScene.tscn", this);
        }

        public void OpenOptions()
        {
            _options.ShowPanel(this);
        }

        public void OpenCommunity()
        {
        }

        public void OpenCredits()
        {
            _credits.ShowPanel(this);
        }
//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    }
}