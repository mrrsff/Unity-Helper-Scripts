using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace AlphaGame.Core.SaveSystem
{
    public interface IDataPersistence
    {
        void SaveData(GameData gameData);
        void LoadData(GameData gameData);
    }
}