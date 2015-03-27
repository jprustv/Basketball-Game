using UnityEngine;
using System.Collections;

public class Shoot : MonoBehaviour
{
	public GameObject ball; //reference to the ball prefab, set in editor
	private Vector3 throwSpeed = new Vector3(3, 10, 0); //This value is a sure basket, we'll modify this using the forcemeter
	private Vector3 ballPos; //starting ball position
	private bool thrown = false; //if ball has been thrown, prevents 2 or more balls
	private GameObject ballClone; //we don't use the original prefab
	
	public GameObject availableShotsGO; //ScoreText game object reference
	private int availableShots = 5;

	public GameObject meter; //references to the force meter
	public GameObject arrow;
	private float arrowSpeed; //Difficulty, higher value = faster arrow movement
	public int difficulty = 1; // 1 is easy. 2 is normal. 3 is hard
	private bool right = true; //used to revers arrow movement
	
	public GameObject gameOver; //game over text

	private GameObject Player; // Jogador

	private int throwSpeedCount =0;
	private float throwSpeedMultiplier;

	void Start()
	{
		/* Increase Gravity */
		Physics.gravity = new Vector3(0, -20, 0);
		Player = GameObject.Find ("Player");
		switch (difficulty) {
			case 1: arrowSpeed=0.001f; break;
			case 2: arrowSpeed=0.002f; break;
			case 3: arrowSpeed=0.003f; break;
		}

	}

	void FixedUpdate()
	{
		/* Move Meter Arrow */

		if (arrow.transform.position.x < 0.1587393 && right)
		{
			arrow.transform.position += new Vector3(arrowSpeed, 0, 0);
			throwSpeedCount++;
		}
		if (arrow.transform.position.x >= 0.1587393)
		{
			right = false;
		}
		if (right == false)
		{
			arrow.transform.position -= new Vector3(arrowSpeed, 0, 0);
			throwSpeedCount--;
		}
		if ( arrow.transform.position.x <= -0.05735791)
		{
			right = true;
		}

		/* Shoot ball on Tap */
		
		if (Input.GetButton("Fire1") && !thrown && availableShots > 0)
		{
			arrowSpeed=0;
			thrown = true;
			availableShots--;
			//availableShotsGO.GetComponent().text = availableShots.ToString();
			availableShotsGO.guiText.text = availableShots.ToString();

			Vector3 playerPos = Player.transform.position;
			ballPos.x=playerPos.x+0.234f;
			ballPos.y=playerPos.y+1.4509f;
			ballPos.z=playerPos.z-0.0234f;
			ballClone = Instantiate(ball, ballPos, transform.rotation) as GameObject;

			// Acerta o multiplicador para o meio da force bar
			if (throwSpeedCount*difficulty>=90 && throwSpeedCount*difficulty<=130)
				if (throwSpeedCount*difficulty>102 && throwSpeedCount*difficulty<118) // Bem no meio
					throwSpeedMultiplier=500;
				else
					throwSpeedMultiplier=throwSpeedCount*3.55f;
			// Acerta o multiplicador para antes do meio
			else if(throwSpeedCount*difficulty<100)
				throwSpeedMultiplier=(throwSpeedCount*difficulty)*0.4f;
				if (throwSpeedCount*difficulty<15) // Muito fraco
					throwSpeedMultiplier-=400;
				else if(throwSpeedCount*difficulty<40) // Fraquinho
					throwSpeedMultiplier-=200;
			// Acerta o multiplicador para depois do meio
			else if(throwSpeedCount*difficulty>120)
				throwSpeedMultiplier=(throwSpeedCount*difficulty)*5;
				if (throwSpeedCount*difficulty>205) // Muito forte
					throwSpeedMultiplier+=400;
				else if(throwSpeedCount*difficulty>180) // Meio forte
					throwSpeedMultiplier+=200;

		//	Debug.Log ((throwSpeedCount*difficulty).ToString());
		//	Debug.Log(throwSpeedMultiplier.ToString());

			throwSpeed.y = throwSpeed.y + throwSpeedMultiplier/80 +5;

	//		throwSpeed.z = Random.Range (-1,1);
			
			ballClone.rigidbody.AddForce(throwSpeed, ForceMode.Impulse);

			audio.Play(); //play shoot sound

		}

		/* Remove Ball when it hits the floor */
		
		if (ballClone != null && ballClone.transform.position.y < 0.25)
		{

			Destroy(ballClone);
			thrown = false;
			throwSpeed = new Vector3(3, 10, 0);//Reset perfect shot variable
			switch (difficulty) {
				case 1: arrowSpeed=0.001f; break;
				case 2: arrowSpeed=0.002f; break;
				case 3: arrowSpeed=0.003f; break;
			}
			arrow.transform.position = new Vector3(-0.05735791f,0.411829f,0);
			right=true;
			throwSpeedCount=0;

			/* Check if out of shots */
			
		if (availableShots == 0)
			{
				//arrow.renderer.enabled = false;
				arrow.guiTexture.enabled=false;
				Invoke("restart", 2);
			}
		}

	}

	void restart()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}