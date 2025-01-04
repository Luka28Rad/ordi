using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mirror;
using UnityEngine.SceneManagement;

public class PlayerMovementMultiplayerController : NetworkBehaviour
{
           public float speed = 0.1f;

        public GameObject playerModel;
        public SpriteRenderer PlayerMesh;
        public Sprite[] PlayerColors;

        void Start(){
            playerModel.SetActive(false);
        }

    void Update(){
        if(SceneManager.GetActiveScene().name == "MultiplayerLevel"){
            if(playerModel.activeSelf == false){SetPosition(); playerModel.SetActive(true); PlayerCosmeticsSetup();}
           // if(isOwned) Movement();
        }  else if(SceneManager.GetActiveScene().name == "LobbyScene"){
            var spriteRenderer = playerModel.GetComponent<SpriteRenderer>();
            var playerController = playerModel.GetComponent<PlayerControllerMP>();
            playerModel.SetActive(false);
            if (spriteRenderer != null) spriteRenderer.enabled = true;
            if (playerController != null) playerController.enabled = true;
        }
    }

    public void SetPosition(){
        transform.position = new Vector3(0,0,0); 
        transform.rotation = Quaternion.Euler(0f, 0f, 0f);
        transform.GetChild(0).transform.GetChild(1).transform.position = new Vector3(Random.Range(-4.5f,4.5f),0.5f,0);
        transform.GetChild(0).transform.GetChild(1).transform.rotation = Quaternion.Euler(0f, 0f, 0f);
    }

/*        void Movement()
        {
            float h = Input.GetAxis("Horizontal");
            ////float v = Input.GetAxis("Vertical");

            //Vector3 dir = new Vector3(h, 0, v);
            //transform.position += dir.normalized * (Time.deltaTime * speed);
            //transofrm.position += dir * speed;
        }*/
    
    public void PlayerCosmeticsSetup(){
        PlayerMesh.sprite = PlayerColors[GetComponent<PlayerObjectController>().PlayerColor];
    }
}
