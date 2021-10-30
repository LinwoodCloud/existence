using System;
using ExistenceDot.Level;
using Godot;

namespace ExistenceDot.QuestSystem
{
    [Serializable]
    public abstract class QuestElement : Spatial
    {
        private PlayerScript _player;

        public override void _Ready()
        {
            _player = GetTree().Root.GetNode<PlayerScript>("Player");
        }
    }
}