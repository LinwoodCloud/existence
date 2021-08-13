using Godot;

namespace ExistenceDot.Level
{
    public class PlayerScript : KinematicBody
    {
        [Export()]
        private int speed;
        private AnimationTree animTree;
        private readonly NodePath animTreePath = new NodePath("AnimationTree");

        public override void _Ready()
        {
            animTree = GetNode<AnimationTree>(animTreePath);
        }

        public override void _PhysicsProcess(float delta)
        {
            var stateMachine = (AnimationNodeStateMachinePlayback)animTree.Get("parameters/playback");
            var rootMotion = animTree.GetRootMotionTransform();
            var v = rootMotion.origin / delta;
            GD.Print(stateMachine.GetCurrentNode());
            var playingAnim = "Idle";
            if (Input.IsActionPressed("ui_select"))
                playingAnim = "Walking";
            if(playingAnim != stateMachine.GetCurrentNode())
                stateMachine.Travel(playingAnim);
            MoveAndSlide(v, Vector3.Up);
        }
    }
}