#pragma strict
var pieceOfCake = false; //essas sÃ£o as opÃ§Ã£o q vc marca, essa indica sair do game!
var easy = false;  //essa indica ir para o primeiro level
var normal = false;
var hard = false;  //essa ir para o segundo level

   // e essa ir para o menu (colocar na opÃ§Ã£o creditos)
function OnMouseEnter() //isso significa "Ao Mouse Entrar"

{
    
//mudar a cor do texto quando encostar o mouse

	GetComponent.<Renderer>().material.color = Color.yellow;

}


function OnMouseExit() //Ao mause Sair

{
    //volta a cor normal do texto quando encostar o mouse
	GetComponent.<Renderer>().material.color = Color.white;

}


function OnMouseUp() //Ao clik do mause

{
//muda a cor
	
	GetComponent.<Renderer>().material.color = Color.green;
    //Sair do game quando clika

	var dif : GameObject;
	dif = GameObject.Find("varDifficulty");				
	
	if( pieceOfCake )	dif.GetComponent.<GUIText>().text = "1";
	if( easy )	dif.GetComponent.<GUIText>().text = "2";
	if( normal )	dif.GetComponent.<GUIText>().text = "3";		
	if( hard )	dif.GetComponent.<GUIText>().text = "4";

	DontDestroyOnLoad(dif);
	Application.LoadLevel("Gameplay");
			
}
	
	
function OnMouseDown() //Ao soltar o clik do mouse

{
    //mudar a cor do texto quando clika o mouse
	
      GetComponent.<Renderer>().material.color = Color.black;

}