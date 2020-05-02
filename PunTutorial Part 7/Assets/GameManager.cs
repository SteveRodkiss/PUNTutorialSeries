using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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
        //allocate a team now?
        int numBlue = NumInTeam(1);
        int numRed = NumInTeam(2);
        if (numBlue <= numRed)
        {
            Debug.Log("Allocated to blue Team");
            PhotonTeamExtensions.JoinTeam(PhotonNetwork.LocalPlayer, "Blue");
            //blue team prefab
            PhotonNetwork.Instantiate(bluePlayerPrefab.name, new Vector3(0, 5, 0), Quaternion.identity);
        }
        else
        {
            Debug.Log("Allocated to red Team");
            PhotonTeamExtensions.JoinTeam(PhotonNetwork.LocalPlayer, "Red");
            //red team prefab
            PhotonNetwork.Instantiate(redPlayerPrefab.name, new Vector3(0, 5, 0), Quaternion.identity);
        }
    }

    int NumInTeam(int teamIndex)
    {
        if (PhotonTeamsManager.Instance.TryGetTeamMembers((byte)teamIndex, out Player[] members))
        {
            return members.Length;
        }
        return 0;
    }


    public void Quit()
    {
        PhotonNetwork.LeaveRoom();
    }

    public override void OnLeftRoom()
    {
        PhotonNetwork.LoadLevel(0);
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

}
