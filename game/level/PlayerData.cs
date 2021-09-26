using System;
using System.Collections.Generic;
using Godot;
using Godot.Collections;

namespace ExistenceDot.Level
{
    [Serializable]
    public struct PlayerData
    {
        public PlayerData(Array<string> finishedQuests = null, string currentScene = "", Vector3? checkpoint = null)
        {
            FinishedQuests = finishedQuests ?? new Array<string>();
            CurrentScene = currentScene;
            Checkpoint = checkpoint;
        }

        public Vector3? Checkpoint { get; set; }
        public string CurrentScene { get; set; }
        public Array<string> FinishedQuests { get; set; }

        public void Save()
        {
            var saveGame = new File();
            saveGame.Open("user://savegame.save", File.ModeFlags.Write);
            saveGame.StoreString(JSON.Print(new Godot.Collections.Dictionary<string, object>
            {
                {"checkpoint", Checkpoint},
                {"current_scene", CurrentScene},
                {"finished_quests", FinishedQuests}
            }));
        }

        public static PlayerData Load()
        {
            var saveGame = new File();
            if (!saveGame.FileExists("user://savegame.save"))
                return new PlayerData();
            saveGame.Open("user://savegame.save", File.ModeFlags.Read);
            var text = saveGame.GetAsText();
            if (text.Empty())
                return new PlayerData();
            var dict = JSON.Parse(text);
            if (dict.Error != 0 && dict.Result != null)
            {
                GD.Print("Error:", dict.ErrorLine);
                saveGame.Close();
                return new PlayerData();
            }
            if (!(dict.Result is Godot.Collections.Dictionary<string, object> data))
            {
                saveGame.Close();
                return new PlayerData();
            }
            saveGame.Close();
            return Load(data);
        }

        private static PlayerData Load(IDictionary<string, object> data)
        {
            return new PlayerData()
            {
                Checkpoint = data["checkpoint"] as Vector3?,
                CurrentScene = (data["current_scene"] as string)!,
                FinishedQuests = (data["finished_quests"] as Array<string>)!
            };
        }
    }
}