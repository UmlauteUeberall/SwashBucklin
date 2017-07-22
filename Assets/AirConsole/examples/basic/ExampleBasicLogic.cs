using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using NDream.AirConsole;
using Newtonsoft.Json.Linq;

public class ExampleBasicLogic : MonoBehaviour {

	public GameObject logo;
	public Renderer profilePicturePlaneRenderer;
	private bool turnLeft;
	private bool turnRight;

	private float highScore = 0;

#if !DISABLE_AIRCONSOLE
	void Awake () {
		// register events
		AirConsole.instance.onReady += OnReady;
		AirConsole.instance.onMessage += OnMessage;
		AirConsole.instance.onConnect += OnConnect;
		AirConsole.instance.onDisconnect += OnDisconnect;
		AirConsole.instance.onDeviceStateChange += OnDeviceStateChange;
		AirConsole.instance.onCustomDeviceStateChange += OnCustomDeviceStateChange;
		AirConsole.instance.onDeviceProfileChange += OnDeviceProfileChange;
		AirConsole.instance.onAdShow += OnAdShow;
		AirConsole.instance.onAdComplete += OnAdComplete;
		AirConsole.instance.onGameEnd += OnGameEnd;
		AirConsole.instance.onHighScores += OnHighScores;
		AirConsole.instance.onHighScoreStored += OnHighScoreStored;
		AirConsole.instance.onPersistentDataStored += OnPersistentDataStored;
		AirConsole.instance.onPersistentDataLoaded += OnPersistentDataLoaded;
		AirConsole.instance.onPremium += OnPremium;
	}

	void OnReady (string code) {
	}

	void OnMessage (int from, JToken data) {
		// Rotate the AirConsole Logo to the right
		if ((string)data == "left") {
			turnLeft = true;
			turnRight = false;
		}

		// Rotate the AirConsole Logo to the right
		if ((string)data == "right") {
			turnLeft = false;
			turnRight = true;
		}

		// Stop rotating the AirConsole Logo
		//'stop' is sent when a button on the controller is released
		if ((string)data == "stop") {
			turnLeft = false;
			turnRight = false;
		}

		//Show an Ad
		if ((string)data == "show_ad") {
			AirConsole.instance.ShowAd ();
		}
	}

	void OnConnect (int device_id) {
		//Log to on-screen Console
	}

	void OnDisconnect (int device_id) {
		//Log to on-screen Console
	}

	void OnDeviceStateChange (int device_id, JToken data) {
		//Log to on-screen Console
	}

	void OnCustomDeviceStateChange (int device_id, JToken custom_data) {
		//Log to on-screen Console
	}

	void OnDeviceProfileChange (int device_id) {
		//Log to on-screen Console
	}

	void OnAdShow () {
		//Log to on-screen Console
	}

	void OnAdComplete (bool adWasShown) {
		//Log to on-screen Console
	}

	void OnGameEnd () {
		Debug.Log ("OnGameEnd is called");
		Camera.main.enabled = false;
		Time.timeScale = 0.0f;
	}

	void OnHighScores (JToken highscores) {
		//Log to on-screen Console
		//logWindow.text = logWindow.text.Insert (0, "Converted Highscores: " + HighScoreHelper.ConvertHighScoresToTables(highscores).ToString() + " \n \n");
	}

	void OnHighScoreStored (JToken highscore) {
		//Log to on-screen Console
		if (highscore == null) {
		} else {
		}		
	}

	void OnPersistentDataStored (string uid) {
		//Log to on-screen Console
	}

	void OnPersistentDataLoaded (JToken data) {
		//Log to on-screen Console
	}

	void OnPremium(int device_id){
		//Log to on-screen Console
	}

	void Update () {
		//If any controller is pressing a 'Rotate' button, rotate the AirConsole Logo in the scene
		if (turnLeft) {
			this.logo.transform.Rotate (0, 0, 2);
		
		} else if (turnRight) {
			this.logo.transform.Rotate (0, 0, -2);
		}
	}

	public void SendMessageToController1 () {
		//Say Hi to the first controller in the GetControllerDeviceIds List.

		//We cannot assume that the first controller's device ID is '1', because device 1 
		//might have left and now the first controller in the list has a different ID.
		//Never hardcode device IDs!
		int idOfFirstController = AirConsole.instance.GetControllerDeviceIds () [0];

		AirConsole.instance.Message (idOfFirstController, "Hey there, first controller!");

		//Log to on-screen Console
	}

	public void BroadcastMessageToAllDevices () {
		AirConsole.instance.Broadcast ("Hey everyone!");
	}

	public void DisplayDeviceID () {
		//Get the device id of this device
		int device_id = AirConsole.instance.GetDeviceId ();

		//Log to on-screen Console		
	}

