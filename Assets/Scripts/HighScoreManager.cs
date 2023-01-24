using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class HighScoreManager : MonoBehaviour
{
    public static HighScoreManager Instance { get; set; }
    public string highscorerName;
    public int highScore;

    private void Awake()
    {
        if (Instance != null) Destroy(gameObject);
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            LoadHighScore();
        }
    }

    [Serializable]
    class HighestScore
    {
        public string highscorerName;
        public int highScore;
    }

    public void SaveHighScore()
    {
        HighestScore data = new HighestScore();
        data.highscorerName = highscorerName;
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
            highscorerName = data.highscorerName;
            highScore = data.highScore;
        }
    }
}
