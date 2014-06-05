using UnityEngine;
using System.Collections;

public class kart : MonoBehaviour
{
    public float topSpeed;
    public float acceleration;
    public float currentSpeed;
    public int turnSpeed;
    bool isMovingForward;
    private float lastSynchronizationTime = 0f;
    private float syncDelay = 0f;
    private float syncTime = 0f;
    private Vector3 syncStartPosition = Vector3.zero;
    private Vector3 syncEndPosition = Vector3.zero;

    void OnSerializeNetworkView(BitStream stream, NetworkMessageInfo info)
    {
        Vector3 syncPosition = Vector3.zero;
        Vector3 syncVelocity = Vector3.zero;
        if (stream.isWriting)
        {
            syncPosition = rigidbody.position;
            stream.Serialize(ref syncPosition);

            syncPosition = rigidbody.velocity;
            stream.Serialize(ref syncVelocity);
        }
        else
        {
            stream.Serialize(ref syncPosition);
            stream.Serialize(ref syncVelocity);

            syncTime = 0f;
            syncDelay = Time.time - lastSynchronizationTime;
            lastSynchronizationTime = Time.time;

            syncEndPosition = syncPosition + syncVelocity * syncDelay;
            syncStartPosition = rigidbody.position;
        }
    }

    void Awake()
    {
        lastSynchronizationTime = Time.time;

    }

    void Update()
    {
        if (networkView.isMine)
        {
            InputMovement();
            InputColorChange();
        }
        else
        {
            SyncedMovement();
        }
    }


    private void InputMovement()
    {
        //Gas 
        if (Input.GetKey(KeyCode.W))
        {
            if (currentSpeed < topSpeed)
            {
                //Acceleration gets added to currentSpeed
                currentSpeed += acceleration;
            }
            isMovingForward = true;
        }
        else
        {
            if (0 < currentSpeed)
            {
                //Acceleration gets lowered if you let go of the gas
                currentSpeed -= acceleration;
            }
            if (currentSpeed < 0)
            {
                currentSpeed = 0.0f;
				isMovingForward = false;
            }
        }
        //turn Right
        if (Input.GetKey(KeyCode.D) && currentSpeed > 0)
        {
            Quaternion newRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w); ;
            newRotation *= Quaternion.Euler(0, turnSpeed, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime);
        }//turn Left
        if (Input.GetKey(KeyCode.A) && currentSpeed > 0)
        {
            Quaternion newRotation = new Quaternion(transform.rotation.x, transform.rotation.y, transform.rotation.z, transform.rotation.w); ;
            newRotation *= Quaternion.Euler(0, -turnSpeed, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime);
        }
        //Constant movement after accelerating
		if (isMovingForward)
        {
            transform.Translate(0, 0, currentSpeed * Time.deltaTime);

        }
    }

    private void SyncedMovement()
    {
        syncTime += Time.deltaTime;

        rigidbody.position = Vector3.Lerp(syncStartPosition, syncEndPosition, syncTime / syncDelay);
    }


    private void InputColorChange()
    {
        if (Input.GetKeyDown(KeyCode.R))
            ChangeColorTo(new Vector3(Random.Range(0f, 1f), Random.Range(0f, 1f), Random.Range(0f, 1f)));
    }

    [RPC]
    void ChangeColorTo(Vector3 color)
    {
        renderer.material.color = new Color(color.x, color.y, color.z, 1f);

        if (networkView.isMine)
            networkView.RPC("ChangeColorTo", RPCMode.OthersBuffered, color);
    }
}
