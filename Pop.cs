using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Pop : MonoBehaviour {
  
  [SerializeField]
  private int size;
  [SerializeField]
  private Transform target;
  
  private Dot[] dots;
  
  [SerializeField]
  private Dot dot;
  
  private float Eval() {
    float fitness = 0f;
    for (int i = 0; i < size; i++) {
      fitness += dots[i].Eval();
    }
    return fitness;
  }
  
  private float fitness;
  private Dot GetParent() {
    float choice = Random.value * fitness;
    float sum = 0f;
    for (int i = 0; i < size; i++) {
      sum += dots[i].Eval();
      if (choice < sum) {
        return dots[i];
      }
    }
    return null;
  }
  
  private void Evolve() {
    fitness = Eval();
    Dot[] nextGen = new Dot[size];
    for (int i = 0; i < size; i++) {
      nextGen[i] = GetParent();
    }
    for (int i = 0; i < size; i++) {
      dots[i].Reset(nextGen[i].GetMvmts());
    }
  }
  
  private void Start() {
    dots = new Dot[size];
    for (int i = 0; i < size; i++) {
      dots[i] = Instantiate(dot, transform.position, Quaternion.identity, transform);
      dots[i].target = target;
    }
  }
  
  private int step;
  private void FixedUpdate() {
    step++;
    if (step == 400) {
      Evolve();
      for (int i = 0; i < size; i++) {
        dots[i].Mutate();
      }
      step = 0;
    }
  }
}