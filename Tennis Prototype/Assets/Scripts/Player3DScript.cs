using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Rewired;
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

    public GameObject Racket;
    
    //Private
    private Rewired.Player rewiredPlayer;
    private Vector3 moveVector;
    private Vector3 racketVector;
    
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
        Walk();
        RacketMove();
    }
    
    private void Walk()
    {
        //Check for walking
        moveVector = new Vector3(rewiredPlayer.GetAxis2DRaw("Horizontal", "Vertical").x, 0, rewiredPlayer.GetAxis2DRaw("Horizontal", "Vertical").y);

        transform.position += Vector3.ClampMagnitude(moveVector, 1) * Speed * Time.deltaTime;
    }

    private void RacketMove()
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
    }
}
