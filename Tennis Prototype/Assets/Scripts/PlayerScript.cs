using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using System.Resources;
using UnityEngine;
using Rewired;
using DG.Tweening;
using TMPro;
using Quaternion = UnityEngine.Quaternion;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

public class PlayerScript : MonoBehaviour
{
    
    //Public
    public float Speed;
    public float DashSpeed;
    public float DashTime;
    public float DashRecoveryTime;
    public float SwingSpeed;
    public float RacketRotateSpeed;
    public Vector3 RacketMinScale;
    public Vector3 RacketMaxScale;
    public float RacketScaleSpeed;
    public float SwingTime;
    public float ReleaseSensitivity;
    public int PlayerNumber;
    public GameObject ForeHandPivot;
    public GameObject BackHandPivot;
    public GameObject leftRacket;
    public GameObject rightRacket;
    
    //Private
    private Rewired.Player rewiredPlayer;
    private Vector3 dashVector;
    private float timer;
    private float swingStrength;
    private bool swinging;
    
    

    private enum SwingSpin
    {
        TopSpin,
        Slice,
        Lob,
        
    }

    private SwingSpin spin;

    private enum SwingType
    {
        Forehand,
        Backhand,
        Overhead
    }

    private SwingType type;
    
    //State
    private enum PlayerState
    {
        Neutral,
        Dashing,
        DashRecovery,
        SwingWindup,
        Swinging,
        SwingRecovery
    }

    private PlayerState state;
    

