using Godot;

namespace ExistenceDot
{
    public class CreditsScript : AnimatedPanel
    {
        private RichTextLabel _content;
        public override void _Ready()
        {
            base._Ready();

            _content = GetNode<RichTextLabel>("ScrollContainer/RichTextLabel");
            _content.Connect("meta_clicked", this, nameof(OpenMeta));
            GetNode<Button>("HBoxContainer/CloseButton").Connect("pressed", this, nameof(HidePanel));
        }

        public void OpenMeta(dynamic meta)
        {
            if(meta is string)
                OS.ShellOpen(meta);
        }
    }
}