using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Dot : MonoBehaviour {
  
  [SerializeField]
  private float speed;
  [SerializeField]
  private float acc;
  
  [HideInInspector]
  public Vector2[] mvmts;
  private int step;
  private bool won;
  
  [Header("Components")]
  [SerializeField]
  private Rigidbody2D rb;
  
  [Header("Debug")]
  public Transform target;
  
  public Vector2[] GetMvmts() {
    Vector2[] mvmts = new Vector2[this.mvmts.Length];
    System.Array.Copy(this.mvmts, mvmts, this.mvmts.Length);
    return mvmts;
  }
  
  public float Eval() {
    if (!won) {
      float dist = ((Vector2)target.position - rb.position).sqrMagnitude;
      return 1f / dist;
    } else {
      return 100000f / (step * step);
    }
  }
  
  public void Mutate() {
    for (int i = 0; i < mvmts.Length; i++) {
      if (Random.value < 0.03f) {
        mvmts[i] = Random.insideUnitCircle;
      }
    }
  }
  
  public void Reset(Vector2[] mvmts) {
    this.mvmts = mvmts;
    rb.position = transform.parent.position;
    rb.velocity = Vector2.zero;
    step = 0;
    won = false;
  }
  
  private void InitMvmts() {
    mvmts = new Vector2[400];
    for (int i = 0; i < mvmts.Length; i++) {
      mvmts[i] = Random.insideUnitCircle;
    }
  }
  
  private void Start() {
    rb.drag = acc / speed;
    InitMvmts();
  }
  
  private void FixedUpdate() {
    if (step < mvmts.Length && !won) {
      rb.AddForce(mvmts[step++] * acc);
    } else {
      // Debug.Log(Eval());
    }
  }
}