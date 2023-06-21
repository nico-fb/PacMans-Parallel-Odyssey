using UnityEngine;

[CreateAssetMenu(fileName = "ScoreKeeper", menuName = "Scores")]
public class ScoreKeeper : ScriptableObject
{
    public int previousScore = 0;
    public int highScore = 0;

    void Awake(){
        highScore = PlayerPrefs.GetInt("highScore");
    }

}
