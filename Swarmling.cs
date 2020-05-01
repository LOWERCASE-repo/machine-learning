using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class Swarmling : MonoBehaviour {
	
	[SerializeField]
	internal Rigidbody2D rb;
	
	internal Rigidbody2D target;
	internal Brain brain;
	
	private void FixedUpdate() {
		Vector2 dir = (target.position - (Vector2)transform.position);
		dir = dir.normalized;
		float[] output = brain.Eval(dir.x, dir.y);
		rb.velocity = new Vector2(output[0], output[1]) * 10f;
		brain.fitness -= (target.position - (Vector2)transform.position).sqrMagnitude;
	}
}
