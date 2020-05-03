using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

public class GameManager : MonoBehaviourPunCallbacks
{

    public GameObject redPlayerPrefab;
    public GameObject bluePlayerPrefab;
    public GameObject pauseCanvas;
    public bool paused = false;

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
            PhotonNetwork.Instantiate(team == 0 ? bluePlayerPrefab.name : redPlayerPrefab.name, new Vector3(0, 5, 0), Quaternion.identity);
        }
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
