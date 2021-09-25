using Godot;

namespace ExistenceDot
{
    public class AnimatedPanel : Panel
    {
        private AnimationPlayer _animPlayer;
        private AnimatedPanel _backPanel;

        public override void _Ready()
        {
            _animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        }

        public void ShowPanel(AnimatedPanel backPanel = null)
        {
            Visible = true;
            _animPlayer.Play("OpenPanel");
            _backPanel = backPanel;
            backPanel?.HidePanel();
        }

        public void HidePanel()
        {
            _animPlayer.Play("ClosePanel");
            _backPanel?.ShowPanel();
        }

        private void PanelShown()
        {
            
        }

        private void PanelHidden()
        {
            Visible = false;
        }
    }
}