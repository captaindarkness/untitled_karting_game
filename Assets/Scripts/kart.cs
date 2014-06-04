using UnityEngine;
using System.Collections;

public class kart : MonoBehaviour 
{

	public float topSpeed;
	public float acceleration;
	public float currentSpeed;
	public int turnSpeed;
	bool isMoving;
	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		//Gas 
		if(Input.GetKey(KeyCode.W))
		{
			if (currentSpeed < topSpeed) 
			{
				//Acceleration gets added to currentSpeed
				currentSpeed += acceleration;
			}
			isMoving = true;
		}
		else
		{
			if (0 < currentSpeed) 
			{
				//Acceleration gets lowered if you let go of the gas
				currentSpeed -= acceleration;
			}
			if(currentSpeed < 0){
				currentSpeed = 0.0f;
				isMoving = false;
			}
		}
		//turn Right
		if(Input.GetKey(KeyCode.D) && currentSpeed > 0)
		{
			Quaternion newRotation = new Quaternion(transform.rotation.x,transform.rotation.y,transform.rotation.z,transform.rotation.w);;
			newRotation *= Quaternion.Euler(0, turnSpeed, 0);
			transform.rotation= Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime);
		}//turn Left
		if(Input.GetKey(KeyCode.A) && currentSpeed > 0)
		{
			Quaternion newRotation = new Quaternion(transform.rotation.x,transform.rotation.y,transform.rotation.z,transform.rotation.w);;
			newRotation *= Quaternion.Euler(0, -turnSpeed, 0);
			transform.rotation= Quaternion.Slerp(transform.rotation, newRotation, Time.deltaTime);
		}
		//Constant movement after accelerating
		if(isMoving)
		{
			transform.Translate(0, 0, currentSpeed * Time.deltaTime);

		}
	}
}
