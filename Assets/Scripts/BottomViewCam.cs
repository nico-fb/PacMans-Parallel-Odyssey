using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BottomViewCam : MonoBehaviour
{
    [SerializeField] Transform pac;

    // Update is called once per frame
    void Update()
    {
        transform.position = new Vector3(pac.position.x, transform.position.y, pac.position.z);
    }
}

