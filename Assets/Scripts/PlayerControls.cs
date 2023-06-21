using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerControls : MonoBehaviour
{
    Rigidbody rb;
    [SerializeField] float speed = 3f;

    [SerializeField] RedGhostMovement redGhost; //blinky
    [SerializeField] PinkGhostMovement pinkGhost; //pinky
    [SerializeField] CyanGhostMovement cyanGhost; //inky
    [SerializeField] OrangeGhostMovement orangeGhost; //clyde

    [SerializeField] Camera underCam;
    [SerializeField] Camera overCam;

    [SerializeField] AudioSource pelletEatingSound;
    [SerializeField] AudioSource energizerSound;
    [SerializeField] AudioSource ghostDeathSound;
    [SerializeField] AudioSource deathSound;
    [SerializeField] AudioSource portalSound;

    [SerializeField] TMPro.TextMeshProUGUI pausedText;

    Vector3 startPos;
    ArrayList eatenPellets;

    int score;
    bool isPaused;

    [SerializeField] ScoreKeeper scoreKeeper;

    // Start is called before the first frame update
    void Start()
    {
        eatenPellets = new ArrayList();
        Application.targetFrameRate = 60;
        rb = GetComponent<Rigidbody>();
        startPos = new Vector3(0f,1f,-5.5f);
        score = 0;
        isPaused = false;
    }

    // Update is called once per frame
    void Update()
    {
        float horizontalInput = Input.GetAxisRaw("Horizontal");
        float verticalInput = Input.GetAxisRaw("Vertical");
        if (rb.position.y > 0){
            rb.velocity = (transform.forward * verticalInput + transform.right * horizontalInput) * speed; 
        } else {
            rb.velocity = (transform.forward * -verticalInput + transform.right * -horizontalInput) * speed; 
        }
        pauseGame();
        toggleMiniMap();
    }

    void pauseGame(){
        if (Input.GetKeyDown(KeyCode.P)){
            if (isPaused) {
                Time.timeScale = 1f;
                pausedText.text = "";
                isPaused = false;
            } else {
                Time.timeScale = 0f;
                pausedText.text = "PAUSED";
                isPaused = true;
            }
        }
    }

    void toggleMiniMap(){
        if (Input.GetKey(KeyCode.Space)){
            if (rb.position.y < 0){
                    underCam.depth = 0.5f;
                } else {
                    overCam.depth = 0.5f;
                }
        } else {
            underCam.depth = -2f;
            overCam.depth = -2f;
        }
        if (isPaused){
            if (rb.position.y < 0){
                    underCam.depth = 0.5f;
                } else {
                    overCam.depth = 0.5f;
                }
        }
    }

    void OnTriggerEnter(Collider col){
        //interaction with portals
        if (col.tag == "Portal"){
            portalSound.Play();
            if (rb.position.x < -2)
            {
                rb.position = new Vector3(9f, rb.position.y, .5f);
            } else if (rb.position.x > 2)
            {
                rb.position = new Vector3(-9f, rb.position.y, .5f);
            } else 
            {
                rb.position = new Vector3(0f, -rb.position.y, 2.5f);
                if (rb.position.y < 0){
                    rb.position = new Vector3(0f, -1f, 2.5f);
                    underCam.depth = 0.5f;
                    rb.position = new Vector3(0f, -1f, 2.5f);
                } else {
                    rb.position = new Vector3(0f, 1f, 2.5f);
                    underCam.depth = -0.5f;
                    rb.position = new Vector3(0f, 1f, 2.5f);
                }
            }
        }
        //interaction with energizers
        if (col.tag == "Energizer"){
            score += 5;
            col.gameObject.SetActive(false);
            redGhost.frightenedMode();
            pinkGhost.frightenedMode();
            cyanGhost.frightenedMode();
            orangeGhost.frightenedMode();
            energizerSound.Play();
            eatenPellets.Add(col.gameObject);
        }
        //interaction with pellets
        if (col.tag == "Pellet"){
            col.gameObject.SetActive(false);
            pelletEatingSound.Play();
            score += 1;
            eatenPellets.Add(col.gameObject);
        }
        //interaction with blinky
        if (col.tag == "RedGhost" && redGhost.getMode() != 2){
            onDeath();
        }
        if (col.tag == "RedGhost" && redGhost.getMode() == 2){
            score += 20;
            ghostDeathSound.Play();
            redGhost.eatenGhost();
        }
        //interaction with pinky
        if (col.tag == "PinkGhost" && pinkGhost.getMode() != 2){
            onDeath();
        }
        if (col.tag == "PinkGhost" && pinkGhost.getMode() == 2){
            score += 20;
            ghostDeathSound.Play();
            pinkGhost.eatenGhost();
        }
        //interaction with inky
        if (col.tag == "CyanGhost" && cyanGhost.getMode() != 2){
            onDeath();
        }
        if (col.tag == "CyanGhost" && cyanGhost.getMode() == 2){
            score += 20;
            ghostDeathSound.Play();
            cyanGhost.eatenGhost();
        }
        //interaction with clyde
        if (col.tag == "OrangeGhost" && orangeGhost.getMode() != 2){
            onDeath();
        }
        if (col.tag == "OrangeGhost" && orangeGhost.getMode() == 2){
            score += 20;
            ghostDeathSound.Play();
            orangeGhost.eatenGhost();
        }
    }

    void onDeath(){
        scoreKeeper.previousScore = score;
        if (score > scoreKeeper.highScore){
            scoreKeeper.highScore = score;
            PlayerPrefs.SetInt("highScore", score);
        }

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);


        // deathSound.Play();
        // redGhost.startingSettings();
        // pinkGhost.startingSettings();
        // cyanGhost.startingSettings();
        // orangeGhost.startingSettings();
        // underCam.depth = -.5f;
        // transform.position = startPos;
        // score = 0;
        // foreach (GameObject pellet in eatenPellets){
        //     pellet.SetActive(true);
        // }
        // eatenPellets = new ArrayList();
    }

    public int getScore(){
        return score;
    }

    public Vector3 getPosition(){
        return transform.position;
    }
}
