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
			OS.ShellOpen("https://cosmocity.linwood.dev");
		}

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
	}
}
