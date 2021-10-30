using Godot;

namespace ExistenceDot.PuzzleSystem
{
    public class Button : MeshInstance, IPuzzle
    {
        private AnimationPlayer _animPlayer;

        public Color CurrentColor { get; }

        public override void _Ready()
        {
            _animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        }

        public void PressButton()
        {
        }
    }
}