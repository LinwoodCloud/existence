using System.Collections.Generic;
using ExistenceDot.Level.UI;
using ExistenceDot.QuestSystem;
using Godot;

namespace ExistenceDot.Level
{
    public class PlayerScript : KinematicBody
    {
        private readonly Queue<Quest> _questDisplayQueue = new Queue<Quest>();

        [Export] private int _acceleration = 3;

        private AnimationTree _animTree = null!;
        private Camera _camera = null!;

        [Export] private int _deAcceleration = 5;

        [Export] private int _deathPoint = 0;

        [Export] private float _gravity = -9.8f;

        private PauseMenuScript _pauseMenu;
        private QuestDisplay _questDisplay;

        [Export] private int _speed = 6;

        private Vector3 _velocity;

        public override void _Ready()
        {
            var playerData = PlayerData.Load();
            playerData.Checkpoint = GlobalTransform.origin;
            playerData.Save();
            _animTree = GetNode<AnimationTree>("AnimationTree");
            _camera = GetNode<Camera>("Target/Camera");
            _pauseMenu = GetNode<PauseMenuScript>("Pause");
            SetPhysicsProcess(true);
        }

        public override void _PhysicsProcess(float delta)
        {
            if (GlobalTransform.origin.y < _deathPoint)
            {
                TeleportToCheckpoint();
                return;
            }

            var dir = new Vector3();
            var gt = _camera.GlobalTransform;
            var isMoving = false;
            if (Input.IsActionPressed("move_forward"))
            {
                dir += -gt.basis[2];
                isMoving = true;
            }

            if (Input.IsActionPressed("move_backward"))
            {
                dir += gt.basis[2];
                isMoving = true;
            }

            if (Input.IsActionPressed("move_left"))
            {
                dir += -gt.basis[0];
                isMoving = true;
            }

            if (Input.IsActionPressed("move_right"))
            {
                dir += gt.basis[0];
                isMoving = true;
            }

            if (Input.IsActionJustPressed("interact"))
            {
                GD.Print("queue quest");
                ShowQuest(new Quest { Name = "TestQuest", Description = "This is a description" });
            }

            if (Input.IsActionJustPressed("ui_cancel"))
            {
                _pauseMenu.TogglePause();
            }

            dir.y = 0;
            dir = dir.Normalized();
            _velocity.y += delta * _gravity;
            var hv = _velocity;
            hv.y = 0;
            var newPos = dir * _speed;
            var accel = _deAcceleration;
            if (dir.Dot(hv) > 0)
                accel = _acceleration;
            hv = hv.LinearInterpolate(newPos, accel * delta);
            _velocity.x = hv.x;
            _velocity.z = hv.z;
            _velocity = MoveAndSlide(_velocity, new Vector3(0, 1, 0));
            if (isMoving)
            {
                var angle = Mathf.Atan2(hv.x, hv.z);
                var charRot = Rotation;
                charRot.y = angle;
                Rotation = charRot;
            }

            var stateMachine = (AnimationNodeStateMachinePlayback)_animTree.Get("parameters/playback");
            var playingAnim = "Idle";
            if (isMoving)
                playingAnim = "Walking";
            if (playingAnim != stateMachine.GetCurrentNode())
                stateMachine.Travel(playingAnim);
        }

        public void TeleportToCheckpoint()
        {
            var playerData = PlayerData.Load();
            if (playerData.Checkpoint != null)
                GlobalTransform = new Transform(GlobalTransform.basis, (Vector3)playerData.Checkpoint);
        }

        private void ShowQuest(Quest quest)
        {
            _questDisplayQueue.Enqueue(quest);
            ShowNewQuest();
        }

        private void ShowNewQuest()
        {
            if (_questDisplay != null || _questDisplayQueue.Count == 0)
                return;
            GD.Print("Showing quest....");
            var scene = ResourceLoader.Load<PackedScene>("level/ui/quest.tscn");
            _questDisplay = scene.Instance<QuestDisplay>();
            AddChild(_questDisplay);
            _questDisplay.Show(_questDisplayQueue.Dequeue());
            _questDisplay.Connect("tree_exited", this, nameof(RemoveQuestDisplay));
        }

        private void RemoveQuestDisplay()
        {
            _questDisplay = null;
            ShowNewQuest();
        }
    }
}