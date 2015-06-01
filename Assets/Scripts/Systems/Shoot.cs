using UnityEngine;
using System;
using System.Collections.Generic;
public class Shoot : MonoBehaviour
{
	public List<UniMoveController> moves = new List<UniMoveController>();
	private UniMoveController currentMove;

	public GameObject ball; //reference to the ball prefab, set in editor
	private Vector3 throwSpeed = new Vector3(3, 10, 0); //This value is a sure basket, we'll modify this using the forcemeter
	private Vector3 ballPos; //starting ball position
	private bool thrown = false; //if ball has been thrown, prevents 2 or more balls
	private GameObject ballClone; //we don't use the original prefab
	
	public GameObject availableShotsGO; //ScoreText game object reference
	public int availableShots = 5;

	public GameObject meter; //references to the force meter
	public GameObject arrow;
	private float arrowSpeed; //Difficulty, higher value = faster arrow movement
	public int difficulty = 1; // 1 is easy. 2 is normal. 3 is hard
	private bool right = true; //used to revers arrow movement
	
	public GameObject gameOver; //game over text

	private GameObject Player; // Jogador

	private int throwSpeedCount =0;
	private float throwSpeedMultiplier;

//	public Camera focusCamera;

	private GameObject hoopRing;

	private float leftBorder = 0.089f;
	private float rightBorder = 0.289f;

	private bool respawnControl = false;

	private bool shootBlocked = false;

//	private GameObject TimerHUD;
//	private TimerScript timerScript;

	private GameObject varDifficulty;

	void Start()
	{
		arrow.transform.position = new Vector3(leftBorder,0.487829f,0);

		/* Increase Gravity */
		Physics.gravity = new Vector3(0, -20, 0);
		Player = GameObject.Find ("Player");

		varDifficulty = GameObject.Find ("varDifficulty");
		if (varDifficulty != null)
			difficulty = int.Parse (varDifficulty.GetComponent<GUIText> ().text);
		else
			difficulty = 3;

		switch (difficulty) {
			case 1: arrowSpeed=0.001f; break;
			case 2: arrowSpeed=0.002f; break;
			case 3: arrowSpeed=0.003f; break;
			case 4: arrowSpeed=0.004f; break;
		}

		//TimerHUD = GameObject.Find("TimerHUD");
		//timerScript = (TimerScript) TimerHUD.GetComponent(typeof(TimerScript));
		Time.maximumDeltaTime = 0.1f;
		
		int count = UniMoveController.GetNumConnected();
		
		// Iterate through all connections (USB and Bluetooth)
		for (int i = 0; i < count; i++) 
		{
			UniMoveController move = gameObject.AddComponent<UniMoveController>();	// It's a MonoBehaviour, so we can't just call a constructor
			
			// Remember to initialize!
			if (!move.Init(i))  // TENTA INICIALIZAR
			{	 // QUANDO RESTARTA O PROGRAMA ELE TENTA INICIALIZAR E NAO CONSEGUE PQ JA ESTA INICIADO
				// ENTAO ELE DESTROI AS CONEXOES ABERTAS
				// E POR ISSO DESLIGA O CONTROLE
				// === EU ACHO... === 
				Destroy(move);	// If it failed to initialize, destroy and continue on
				continue;
			}
			
			// This example program only uses Bluetooth-connected controllers
			PSMoveConnectionType conn = move.ConnectionType;
			if (conn == PSMoveConnectionType.Unknown || conn == PSMoveConnectionType.USB) 
			{
				Destroy(move);
			}
			else 
			{
				moves.Add(move);
				
				move.OnControllerDisconnected += HandleControllerDisconnected;
				
				// Start all controllers with a white LED
				move.SetLED(Color.white);
				Debug.Log ("PSMove conectado.");
			}
		}
		if (moves.Count<=0)	Debug.Log ("No Bluetooth-connected controllers found. Make sure one or more are both paired and connected to this computer.");
	}

