using UnityEngine;
using System.Collections;

public class IKControl : MonoBehaviour {
	
	public Transform[] bones;
	public Transform targetPos;
	public IKSolver solver;
	public bool SolveNow=false;
	public Vector3 targetVector;
	
	// Use this for initialization
	void Start () {
		//bones = this.GetComponentsInChildren<Transform>();
		solver = new IKCCD2D();
	}

	// Update is called once per frame
	void Update () {
	
		if(SolveNow)
		{
			Debug.Log(targetVector);
			// Solve the inverse kinematics
			solver.Solve( bones, targetVector );
		}
	}
}
