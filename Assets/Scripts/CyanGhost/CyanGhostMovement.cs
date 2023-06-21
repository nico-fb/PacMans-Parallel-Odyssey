using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CyanGhostMovement : MonoBehaviour
{

    public enum ghostDir
    {
        N, E, S, W
    }

    Rigidbody rb;
    [SerializeField] Rigidbody pac;
    [SerializeField] Material frightenedMaterial;
    [SerializeField] Material normalMaterial;
    [SerializeField] Rigidbody blinky;

    public bool canMoveN = false;
    public bool canMoveE = false;
    public bool canMoveS = false;
    public bool canMoveW = false;

    Dictionary<ghostDir, Vector3> nextDirectionVelocities;
    Dictionary<ghostDir, ghostDir> oppositeDirection;
    ghostDir currentDirection;

    int mode = 0; //0=scatter, 1=chase, 2=frightened

    int turnDelay = 0;
    [SerializeField] int speed = 3;
    [SerializeField] int moveSpeed = 3;
    [SerializeField] int frightSpeed = 2;
    int skipFrames;
    int fps = 60;
    int tick = 0;
    int frightenedTick = 0;

    [SerializeField] AudioSource energizerOverSound;

    // Start is called before the first frame update
    void Start(){

        skipFrames = (24/speed);
        rb = GetComponent<Rigidbody>();
        
        startingSettings();

        nextDirectionVelocities = new Dictionary<ghostDir, Vector3>(){
	        {ghostDir.N, new Vector3(0,0,1)},
	        {ghostDir.E, new Vector3(1,0,0)},
	        {ghostDir.S, new Vector3(0,0,-1)},
            {ghostDir.W, new Vector3(-1,0,0)}
        };
        oppositeDirection = new Dictionary<ghostDir, ghostDir>(){
	        {ghostDir.N, ghostDir.S},
	        {ghostDir.E, ghostDir.W},
	        {ghostDir.S, ghostDir.N},
            {ghostDir.W, ghostDir.E}
        };
    }

    // Update is called once per frame
    void Update(){

        if (tick < (84 * fps + 2)){
            modeChanger();
        }
        frightenedModeChanger();

        if (turnDelay % (skipFrames) == 0){

            var possibleDirections = new ArrayList();

            if (canMoveN){
                possibleDirections.Add(ghostDir.N);
            }
            if (canMoveE){
                possibleDirections.Add(ghostDir.E);
            }
            if (canMoveS){
                possibleDirections.Add(ghostDir.S);
            }
            if (canMoveW){
                possibleDirections.Add(ghostDir.W);
            }

            if (rb.velocity.x > 0){
                currentDirection = ghostDir.E;
                possibleDirections.Remove(ghostDir.W);
            } 
            if (rb.velocity.x < 0){
                currentDirection = ghostDir.W;
                possibleDirections.Remove(ghostDir.E);
            }
            if (rb.velocity.z > 0){
                currentDirection = ghostDir.N;
                possibleDirections.Remove(ghostDir.S);
            }
            if (rb.velocity.z < 0){
                currentDirection = ghostDir.S;
                possibleDirections.Remove(ghostDir.N);
            }

            ghostDir nextDirection = currentDirection;
            if (mode == 0){nextDirection = findMoveScatter(currentDirection, possibleDirections);}
            if (mode == 1){nextDirection = findMoveChase(currentDirection, possibleDirections);}
            if (mode == 2){nextDirection = findMoveFrightened(currentDirection, possibleDirections);}

            rb.velocity = nextDirectionVelocities[nextDirection] * speed;

            if (currentDirection != nextDirection){
                turnDelay++;
            }
            
        } else {
            turnDelay++;
        }
    }

    /*
        This method handles mode changes
    */
    void modeChanger(){
        if (mode != 2){tick++;}
        if (tick == 7 * fps){chaseMode();}
        if (tick == 27 * fps){scatterMode();}
        if (tick == 34 * fps){chaseMode();}
        if (tick == 54 * fps){scatterMode();}
        if (tick == 59 * fps){chaseMode();}
        if (tick == 79 * fps){scatterMode();}
        if (tick == 84 * fps){chaseMode();}
        
    }
    void frightenedModeChanger(){
        if (mode == 2){frightenedTick++;}
        if (frightenedTick == 6 * fps){
            chaseMode();
            frightenedTick = 0;
            energizerOverSound.Play();
        }
    }

    /*
        This method handles calculating the next ghost move when it is in chase mode
    */
    ghostDir findMoveChase(ghostDir currentDirection, ArrayList possibleDirections){

        var newGhostPos = new Dictionary<ghostDir, Vector3>(){
	        {ghostDir.N, rb.position + new Vector3(0,0,1)},
	        {ghostDir.E, rb.position + new Vector3(1,0,0)},
	        {ghostDir.S, rb.position + new Vector3(0,0,-1)},
            {ghostDir.W, rb.position + new Vector3(-1,0,0)}
        };

        Vector3 pacAttack = pac.position + pac.transform.forward * 2;
        Vector3 target = pacAttack + (pacAttack - blinky.position);
        float minDistance = float.PositiveInfinity;
        ghostDir nextDirection = currentDirection;
        
        foreach (ghostDir dir in possibleDirections){
            float dist = Vector3.Distance(target, newGhostPos[dir]);
            if (dist < minDistance){
                minDistance = dist;
                nextDirection = dir;
            }
        }
        return nextDirection;
    }

    /*
        This method handles calculating the next ghost move when it is in scatter mode
    */
    ghostDir findMoveScatter(ghostDir currentDirection, ArrayList possibleDirections){

        var newGhostPos = new Dictionary<ghostDir, Vector3>(){
	        {ghostDir.N, rb.position + new Vector3(0,0,1)},
	        {ghostDir.E, rb.position + new Vector3(1,0,0)},
	        {ghostDir.S, rb.position + new Vector3(0,0,-1)},
            {ghostDir.W, rb.position + new Vector3(-1,0,0)}
        };

        Vector3 target = new Vector3(9, 0, -13);
        float minDistance = float.PositiveInfinity;
        ghostDir nextDirection = currentDirection;
        
        foreach (ghostDir dir in possibleDirections){
            float dist = Vector3.Distance(target, newGhostPos[dir]);
            if (dist < minDistance){
                minDistance = dist;
                nextDirection = dir;
            }
        }
        return nextDirection;
    }

    /*
        This method handles calculating the next ghost move when it is in frightened mode
    */
    ghostDir findMoveFrightened(ghostDir currentDirection, ArrayList possibleDirections){

        System.Random rnd = new System.Random();
        int length = possibleDirections.Count;
        if (length == 0){
            return currentDirection;
        }
        return (ghostDir) possibleDirections[rnd.Next(0, length)];

    }

    /*
        This method updates speed and the number of frames skipped due to turn delay
    */    
    void updateSpeed(int newSpeed){
        speed = newSpeed;
        skipFrames = (24/speed);
    }

    /*
        This method makes the ghost switch directions when it hits a portal
    */
    void OnTriggerEnter(Collider col){
        if (col.tag == "Portal" && col.transform.position.x != 0){
            currentDirection = oppositeDirection[currentDirection];
            rb.velocity = nextDirectionVelocities[currentDirection] * speed;
        }
    }

    /*
        Method for setting the starting settings of the ghost
    */
    public void startingSettings(){
        rb.position = transform.parent.position + new Vector3(1,0,0);
        currentDirection = ghostDir.N;
        rb.velocity = new Vector3(0,0,1) * moveSpeed;
        scatterMode();
        tick = 0;
        turnDelay = 1;
    }

    /*
        When a ghost is eaten, this method is called to reset its position and velocity
    */
    public void eatenGhost(){
        chaseMode();
        rb.position = transform.parent.position;
        rb.velocity = new Vector3(0,0,1) * moveSpeed;
        currentDirection = ghostDir.N;
        turnDelay = 1;
    }

    /*
        Changes the ghost mode to Scatter mode
    */
    public void scatterMode(){
        mode = 0;
        updateSpeed(moveSpeed);
        transform.Find("GhostObject").gameObject.GetComponent<MeshRenderer>().material = normalMaterial;
        transform.Find("GhostObjectUnder").gameObject.GetComponent<MeshRenderer>().material = normalMaterial;
    }

    /*
        Changes the ghost mode to Chase mode
    */
    public void chaseMode(){
        mode = 1;
        updateSpeed(moveSpeed);
        transform.Find("GhostObject").gameObject.GetComponent<MeshRenderer>().material = normalMaterial;
        transform.Find("GhostObjectUnder").gameObject.GetComponent<MeshRenderer>().material = normalMaterial;
    }

    /*
        Changes the ghost mode to Frightened mode
    */
    public void frightenedMode(){
        frightenedTick = 0;
        mode = 2;
        updateSpeed(frightSpeed);
        currentDirection = oppositeDirection[currentDirection];
        rb.velocity = nextDirectionVelocities[currentDirection] * speed;
        turnDelay = 1;
        transform.Find("GhostObject").gameObject.GetComponent<MeshRenderer>().material = frightenedMaterial;
        transform.Find("GhostObjectUnder").gameObject.GetComponent<MeshRenderer>().material = frightenedMaterial;
    }

    /*
        Getter method for ghost's mode
    */
    public int getMode(){
        return mode;
    }

}
