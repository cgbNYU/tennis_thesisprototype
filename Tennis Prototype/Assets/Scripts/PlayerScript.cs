using System.Collections;
using System.Collections.Generic;
using System.Resources;
using UnityEngine;
using Rewired;

public class PlayerScript : MonoBehaviour
{
    
    //Public
    public float Speed;
    public float DashSpeed;
    public float DashTime;
    public float DashRecoveryTime;
    public float SwingSpeed;
    public float SwingTime;
    public int PlayerNumber;
    
    //Private
    private Rewired.Player rewiredPlayer;
    private Vector3 dashVector;
    private float timer;
    
    //State
    private enum PlayerState
    {
        Neutral,
        Dashing,
        DashRecovery,
        Swinging,
        SwingRecovery
    }

    private PlayerState state;
    

    // Start is called before the first frame update
    void Start()
    {
        state = PlayerState.Neutral;
        rewiredPlayer = ReInput.players.GetPlayer(PlayerNumber);
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
            case PlayerState.Swinging:
                Swing();
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
        
    }

    private void Swing()
    {
        
    }
}
