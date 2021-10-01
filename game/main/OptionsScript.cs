using System;
using System.Collections.Generic;
using Godot;

namespace ExistenceDot
{
    public class OptionsScript : AnimatedPanel
    {
        private Label _fpsLabel;

        private Slider _fpsSlider;

        private Button _revertButton, _applyButton;
        private CheckBox _fullscreenCheckBox;

        private GameOptions _options;

        public struct GameOptions
        {
            public GameOptions(string locale = "en", bool fullscreen = true, int fps = 0)
            {
                Locale = locale;
                Fullscreen = fullscreen;
                Fps = fps;
            }

            public string Locale { get; set; }
            public bool Fullscreen { get; set; }
            public int Fps { get; set; }

            public void Save()
            {
                var saveOptions = new File();
                saveOptions.Open("user://options.save", File.ModeFlags.Write);
                saveOptions.StoreString(JSON.Print(new Godot.Collections.Dictionary<string, object>
                {
                    {"locale", Locale},
                    {"fullscreen", Fullscreen},
                    {"fps", Fps}
                }));
                saveOptions.Close();
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
                GD.Print(text);
                var dict = JSON.Parse(text);
                if (dict.Error != Error.Ok )
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
                return new GameOptions()
                {
                    Locale = data["locale"] as string ?? "en",
                    Fullscreen = data["fullscreen"] as bool? ?? true,
                    Fps = data["fps"] as int? ?? 0
                };
            }
        }

        public override void _Ready()
        {
            base._Ready();

            _fpsSlider = GetNode<HSlider>("ScrollContainer/VBoxContainer/FPS/HSlider");
            _fpsLabel = GetNode<Label>("ScrollContainer/VBoxContainer/FPS/ValueLabel");
            _fpsSlider.Connect("value_changed", this, nameof(FpsChanged));

            _revertButton = GetNode<Button>("HBoxContainer/RevertButton");
            _revertButton.Connect("pressed", this, nameof(HidePanel));
            _applyButton = GetNode<Button>("HBoxContainer/ApplyButton");
            _applyButton.Connect("pressed", this, nameof(ApplyOptions));
            Visible = false;

            _fullscreenCheckBox = GetNode<CheckBox>("ScrollContainer/VBoxContainer/Fullscreen/CheckBox");
            _fullscreenCheckBox.Connect("pressed", this, nameof(FullscreenChanged));
            UpdateOption();
        }

        private void UpdateFullscreen()
        {
            _fullscreenCheckBox.Pressed = _options.Fullscreen;
        }

        private void FullscreenChanged()
        {
            _options.Fullscreen = _fullscreenCheckBox.Pressed;
        }

        private void UpdateOption()
        {
            UpdateFullscreen();
            UpdateFps();
        }

        public override void ShowPanel(AnimatedPanel backPanel = null)
        {
            _options = GameOptions.Load();
            UpdateFullscreen();
            base.ShowPanel(backPanel);
        }

        private void ApplyOptions()
        {
            _options.Save();
            HidePanel();
        }

        private void FpsChanged(float value)
        {
            _options.Fps = (int)value;
            UpdateFps();
        }

        private void UpdateFps()
        {
            var fps = Convert.ToInt32(_options.Fps);
            if (fps <= 0)
                _fpsLabel.Text = "VSync";
            else
                _fpsLabel.Text = fps.ToString();
        }
    }
}