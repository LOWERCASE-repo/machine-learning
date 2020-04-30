using UnityEngine;
using System.Collections.Generic;

internal class Trainer : MonoBehaviour {
	
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
	
	
}
