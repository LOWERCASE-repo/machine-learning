using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

internal class Trainer : MonoBehaviour {
	
	[SerializeField]
	private Swarmling swarmlingFab;
	[SerializeField]
	private Rigidbody2D target;
	
	private int popSize = 100;
	private List<Brain> brains;
	private List<Swarmling> swarmlings;
	
	private void Start() {
		brains = new List<Brain>();
		swarmlings = new List<Swarmling>();
		for (int i = 0; i < popSize; i++) {
			Brain brain = new Brain(2, 5, 5, 2);
			brain.Mutate();
			brains.Add(brain);
			Swarmling swarmling = Instantiate(swarmlingFab, Vector3.zero, Quaternion.AngleAxis(Random.value * 360f, Vector3.forward));
			swarmling.brain = brain;
			swarmling.target = target;
			swarmlings.Add(swarmling);
		}
		StartCoroutine(Evolve());
	}
	
	private IEnumerator Evolve() {
		target.position = Random.insideUnitCircle * 10f;
		target.velocity = Random.insideUnitCircle * 3f;
		yield return new WaitForSecondsRealtime(6f);
		brains = brains.OrderByDescending(i => i.fitness).ToList();
		int third = popSize / 3;
		for (int i = third; i < popSize - third; i++) {
			brains[i + third] = new Brain(brains[i]);
			brains[i] = new Brain(brains[i - third]);
		}
		for (int i = 0; i < popSize; i++) {
			swarmlings[i].brain = brains[i];
		}
		foreach (Swarmling swarmling in swarmlings) {
			swarmling.transform.position = Vector3.zero;
			swarmling.rb.velocity = Vector2.zero;
		}
		StartCoroutine(Evolve());
	}
}
