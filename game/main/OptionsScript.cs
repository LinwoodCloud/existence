using System;
using System.Collections.Generic;
using Godot;

namespace ExistenceDot
{
    public class OptionsScript : AnimatedPanel
    {
        private TextEdit _customResolutionEdit;
        private Label _fpsLabel;

        private Slider _fpsSlider;
        private OptionButton _resolutionOption;

        private Button _revertButton, _applyButton;

        private readonly Vector2[] _defaultResolutions = new[]
        {
            new Vector2(640, 360),
            new Vector2(800, 600),
            new Vector2(1024, 768),
            new Vector2(1280, 720),
            new Vector2(1280, 800),
            new Vector2(1280, 1024),
            new Vector2(1360, 768),
            new Vector2(1440, 900),
            new Vector2(1536, 864),
            new Vector2(1600, 900),
            new Vector2(1680, 1050),
            new Vector2(1920, 1080),
            new Vector2(1920, 1152),
            new Vector2(1920, 1200),
            new Vector2(2048, 1152),
            new Vector2(2560, 1080),
            new Vector2(2560, 1440),
            new Vector2(3440, 1440),
            new Vector2(3840, 2160)
        };

        private GameOptions _options;

        public struct GameOptions
        {
            public Vector2? Resolution { get; set; }
            public string Locale { get; set; }

            public void Save()
            {
                var saveGame = new File();
                saveGame.Open("user://options.save", File.ModeFlags.Write);
                saveGame.StoreString(JSON.Print(new Godot.Collections.Dictionary<string, object>
                {
                    {"resolution", Resolution == null ? null : new Godot.Collections.Dictionary(){
                        {"x", Resolution?.x},
                        {"y", Resolution?.y}
                    }},
                    {"locale", Locale}
                }));
            }

            public static GameOptions Load()
            {
                var saveOptions = new File();
                if (!saveOptions.FileExists("user://options.save"))
                    return new GameOptions();
                saveOptions.Open("user://options.save", File.ModeFlags.Read);
                var text = saveOptions.GetAsText();
                if (text.Empty())
                    return new GameOptions();
                var dict = JSON.Parse(text);
                if (dict.Error != Error.Ok || dict.Result != null)
                {
                    GD.Print("Error:", dict.ErrorLine);
                    saveOptions.Close();
                    return new GameOptions();
                }
                if (!(dict.Result is Godot.Collections.Dictionary<string, object> data))
                {
                    saveOptions.Close();
                    return new GameOptions();
                }
                saveOptions.Close();
                return Load(data);
            }

            private static GameOptions Load(IDictionary<string, object> data)
            {
                var resolution = (data["resolution"] as Godot.Collections.Dictionary<string, object>);
                return new GameOptions()
                {
                    Resolution = resolution == null ? null : new Vector2((float) resolution!["x"], (float) resolution!["y"]) as Godot.Vector2?,
                    Locale = data["locale"] as string
                };
            }
        }

        public override void _Ready()
        {
            base._Ready();
            _resolutionOption = GetNode<OptionButton>("ScrollContainer/VBoxContainer/Resolution/OptionButton");
            _customResolutionEdit = GetNode<TextEdit>("ScrollContainer/VBoxContainer/Resolution/TextEdit");
            _customResolutionEdit.Connect("text_changed", this, nameof(CustomResolutionOptionChanged));
            _resolutionOption.Connect("item_selected", this, nameof(ResolutionOptionChanged));

            _fpsSlider = GetNode<HSlider>("ScrollContainer/VBoxContainer/FPS/HSlider");
            _fpsLabel = GetNode<Label>("ScrollContainer/VBoxContainer/FPS/ValueLabel");
            _fpsSlider.Connect("value_changed", this, nameof(FPSChanged));

            _revertButton = GetNode<Button>("HBoxContainer/RevertButton");
            _revertButton.Connect("pressed", this, nameof(HidePanel));
            _applyButton = GetNode<Button>("HBoxContainer/ApplyButton");
            _applyButton.Connect("pressed", this, nameof(ApplyOptions));
            Visible = false;

            _resolutionOption.Clear();
            _resolutionOption.AddItem("Default resolution", -2);
            _resolutionOption.AddItem("Custom", -1);
            _resolutionOption.AddSeparator();
            for (var i = 0; i < _defaultResolutions.Length; i++)
            {
                var resolution = _defaultResolutions[i];
                _resolutionOption.AddItem(resolution.x + "x" + resolution.y, i);
            }
            _options = GameOptions.Load();
            UpdateResolution();
        }

        public override void ShowPanel(AnimatedPanel backPanel = null)
        {
            _options = GameOptions.Load();
            base.ShowPanel(backPanel);
        }

        private void ApplyOptions()
        {
            _options.Save();
            HidePanel();
        }

        private void ResolutionOptionChanged(int index)
        {
            if (index <= 0)
                _options.Resolution = null;
            else if (index == 1)
                _options.Resolution = new Vector2(1000, 1000);
            else
                _options.Resolution = _defaultResolutions[index];
            UpdateResolution();
        }

        private void CustomResolutionOptionChanged()
        {
            try
            {
                var text = _customResolutionEdit.Text;
                var numbers = text.Split("x");
                if (numbers.Length != 2)
                    return;
                var x = int.Parse(numbers[0]);
                var y = int.Parse(numbers[1]);
                _options.Resolution = new Vector2(x, y);
            }
            finally
            {
                UpdateResolution();
            }
        }

        private void UpdateResolution()
        {
            var index = -2;
            if (_options.Resolution != null)
                index = Array.IndexOf(_defaultResolutions, _options.Resolution);
            _resolutionOption.Selected = index;
            _customResolutionEdit.Visible = index == -1;
            if (index == -1)
            {
                var customResolutionText = _options.Resolution?.x + "x" + _options.Resolution?.y;
                if (!customResolutionText.Equals(_customResolutionEdit.Text))
                    _customResolutionEdit.Text = customResolutionText;
            }
        }

        private void FPSChanged(float value)
        {
            var fps = Convert.ToInt32(value);
            if (fps <= 0)
                _fpsLabel.Text = "VSync";
            else
                _fpsLabel.Text = fps.ToString();
        }
    }
}