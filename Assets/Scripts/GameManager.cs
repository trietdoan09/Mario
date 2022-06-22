    using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private Player Player;



    public Text timeText;
    public Text scoreText;
    public Text healText;

    private int Time;
    private int Score;
    private int Heal;

    private void Awake()
    {
        Player = FindObjectOfType<Player>();
        //spawner = FindObjectOfType<Spawner>();
    }
    void Start()
    {
        Time = 200;
        timeText.text = Time.ToString();

        Score = 0;
        scoreText.text = Score.ToString();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Gtime()
    {
        Time--;
        timeText.text = Time.ToString();
    }

    public void IncreaseScore()
    {
        Score++;
        scoreText.text = Score.ToString();
    }
}
