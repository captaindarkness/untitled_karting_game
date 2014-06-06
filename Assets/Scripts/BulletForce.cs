using UnityEngine;
using System.Collections;

public class BulletForce : MonoBehaviour {

	public float power;
	public float lifeSpan;

	// Use this for initialization
	void Start () {
		power = 10.0f;
	}
	
	// Update is called once per frame
	void Update () {
		transform.Translate(0, 0, power * Time.deltaTime);

		Destroy(gameObject, lifeSpan);
	}
}
