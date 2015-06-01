#pragma strict
var bola: GameObject;
var p:float;
function Start () {
 p = 5.2;
}

function Update () {
	bola.transform.position.z = bola.transform.position.z -0.3;
	bola.transform.Rotate(0,40*Time.deltaTime,0);
}