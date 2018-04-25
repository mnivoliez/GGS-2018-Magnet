using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using CotcSdk;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour {

	private Cloud Cloud;

	private GameState gameState;
	// Input field
	public InputField EmailInput;
	public InputField PasswordInput;
	// Default parameters
	private const string DefaultEmailAddress = "me@localhost.localdomain";
	private const string DefaultPassword = "Pass1234";

	// Use this for initialization
	void Start() {
		gameState = GameState.GetInstance();
		// Link with the CotC Game Object
		var cb = FindObjectOfType<CotcGameObject>();
		if (cb == null) {
			Debug.LogError("Please put a Clan of the Cloud prefab in your scene!");
			return;
		}
		// Log unhandled exceptions (.Done block without .Catch -- not called if there is any .Then)
		Promise.UnhandledException += (object sender, ExceptionEventArgs e) => {
			Debug.LogError("Unhandled exception: " + e.Exception.ToString());
		};
		// Initiate getting the main Cloud object
		cb.GetCloud().Done(cloud => {
			Cloud = cloud;
			// Retry failed HTTP requests once
			Cloud.HttpRequestFailedHandler = (HttpRequestFailedEventArgs e) => {
				if (e.UserData == null) {
					e.UserData = new object();
					e.RetryIn(1000);
				}
				else
					e.Abort();
			};
			Debug.Log("Setup done");
		});
		// Use a default text in the e-mail address
		EmailInput.text = DefaultEmailAddress;
		PasswordInput.text = DefaultPassword;
	}
	
	// Log in by e-mail
	public void DoLoginEmail() {
		// You may also not provide a .Catch handler and use .Done instead of .Then. In that
		// case the Promise.UnhandledException handler will be called instead of the .Done
		// block if the call fails.
		Cloud.Login(
			network: LoginNetwork.Email.Describe(),
			networkId: EmailInput.text,
			networkSecret: PasswordInput.text)
		.Done(this.DidLogin);
	}

	// Invoked when any sign in operation has completed
	private void DidLogin(Gamer newGamer) {
		if (gameState.Gamer != null) {
			Debug.LogWarning("Current gamer " + gameState.Gamer.GamerId + " has been dismissed");
			gameState.Loop.Stop();
		}
		gameState.Gamer = newGamer;
		gameState.Loop = gameState.Gamer.StartEventLoop();
		gameState.Loop.ReceivedEvent += Loop_ReceivedEvent;
		Debug.Log("Signed in successfully (ID = " + gameState.Gamer.GamerId + ")");
		LoadMainMenu();
	}

	private void Loop_ReceivedEvent(DomainEventLoop sender, EventLoopArgs e) {
		Debug.Log("Received event of type " + e.Message.Type + ": " + e.Message.ToJson());
	}

	private void LoadMainMenu()
	{
		SceneManager.LoadScene("MainMenu");
	}

	public void QuitGame() {
		Application.Quit();
	}
}
