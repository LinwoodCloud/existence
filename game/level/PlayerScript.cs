#nullable enable
using Godot;
using System;
using System.Collections.Generic;
using ExistenceDot.Level.UI;


namespace ExistenceDot.Level
{
    public class PlayerScript : KinematicBody
    {
        [Export()]
        private float _gravity = -9.8f;
        [Export()]
        private int _deathpoint = 0;
        [Export()]
        private int _speed = 6;
        [Export()]
        private int _acceleration = 3;
        [Export()]
        private int _deAcceleration = 5;
        private Vector3 _velocity;
        private AnimationTree _animTree = null!;
        private Camera _camera = null!;
        private PlayerData Data { get; } = new PlayerData();
        private QuestDisplay? _questDisplay = null;
        private Queue<Quest.Quest> _questDisplayQueue = new Queue<Quest.Quest>();

        [Serializable]

        public class PlayerData : Godot.Object
        {
            public Vector3? Checkpoint { get; set; }
            public string CurrentScene { get; set; }
            public List<string> FinishedQuests { get; set; }

            public PlayerData(List<string>? finishedQuests = null, string currentScene = "", Vector3? checkpoint = null)
            {
                FinishedQuests = finishedQuests ?? new List<string>();
                CurrentScene = currentScene;
                Checkpoint = checkpoint;
            }

            public Godot.Collections.Dictionary<string, object> Save() => new Godot.Collections.Dictionary<string, object>(){
                { "checkpoint", Checkpoint },
                { "current_scene", CurrentScene },
                { "finished_quests", FinishedQuests }
            };

            public void Load(Godot.Collections.Dictionary<string, object>? data)
            {
                if (data == null)
                    return;
                Checkpoint = data["checkpoint"] as Vector3?;
                CurrentScene = (data["current_scene"] as string)!;
                FinishedQuests = (data["finished_quests"] as List<string>)!;
            }
        }

        public override void _Ready()
        {
            Data.Checkpoint = GlobalTransform.origin;
            _animTree = GetNode<AnimationTree>("AnimationTree");
            _camera = GetNode<Camera>("Target/Camera");
            SetPhysicsProcess(true);

            LoadData();
            SaveData();
        }


        public void LoadData()
        {
            var saveGame = new File();
            if (!saveGame.FileExists("user://savegame.save"))
                return;
            saveGame.Open("user://savegame.save", File.ModeFlags.Read);
            string text = saveGame.GetAsText();
            JSONParseResult dict = JSON.Parse(text);
            saveGame.Close();
            if (dict.Error != 0)
            {
                GD.Print("Error:", dict.ErrorLine);
            }
            else
            {
                if (dict.Result != null)
                    Data.Load(dict.Result as Godot.Collections.Dictionary<string, object>);
            }

        }


        public void SaveData()
        {
            var saveGame = new File();
            saveGame.Open("user://savegame.save", File.ModeFlags.Write);
            var json = JSON.Print(Data.Save());
            saveGame.StoreString(json);
            saveGame.Close();
        }

        public override void _PhysicsProcess(float delta)
        {
            if (GlobalTransform.origin.y < _deathpoint)
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
                ShowQuest(new Quest.Quest(){Name = "TestQuest", Description = "This is a description"});
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
            if (Data.Checkpoint != null)
                GlobalTransform = new Transform(GlobalTransform.basis, (Vector3)Data.Checkpoint);
        }

        private void ShowQuest(Quest.Quest quest)
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