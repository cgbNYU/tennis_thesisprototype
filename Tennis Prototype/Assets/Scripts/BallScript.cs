using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallScript : MonoBehaviour
{
    //Public
    public float MaxSpeed;
    public float Gravity;
    public float RacketBounce;
    public float GroundBounce;
    public float WallBounce;
    public Transform LeftSpawn;
    public Transform RightSpawn;
    
    //Private
    private Vector3 direction;

    private float speed;

    private float height;

    private float verticalVelocity;

    private GameObject player1;

    private GameObject player2;

    private bool secondBounce;

    private bool bouncing;
    
    // Start is called before the first frame update
    void Start()
    {
        player1 = GameObject.Find("Player1");
        player2 = GameObject.Find("Player2");
        height = 3f;
        bouncing = false;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
        //Drop();

        if (Input.GetKeyDown(KeyCode.Q))
        {
            transform.position = LeftSpawn.position;
            direction = Vector3.zero;
            speed = 0;
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            transform.position = RightSpawn.position;
            direction = Vector3.zero;
            speed = 0;
        }
    }

    private void Move()
    {
        transform.position += direction * speed * Time.deltaTime;
    }

    private void Drop()
    {
        //Ball steadily drops over time
        verticalVelocity -= Gravity * Time.deltaTime;
        height += verticalVelocity * Time.deltaTime;
        
        Debug.Log("height = " + height);

        //If it hits the ground/0 it bounces
        if (height <= 0 && !bouncing)
        {
            Bounce();
        }
        else if (bouncing && height > 0)
            bouncing = false;
    }

    private void Bounce()
    {
        //Ball checks to see if this is the second bounce
        if (secondBounce)
        {
            Debug.Log("Point!");
            Point();
        }
        
        //If not, bounces back to a certain height and adjusts spin/speed/vector accordingly
        else
        {
            secondBounce = true;
            verticalVelocity += Mathf.Abs(verticalVelocity) * GroundBounce;
            Debug.Log("Bounce!");
        }
    }

    private void Point()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Racket"))
        {
            //grab info from the racket
            PlayerScript racketInfo = other.GetComponentInParent<PlayerScript>();
            
            //set direction based on what direction is held
            if (racketInfo.moveVector.normalized == Vector3.zero)
            {
                direction = -direction;
            }
            else
                direction = racketInfo.moveVector.normalized;
            
            
            //set speed based on racket size
            if (racketInfo.swinging)
            {
                speed = MaxSpeed * racketInfo.gameObject.transform.localScale.x;
            }
            else
            {
                speed = MaxSpeed / 5;
            }
            

            //add to height
            verticalVelocity += RacketBounce;
        }
    }
}
