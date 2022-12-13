using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class ConveyorTurn : MonoBehaviour
{

	public float speed = 2.0f;

	void FixedUpdate()
	{
		if (ScoreScript.state == ScoreScript.GameState.Running) { 
			Rigidbody rigidbody = GetComponent<Rigidbody>();
			rigidbody.position -= Vector3.Normalize(transform.forward + transform.right )* speed * Time.deltaTime;
			rigidbody.MovePosition(rigidbody.position + Vector3.Normalize(transform.forward + transform.right) * speed * Time.deltaTime);
		}
	}

}
