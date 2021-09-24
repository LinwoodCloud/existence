using ExistenceDot.Level;
using Godot;

namespace ExistenceDot.Quest
{
    public abstract class QuestElement : Spatial
    {
        private PlayerScript _player;
        public Quest NextQuest { get; }

        public override void _Ready()
        {
            _player = GetTree().Root.GetNode<PlayerScript>("Player");
        }
    }
}