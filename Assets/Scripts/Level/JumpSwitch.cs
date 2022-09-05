using System;
using System.Collections.Generic;
using System.Collections;
using NodeCanvas.Framework;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;
using Photon.Pun;

namespace Level
{
    public class JumpSwitch : MonoBehaviour
    {
        private bool player1;
        private bool player2;

        [Title("Button Logic")]
        [SerializeField] private bool isWeighted = false;
        [DisableIf("isWeighted")] [SerializeField] private bool isToggle = true;
        [SerializeField, ReadOnly] private bool isActive;
        [SerializeField] private float activationCooldown = 0.2f;
        [ShowInInspector, ReadOnly] private bool onCooldown = false;
        
        [ReadOnly] public bool SomethingStandingOnButton = false;

        [Title("Activatable Object")] [HideLabel]
        
        public List<Activatable> activatableList;

        [HideInInspector] public UnityEvent pressed;

        private Animator animator;
        private PhotonView view;

        private void Start()
        {
            animator = GetComponent<Animator>();
            view = GetComponent<PhotonView>();
            //player1 = GameObject.FindWithTag("Player1").GetComponent<PlayerController>().isGrounded;
            //player2 = GameObject.FindWithTag("Player2").GetComponent<PlayerController>().isGrounded;

            pressed ??= new UnityEvent();
            pressed.AddListener(PressSwitch);
        }


        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerController>())
            {
                if (!collision.gameObject.GetComponent<PlayerController>().isGrounded) return;
            }
            
            if (collision.gameObject.GetComponent<Crate>())
            {
                if (!collision.gameObject.GetComponent<Crate>().isGrounded) return;
            }
            
            if(collision.CompareTag("Player1") || collision.CompareTag("Player2"))
            {
                FindObjectOfType<AudioManager>().Play("JumpSwitchOn");
                view.RPC("PressSwitch", RpcTarget.All);
            } 
        }

        private void OnTriggerExit2D(Collider2D collision)
        {
            if (collision.gameObject.GetComponent<PlayerController>())
            {
                if (!collision.gameObject.GetComponent<PlayerController>().isGrounded) return;
            }

            if (collision.gameObject.GetComponent<Crate>())
            {
                if (!collision.gameObject.GetComponent<Crate>().isGrounded) return;
            }

            if (collision.CompareTag("Player1") || collision.CompareTag("Player2"))
            {
                view.RPC("PressSwitch", RpcTarget.All);
                FindObjectOfType<AudioManager>().Play("JumpSwitchOff");
            }

        }

        private void Update()
        {
            if (isActive)
            {
                animator.SetBool("SwitchDown", true);
                animator.SetBool("SwitchUp", false);
            } else if (!isActive)
            {
                animator.SetBool("SwitchDown", false);
                animator.SetBool("SwitchUp", true);
            }

            if (!isWeighted) return;
            if (SomethingStandingOnButton) return;
            
            isActive = false;

            foreach (var a in activatableList)
            {
                a.isActive = isActive;
            }
        }

        [PunRPC]
        [Title("Switch Action")]
        [Button(ButtonStyle.Box)]
        private void PressSwitch()
        {
            if (SomethingStandingOnButton || onCooldown) return;
            if (isToggle)
            {
                FindObjectOfType<AudioManager>().Play("JumpSwitchOn");
                isActive = !isActive;
            }
            else
            {
                
                if (!isActive)
                {
                    
                    isActive = true;
                }
            }

            StartCoroutine(WaitForCooldown(activationCooldown));

            foreach (var a in activatableList)
            {
                a.isActive = isActive;
            }
        
            //Debug.Log("isActive = " + isActive);
        }

        private IEnumerator WaitForCooldown(float duration)
        {
            onCooldown = true;
            yield return new WaitForSeconds(duration);
            onCooldown = false;
        }
    }
}
