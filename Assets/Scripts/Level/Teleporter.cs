using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Teleporter : Activatable
{
    public string sceneToLoad;

    private Animator animator;

    public bool alwaysActive = false;

    List<string> currentCollision = new List<string>();

    private void Start()
    {
        animator = GetComponent<Animator>();

        if (alwaysActive)
        {
            Activate();
        }
    }

    private void Open()
    {
        animator.SetBool("Closed", false);
        animator.SetBool("Open", true);
    }

    private void Close()
    {
        animator.SetBool("Open", false);
        animator.SetBool("Closed", true);
    }

    private void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            currentCollision.Add(collision.tag);

            if (currentCollision.Contains("Player1") && currentCollision.Contains("Player2"))
            {
                if (isActive)
                {
                    //view.RPC("Teleport", RpcTarget.All, sceneToLoad);
                    //if (PhotonNetwork.IsMasterClient)
                    {
                        //FindObjectOfType<AudioManager>().Play("Victory");
                        //SceneManager.LoadScene(sceneToLoad);
                    }
                    
                    //PhotonNetwork.LoadLevel(sceneToLoad);
                }
            }
        }
    }

    public void Teleport(string scene)
    {
        SceneManager.LoadScene(scene);
        //PhotonNetwork.LoadLevel(sceneToLoad);
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
        {
            currentCollision.Remove(collision.tag);
        }
    }

    public void Activate()
    {
        isActive = true;
        Open();
    }

    public void Deactivate()
    {
        isActive = false;
        Close();
    }

}

