using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Rewired;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class Player3DScript : MonoBehaviour
{
    
    //Public
    public float Speed;
    public int PlayerNumber;
    public Vector3 RacketNeutralPos;
    public Vector3 RacketNeutralRot;
    public float Radius;
    public float RacketSpeed;
    //Swing Zone Variables
    //This is just the max y variable of the zone
    public float OverheadMax;
    public float TopMax;
    public float FlatMax; //anything higher than this is a volley

    public GameObject Racket;
    public GameObject Arm;

    public Text RacketVectorDebugText;
    
    //Private
    private Rewired.Player rewiredPlayer;
    private Vector3 moveVector;
    private Vector3 racketVector;
    
    //State
    private enum PlayerState
    {
        Idle,
        Walking,
        TopSpin,
        Slice,
        Volley,
        Overhead
    }

    private PlayerState state;
    
    // Start is called before the first frame update
    void Start()
    {
        rewiredPlayer = ReInput.players.GetPlayer(PlayerNumber);
        moveVector = Vector3.zero;
        Racket.transform.localPosition = RacketNeutralPos;
        Racket.transform.localRotation = Quaternion.Euler(RacketNeutralRot);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case PlayerState.Idle:
                Walk();
                IdleSwing();
                break;
            case PlayerState.Walking:
                Walk();
                break;
        }
    }
    
    private void Walk()
    {
        //Check for walking
        moveVector = new Vector3(rewiredPlayer.GetAxis2DRaw("Horizontal", "Vertical").x, 0, rewiredPlayer.GetAxis2DRaw("Horizontal", "Vertical").y);

        transform.position += Vector3.ClampMagnitude(moveVector, 1) * Speed * Time.deltaTime;
    }

    private void IdleSwing()
    {
        racketVector = new Vector3(rewiredPlayer.GetAxis2DRaw("HorizontalAim", "VerticalAim").x, rewiredPlayer.GetAxis2DRaw("HorizontalAim", "VerticalAim").y, 0);
        RacketVectorDebugText.text = "Racket X = " + Math.Round(racketVector.x, 3) + "Racket Y = " + Math.Round(racketVector.y, 3);
        
        if (racketVector.y <= OverheadMax) //Overhead
        {
            if (racketVector.x > 0) //right
            {
                
            }
            else if (racketVector.x <= 0) //left
            {
                
            }
        }
        else if (racketVector.y <= TopMax) //Top Spin
        {
            if (racketVector.x > 0) //right
            {
                
            }
            else if (racketVector.x <= 0) //left
            {
                
            }
        }
        else if (racketVector.y <= FlatMax) //Flat
        {
            if (racketVector.x > 0) //right
            {
                
            }
            else if (racketVector.x <= 0) //left
            {
                
            }
        }
        else //Volley
        {
            if (racketVector.x > 0) //right
            {
                
            }
            else if (racketVector.x <= 0) //left
            {
                
            }
        }
    }

    #region Old Functions

    /*private void RacketMove()
    {
        racketVector = new Vector3(rewiredPlayer.GetAxis2DRaw("HorizontalAim", "VerticalAim").x, rewiredPlayer.GetAxis2DRaw("HorizontalAim", "VerticalAim").y, 0);

        if (racketVector == Vector3.zero)
        {
            Racket.transform.localPosition = RacketNeutralPos;
            Racket.transform.rotation = Quaternion.Euler(RacketNeutralRot);
        }
        else
        {
            //Right side
            if (racketVector.x > 0)
            {
                float theta = Mathf.Asin(racketVector.y);
                Vector3 racketAngle = new Vector3(0, 0, Mathf.Rad2Deg * theta);
                Vector3 distance = racketVector - Racket.transform.localPosition;
                Racket.transform.localPosition += distance * RacketSpeed * Time.deltaTime;
                Racket.transform.rotation = Quaternion.Euler(racketAngle);
            }
            //left side
            else if (racketVector.x <= 0)
            {
                float theta = Mathf.Asin(racketVector.y);
                Vector3 racketAngle = new Vector3(0, 180, Mathf.Rad2Deg * theta);
                Vector3 distance = racketVector - Racket.transform.localPosition;
                Racket.transform.localPosition += distance * RacketSpeed * Time.deltaTime;
                Racket.transform.rotation = Quaternion.Euler(racketAngle);
            }
        }
    }*/

    #endregion
}
