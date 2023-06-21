using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGhostNBound : MonoBehaviour
{
    [SerializeField] RedGhostMovement ghost;

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
