using UnityEngine;
using System.Collections;

public class Basket : MonoBehaviour
{
	public GameObject score; //reference to the ScoreText gameobject, set in editor
//	public AudioClip basket; //reference to the basket sound

	private bool displayLabel = false;
	public Texture2D cestaTexture;
	public Texture2D quaseTexture;
	private Texture2D msgTexture;
	private bool scoreControl = false;
	
	void OnCollisionEnter() //if ball hits board
	{
		Debug.Log ("Acertou a tabela!");
		msgTexture = quaseTexture;

		StartCoroutine (FlashLabel ());
	//	audio.Play(); //plays the hit board sound
	}
	
	void OnTriggerEnter() //if ball hits basket collider
	{
		if (scoreControl == false) {
			string guiScore = score.GetComponent<GUIText>().text;
			int currentScore = int.Parse (guiScore) + 1;
			//int currentScore = int.Parse(score.guiText.ToString()) + 1; //add 1 to the score
			score.GetComponent<GUIText>().text = currentScore.ToString ();
			Debug.Log ("CESTAA!");
			msgTexture = cestaTexture;

			StartCoroutine (FlashLabel ());
		//	scoreControl = true;
		}


	//	AudioSource.PlayClipAtPoint(basket, transform.position); //play basket sound
	}

	IEnumerator FlashLabel() {
		// Fancy pants flash of label on and off   
		int i = 0;
		while (i<3) {
			displayLabel = true;
			yield return new WaitForSeconds(0.5f);
			displayLabel = false;
			yield return new WaitForSeconds(0.5f);
			i++;
		}
	}
	
	void OnGUI() {
		if (displayLabel == true) {
			//GUI.Label (new Rect (120, -1, cestaTexture.width-12, cestaTexture.height-12), cestaTexture);
			int scWidth = Screen.width;
			int scHeigth = Screen.height;
			GUI.Label (new Rect (scWidth/2.6f, scHeigth/7, scWidth/4, scHeigth/4), msgTexture);


		}
		
	}
}