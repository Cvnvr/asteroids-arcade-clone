using UnityEngine;

[CreateAssetMenu(menuName = "Data Objects/Level Information")]
public class LevelInformation : ScriptableObject
{
    public int CurrentLives;
    public int CurrentLevel;
    public Color LevelColor;
    public int CurrentScore;
}