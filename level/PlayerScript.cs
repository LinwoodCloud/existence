using Godot;

namespace ExistenceDot.Level
{
    public class PlayerScript : KinematicBody
    {
        [Export()]
        private float _gravity = -9.8f;
        [Export()]
        private int _speed = 6;
        [Export()]
        private int _acceleration = 3;
        [Export()]
        private int _deAcceleration = 5;
        private Vector3 _velocity;
        private AnimationTree _animTree;
        private Camera _camera;

        public override void _Ready()
        {
            _animTree = GetNode<AnimationTree>("AnimationTree");
            _camera = GetNode<Camera>("Target/Camera");
            SetPhysicsProcess(true);
        }

        public override void _PhysicsProcess(float delta)
        {
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
    }
}