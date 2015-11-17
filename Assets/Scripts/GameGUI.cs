using UnityEngine;
using System.Collections;


public class GameGUI : MonoBehaviour {
	
	public static GameGUI SP;

	// CONSTANTE TURBO ELEMENT
	public static int turboElement;
	public static int maxTurboElement=0;
	public Texture2D barreTurboElement;
	
	
	void Awake()
	{
		SP = this;
	}
	
	void OnGUI()
	{
		GUILayout.Space(3);

		// BARRE POUR L'AFFICHAGE DU TURBO ELEMENT
		GUI.DrawTexture(new Rect(350,380,barreTurboElement.width * turboElement / 200, 20), barreTurboElement);
	}
}
