using System;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;

namespace Level
{
    public class Crate : MonoBehaviour
    {
        private BoxCollider2D mainCollider;
        public LayerMask groundLayer;

        private SpriteRenderer sr;
        private Rigidbody2D rb;
        //private JumpSwitch currentSwitch;
        
        public bool isGrounded = false;

        [SerializeField] private PhysicsMaterial2D sideFriction, groundFriction;

        private Transform root;

        private void Start()
        {
            mainCollider = GetComponent<BoxCollider2D>();
            sr = GetComponent<SpriteRenderer>();
            rb = GetComponent<Rigidbody2D>();
            root = transform.parent;
        }

        private void Update()
        {
            GroundCheck();
            ChangeFriction();
        }

        private void GroundCheck()
        {
            Bounds colliderBounds = mainCollider.bounds;
            float colliderRadius = mainCollider.size.x * 0.4f * Mathf.Abs(transform.localScale.x);
            Vector3 groundCheckPos = colliderBounds.min + new Vector3(colliderBounds.size.x * 0.5f, colliderRadius * 0.8f, 0f);

            Collider2D[] colliders = Physics2D.OverlapCircleAll(groundCheckPos, colliderRadius, groundLayer);

            isGrounded = false;
            if (colliders.Length > 0)
            {
                foreach (var t in colliders)
                {
                    if (t == mainCollider) continue; // IDE told me to do this because of nesting so here we fucking go
                    isGrounded = true;
                    break;
                }
            }
            
            if (!isGrounded)
            {

                return;
            }
        
            Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(0, colliderRadius, 0), isGrounded ? Color.green : Color.red);
            Debug.DrawLine(groundCheckPos, groundCheckPos - new Vector3(colliderRadius, 0, 0), isGrounded ? Color.green : Color.red);
        }

        private void ChangeFriction()
        {
            GetComponent<Rigidbody2D>().sharedMaterial = isGrounded ? groundFriction : sideFriction;
            //GetComponent<Rigidbody2D>().sharedMaterial = isGrounded ? Resources.Load("Materials/GroundFriction") as PhysicsMaterial2D : Resources.Load("Materials/SideFriction") as PhysicsMaterial2D;
        }

        [PunRPC]
        private void EnableCrate()
        {
            this.gameObject.SetActive(true);
            //rb.bodyType = RigidbodyType2D.Dynamic;
            //this.gameObject.GetComponent<SpriteRenderer>().enabled = true;
            //this.gameObject.layer = 9;
        }

        [PunRPC]
        private void DisableCrate()
        {
            this.gameObject.SetActive(false);
            //rb.bodyType = RigidbodyType2D.Kinematic;
            //this.gameObject.GetComponent<SpriteRenderer>().enabled = false;
            //this.gameObject.layer = 7;
        }

        [PunRPC]
        private void ParentPlayer1()
        {
            PlayerController[] players = FindObjectsOfType<PlayerController>();

            foreach (PlayerController p in players)
            {
                if (p.CompareTag("Player1")) transform.SetParent(p.transform);
            }
        }

        [PunRPC]
        private void ParentPlayer2()
        {
            PlayerController[] players = FindObjectsOfType<PlayerController>();

            foreach (PlayerController p in players)
            {
                if (p.CompareTag("Player2")) transform.SetParent(p.transform);
            }
        }

        [PunRPC]
        private void UnparentPlayer()
        {
            transform.SetParent(null);
        }
    }
}
