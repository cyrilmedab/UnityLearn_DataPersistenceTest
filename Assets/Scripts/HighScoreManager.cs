using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance { get; set; }
    public int highScore;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    [Serializable]
    class HighestScore
    {
        public int highScore;
    }

    public void SaveHighScore()
    {
        HighestScore data = new HighestScore();
        data.highScore = highScore;

        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "/highscoresave.json", json);
    }

    public void LoadHighScore()
    {
        string path = Application.persistentDataPath + "/highscoresave.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            HighestScore data = JsonUtility.FromJson<HighestScore>(json);
            highScore = data.highScore;
        }
    }
}
