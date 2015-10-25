﻿using UnityEngine;
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
		GUI.DrawTexture(new Rect(10,10,barreTurboElement.width * turboElement / 100, barreTurboElement.height), barreTurboElement);
	}
}
