using Godot;
using System;

public class PauseMenuScript : Node
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
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
        GetTree().ChangeScene("res://main/menu.tscn");
    }

    private void QuitGame()
    {
        GetTree().Quit();
    }

    private void OpenOptions()
    {
        
    }

    private void ResumeGame()
    {
        GetTree().Paused = false;
    }
}
