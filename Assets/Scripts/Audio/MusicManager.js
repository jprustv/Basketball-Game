#pragma strict

var NewMusic: AudioClip; //Pick an audio track to play.

function Awake () { 
	var go = GameObject.Find("MusicPlayer"); 
	//Finds the game object called MusicPlayer, if it goes by a different name, change this. 
	if (go.GetComponent.<AudioSource>().clip.name != NewMusic.name || go.GetComponent.<AudioSource>().isPlaying==false){
		go.GetComponent.<AudioSource>().clip = NewMusic; //Replaces the old audio with the new one set in the inspector. 
		go.GetComponent.<AudioSource>().Play(); //Plays the audio. 
	}
}