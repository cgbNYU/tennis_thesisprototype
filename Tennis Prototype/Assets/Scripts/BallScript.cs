using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    //Public
    public float MaxSpeed;
    public float Gravity;
    public float Bounce;
    public float RacketBounce;
    
    //Private
    private Vector3 direction;

    private float speed;

    private float height;

    private GameObject player1;

    private GameObject player2;
    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
