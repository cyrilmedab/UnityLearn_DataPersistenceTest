using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;

    public PauseUIHandler pauseHandler;

    public Text ScoreText;
    public Text LeaderboardText;
    public GameObject GameOverText;
    public GameObject NewHighscoreScreen;
    [SerializeField] private TMP_InputField inputField;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;
    private bool isWaitingForName = false;
    private string m_PlayerName;

    
    // Start is called before the first frame update
    void Start()
    {
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);

        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        LoadLeaderboard();
    }

    private void Update()
    {
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_Started)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                if (PauseUIHandler.isPaused) pauseHandler.ResumeGame();
                else pauseHandler.PauseGame();
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space) && !isWaitingForName)
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
        m_GameOver = true;

        if (isHighScore())
        {
            NewHighscoreScreen.SetActive(true);
            isWaitingForName = true;
            inputField.ActivateInputField();
            StartCoroutine(WaitingForName());
        }
        else
        {
            GameOverText.SetActive(true);
        }
    }

    private IEnumerator WaitingForName()
    {
        while(isWaitingForName)
        {
            yield return null;
        }

        UpdateHighscore();
        SceneManager.LoadScene(0);
    }

    public void ReceivePlayerName()
    {
        isWaitingForName = false;
        m_PlayerName = inputField.text;
    }

    public bool isHighScore()
    {
        return m_Points > HighScoreManager.Instance.highScore;
    }

    public void UpdateHighscore()
    {
        HighScoreManager.Instance.highscorerName = m_PlayerName;
        HighScoreManager.Instance.highScore = m_Points;
        HighScoreManager.Instance.SaveHighScore();
        LoadLeaderboard();
    }

    public void LoadLeaderboard()
    {
        if (HighScoreManager.Instance != null)
        {
            if(HighScoreManager.Instance.highScore == 0)
            {
                LeaderboardText.text = "No highscore yet!";
            }
            else
            {
                LeaderboardText.text = $"Highscore: " +
                $" {HighScoreManager.Instance.highScore}  --" +
                $"  {HighScoreManager.Instance.highscorerName}";
            }
        }
    }
}
