using System;
using Godot;

namespace ExistenceDot.Loading
{
    public class LoadingScript : AnimatedPanel
    {
        private ResourceInteractiveLoader _loader;
        private Node _currentScene;
        private ProgressBar _progressBar;
        private const int TimeMax = 100;
        private int _waitFrames = 0;
        private Label _hintText;
        private static readonly string[] HintTexts = new[]
        {
            "This could be your advertisement",
            "I'm watching you",
            "hint.txt",
            "username:user,password:password123"
        };
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            base._Ready();
            var root = GetTree().Root;
            _currentScene = root.GetChild(root.GetChildCount() - 1);
            _progressBar = GetNode<ProgressBar>("ProgressBar");
            _hintText = GetNode<Label>("Hint");
        }

        public void LoadScene(string path, AnimatedPanel backPanel = null)
        {
            if (_loader != null)
                return;
            
            _loader = ResourceLoader.LoadInteractive(path);
            if (_loader == null)
            {
                ShowError();
                return;
            }
            _hintText.Text = HintTexts[new Random().Next(HintTexts.Length)];
            SetProcess(true);
            ShowPanel(backPanel);
            _waitFrames = 100;
        }

        public override void _Process(float delta)
        {
            if (_loader == null)
            {
                SetProcess(false);
                return;
            }
            if (_waitFrames > 0)
            {
                _waitFrames--;
                return;
            }
            if (_waitFrames == 0)
            {
                _waitFrames--;
                _currentScene.QueueFree();
                return;
            }
            var t = OS.GetTicksMsec();
            while (OS.GetTicksMsec() < t + TimeMax)
            {
                var err = _loader.Poll();
                if (err == Error.FileEof)
                {
                    var resource = _loader.GetResource();
                    _loader.Dispose();
                    _loader = null;
                    SetNewScene(resource as PackedScene);
                    break;
                }
                if (err == Error.Ok)
                {
                    UpdateProgress();
                }
                else
                {
                    ShowError();
                    _loader = null;
                    break;
                }
            }
        }

        private void UpdateProgress()
        {
            var progress = (double) _loader.GetStage() / _loader.GetStageCount();
            _progressBar.Value = Math.Round(progress * 100);
        }

        private void SetNewScene(PackedScene sceneResource)
        {
            _currentScene = sceneResource.Instance();
            HidePanel();
            GetNode("/root").AddChild(_currentScene);
        }

        private void ShowError()
        {
            HidePanel();
            _loader.Dispose();
            _loader = null;
            GD.Print("Error while loading the scene!");
        }

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
    }
}
