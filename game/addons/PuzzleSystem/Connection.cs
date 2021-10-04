using Godot;

namespace ExistenceDot.PuzzleSystem
{
    public interface Connection : IPuzzle
    {
        public void onActivate(Spatial spatial, Color color);
    }
}