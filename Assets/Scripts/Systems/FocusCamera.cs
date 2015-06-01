using UnityEngine;
using System.Collections;

public class FocusCamera : MonoBehaviour {
	public Camera SSCamera;
	private GameObject SSPanel;
	private Texture2D screenShot;
	private bool displaySS=false;
	private int imWidth = 240;
	private int imHeight = 180;

	void OnTriggerEnter() //if ball hits basket collider
	{
		int scWidth = Screen.width;
		//int scHeigth = Screen.height;
		if (scWidth > 800) {
			imWidth = 320;
			imHeight = 240;
		}
		else if (scWidth > 1200) {
			imWidth = 400;
			imHeight = 300;
		}
		RenderTexture rt = new RenderTexture(imWidth,imHeight,24);
		SSCamera.targetTexture = rt;
		screenShot = new Texture2D(imWidth,imHeight,TextureFormat.RGB24, false);
		SSCamera.Render ();
		RenderTexture.active = rt;
		screenShot.ReadPixels(new Rect(0,0, imWidth,imHeight), 0, 0);
		SSCamera.targetTexture=null;
		RenderTexture.active=null;
		Destroy(rt);

		byte[] bytes = screenShot.EncodeToPNG();
		screenShot.LoadImage (bytes);

		StartCoroutine (ShowSS ());

	}

	IEnumerator ShowSS() {
		// Fancy pants flash of label on and off   
		displaySS = true;
		yield return new WaitForSeconds(3);
		displaySS = false;
	}

	void OnGUI() {
		if (displaySS == true) {
			int scWidth = Screen.width;
			int scHeigth = Screen.height;
			if (scWidth > 400)
				GUI.Label (new Rect (scWidth/2.75f, scHeigth/3, scWidth/2.5f, scHeigth/2.5f), screenShot);
			else if (scWidth > 800)
				GUI.Label (new Rect (scWidth/3, scHeigth/3, scWidth/1.5f, scHeigth/1.5f), screenShot);
			else if (scWidth > 1200)
				GUI.Label (new Rect (scWidth/3.5f, scHeigth/3, scWidth/2.5f, scHeigth/2.5f), screenShot);
		}
	}

}
