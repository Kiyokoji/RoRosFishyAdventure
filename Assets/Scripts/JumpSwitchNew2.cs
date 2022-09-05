using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Level;

public class JumpSwitchNew2 : MonoBehaviour
{
    PhotonView view;

    private bool active;

    public float speed = 2f;

    public List<Activatable> activatableList;

    public bool weighted = false;
    private bool toggled;

    public GameObject waypoint1;
    public GameObject waypoint2;

    private bool isToggled;


    void Start()
    {
        view = GetComponent<PhotonView>();
    }


    private void FixedUpdate()
    {
        if (active)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoint2.transform.position, speed * Time.deltaTime);
        } else if (!active && !isToggled)
        {
            transform.position = Vector2.MoveTowards(transform.position, waypoint1.transform.position, speed * Time.deltaTime);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (weighted)
        {
            if (!toggled && (collision.CompareTag("Player1") || collision.CompareTag("Player2") || collision.CompareTag("Crate")))
            {
                //FindObjectOfType<AudioManager>().Play("JumpSwitchOn");
                view.RPC("ActivateSwitch", RpcTarget.All);
                toggled = true;
            }
            else if (toggled && (collision.CompareTag("Player1") || collision.CompareTag("Player2") || collision.CompareTag("Crate")))
            {
                //FindObjectOfType<AudioManager>().Play("JumpSwitchOff");
                view.RPC("DeactivateSwitch", RpcTarget.All);
                toggled = false;
            }
        }
        else
        {
            if (collision.CompareTag("Player1") || collision.CompareTag("Player2") || collision.CompareTag("Crate"))
            {
                //FindObjectOfType<AudioManager>().Play("JumpSwitchOn");
                view.RPC("ActivateSwitch", RpcTarget.All);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player1") || collision.CompareTag("Player2") || collision.CompareTag("Crate"))
        {
            if (!toggled)
            {
                //FindObjectOfType<AudioManager>().Play("JumpSwitchOff");
                view.RPC("DeactivateSwitch", RpcTarget.All);
            }
        }
    }

    [PunRPC]
    public void ActivateSwitch()
    {
        active = true;
        FindObjectOfType<AudioManager>().Play("JumpSwitchOn");

        foreach (var a in activatableList)
        {
            a.GetComponent<PhotonView>().RPC("Activate", RpcTarget.All);
            a.isActive = true;
        }
    }

    [PunRPC]
    public void DeactivateSwitch()
    {
        active = false;
        FindObjectOfType<AudioManager>().Play("JumpSwitchOff");

        foreach (var a in activatableList)
        {
            a.GetComponent<PhotonView>().RPC("Deactivate", RpcTarget.All);
            a.isActive = false;
        }
    }
}

