using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CotcSdk;

public class GameState {

	private static GameState INSTANCE;
	// The gamer is the base to perform most operations. A gamer object is obtained after successfully signing in.
	public Gamer Gamer;
	// When a gamer is logged in, the loop is launched for domain private. Only one is run at once.
	public DomainEventLoop Loop;

	private GameState() {}

	public static GameState GetInstance() {
		if(INSTANCE == null) {
			INSTANCE = new GameState();
		}
		return INSTANCE;
	}
}
