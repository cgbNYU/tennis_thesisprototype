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
    public float FlatMax;
    public float VolleyMax;
    public float SliceMax; //probably not necessary, because anything higher than Volley should be a slice ergo.
    
    //Swing Rotations and Positions
    public Vector3 FlatRotation;

    public GameObject Racket;
    public GameObject Arm;

    public Text RacketVectorDebugText;
    
    //Private
    private Rewired.Player rewiredPlayer;
    private Vector3 moveVector;
    private Vector3 racketVector;
    private int swingSide = 0;
    private Vector3 lastRacketVector;
    
    //State
    private enum PlayerState
    {
        Idle,
        Walking,
        FlatWindup,
        FlatSwing,
        TopWindup,
        TopSwing,
        SliceWindup,
        SliceSwing,
        Volley,
        OverheadWindup,
        OverheadSwing
    }

    private PlayerState state;
    
    // Start is called before the first frame update
    void Start()
    {
        rewiredPlayer = ReInput.players.GetPlayer(PlayerNumber);
        moveVector = Vector3.zero;
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
            case PlayerState.FlatWindup:
                FlatWindup();
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
            else if (racketVector.x < 0) //left
            {
                
            }
        }
        else if (racketVector.y <= TopMax) //Top Spin
        {
            if (racketVector.x > 0) //right
            {
                
            }
            else if (racketVector.x < 0) //left
            {
                
            }
        }
        else if (racketVector.y <= FlatMax) //Flat
        {
            if (racketVector.x > 0) //right
            {
                Vector3 armRotation = new Vector3(0, FlatRotation.y * Mathf.Sign(racketVector.x) * racketVector.y, 0);
                Arm.transform.rotation = Quaternion.Euler(FlatRotation);
                swingSide = 1;
                lastRacketVector = racketVector;
                //state = PlayerState.FlatWindup;
            }
            else if (racketVector.x < 0) //left
            {
                Vector3 armRotation = new Vector3(0, FlatRotation.y * Mathf.Sign(racketVector.x) * racketVector.y, 0);
                Arm.transform.rotation = Quaternion.Euler(FlatRotation);
                swingSide = -1;
                lastRacketVector = racketVector;
                //state = PlayerState.FlatWindup;
            }
        }
        else if (racketVector.y <= VolleyMax) //Volley
        {
            if (racketVector.x > 0) //right
            {
                
            }
            else if (racketVector.x < 0) //left
            {
                
            }
        }
        else //Slice
        {
            if (racketVector.x > 0) //right
            {
                
            }
            else if (racketVector.x < 0) //left
            {
                
            }
        }
    }

    public void FlatWindup()
    {
        //Racket is held back and swings forward based on the y axis of the stick
        racketVector = new Vector3(rewiredPlayer.GetAxis2DRaw("HorizontalAim", "VerticalAim").x, rewiredPlayer.GetAxis2DRaw("HorizontalAim", "VerticalAim").y, 0);
        RacketVectorDebugText.text = "Racket X = " + Math.Round(racketVector.x, 3) + "Racket Y = " + Math.Round(racketVector.y, 3);


       /* Vector3 newRotation = new Vector3(FlatRotation.x * racketVector.x, Mathf.Sign(racketVector.x) * (racketVector.y * FlatRotation.y),
            FlatRotation.z * racketVector.z);
        Arm.transform.rotation = Quaternion.Euler(newRotation);*/

        float distance = lastRacketVector.y - racketVector.y;
        Arm.transform.rotation = Quaternion.Euler(new Vector3(0, Arm.transform.rotation.y + distance, 0));

        lastRacketVector = racketVector;
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