	void FixedUpdate()
	{
		/* Move Meter Arrow */
		if (arrow.transform.position.x < rightBorder && right)
		{
			arrow.transform.position += new Vector3(arrowSpeed, 0, 0);
			throwSpeedCount++;
		}
		if (arrow.transform.position.x >= rightBorder)
		{
			right = false;
		}
		if (right == false)
		{
			arrow.transform.position -= new Vector3(arrowSpeed, 0, 0);
			throwSpeedCount--;
		}
		if (arrow.transform.position.x <= leftBorder)
		{
			right = true;
		}


		foreach (UniMoveController move in moves) {
			if (move.Disconnected) {
				currentMove = null;
				continue;
			}
			currentMove = move;
			move.SetRumble (move.Trigger);
		} 
		
		/* Shoot ball on Tap */
		//if ((Input.GetButton("Fire1") || move.GetButtonDown(PSMoveButton.Trigger)) && !thrown && availableShots > 0 && shootBlocked==false)
		bool shootButtonDown;
		if (currentMove == null)
			shootButtonDown = false;
		else
			shootButtonDown = currentMove.GetButtonDown (PSMoveButton.Trigger);
		if ((Input.GetButton("Fire1") || shootButtonDown) && !thrown && availableShots > 0 && shootBlocked==false)
		{ 
			//Debug.Log(moves[0].Acceleration.x);
			shootBlocked=true;
			arrowSpeed=0;
			thrown = true;
			availableShots--;
			//availableShotsGO.GetComponent().text = availableShots.ToString();
			availableShotsGO.GetComponent<GUIText>().text = availableShots.ToString();

			Vector3 playerPos = Player.transform.position;
			ballPos.x=playerPos.x+0.234f;
			ballPos.y=playerPos.y+1.4509f;
			ballPos.z=playerPos.z-0.0234f;
			ballClone = Instantiate(ball, ballPos, transform.rotation) as GameObject;
			respawnControl=false;

			if (throwSpeedCount*difficulty>97 && throwSpeedCount*difficulty<123) // Bem no meio
				throwSpeedMultiplier=11;
			else
				throwSpeedMultiplier=(throwSpeedCount*difficulty)/10; 

			if (throwSpeedMultiplier==12) throwSpeedMultiplier=12.5f; // BUG FIX (Gambiarra)

			hoopRing = GameObject.Find ("Ring");
			Vector3 heading = hoopRing.transform.position - Player.transform.position;
			throwSpeed.x = heading.x;
			throwSpeed.z = heading.z + Player.transform.rotation.y/1000;
			throwSpeed.y = heading.y + throwSpeedMultiplier-2.5f;

			ballClone.GetComponent<Rigidbody>().AddForce(throwSpeed, ForceMode.Impulse);

			GetComponent<AudioSource>().Play(); //play shoot sound
		}

		/* Remove Ball when it hits the floor */
		if (ballClone != null && ballClone.transform.position.y < 0.25)
		{ /* Fim da rodada */

			Destroy(ballClone,3);
			thrown = false;
			throwSpeed = new Vector3(3, 10, 0);//Reset perfect shot variable
			switch (difficulty) {
				case 1: arrowSpeed=0.001f; break;
				case 2: arrowSpeed=0.002f; break;
				case 3: arrowSpeed=0.003f; break;
				case 4: arrowSpeed=0.004f; break;
			}
			arrow.transform.position = new Vector3(leftBorder,0.487829f,0);
			right=true;
			throwSpeedCount=0;

			//Component TimerScript = GetComponent(Timer);


			Respawn ();
			StartCoroutine(enableShoot()); // Destrava o click

			/* Check if out of shots */
		


			if (availableShots == 0)
			{
				//arrow.renderer.enabled = false;
				arrow.GetComponent<GUITexture>().enabled=false;
				Invoke("restart", 2);
			}
		}

		/*if (timerScript.getCurrentTime()<=0){
			//Debug.Log ("Time Over!");
			arrow.guiTexture.enabled = false;
			//timerScript.restartTimer();
		//	TimerHUD = null;
		//	timerScript = null;
			//Destroy(TimerHUD);
			//availableShots = 0;
			timerScript.restartTimer();
			Invoke ("restart", 2);
		}*/
	}

	System.Collections.IEnumerator enableShoot() { // Destrava o click
		yield return new WaitForSeconds(3f);
		shootBlocked = false;
		StopAllCoroutines (); // O certo seria finalizar so esta thread, mas so consegui usando StopAllCoroutines()!
	}

	void Respawn(){
		if (respawnControl==false) {
			//float newX;
			float newZ;
			hoopRing = GameObject.Find ("Ring");

			// direçao para olhar
		//	Vector3 heading = hoopRing.transform.position - Player.transform.position;
			//Vector3 playerAngles = Player.transform.eulerAngles;
			Vector3 playerPosition = Player.transform.position;
			newZ = UnityEngine.Random.Range (3.0f,16.0f);
			Player.transform.position = new Vector3 (playerPosition.x, playerPosition.y, newZ);

			respawnControl=true;

	/*		Debug.Log (Player.transform.eulerAngles.y);
			Player.transform.eulerAngles = new Vector3 (playerAngles.x, playerAngles.y+(heading.z*10), playerAngles.z);
			respawnControl = true; */
		}
	}
	void HandleControllerDisconnected (object sender, EventArgs e)
	{
		// TODO: Remove this disconnected controller from the list and maybe give an update to the player
	}
	void restart()
	{
		Application.LoadLevel(Application.loadedLevel);
	}
}