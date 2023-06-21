using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RedGhostEBound : MonoBehaviour
{
    [SerializeField] RedGhostMovement ghost;

    void OnTriggerEnter(Collider col){
        if (col.tag == "Wall" || col.tag == "Portal"){
            ghost.canMoveE = false;
        }
    }
    void OnTriggerStay(Collider col){
        if (col.tag == "Wall" || col.tag == "Portal"){
            ghost.canMoveE = false;
        }
    }
    void OnTriggerExit(Collider col){
        if (col.tag == "Wall" || col.tag == "Portal"){
            ghost.canMoveE = true;
        }
    }
}
