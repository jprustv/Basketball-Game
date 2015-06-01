using UnityEngine;
using System.Collections;

public class HUDController : MonoBehaviour {

	public GUIText Score;
	public GUIText BallsLeftText;
	public GUIText ScoreText;
	public GUITexture Logomarca;
	public GUIText BallsLeft;

	// Use this for initialization
	void Start () {
		int scWidth = Screen.width;

		if (scWidth > 1200) {
			Score.transform.position = new Vector3 (Score.transform.position.x-0.05f, Score.transform.position.y, Score.transform.position.z);
			Score.fontSize = 80;
			ScoreText.transform.position = new Vector3 (ScoreText.transform.position.x-0.02f, ScoreText.transform.position.y, ScoreText.transform.position.z);
			ScoreText.fontSize = 140;
			BallsLeft.transform.position = new Vector3 (BallsLeft.transform.position.x-0.05f, BallsLeft.transform.position.y, BallsLeft.transform.position.z);
			BallsLeft.fontSize = 80;
			BallsLeftText.transform.position = new Vector3 (BallsLeftText.transform.position.x-0.02f, BallsLeftText.transform.position.y, BallsLeftText.transform.position.z);
			BallsLeftText.fontSize = 140;
		}

			//(Screen.height / defaultScreenSize) * defaultFontSize

	//	GUI.Label (new Rect (scWidth/2.6f, scHeigth/7, scWidth/4, scHeigth/4), msgTexture);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
