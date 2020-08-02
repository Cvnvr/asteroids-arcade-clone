using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScoreList
{
    public List<Score> scores;
}

public class Score
{
    public int highScore;
    public string tag;

    public Score(int highScore, string tag)
    {
        this.highScore = highScore;
        this.tag = tag;
    }
}