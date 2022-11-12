using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class LightSwitch : Activatable
{
    private SpriteRenderer sr;
    private UnityEngine.Rendering.Universal.Light2D backlight;

    public List<Activatable> activatableList;

    [Header("Inactive Light")][Range(0f, 2f)]
    public float inactive = 0.5f;
    [Header("Active Light")][Range(0f, 2f)]
    public float active = 1f;

    private Color inactiveColor;
    private Color activeColor;

    [SerializeField] private bool player1;
    [SerializeField] private bool player2;

    private void Start()
    {
        sr = GetComponent<SpriteRenderer>();
        backlight = GetComponentInChildren<UnityEngine.Rendering.Universal.Light2D>();

        inactiveColor = sr.color;
        activeColor = sr.color;

        inactiveColor.a = 0.3f;
        activeColor.a = 1.0f;

        backlight.intensity = inactive;

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (player1 && !player2 && collision.tag == "LightPlayer1")
        {
            LightOn();
        }

        if (!player1 && player2 && collision.tag == "LightPlayer2")
        {
            LightOn();

        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (player1 && collision.tag == "LightPlayer1")
        {
            LightOff();
        }

        else if (player2 && collision.tag == "LightPlayer2")
        {
            LightOff();
        }
    }

    private void LightOn()
    {
        foreach (var a in activatableList)
        {
            //a.GetComponent<PhotonView>().RPC("Activate", RpcTarget.All);
            a.isActive = true;
        }
        sr.color = activeColor;
        backlight.intensity = active;
    }

    private void LightOff()
    {
        foreach (var a in activatableList)
        {
            //a.GetComponent<PhotonView>().RPC("Deactivate", RpcTarget.All);
            a.isActive = false;
        }

        sr.color = inactiveColor;
        backlight.intensity = inactive;
    }

}
