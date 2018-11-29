using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.Networking.NetworkSystem;
using UnityEngine.SceneManagement;

public static class gameParametersContainer {
    public static gameParameters gameParam;
} 

//Should be declared identically to the gameParameters class in serverController.
public class gameParameters : MessageBase
{
    public int param;
    public string respawnTime;
    public string[] input;
    public string inputForHint;
    public int[] bitMapSubString1, bitMapSubString2, bitMapSubString3;
    public int[] No_of_blanks;
    public int[] SpawnInterval;
    public bool[] AlphabetsFaceUser;
    public bool[] spin;
    public bool[] repeatSolution;
    public int FlyingSpeed;
    public int RotationSpeed;

    // private enum EHeightLevel{Crouch, Waist, Chest, Head, VerticalReach};
    // private float[] HeightCalibrationData = { 0, 0, 0, 0, 0 };
    // public float CrouchHeight { get { return HeightCalibrationData[(int)EHeightLevel.Crouch]; } private set { } }
    // public float WaistHeight { get { return HeightCalibrationData[(int)EHeightLevel.Waist]; } private set { } }
    // public float ChestHeight { get { return HeightCalibrationData[(int)EHeightLevel.Chest]; } private set { } }
    // public float HeadHeight { get { return HeightCalibrationData[(int)EHeightLevel.Head]; } private set { } }
    // public float VerticalReachHeight { get { return HeightCalibrationData[(int)EHeightLevel.VerticalReach]; } private set { } }

    // private enum EReachability { Close, ArmsLength, OutOfReach };
    // private float[] ReachabilityCalibrationData = { 0, 0, 0 };
    // public float CloseReachability { get { return ReachabilityCalibrationData[(int)EReachability.Close]; } private set { } }
    // public float ArmsLengthReachability { get { return ReachabilityCalibrationData[(int)EReachability.ArmsLength]; } private set { } }
    // public float OutOfReachReachability { get { return ReachabilityCalibrationData[(int)EReachability.OutOfReach]; } private set { } }
}

public class clientController : MonoBehaviour {

	NetworkClient myClient;
    public string serverAddress = "127.0.0.1";
    public static short MSG_GAME_PARAMETERS_START = 1005;
	public static short MSG_GAME_PARAMETERS_UPDATE = 1006;
    float time = 0;

	// Use this for initialization
	void Start () {
        myClient = new NetworkClient();
        myClient.RegisterHandler(MsgType.Connect, OnConnected);
        myClient.RegisterHandler(MSG_GAME_PARAMETERS_START, startGame);
        myClient.RegisterHandler(MSG_GAME_PARAMETERS_UPDATE, updateToGame);
        myClient.RegisterHandler(MsgType.Disconnect, OnDisconnected);
        myClient.Connect(serverAddress, 4444);
        Debug.Log("Attempting to connect to server...");
        //ServerDiscovery sD = new ServerDiscovery();
        //sD.Initialize();
        //sD.StartAsClient();

        //myClient.Connect("10.122.196.176", 4444);
    }

    // Update is called once per frame
    void Update () {
		if (!myClient.isConnected)
        {
            ////Try reconnecting every second if not connected.
            //time += Time.deltaTime;
            //if(time > 10)
            //{
            //    time = 0;
            //    myClient.Connect("127.0.0.1", 4444);
            //    Debug.Log("Attempting to connect to server..."); ;
            //}
            
        }
	}

	public void OnConnected(NetworkMessage netMsg)
    {
        Debug.Log("Connected to server.");
    }
    public void OnDisconnected(NetworkMessage netMsg)
    {
        //ErrorMessage error = netMsg.ReadMessage<ErrorMessage>();
        Debug.Log("Connection error.");
        myClient.Connect(serverAddress, 4444);
        Debug.Log("Attempting to connect to server again..."); ;
    }

    public void startGame(NetworkMessage netMsg)
    {
        Debug.Log("Received Start Message.");
    	gameParameters msg = netMsg.ReadMessage<gameParameters>();
        //Save parameters.
        gameParametersContainer.gameParam = msg;
        //Start the game (container box generation and spawner).
        GameObject.Find("ContainerBox").GetComponent<ContainerGenerator>().start = true;
        //Start the hint generator
        GameObject.Find("Hint").GetComponent<HintGenerator>().start = true;
        GameObject.Find("TimerController").GetComponent<Countdown>().gameStart = true;
    }
    public void updateToGame(NetworkMessage netMsg)
    {
        //TODO: Updating game parameters after the game has started.
    }


}
public class ServerDiscovery : NetworkDiscovery
{
    public override void OnReceivedBroadcast(string fromAddress, string data)
    {
        Debug.Log("Server Found at " + fromAddress);
    }
}
