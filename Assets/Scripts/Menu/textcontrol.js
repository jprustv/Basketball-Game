#pragma strict
var isQuitButton = false; //essas sÃ£o as opÃ§Ã£o q vc marca, essa indica sair do game!
var playgame = false;  //essa indica ir para o primeiro level
var chooseDifficulty = false;
var creditos = false;  //essa ir para o segundo level

var menu = false;    // e essa ir para o menu (colocar na opÃ§Ã£o creditos)


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

var musicPlayer : GameObject;
musicPlayer = GameObject.Find("MusicPlayer");				
DontDestroyOnLoad(musicPlayer);	
			
if( isQuitButton )
	
{
	//executa o comando sair do game
	
         Application.Quit();

}
else
{
    //carrega o jogo
	
    if( playgame )
	{
	
       Application.LoadLevel("Difficulty"); //Esse carrega o level 1, ou seja a primeira faze
	
   }
	
}
	
if( creditos )
	{
	
        Application.LoadLevel(2); //Esse carrega o level 2, q Ã© os creditos
	
}
	
if( menu )
	{
	
     Application.LoadLevel(0); //Esse volta ao menu, Ã© o level 0
	
}

if (chooseDifficulty)
{
	Application.LoadLevel(0);	
}
			
}
	
	
function OnMouseDown() //Ao soltar o clik do mouse

{
    //mudar a cor do texto quando clika o mouse
	
      GetComponent.<Renderer>().material.color = Color.black;

}