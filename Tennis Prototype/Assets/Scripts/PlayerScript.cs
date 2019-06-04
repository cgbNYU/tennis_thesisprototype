using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class PlayerScript : MonoBehaviour
{
    
    //Public
    public float Speed;
    public int PlayerNumber;
    
    //Private
    private Rewired.Player rewiredPlayer;
    
    //State
    private enum PlayerState
    {
        Idle,
        Walking,
        Dashing,
        Swinging
    }

    private PlayerState state;
    

    // Start is called before the first frame update
    void Start()
    {
        state = PlayerState.Idle;
        rewiredPlayer = ReInput.players.GetPlayer(PlayerNumber);
    }

    // Update is called once per frame
    void Update()
    {
        switch (state)
        {
            case PlayerState.Idle:
                Idle();
                break;
            case PlayerState.Walking:
                Walk();
                break;
            case PlayerState.Dashing:
                Dash();
                break;
            case PlayerState.Swinging:
                Swing();
                break;
            default:
                break;
        }
    }

    private void Idle()
    {
        //Check for walking
        
        //Check for swinging
    }
    
    private void Walk()
    {
        //Check for walking
        
        //Check for swinging
        
        //check for dashing
    }

    private void Dash()
    {
        
    }

    private void Swing()
    {
        
    }
}
