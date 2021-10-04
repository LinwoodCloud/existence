using Godot;

namespace ExistenceDot.PuzzleSystem
{
    public class Wire : Connection
    {
        public void onActivate(Spatial spatial, Color color)
        {
        }

        public Color CurrentColor { get; }
    }
}