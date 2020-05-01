using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class Gun : MonoBehaviourPunCallbacks
{
    public Transform gunTransform;
    public ParticleSystem ps;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            if (Input.GetMouseButtonDown(0))
            {
                //shoot
                photonView.RPC("RPC_Shoot", RpcTarget.All);
            }
        }
    }

    //get called on all instances of the viewID
    [PunRPC]
    void RPC_Shoot()
    {
        //shoot
        ps.Play();
        Ray ray = new Ray(gunTransform.position, gunTransform.forward);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f))
        {
            //we hit something
            Debug.Log($"Raycast hit {hit.collider.name}");
            //check hit has health script we can take some value off health.
            var enemyHealth = hit.collider.GetComponent<Health>();
            if (enemyHealth)
            {
                //we have hit an enemy- do damage
                enemyHealth.TakeDamage(20);
                Debug.Log("We hit a player!!");
            }
        }
    }

}
