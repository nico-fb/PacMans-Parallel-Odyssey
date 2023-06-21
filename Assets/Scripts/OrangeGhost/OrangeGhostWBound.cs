using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrangeGhostWBound : MonoBehaviour
{
    [SerializeField] OrangeGhostMovement ghost;

    void OnTriggerEnter(Collider col){
        if (col.tag == "Wall" || col.tag == "Portal"){
            ghost.canMoveW = false;
        }
    }
    void OnTriggerStay(Collider col){
        if (col.tag == "Wall" || col.tag == "Portal"){
            ghost.canMoveW = false;
        }
    }
    void OnTriggerExit(Collider col){
        if (col.tag == "Wall" || col.tag == "Portal"){
            ghost.canMoveW = true;
        }
    }
}
