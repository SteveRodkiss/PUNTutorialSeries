using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using UnityEngine.UI;

public class GameManager : MonoBehaviourPunCallbacks
{

    public GameObject redPlayerPrefab;
    public GameObject bluePlayerPrefab;
    public GameObject pauseCanvas;
    public bool paused = false;

    public static GameManager instance;
    public static int blueScore = 0;
    public static int redScore = 0;
    public Text blueScoreText;
    public Text redScoreText;


    private void Start()
    {
        //set paused state
        SetPaused();

        //check that we dont have a local instance before we instantiate the prefab
        if (NetworkPlayerManager.localPlayerInstance == null)
        {
            //instantiate the correct player based on the team
            int team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
            Debug.Log($"Team number {team} is being instantiated");
            //instantiate the blue player if team is 0 and red if it is not
            if (team == 0)
            {
                //get a spawn for the correct team
                Transform spawn = SpawnManager.instance.GetTeamSpawn(0);
                PhotonNetwork.Instantiate(bluePlayerPrefab.name, spawn.position, spawn.rotation);
            }
            else
            {
                //now for the red team
                Transform spawn = SpawnManager.instance.GetTeamSpawn(1);
                PhotonNetwork.Instantiate(redPlayerPrefab.name, spawn.position, spawn.rotation);
            }
            
        }
    }

    private void Awake()
    {
        SetScoreText();
        instance = this;
    }

    void SetScoreText()
    {
        redScoreText.text = redScore.ToString();
        blueScoreText.text = blueScore.ToString();
    }


    public void Quit()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        SceneManager.LoadScene(0);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            paused = !paused;
            SetPaused();
        }
        SetScoreText();
    }

    void SetPaused()
    {
        //set the canvas
        pauseCanvas.SetActive(paused);
        //set the cursoro lock
        Cursor.lockState = paused ? CursorLockMode.None : CursorLockMode.Locked;
        //set the cursoro visible
        Cursor.visible = paused;
    }

    //reload the level when anyone leaves or joins?- That is done in the demo but is it needed?
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        ReloadLevel();
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        ReloadLevel();
    }

    public void ReloadLevel()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            Debug.Log("Reloading Level");
            PhotonNetwork.LoadLevel(1);
        }
    }


}
