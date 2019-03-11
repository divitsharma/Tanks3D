using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMotor : MonoBehaviour {

    Rigidbody rb;

    [SerializeField]
    float manualRotation = 0;

    [SerializeField]
    float maxSpeed = 0; // units per s
    [SerializeField]
    public float lerpFactor;

    float sSinceUpdate = 0;
    Vector3 velocity;
    float nextRotation;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        velocity = Vector3.zero;
        nextRotation = -transform.rotation.eulerAngles.y;
	}

    void FixedUpdate()
    {
        //Debug.Log(nextRotation);
        //RotateTo(manualRotation);

        sSinceUpdate += Time.deltaTime;
        // unclamped, so will keep going forever when no data is received
        rb.MovePosition(rb.position + (velocity * Time.deltaTime));
        float rotation = Mathf.LerpAngle(rb.rotation.eulerAngles.y, -nextRotation, lerpFactor * sSinceUpdate);//needs to change with time!
        rb.rotation = Quaternion.Euler(0f, rotation, 0f);
    }

    //public void Move(ClientUpdateMessage message)
    //{
    //    if (message.relativeRotation)
    //        RelativeRotateTo(message.rotateTo);
    //    else
    //        RotateTo(message.rotateTo);

    //    AddVelocity(message.speedScale);
    //}


    public void RotateTo(float rotateTo)
    {
        nextRotation = rotateTo;
    }

    void RelativeRotateTo(float rotateTo)
    {
        //rb.rotation.eulerAngles = new Vector3(0f, Mathf.LerpAngle(rb.rotation.eulerAngles.y, rb.rotation.eulerAngles.y + rotateTo, lerpFactor * Time.deltaTime), 0f);
    }

    public void AddVelocity(float speedScale)
    {
        sSinceUpdate = 0;
        velocity = transform.forward.normalized * maxSpeed * speedScale;
        // next position to be in after 1 second
        //nextPosition = transform.position + velocity;
        
    }

}
