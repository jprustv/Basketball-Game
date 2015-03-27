using UnityEngine;
using System.Collections;

public class Basket : MonoBehaviour
{
	public GameObject score; //reference to the ScoreText gameobject, set in editor
//	public AudioClip basket; //reference to the basket sound

	private bool displayLabel = false;
	public Texture2D cestaTexture;
	
	void OnCollisionEnter() //if ball hits board
	{
		Debug.Log ("Acertou a tabela!");
	//	audio.Play(); //plays the hit board sound
	}
	
	void OnTriggerEnter() //if ball hits basket collider
	{
		string guiScore = score.guiText.text;
		int currentScore = int.Parse (guiScore)+1;
		//int currentScore = int.Parse(score.guiText.ToString()) + 1; //add 1 to the score
		score.guiText.text = currentScore.ToString ();
		Debug.Log ("CESTAA!");

		StartCoroutine (FlashLabel ());


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
		if (displayLabel == true)
		//	GUILayout.Label("CESTA");
			GUI.Label (new Rect (130, 100, cestaTexture.width, cestaTexture.height),cestaTexture);

		
	}
}