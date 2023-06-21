using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomScore : MonoBehaviour
{
    [SerializeField] PlayerControls pac;
    TMPro.TextMeshPro text;

    void Start()
    {
        text = GetComponent<TMPro.TextMeshPro>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = pac.getPosition() + new Vector3(0f, -5f, 2.8f);
        text.text = "Score: " + pac.getScore();
    }
}
