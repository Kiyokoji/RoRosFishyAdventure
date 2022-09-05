using System;
using Sirenix.OdinInspector;
using UnityEngine;
using Photon.Pun;

namespace Level
{
    public class Gate : Activatable
    {
        PhotonView view;
        private SpriteRenderer sr;
        private Animator animator;
        public float speed = 1f;
        private static readonly int IsActiveID = Animator.StringToHash("IsActive");

        private bool hasChanged, previousActiveState;

        private void Start()
        {
            view = GetComponent<PhotonView>();
            sr = GetComponent<SpriteRenderer>();
            animator = GetComponent<Animator>();
            
            hasChanged = false;
            previousActiveState = isActive;

            animator.speed = speed;
        }

        private void Update()
        {
            hasChanged = previousActiveState != isActive;
            if (!hasChanged) return;
            previousActiveState = isActive;

            view.RPC("Activate", RpcTarget.All);

        }

        [PunRPC]
        public void Activate()
        {
            animator.SetBool(IsActiveID, isActive);
            hasChanged = false;
        }

        [PunRPC]
        public void Deactivate()
        {

        }

    }
}
