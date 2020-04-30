using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Swarmling : MonoBehaviour {
	private bool initialized = false;
	
	private Transform hex;
	private Brain net;
	
	[SerializeField]
	private Rigidbody2D rb;
	private Material[] mats;
	
	internal void Init(Brain net, Transform hex) {
		this.hex = hex;
		this.net = net;
		initialized = true;
	}
	
	private void FixedUpdate () {
		if (initialized == true) {
			float distance = Vector2.Distance(transform.position, hex.position);
			if (distance > 20f)
			distance = 20f;
			
			float angle = transform.eulerAngles.z % 360f;
			if (angle < 0f)
			angle += 360f;
			
			Vector2 deltaVector = (hex.position - transform.position).normalized;
			
			float rad = Mathf.Atan2(deltaVector.y, deltaVector.x);
			rad *= Mathf.Rad2Deg;
			
			rad = rad % 360;
			if (rad < 0) rad = 360 + rad;
			rad = 90f - rad;
			if (rad < 0f) rad += 360f;
			rad = 360 - rad;
			rad -= angle;
			if (rad < 0) rad = 360 + rad;
			if (rad >= 180f) {
				rad = 360 - rad;
				rad *= -1f;
			}
			rad *= Mathf.Deg2Rad / Mathf.PI;
			
			float[] output = net.Eval(new float[] {rad});
			// float[] output = net.Eval(new float[] {transform.rotation.eulerAngles.z, deltaVector.x, deltaVector.y});
			rb.velocity = 5f * transform.up;
			rb.angularVelocity = 500f * output[0];
			// net.fitness += (1f - Mathf.Abs(inputs[0]));
			net.fitness -= (hex.position - transform.position).sqrMagnitude;
		}
	}
}
