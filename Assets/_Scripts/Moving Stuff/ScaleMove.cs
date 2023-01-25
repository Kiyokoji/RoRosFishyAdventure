using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleMove : MonoBehaviour
{
    public Scale scale;
    
    public Transform _up;
    public Transform _down;
    public Transform _default;
    private int weight;
    
    public bool isLeftScale;
    
    private float _speed;

    private void Awake()
    {
        _speed = scale.speed;
        
        scale.leftTrigger = true;
        scale.rightTrigger = true;
    }

    public void GoUp()
    {
        this.transform.position = 
            Vector2.MoveTowards(
                this.transform.position, 
                _up.position, 
                _speed * Time.fixedDeltaTime
            );
    }
    
    public void GoDown()
    {
        this.transform.position = 
            Vector2.MoveTowards(
                this.transform.position, 
                _down.position, 
                _speed * Time.fixedDeltaTime
            );
    }

    public void GoDefault()
    {
        this.transform.position = 
            Vector2.MoveTowards(
                this.transform.position, 
                _default.position, 
                _speed * Time.fixedDeltaTime
            );
    }

    private void OnTriggerStay2D(Collider2D col)
    {
        if (col.CompareTag("Player1") || col.CompareTag("Player2") || col.CompareTag("Object"))
        {
            weight = col.GetComponent<PlayerController.PlayerController>().weight;
            
            if (isLeftScale)
            {
                scale.leftTrigger = true;
                
            }
            else
            {
                scale.rightTrigger = true;
            }
        }
        
        if (col.CompareTag("Player1") || col.CompareTag("Player2"))
        {
            weight = col.GetComponent<PlayerController.PlayerController>().weight;
            
        }     
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player1") || col.CompareTag("Player2"))
        {
            weight = col.GetComponent<PlayerController.PlayerController>().weight;
            if (isLeftScale) { scale.leftObjects += weight; }
            else { scale.rightObjects += weight; }
        }

        if (col.CompareTag("Object"))
        {
            col.GetComponent<CrateDisable>().onScale = true;
            col.GetComponent<CrateDisable>().SetScaleMove(this);
        
            if (isLeftScale) { scale.leftObjects++; }
            else { scale.rightObjects++; }
        }
        /*

        if (col.CompareTag("Player1") || col.CompareTag("Player2") || col.CompareTag("Object"))
        {
            if (isLeftScale)
            {
                if (col.transform.GetComponent<PlayerController.PlayerController>() != null && col.transform.GetComponent<PlayerController.PlayerController>().hasCrate)
                {
                    scale.leftObjects += 2;
                }
                else
                {
                    scale.leftObjects++;
                }
            }
            else
            {
                if (col.transform.GetComponent<PlayerController.PlayerController>() != null && col.transform.GetComponent<PlayerController.PlayerController>().hasCrate)
                {
                    scale.rightObjects += 2;
                }
                else
                {
                    scale.rightObjects++;
                }
            }
        }
        
        if (col.CompareTag("Object"))
        {
            col.GetComponent<CrateDisable>().onScale = true;
            col.GetComponent<CrateDisable>().SetScaleMove(this);
        }
        
        */
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player1") || col.CompareTag("Player2"))
        {
            if (isLeftScale)
            {
                scale.leftObjects -= weight;
                
                if (scale.leftTrigger != true)
                {
                    scale.leftTrigger = false;
                }
            }
            else
            {
                scale.rightObjects -= weight;
                
                if (scale.rightTrigger != true)
                {
                    scale.rightTrigger = false;
                }
            }
        }

        if (col.CompareTag("Object"))
        {
            col.GetComponent<CrateDisable>().onScale = false;
            col.GetComponent<CrateDisable>().ResetScaleMove();
        
            if (isLeftScale) { scale.leftObjects--; }
            else { scale.rightObjects--; }
        }
        
        /*
        if (col.CompareTag("Player1") || col.CompareTag("Player2") || col.CompareTag("Object"))
        {
            if (isLeftScale)
            {
                if (scale.leftTrigger != true)
                {
                    scale.leftTrigger = false;
                }
                
                if (col.transform.GetComponent<PlayerController.PlayerController>() != null && col.transform.GetComponent<PlayerController.PlayerController>().hasCrate)
                {
                    scale.leftObjects -= 2;
                }
                else
                {
                    scale.leftObjects--;
                }
            }
            else
            {
                if (scale.rightTrigger != true)
                {
                    scale.rightTrigger = false;
                }
                
                if (col.transform.GetComponent<PlayerController.PlayerController>() != null && col.transform.GetComponent<PlayerController.PlayerController>().hasCrate)
                {
                    scale.rightObjects -= 2;
                }
                else
                {
                    scale.rightObjects--;
                }
            }
        }
        
        if (col.CompareTag("Object"))
        {
            col.GetComponent<CrateDisable>().onScale = false;
            col.GetComponent<CrateDisable>().ResetScaleMove();
        }
        
        */
    }
}
