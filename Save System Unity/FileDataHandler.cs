using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using Newtonsoft.Json;

namespace AlphaGame.Core.SaveSystem
{
    public class FileDataHandler
    {
        public string path= Globals.savePath;
        public string fileName="save1.json";
        public FileDataHandler(string fileName)
        {
            this.fileName = fileName;
        }
        public void SaveGame(GameData gameData)
        {
            if(!Directory.Exists(path)) Directory.CreateDirectory(path);
            string json = JsonConvert.SerializeObject(gameData, Formatting.Indented, new JsonSerializerSettings { ReferenceLoopHandling = ReferenceLoopHandling.Ignore });
            try
            {
                using(FileStream fs = File.Create(Path.Combine(path, fileName)))
                {
                    using(StreamWriter sw = new StreamWriter(fs))
                    {
                        sw.Write(json);
                    }
                }
                Debug.Log("Game saved successfully to " + Path.Combine(path, fileName));
            }
            catch(IOException e)
            {
                Debug.Log("Error occured while saving game: " + e.Message);
            }
        }
        public GameData LoadGame()
        {
            if(!Directory.Exists(path)) Directory.CreateDirectory(path);
            string json = "";
            using(FileStream fs = File.OpenRead(Path.Combine(path, fileName)))
            {
                using(StreamReader sr = new StreamReader(fs))
                {
                    json = sr.ReadToEnd();
                }
            }
            List<string> errors = new List<string>();
            GameData gameData = JsonConvert.DeserializeObject<GameData>(json, new JsonSerializerSettings { Error = delegate (object sender, Newtonsoft.Json.Serialization.ErrorEventArgs args)
            {
                errors.Add(args.ErrorContext.Error.Message);
                args.ErrorContext.Handled = true;
            } });
            if(errors.Count > 0)
            {
                Debug.LogError("Errors occured while loading game: " + string.Join(", ", errors.ToArray())); 
                return null;
            }
            return gameData;
        }
    }
}