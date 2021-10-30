using Godot;
using Godot.Collections;

namespace ExistenceDot.PuzzleSystem
{
    public class Lever : MeshInstance, IPuzzle
    {
        private AnimationPlayer _animPlayer;
        [Export] public Array<Connection> Connections { get; set; } = new Array<Connection>();
        [Export] public Color ActivatedColor { get; set; }
        [Export] public Color DeactivatedColor { get; set; }
        [Export] public bool Activated { get; set; }

        public Color CurrentColor => Activated ? ActivatedColor : DeactivatedColor;

        public override void _Ready()
        {
            _animPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
            UpdateShader();
        }

        public void Toggle()
        {
            if (Activated)
                Deactivate();
            else
                Activate();
        }

        public void Activate()
        {
            if (Activated)
                return;
            _animPlayer.Play("Activate");
            UpdateShader();
        }


        public void Deactivate()
        {
            if (!Activated)
                return;
            _animPlayer.Play("Deactivate");
            UpdateShader();
        }

        public void UpdateShader()
        {
            if (!(MaterialOverride is ShaderMaterial))
                return;
            var material = MaterialOverride as ShaderMaterial;
            material?.SetShaderParam("recolored", CurrentColor);
        }
    }
}