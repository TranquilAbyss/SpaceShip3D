using UnityEngine;
using System.Collections;

public class PlayerMovement : MonoBehaviour {
	public float movementSpeed = 1;

	public Vector3 minBoundary;
	public Vector3 maxBoundary;


	// Use this for initialization
	void Start () {
		
	
	}
	
	// Update is called once per frame
	void Update () {

		//keyboard movement
		if(Input.GetKey(KeyCode.W))
			gameObject.transform.Translate(movementSpeed * Vector3.forward * Time.deltaTime);
		if(Input.GetKey(KeyCode.S))
			gameObject.transform.Translate(movementSpeed * Vector3.back * Time.deltaTime);
		if(Input.GetKey(KeyCode.A))
			gameObject.transform.Translate(movementSpeed * Vector3.left * Time.deltaTime);
		if(Input.GetKey(KeyCode.D))
			gameObject.transform.Translate(movementSpeed * Vector3.right * Time.deltaTime);
		if(Input.GetKey(KeyCode.Space))
			gameObject.transform.Translate(movementSpeed * Vector3.up * Time.deltaTime);
		if(Input.GetKey(KeyCode.LeftShift))
			gameObject.transform.Translate(movementSpeed * Vector3.down * Time.deltaTime);

		transform.position = new Vector3 
		(
			Mathf.Clamp(transform.position.x,minBoundary.x,maxBoundary.x),
			Mathf.Clamp(transform.position.y,minBoundary.y,maxBoundary.y),
			Mathf.Clamp(transform.position.z,minBoundary.z,maxBoundary.z)
		);
		
	}
}