    // Start is called before the first frame update
    void Start()
    {
        state = PlayerState.Neutral;
        rewiredPlayer = ReInput.players.GetPlayer(PlayerNumber);
        RacketMinScale = leftRacket.transform.localScale;
        swinging = false;
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case PlayerState.Neutral:
                
                Walk();
                DashCheck();
                SwingCheck();
                break;
            case PlayerState.Dashing:
                Dash();
                break;
            case PlayerState.DashRecovery:
                DashRecovery();
                break;
            case PlayerState.SwingWindup:
                SwingRelease();
                break;
            case PlayerState.Swinging:
                Swing();
                break;
            case PlayerState.SwingRecovery:
                break;
            default:
                break;
        }
    }
    
    private void Walk()
    {
        //Check for walking
        Vector3 moveVector = new Vector3(rewiredPlayer.GetAxis2DRaw("Horizontal", "Vertical").x, rewiredPlayer.GetAxis2DRaw("Horizontal", "Vertical").y, 0);

        transform.position += moveVector.normalized * Speed * Time.deltaTime;
    }

    private void DashCheck()
    {
        if (rewiredPlayer.GetButtonDown("Dash") && rewiredPlayer.GetAxis2DRaw("Horizontal", "Vertical") != Vector2.zero)
        {
            dashVector = new Vector3(rewiredPlayer.GetAxis2DRaw("Horizontal", "Vertical").x, rewiredPlayer.GetAxis2DRaw("Horizontal", "Vertical").y, 0);
            state = PlayerState.Dashing;
        }
    }

    private void Dash()
    {
        transform.position += dashVector.normalized * DashSpeed * Time.deltaTime;
        timer += Time.deltaTime;
        if (timer >= DashTime)
        {
            dashVector = Vector3.zero;
            timer = 0;
            state = PlayerState.DashRecovery;
        }
    }

    private void DashRecovery()
    {
        timer += Time.deltaTime;
        if (timer >= DashRecoveryTime)
        {
            timer = 0;
            state = PlayerState.Neutral;
        }
    }

    private void SwingCheck()
    {
        if (rewiredPlayer.GetButtonDown("LeftWindup"))
        {
            //forehand
            type = SwingType.Forehand;
            ForeHandPivot.SetActive(true);
            state = PlayerState.SwingWindup;
        }
        else if (rewiredPlayer.GetButtonDown("RightWindup"))
        {
            //backhand
            type = SwingType.Backhand;
            BackHandPivot.SetActive(true);
            state = PlayerState.SwingWindup;
        }
    }

    private void SwingRelease()
    {
        Debug.Log(type);
        if (type == SwingType.Forehand)
        {
            leftRacket.transform.Rotate(Vector3.forward * RacketRotateSpeed * Time.deltaTime);
            if (rewiredPlayer.GetButtonUp("LeftWindup"))
            {
                //left swing
                swinging = true;
                state = PlayerState.Swinging;
            }
        }        
        else if (type == SwingType.Backhand)
        {
            rightRacket.transform.Rotate(-Vector3.forward * RacketRotateSpeed * Time.deltaTime);
            if (rewiredPlayer.GetButtonUp("RightWindup"))
            {
                //left swing
                swinging = true;
                state = PlayerState.Swinging;
            }
        }   
    }

    private void Swing()
    {
        if (type == SwingType.Forehand && swinging)
        {
            Vector3 scaleUp = new Vector3(leftRacket.transform.localScale.x + RacketScaleSpeed * Time.deltaTime,
                leftRacket.transform.localScale.y + RacketScaleSpeed * Time.deltaTime,
                leftRacket.transform.localScale.z + RacketScaleSpeed * Time.deltaTime);

            leftRacket.transform.localScale = scaleUp;
            if (leftRacket.transform.localScale.x >= RacketMaxScale.x)
            {
                swinging = false;
            }
        }else if (type == SwingType.Forehand && !swinging)
        {
            Vector3 scaleDown = new Vector3(leftRacket.transform.localScale.x - RacketScaleSpeed * Time.deltaTime,
                leftRacket.transform.localScale.y - RacketScaleSpeed * Time.deltaTime,
                leftRacket.transform.localScale.z - RacketScaleSpeed * Time.deltaTime);
            
            leftRacket.transform.localScale = scaleDown;
            if (leftRacket.transform.localScale.x <= RacketMinScale.x)
            {
                SwingReset();
            }
        }
        
        if (type == SwingType.Backhand && swinging)
        {
            Vector3 scaleUp = new Vector3(rightRacket.transform.localScale.x + RacketScaleSpeed * Time.deltaTime,
                rightRacket.transform.localScale.y + RacketScaleSpeed * Time.deltaTime,
                rightRacket.transform.localScale.z + RacketScaleSpeed * Time.deltaTime);

            rightRacket.transform.localScale = scaleUp;
            if (rightRacket.transform.localScale.x >= RacketMaxScale.x)
            {
                swinging = false;
            }
        }else if (type == SwingType.Backhand && !swinging)
        {
            Vector3 scaleDown = new Vector3(rightRacket.transform.localScale.x - RacketScaleSpeed * Time.deltaTime,
                rightRacket.transform.localScale.y - RacketScaleSpeed * Time.deltaTime,
                rightRacket.transform.localScale.z - RacketScaleSpeed * Time.deltaTime);
            
            rightRacket.transform.localScale = scaleDown;
            if (rightRacket.transform.localScale.x <= RacketMinScale.x)
            {
                SwingReset();
            }
        }
    }

    private void SwingReset()
    {
        leftRacket.transform.localScale = RacketMinScale;
        ForeHandPivot.SetActive(false);
        rightRacket.transform.localScale = RacketMinScale;
        BackHandPivot.SetActive(false);
        swinging = false;
        state = PlayerState.Neutral;
    }
    
    #region Backup Functions
    /*private void SwingRelease()
    {
        if (type == SwingType.Forehand)
        {
            //Debug.Log("Swing strength = " + rewiredPlayer.GetAxis("Forehand"));
            float strength = rewiredPlayer.GetAxis("Forehand");
            ForeHandPivot.transform.localRotation = Quaternion.Euler(new Vector3(0, 0, 75 * strength));
            if (rewiredPlayer.GetAxisDelta("Forehand") <= -ReleaseSensitivity)
            {
                swingStrength = strength;
                state = PlayerState.Swinging;
            }
        }
    }*/
    
    /*private void Swing()
    {
        if (type == SwingType.Forehand)
        {
            Tween swingTween = ForeHandPivot.transform.DORotate(new Vector3(0, 0, 75), SwingSpeed * swingStrength);
            swingTween.SetEase(Ease.OutCirc);
            swingTween.Play();
            swingTween.OnComplete(SwingReset);
        }
    }*/
    
    /*private void SwingReset()
    {
        ForeHandPivot.transform.rotation = Quaternion.Euler(Vector3.zero);
        ForeHandPivot.SetActive(false);
        BackHandPivot.transform.rotation = Quaternion.Euler(Vector3.zero);
        BackHandPivot.SetActive(false);
        state = PlayerState.Neutral;
        Debug.Log("Neutral");
    }*/
    #endregion
}
