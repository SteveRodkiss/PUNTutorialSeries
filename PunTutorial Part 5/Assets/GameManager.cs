using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class GameManager : MonoBehaviourPunCallbacks
{

    public GameObject playerPrefab;
    public GameObject pauseCanvas;
    public bool paused = false;

    // Start is called before the first frame update
    void Start()
    {
        PhotonNetwork.Instantiate(playerPrefab.name, new Vector3(0, 5, 0), Quaternion.identity);
        SetPaused();
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
