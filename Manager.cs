using UnityEngine;
using System.Collections.Generic;

internal class Manager : MonoBehaviour {
	
	[SerializeField]
	private Swarmling swarmling;
	[SerializeField]
	private GameObject hex;
	
	private bool isTraining = false;
	private int populationSize = 50;
	private int generationNumber = 0;
	private int[] layers = new int[] { 1, 5, 5, 1 };
	private List<Brain> brains;
	private List<Swarmling> swarmlingList;
	
	private void Timer() {
		isTraining = false;
	}
	
	private void Update () {
		if (isTraining == false) {
			if (generationNumber == 0) {
				InitSwarmlingBrains();
			} else {
				brains.Sort();
				for (int i = 0; i < populationSize / 2; i++) {
					brains[i] = new Brain(brains[i+(populationSize / 2)]);
					brains[i].Mutate();
					brains[i + (populationSize / 2)] = new Brain(brains[i + (populationSize / 2)]); //too lazy to write a reset neuron matrix values method....so just going to make a deepcopy lol
				}
				for (int i = 0; i < populationSize; i++) {
					brains[i].fitness = 0f;
				}
			}
			generationNumber++;
			isTraining = true;
			Invoke("Timer", 10f);
			CreateSwarmlingBodies();
		}
		
		if (Input.GetMouseButtonDown(0)) {
			hex.transform.position = (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition);
		}
	}
	
	private void CreateSwarmlingBodies() {
		if (swarmlingList != null) {
			for (int i = 0; i < swarmlingList.Count; i++) {
				GameObject.Destroy(swarmlingList[i].gameObject);
			}
		}
		
		swarmlingList = new List<Swarmling>();
		for (int i = 0; i < populationSize; i++) {
			Swarmling boomer = Instantiate(swarmling, Vector3.zero, Quaternion.identity);
			boomer.Init(brains[i],hex.transform);
			swarmlingList.Add(boomer);
		}
	}
	
	private void InitSwarmlingBrains() {
		if (populationSize % 2 != 0) populationSize = 20;
		brains = new List<Brain>();
		for (int i = 0; i < populationSize; i++) {
			Brain net = new Brain(layers);
			net.Mutate();
			brains.Add(net);
		}
	}
}
