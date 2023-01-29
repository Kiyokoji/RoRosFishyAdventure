using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD.Studio;

public class PlayerFootsteps : MonoBehaviour
{
    private enum CurrentTerrain
    {
        Wood,
        Metal,
        Stone,
        Gravel,
        Water,
        Grass
    };

    public Transform feet;
    public float checkDistance = 1f;
    [SerializeField] private CurrentTerrain currentTerrain;
    private FMOD.Studio.EventInstance footsteps;

    private void Update()
    {
        Debug.Log(currentTerrain);
        
        DetectTerrain();
    }

    private void DetectTerrain()
    {
        RaycastHit2D[] hit;

        hit = Physics2D.RaycastAll(feet.position, Vector2.down, checkDistance);

        foreach (RaycastHit2D rayHit in hit)
        {
            if (rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Wood"))
            {
                currentTerrain = CurrentTerrain.Wood;
            }
            else if (rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Stone"))
            {
                currentTerrain = CurrentTerrain.Stone;
            }
            else if (rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Metal"))
            {
                currentTerrain = CurrentTerrain.Metal;
            }
            else if (rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Gravel"))
            {
                currentTerrain = CurrentTerrain.Gravel;
            }
            else if (rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Water"))
            {
                currentTerrain = CurrentTerrain.Water;
            }
            else if (rayHit.transform.gameObject.layer == LayerMask.NameToLayer("Grass"))
            {
                currentTerrain = CurrentTerrain.Grass;
            }
        }
    }
    
    public void SelectAndPlayFootstep()
    {
        switch (currentTerrain)
        {
            case CurrentTerrain.Wood:
                PlayFootstep(0);
                break;

            case CurrentTerrain.Metal:
                PlayFootstep(1);
                break;

            case CurrentTerrain.Stone:
                PlayFootstep(2);
                break;

            case CurrentTerrain.Gravel:
                PlayFootstep(3);
                break;

            case CurrentTerrain.Water:
                PlayFootstep(4);
                break;

            case CurrentTerrain.Grass:
                PlayFootstep(5);
                break;

            default:
                PlayFootstep(0);
                break;
        }
    }

    public void PlayFootstep(int terrain)
    {
        footsteps = FMODUnity.RuntimeManager.CreateInstance("event:/Footsteps");
        footsteps.setParameterByName("Terrain", terrain);
        //footsteps.set3DAttributes(FMODUnity.RuntimeUtils.To3DAttributes(gameObject));
        footsteps.start();
        footsteps.release();
    }

    
    
}