	public void DisplayNicknameOfFirstController () {
		//We cannot assume that the first controller's device ID is '1', because device 1 
		//might have left and now the first controller in the list has a different ID.
		//Never hardcode device IDs!		
		int idOfFirstController = AirConsole.instance.GetControllerDeviceIds () [0];

		//To get the controller's name right, we get their nickname by using the device id we just saved
		string nicknameOfFirstController = AirConsole.instance.GetNickname (idOfFirstController);

		//Log to on-screen Console
		
	}

	private IEnumerator DisplayUrlPicture (string url) {
		// Start a download of the given URL
		WWW www = new WWW (url);
		
		// Wait for download to complete
		yield return www;
		
		// assign texture
		profilePicturePlaneRenderer.material.mainTexture = www.texture;
		Color color = Color.white;
		color.a = 1;
		profilePicturePlaneRenderer.material.color = color;

		yield return new WaitForSeconds (3.0f);

		color.a = 0;
		profilePicturePlaneRenderer.material.color = color;
		
	}
	
	public void DisplayProfilePictureOfFirstController () {
		//We cannot assume that the first controller's device ID is '1', because device 1 
		//might have left and now the first controller in the list has a different ID.
		//Never hardcode device IDs!		
		int idOfFirstController = AirConsole.instance.GetControllerDeviceIds () [0];
	
		string urlOfProfilePic = AirConsole.instance.GetProfilePicture (idOfFirstController, 512);

		//Log url to on-screen Console
		StartCoroutine (DisplayUrlPicture (urlOfProfilePic));
	}

	public void DisplayAllCustomDataOfFirstController () {
		//We cannot assume that the first controller's device ID is '1', because device 1 
		//might have left and now the first controller in the list has a different ID.
		//Never hardcode device IDs!		
		int idOfFirstController = AirConsole.instance.GetControllerDeviceIds () [0];

		//Get the Custom Device State of the first Controller
		JToken data = AirConsole.instance.GetCustomDeviceState (idOfFirstController);
		
		if (data != null) {
			
			// Check if data has multiple properties
			if (data.HasValues) {
				
				// go through all properties
				foreach (var prop in ((JObject)data).Properties()) {
					}

			} else {
				//If there's only one property, log it to on-screen Console
				
			}
		} else {
		}
	}

	public void DisplayCustomPropertyHealthOnFirstController () {
		//We cannot assume that the first controller's device ID is '1', because device 1 
		//might have left and now the first controller in the list has a different ID.
		//Never hardcode device IDs!		
		int idOfFirstController = AirConsole.instance.GetControllerDeviceIds () [0];
		
		//Get the Custom Device State of the first Controller
		JToken data = AirConsole.instance.GetCustomDeviceState (idOfFirstController);

		//If it exists, get the data's health property and cast it as int
		if (data != null && data ["health"] != null) {
			int healthOfFirstController = (int)data ["health"];
		} else {
			
		}
	}

	public void SetSomeCustomDataOnScreen () {
		//create some data
		var customData = new { 
			players = AirConsole.instance.GetControllerDeviceIds ().Count,
			started = false,
		};

		//Set that Data as this device's Custom Device State (this device is the Screen)
		AirConsole.instance.SetCustomDeviceState (customData);

		//Log url to on-screen Console
	}

	public void SetLevelPropertyInCustomScreenData () {
		//Set a property 'level' in this devie's custom data (this device is the Screen)
		AirConsole.instance.SetCustomDeviceStateProperty ("level", 1);
	}

	public void DisplayAllCustomDataFromScreen () {
		//The screen always has device id 0. That is the only device id you're allowed to hardcode.
		if (AirConsole.instance.GetCustomDeviceState (0) != null) {


			// Show json string of entries
			foreach (JToken key in AirConsole.instance.GetCustomDeviceState(0).Children()) {
			}
		}
	}

	public void DisplayNumberOfConnectedControllers () {
		//This does not count devices that have been connected and then left,
		//only devices that are still active
		int numberOfActiveControllers = AirConsole.instance.GetControllerDeviceIds ().Count;
	}

	public void SetActivePlayers () {
		//Set the currently connected devices as the active players (assigning them a player number)
		AirConsole.instance.SetActivePlayers ();

		string activePlayerIds = "";
		foreach (int id in AirConsole.instance.GetActivePlayerDeviceIds) {
			activePlayerIds += id + "\n";
		}

		//Log to on-screen Console
	}

	public void DisplayDeviceIDOfPlayerOne () {

		int device_id = AirConsole.instance.ConvertPlayerNumberToDeviceId (0);

		//Log to on-screen Console
		if (device_id != -1) {
		} else {
		}
	}

