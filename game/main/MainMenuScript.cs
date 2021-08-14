using Godot;

namespace ExistenceDot
{
	public class MainMenuScript : Node
	{
		// Declare member variables here. Examples:
		// private int a = 2;
		// private string b = "text";

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			GetNode<Button>("VBoxContainer/StartButton").Connect("pressed", this, nameof(StartGame));
			GetNode<Button>("VBoxContainer/OptionsButton").Connect("pressed", this, nameof(OpenOptions));
			GetNode<Button>("VBoxContainer/WebsiteButton").Connect("pressed", this, nameof(OpenWebsite));
			GetNode<Button>("VBoxContainer/CommunityButton").Connect("pressed", this, nameof(OpenCommunity));
			GetNode<Button>("VBoxContainer/CreditsButton").Connect("pressed", this, nameof(OpenCredits));
			GetNode<Button>("VBoxContainer/QuitButton").Connect("pressed", this, nameof(QuitGame));
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
			GetTree().ChangeScene("res://level/LevelScene.tscn");
		}

		public void OpenOptions()
		{
		}
		public void OpenCommunity()
		{
			
		}
		public void OpenCredits()
		{
		}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	}
}