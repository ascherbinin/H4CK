using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountdownTimer : MonoBehaviour
{
    public float LevelTime;
    private float _timeLeft;


    public Text TimerText;
    // Use this for initialization
    void Start () {
        _timeLeft = LevelTime * Random.Range(0.7f, 1.2f);
    }

    // Update is called once per frame
    void Update()
    {
        if (_timeLeft > 0)
        {
            _timeLeft -= Time.deltaTime;
            int minutes = Mathf.FloorToInt(_timeLeft / 60F);
            int seconds = Mathf.FloorToInt(_timeLeft - minutes * 60);
            var fraction = (_timeLeft * 100) % 100;
            //update the label value
            TimerText.text = string.Format("{0:00} : {1:00} : {2:000}", minutes, seconds, fraction);
        }
        else
        {
            if(!GameManager.instance.IsGameOver)
                GameManager.instance.GameOver();
            TimerText.text = string.Format("{0:00} : {1:00} : {2:000}", 0, 0, 0);
        }
    }

    public void Restart()
    {
        _timeLeft = LevelTime * Random.Range(0.7f, 1.2f);
    }
}
