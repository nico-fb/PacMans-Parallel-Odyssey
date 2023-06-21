using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndHighScoreText : MonoBehaviour
{

    [SerializeField] ScoreKeeper scoreKeeper;


    // Start is called before the first frame update
    void Start()
    {
        GetComponent<TMPro.TextMeshProUGUI>().text = "High Score: " + scoreKeeper.highScore;
    }

}
