using Godot;

namespace ExistenceDot
{
    public class AnimatedPanel : Panel
    {
        private AnimationPlayer _animPlayer;
        private AnimatedPanel _backPanel;
        [Export]
        public bool AnimateIn;

        public override void _Ready()
        {
            _animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            if(AnimateIn)
                ShowPanel();
            else
                Visible = false;
        }

        public virtual void ShowPanel(AnimatedPanel backPanel = null)
        {
            Visible = true;
            _animPlayer.Play("OpenPanel");
            _backPanel = backPanel;
            backPanel?.HidePanel();
        }

        public void HidePanel()
        {
            _animPlayer.Play("ClosePanel");
            if(_backPanel != null && IsInstanceValid(_backPanel))
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