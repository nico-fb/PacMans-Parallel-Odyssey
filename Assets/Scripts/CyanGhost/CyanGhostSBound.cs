using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyanGhostSBound : MonoBehaviour
{
    [SerializeField] CyanGhostMovement ghost;

    void OnTriggerEnter(Collider col){
        if (col.tag == "Wall" || col.tag == "Portal"){
            ghost.canMoveS = false;
        }
    }
    void OnTriggerStay(Collider col){
        if (col.tag == "Wall" || col.tag == "Portal"){
            ghost.canMoveS = false;
        }
    }
    void OnTriggerExit(Collider col){
        if (col.tag == "Wall" || col.tag == "Portal"){
            ghost.canMoveS = true;
        }
    }
}
