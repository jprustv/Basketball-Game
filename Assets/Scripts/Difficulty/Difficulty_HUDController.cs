using UnityEngine;
using System.Collections;

public class Difficulty_HUDController : MonoBehaviour {

	public GUIText ChooseDifficulty;
	// Use this for initialization
	void Start () {
		int scWidth = Screen.width;
		if (scWidth > 1200)	ChooseDifficulty.fontSize = 140;
		else if (scWidth > 1500) ChooseDifficulty.fontSize = 250;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
