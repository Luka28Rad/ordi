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
            if(isOwned) Movement();
        } 
    }

    public void SetPosition(){
        transform.position = new Vector3(Random.Range(-5,5),0.5f,0); 
    }

        void Movement()
        {
            float h = Input.GetAxis("Horizontal");
            float v = Input.GetAxis("Vertical");

            Vector3 dir = new Vector3(h, 0, v);
            transform.position += dir.normalized * (Time.deltaTime * speed);
            //transofrm.position += dir * speed;
        }
    
    public void PlayerCosmeticsSetup(){
        PlayerMesh.sprite = PlayerColors[GetComponent<PlayerObjectController>().PlayerColor];
    }
}
