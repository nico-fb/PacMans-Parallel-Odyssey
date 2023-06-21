using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndScoreText : MonoBehaviour
{

    [SerializeField] ScoreKeeper scoreKeeper;


    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        GetComponent<TMPro.TextMeshProUGUI>().text = "Score: " + scoreKeeper.previousScore;
    }

}
