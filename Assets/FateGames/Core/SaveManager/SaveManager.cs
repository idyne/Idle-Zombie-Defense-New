using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEditor;
using UnityEngine;

namespace FateGames.Core
{
    [CreateAssetMenu(menuName = "Fate/Manager/SaveManager")]
    public class SaveManager : ScriptableObject
    {
        /* [HideInInspector]*/
        [SerializeField] public bool OverrideSave = false;
        /*[HideInInspector]*/
        [SerializeField] public SaveDataVariable SaveData;
        /* [HideInInspector]*/
        [SerializeField] public SaveDataVariable OverrideSaveData = null;
        /* [HideInInspector]*/
        [SerializeField] public bool AutoSave = true;
        /* [HideInInspector]*/
        [SerializeField] public float AutoSavePeriod = 10;
        private float lastTotalPlaytimeSaveTime = 0;
        public float TotalPlaytime { get => PlayerPrefs.GetFloat("total_playtime"); set => PlayerPrefs.SetFloat("total_playtime", value); }

        public void Initialize()
        {
            Load();
            if (!OverrideSave && AutoSave)
                RoutineRunner.StartRoutine(SaveRoutine());
        }

        private IEnumerator SaveRoutine()
        {
            Save(SaveData.Value);
            yield return new WaitForSeconds(AutoSavePeriod);
            yield return SaveRoutine();
        }

        private void SaveTotalPlaytime()
        {
            float difference = Time.time - lastTotalPlaytimeSaveTime;
            TotalPlaytime += difference;
            lastTotalPlaytimeSaveTime = Time.time;
        }

        public void Save(SaveData data)
        {
            SaveTotalPlaytime();
            if (OverrideSave) return;
            BinaryFormatter formatter = new();
            string path = Application.persistentDataPath + "/saveData.fate";
            FileStream stream = new(path, FileMode.Create);
            formatter.Serialize(stream, data);
            stream.Close();
            Debug.Log("Saved");
        }

        public void Load()
        {
            if (!OverrideSave)
            {
                string path = Application.persistentDataPath + "/saveData.fate";
                if (File.Exists(path))
                {
                    BinaryFormatter formatter = new();
                    FileStream stream = new(path, FileMode.Open);
                    stream.Position = 0;
                    SaveData data = formatter.Deserialize(stream) as SaveData;
                    stream.Close();
                    SaveData.Value = data;
                }
                else
                {
                    SaveData data = new();
                    SaveData.Value = data;
                    Save(data);
                }
            }
            else
            {
                SaveData data;
                if (OverrideSaveData == null) data = new();
                else data = OverrideSaveData.Value;
                SaveData.Value = data;
            }
        }
#if UNITY_EDITOR
        [MenuItem("Fate/Delete Player Data")]
        static void DeletePlayerData()
        {
            string path = Application.persistentDataPath + "/saveData.fate";
            if (File.Exists(path))
                File.Delete(path);
        }
#endif
    }
}
