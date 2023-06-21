using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PinkGhostNBound : MonoBehaviour
{
    [SerializeField] PinkGhostMovement ghost;

    void OnTriggerEnter(Collider col){
        if (col.tag == "Wall" || col.tag == "Portal"){
            ghost.canMoveN = false;
        }
    }
    void OnTriggerStay(Collider col){
        if (col.tag == "Wall" || col.tag == "Portal"){
            ghost.canMoveN = false;
        }
    }
    void OnTriggerExit(Collider col){
        if (col.tag == "Wall" || col.tag == "Portal"){
            ghost.canMoveN = true;
        }
    }

}
