using ExistenceDot;
using ExistenceDot.Loading;
using Godot;

public class PauseMenuScript : AnimatedPanel
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        base._Ready();
        GetNode<Button>("VBoxContainer/ResumeButton").Connect("pressed", this, nameof(ResumeGame));
        GetNode<Button>("VBoxContainer/OptionsButton").Connect("pressed", this, nameof(OpenOptions));
        GetNode<Button>("VBoxContainer/BackButton").Connect("pressed", this, nameof(BackToMainMenu));
        GetNode<Button>("VBoxContainer/QuitButton").Connect("pressed", this, nameof(QuitGame));
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }

    private void BackToMainMenu()
    {
        ResumeGame();
        var loading = GetNode<LoadingScript>("/root/LoadingScreen/Loading");
        loading.LoadScene("res://main/menu.tscn", this);
    }

    private void QuitGame()
    {
        GetTree().Quit();
    }

    private void OpenOptions()
    {
        var options = ResourceLoader.Load<PackedScene>("res://main/options.tscn").Instance<OptionsScript>();
        GetParent().AddChild(options);
        options.ShowPanel(this);
    }

    public void ResumeGame()
    {
        GetTree().Paused = false;
        HidePanel();
    }

    public void PauseGame()
    {
        GetTree().Paused = true;
        ShowPanel();
    }

    public void TogglePause()
    {
        if(GetTree().Paused)
            ResumeGame();
        else
            PauseGame();
    }
}