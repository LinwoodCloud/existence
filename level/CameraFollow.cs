using Godot;

namespace ExistenceDot.Level
{
    public class CameraFollow : Camera
    {
        // Declare member variables here. Examples:
        // private int a = 2;
        // private string b = "text";
        [Export()]
        private float _distance = 4.0f;
        [Export()]
        private float _height = 2.0f;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            SetPhysicsProcess(true);
            SetAsToplevel(true);
        }

        public override void _PhysicsProcess(float delta)
        {
            var target = GetParent<Spatial>().GlobalTransform.origin;
            var pos = GlobalTransform.origin;
            var up = new Vector3(0, 1, 0);
            var offset = pos - target;
            offset = offset.Normalized() * _distance;
            offset.y = _height;

            pos = target + offset;
            LookAtFromPosition(pos, target, up);
        }   

        //  // Called every frame. 'delta' is the elapsed time since the previous frame.
        //  public override void _Process(float delta)
        //  {
        //      
        //  }
    }
}
