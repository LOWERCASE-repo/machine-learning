using UnityEngine;
using System.Collections;
using System.Collections.Generic;

internal class Trainer : MonoBehaviour {
	
	[SerializeField]
	private Swarmling swarmlingFab;
	[SerializeField]
	private Transform target;
	
	private int popSize = 50;
	private List<Brain> brains;
	private List<Swarmling> swarmlings;
	
	private void Start() {
		int[] brainStruct = new int[] { 1, 5, 5, 1 };
		brains = new List<Brain>();
		swarmlings = new List<Swarmling>();
		for (int i = 0; i < popSize; i++) {
			Brain brain = new Brain(brainStruct);
			brain.Mutate();
			brains.Add(brain);
			Swarmling swarmling = Instantiate(swarmlingFab, Vector3.zero, Quaternion.identity);
			swarmling.brain = brain;
			swarmling.target = target;
			swarmlings.Add(swarmling);
		}
		StartCoroutine(Evolve());
	}
	
	private IEnumerator Evolve() {
		target.transform.position = Random.insideUnitCircle * 10f;
		yield return new WaitForSecondsRealtime(10f);
		brains.Sort();
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
		}
		StartCoroutine(Evolve());
	}
}
