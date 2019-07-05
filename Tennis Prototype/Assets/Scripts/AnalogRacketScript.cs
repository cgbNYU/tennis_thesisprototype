using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Rewired;

public class AnalogRacketScript : MonoBehaviour
{
    //Public
    public int PlayerNumber;
    
    //Private
    private Vector3 moveVector;
    private Rewired.Player rewiredPlayer;
    
    // Start is called before the first frame update
    void Start()
    {
        rewiredPlayer = ReInput.players.GetPlayer(PlayerNumber);
        moveVector = Vector3.zero;
    }

    // Update is called once per frame
    void Update()
    {
        RacketMove();
    }

    public void RacketMove()
    {
        
    }
}
