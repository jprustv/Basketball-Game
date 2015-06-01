using UnityEngine;
using System.Collections;

public class TimerScript : MonoBehaviour {
	public float startTime;
	private string textTime;
	public float currentTime=1f;

	// Use this for initialization
	void Start () {
		currentTime = startTime;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public float getCurrentTime(){
		return currentTime;
	}

	public bool timeOver(){

		if (currentTime <= 0) return true;
		else return false;
	}

	public void restartTimer(){
		currentTime = startTime;
		Debug.Log (currentTime);
	}
	
	void OnGUI () {
		if (!timeOver ()){
			float guiTime = startTime - Time.time;
			currentTime = guiTime;
			/*if (currentTime <= 0) {
				currentTime = 0;
				return;
			}*/
			//int minutes = (int)guiTime / 60;
			int seconds = (int)guiTime % 60;
			int fraction = (int)(guiTime * 100) % 100;

			string textTime = string.Format ("{0:00}:{1:00}", seconds, fraction);
			this.GetComponent<GUIText>().text = textTime;

			int scWidth = Screen.width;
		//	int scHeigth = Screen.height;

			if (scWidth > 800) {
				this.GetComponent<GUIText>().fontSize = 90;
				this.transform.position = new Vector3 (0.39f, 0.95f, 1);
			} else {
				this.transform.position = new Vector3 (0.415f, 0.95f, 1);
			}
		}
	}

}
