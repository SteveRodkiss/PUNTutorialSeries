using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class NetworkManager : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
        {
            Connect();
        }
         
    }
    private void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void Connect()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public void Play()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    //clicked by a button with dynamic variable for what team to join
    public void JoinTeam(int team)
    {
        //do we already have a team?
        if (PhotonNetwork.LocalPlayer.CustomProperties.ContainsKey("Team"))
        {
            //we already have a team- so switch teams
            PhotonNetwork.LocalPlayer.CustomProperties["Team"] = team;
        }
        else
        {
            //we dont have a team yet- create the custom property and set it
            //0 for blue, 1 for red
            //set the player properties of this client to the team they clicked
            ExitGames.Client.Photon.Hashtable playerProps = new ExitGames.Client.Photon.Hashtable
        {
            { "Team", team }
        };
            //set the property of Team to the value the user wants
            PhotonNetwork.SetPlayerCustomProperties(playerProps);
        }
 
        //join the random room and launch game- the GameManager will spawn the correct model in based on the property
        PhotonNetwork.JoinRandomRoom();
    }



    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        Debug.Log("Tried to join a room and failed");
        //most likely no room
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 4 });
    }

    public override void OnJoinedRoom()
    {
        Debug.Log("joined a room- yay!");
        //maybe allocate a team here?
 

        if (PhotonNetwork.IsMasterClient)
        {
            //load the level- this syncs with all clients
            PhotonNetwork.LoadLevel(1);
        }
    }

 

}
