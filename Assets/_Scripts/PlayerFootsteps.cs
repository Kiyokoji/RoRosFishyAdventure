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
        Stone,
        Metal,
        Grass
    };

    [SerializeField] private CurrentTerrain currentTerrain;
    private FMOD.Studio.EventInstance footsteps;

    private void Update()
    {
        DetectTerrain();
    }

    private void DetectTerrain()
    {
        RaycastHit2D[] hit;

        hit = Physics2D.RaycastAll(transform.position, Vector2.down, 10f);

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
            
            case CurrentTerrain.Stone:
                PlayFootstep(2);
                break;
            
            case CurrentTerrain.Grass:
                PlayFootstep(3);
                break;

            case CurrentTerrain.Metal:
                PlayFootstep(1);
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
