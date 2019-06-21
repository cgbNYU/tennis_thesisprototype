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
    public float GroundBounce;
    public float WallBounce;
    
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
        Move();
    }

    private void Move()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Racket"))
        {
            Debug.Log("HIT!");
            //grab info from the racket
            PlayerScript racketInfo = other.GetComponentInParent<PlayerScript>();
            
            //set direction based on what direction is held
            direction = racketInfo.moveVector.normalized;
            Debug.Log("Direction = " + direction);
            
            //set speed based on racket size
            speed = MaxSpeed * racketInfo.gameObject.transform.localScale.x;
            Debug.Log("Speed = " + speed);

            //add to height
        }
    }
}
