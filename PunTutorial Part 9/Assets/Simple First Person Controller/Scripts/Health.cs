using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using ExitGames.Client.Photon;

public class Health : MonoBehaviourPunCallbacks, IPunObservable, IOnEventCallback
{

    Renderer[] visuals;
    public int health = 100;

    //get the team for the respwning
    int team = 0; 


    public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
    {
        //sync health
        if (stream.IsWriting)
        {
            stream.SendNext(health);
        }
        else
        {
            //we are reading
            health = (int)stream.ReceiveNext();
        }
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
    }

    IEnumerator Respawn()
    {
        SetRenderers(false);
        health = 100;
        GetComponent<CharacterController>().enabled = false;
        Transform spawn = SpawnManager.instance.GetTeamSpawn(team);
        transform.position = spawn.position;
        transform.rotation = spawn.rotation;
        GetComponent<CharacterController>().enabled = true;
        yield return new WaitForSeconds(1);        
        SetRenderers(true);
    }

    void SetRenderers(bool state)
    {
        foreach (var renderer in visuals)
        {
            renderer.enabled = state;
        }
    }


    // Start is called before the first frame update
    void Start()
    {
        visuals = GetComponentsInChildren<Renderer>();
        //get the team at the start
        team = (int)PhotonNetwork.LocalPlayer.CustomProperties["Team"];
    }

    // Update is called once per frame
    void Update()
    {
        if (health <= 0)
        {
            if (photonView.IsMine)
            {
                //run the rpc
                photonView.RPC("RPC_PlayerKilled", RpcTarget.All, team);
                StartCoroutine(Respawn());
            }
            
        }
    }

    [PunRPC]
    void RPC_PlayerKilled(int team)
    {
        //what team increment score
        if (team==0)
        {
            GameManager.redScore++;
        }
        else
        {
            GameManager.blueScore++;
        }
    }

    //will fire when event is activated
    public void OnEvent(EventData photonEvent)
    {
        if (photonEvent.Code == GameManager.RestartGameEventCode)
        {
            GameManager.redScore = 0;
            GameManager.blueScore = 0;
            health = 100;
            StartCoroutine(Respawn());
        }
    }

    private void OnEnable()
    {
        PhotonNetwork.AddCallbackTarget(this);
    }
    private void OnDisable()
    {
        PhotonNetwork.RemoveCallbackTarget(this);
    }


}
