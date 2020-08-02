using System;
using System.Collections.Generic;

/// <summary>
/// Was unable to serialize a List<Score> using Unity's JsonUtility
///     so had to create a wrapper (as recommended in this article
///     https://unity3dtuts.com/json-serialization-beyond-limits-in-unity/)
/// </summary>
[Serializable]
public class ScoreList
{
    public List<Score> scores;
}

[Serializable]
public class Score
{
    public int score;
    public string tag;

    public Score(int score, string tag)
    {
        this.score = score;
        this.tag = tag;
    }
}