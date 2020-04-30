using UnityEngine;
using System.Collections.Generic;
using System.Linq;
using System; // TODO switch to linq

internal class Brain : IComparable<Brain> {
	
	internal float fitness;
	private float[][] neurons; // [layer][neuron]
	private float[][][] weights; // [layer][neuron][previous neuron]
	
	internal Brain(int[] neuronCounts) {
		InitNeurons(neuronCounts);
		InitWeights();
	}
	
	internal Brain(Brain network) {
		InitNeurons(network.neurons.Select(i => i.Length).ToArray());
		this.weights = network.weights.Select(i => i.Select(j => j.ToArray()).ToArray()).ToArray();
	}
	
	internal float[] Eval(float[] inputs) {
		neurons[0] = inputs.Select(i => i).ToArray();
		for (int i = 1; i < neurons.Length; i++) {
			for (int j = 0; j < neurons[i].Length; j++) {
				float value = 0f;
				for (int k = 0; k < neurons[i - 1].Length; k++) {
					value += weights[i - 1][j][k] * neurons[i - 1][k];
				}
				neurons[i][j] = value / (Math.Abs(value) + 1f);
			}
		}
		return neurons[neurons.Length - 1];
	}
	
	internal void Mutate() {
		weights = weights.Select(i => i.Select(j => j.Select(k => MutateSingle(k)).ToArray()).ToArray()).ToArray();
	}
	
	private void InitNeurons(int[] neuronCounts) {
		neurons = new float[neuronCounts.Length][];
		for (int i = 0; i < neurons.Length; i++) {
			neurons[i] = new float[neuronCounts[i]];
		}
	}
	
	private void InitWeights() {
		weights = new float[neurons.Length - 1][][];
		for (int i = 1; i < neurons.Length; i++) {
			int previousCount = neurons[i - 1].Length;
			int currentCount = neurons[i].Length;
			weights[i - 1] = new float[currentCount][];
			for (int j = 0; j < currentCount; j++) {
				weights[i - 1][j] = new float[previousCount];
				for (int k = 0; k < previousCount; k++) {
					weights[i - 1][j][k] = UnityEngine.Random.Range(-1f, 1f);
				}
			}
		}
	}
	
	private float MutateSingle(float value) {
		float randomNumber = UnityEngine.Random.Range(0f, 10f);
		if (randomNumber <= 2f) value = 0f;
		else if (randomNumber <= 4f) value = UnityEngine.Random.Range(-1f, 1f);
		else if (randomNumber <= 6f) value *= UnityEngine.Random.Range(0f, 1f);
		else if (randomNumber <= 8f) value *= UnityEngine.Random.Range(1f, 2f);
		else if (randomNumber <= 10f) value = -value;
		return value;
	}
	
	public int CompareTo(Brain other) {
		return Math.Sign(other.fitness - fitness);
	}
}
