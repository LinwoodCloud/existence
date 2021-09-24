using System;
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

        public override void _Ready()
        {
            base._Ready();
            _resolutionOption = GetNode<OptionButton>("ScrollContainer/VBoxContainer/Resolution/OptionButton");
            _customResolutionEdit = GetNode<TextEdit>("ScrollContainer/VBoxContainer/Resolution/TextEdit");
            _resolutionOption.Connect("item_selected", this, nameof(ResolutionOptionChanged));

            _fpsSlider = GetNode<HSlider>("ScrollContainer/VBoxContainer/FPS/HSlider");
            _fpsLabel = GetNode<Label>("ScrollContainer/VBoxContainer/FPS/ValueLabel");
            _fpsSlider.Connect("value_changed", this, nameof(FPSChanged));

            _revertButton = GetNode<Button>("HBoxContainer/RevertButton");
            _revertButton.Connect("pressed", this, nameof(HidePanel));
            _applyButton = GetNode<Button>("HBoxContainer/ApplyButton");
            _applyButton.Connect("pressed", this, nameof(HidePanel));
            Visible = false;
        }

        private void ResolutionOptionChanged(int index)
        {
            var current = _resolutionOption.GetItemText(index);
            _customResolutionEdit.Visible = current == "Custom";
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