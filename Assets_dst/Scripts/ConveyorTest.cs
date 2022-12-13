using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ConveyorTest : MonoBehaviour
{

	public float speed = 2.0f;

	void FixedUpdate()
	{
		Rigidbody rigidbody = GetComponent<Rigidbody>();
		rigidbody.position -= transform.forward * speed * Time.deltaTime;
		rigidbody.MovePosition(rigidbody.position + transform.forward * speed * Time.deltaTime);
	}

}