	public void DisplayServerTime () {
		//Get the Server Time
		float time = AirConsole.instance.GetServerTime ();
		
		//Log to on-screen Console
	}

	public void DisplayIfFirstContrllerIsLoggedIn () {
		//Get the Device Id
		int idOfFirstController = AirConsole.instance.GetControllerDeviceIds () [0];

		bool firstPlayerLoginStatus = AirConsole.instance.IsUserLoggedIn (idOfFirstController);
		
		//Log to on-screen Console
	}

	public void HideDefaultUI () {
		//Hide the Default UI in the Browser Window
		AirConsole.instance.ShowDefaultUI (false);

		//Log to on-screen Console
	}

	public void ShowDefaultUI () {
		//Show the Default UI in the Browser Window
		AirConsole.instance.ShowDefaultUI (true);

		//Log to on-screen Console
	}

	public void NavigateHome () {
		//Navigate back to the AirConsole store
		AirConsole.instance.NavigateHome ();

		//Log to on-screen Console
	}

	public void NavigateToPong () {
		//Navigate to another game
		AirConsole.instance.NavigateTo ("http://games.airconsole.com/pong/");
	}

	public void ShowAd () {
		//Display an Advertisement
		AirConsole.instance.ShowAd ();
		//Log to on-screen Console
	}

	public void IncreaseScore () {
		//increase current score and show on ui
		highScore += 1;
	}

	public void ResetScore () {
		//reset current score and show on ui
		highScore = 0;
	}

	public void RequestHighScores () {
		List <string> ranks = new List<string> ();
		ranks.Add ("world");
		AirConsole.instance.RequestHighScores ("Basic Example", "v1.0", null, ranks, 5, 3);
	}

	public void StoreHighScore () {
		JObject testData = new JObject();
		testData.Add ("test", "data");
		AirConsole.instance.StoreHighScore ("Basic Example", "v1.0", highScore, AirConsole.instance.GetUID(AirConsole.instance.GetMasterControllerDeviceId()), testData);
	}

	public void StoreTeamHighScore () {
		List<string> connectedUids = new List<string> ();
		List<int> deviceIds = AirConsole.instance.GetControllerDeviceIds();

		for (int i = 0; i < deviceIds.Count; i++) {
			connectedUids.Add (AirConsole.instance.GetUID(deviceIds[i]));
		}
		AirConsole.instance.StoreHighScore ("Basic Example", "v1.0", highScore, connectedUids);
	}

	public void StorePersistentData () {
		//Store test data for the master controller
		JObject testData = new JObject();
		testData.Add ("test", "data");
		AirConsole.instance.StorePersistentData("custom_data", testData, AirConsole.instance.GetUID(AirConsole.instance.GetMasterControllerDeviceId()));
	}

	public void RequestPersistentData () {
		List<string> connectedUids = new List<string> ();
		List<int> deviceIds = AirConsole.instance.GetControllerDeviceIds();
		
		for (int i = 0; i < deviceIds.Count; i++) {
			connectedUids.Add (AirConsole.instance.GetUID(deviceIds[i]));
		}
		AirConsole.instance.RequestPersistentData (connectedUids);
	}

	public void IsMasterPremium(){
		bool masterIsPremium = AirConsole.instance.IsPremium (AirConsole.instance.GetMasterControllerDeviceId ());

	}

	public void ShowPremiumDeviceIDs () {

		List<int> premiumDevices = AirConsole.instance.GetPremiumDeviceIds ();

		if (premiumDevices.Count > 0) {
			foreach (int deviceId in premiumDevices){
			}
		} else {
			//Log to on-screen Console
		}

	}

	void OnDestroy () {

		// unregister events
		if (AirConsole.instance != null) {
			AirConsole.instance.onReady -= OnReady;
			AirConsole.instance.onMessage -= OnMessage;
			AirConsole.instance.onConnect -= OnConnect;
			AirConsole.instance.onDisconnect -= OnDisconnect;
			AirConsole.instance.onDeviceStateChange -= OnDeviceStateChange;
			AirConsole.instance.onCustomDeviceStateChange -= OnCustomDeviceStateChange;
			AirConsole.instance.onAdShow -= OnAdShow;
			AirConsole.instance.onAdComplete -= OnAdComplete;
			AirConsole.instance.onGameEnd -= OnGameEnd;
			AirConsole.instance.onHighScores -= OnHighScores;
			AirConsole.instance.onHighScoreStored -= OnHighScoreStored;
			AirConsole.instance.onPersistentDataStored -= OnPersistentDataStored;
			AirConsole.instance.onPersistentDataLoaded -= OnPersistentDataLoaded;
			AirConsole.instance.onPremium -= OnPremium;
		}
	}
#endif
}

