using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Score : MonoBehaviour
{
    [SerializeField] PlayerControls pac;
    TMPro.TextMeshProUGUI text;

    void Start()
    {
        text = GetComponent<TMPro.TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        text.text = "Score: " + pac.getScore();
    }
}
