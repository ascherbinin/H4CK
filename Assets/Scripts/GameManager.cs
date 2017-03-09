using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class GameManager : MonoBehaviour
{
    public static GameManager instance = null;

    public CompleteZone CompleteZone;
    public AudioClip GameOverSound;
    public Spawner Spawner;
    public GameObject UICanvas;
    private CountdownTimer _timer;
    private int Score = 0;
    public Text ScoreText;
    public Text GameOverText;

    public bool IsGameOver { get; set; }

    void Awake()
    {
        //Check if instance already exists
        if (instance == null)
            instance = this;
        else if (instance != this)
            Destroy(gameObject);

        //Sets this to not be destroyed when reloading scene
        DontDestroyOnLoad(gameObject);

        _timer = GetComponent<CountdownTimer>();

        InitGame();
    }


    void InitGame()
    {
        //Call the SetupScene function of the BoardManager script, pass it current level number.
        Spawner.InitCircles();

    }

    public void GameOver()
    {
        IsGameOver = true;
        SoundManager.instance.PlaySingle(GameOverSound);
        Time.timeScale = 0;
        GameOverText.text = "YOU LOSE";
        GameOverText.color = Color.red;
        UICanvas.GetComponent<Canvas>().enabled = !UICanvas.GetComponent<Canvas>().enabled;
    }

    public void CheckCompleteZone()
    {
        var isComplete = CompleteZone.CheckItems();
        if (isComplete)
        {
            Time.timeScale = 0;
            GameOverText.text = "YOU WIN";
            GameOverText.color = Color.green;
            UICanvas.GetComponent<Canvas>().enabled = !UICanvas.GetComponent<Canvas>().enabled;
            UpdateScore(100);
        }
            
    }

    public void RestartLevel()
    {
        if (IsGameOver)
            IsGameOver = false;
        Time.timeScale = 1;
        Spawner.Restart();
        CompleteZone.Restart();
        _timer.Restart();
        UICanvas.GetComponent<Canvas>().enabled = !UICanvas.GetComponent<Canvas>().enabled;
    }

    private void UpdateScore(int amount)
    {
        Score += amount;
        ScoreText.text = string.Format("{0:00000000}", Score);
    }
}
