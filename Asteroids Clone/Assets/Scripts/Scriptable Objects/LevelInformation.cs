using UnityEngine;

/// <summary>
/// Used to contain key game data which needs to be accessed in multiple places
/// </summary>
[CreateAssetMenu(menuName = "Data Objects/Level Information")]
public class LevelInformation : ScriptableObject
{
    public int CurrentLives;
    public int CurrentLevel;
    public Color LevelColor;
    public int CurrentScore;
}