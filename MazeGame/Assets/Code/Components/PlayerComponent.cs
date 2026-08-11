using System.Collections.Generic;
using UnityEngine;
using MazeGame.Core;
using Unity.VisualScripting;

namespace MazeGame.Components
{
    public class PlayerComponent : MonoBehaviour
    {
        public float m_jumpForce = 5f;
        public float m_moveForce = 1f;
        public float currentContactDot = -1f;
        public Vector3 currentContactNormal = Vector3.zero;
        public List<Collision> currentCollisions = new List<Collision>();

        private void OnCollisionEnter(Collision collision)
        {
            //Player component is loaded before assignment to game, so we need to check if player is null.
            if (Game.m_player == null) return;

            ContactPoint contact = collision.GetContact(0); //there shouldn't be multiple contacts in one collision.
            Vector3 newContactNormal = contact.normal;
            float newContactDot = Vector3.Dot(newContactNormal, Vector3.up);

            if (newContactDot < 0) //We don't care about collisions that point downwards for now.
            {
                Debug.Log($"Ignored new contact with: {collision.gameObject.name}");
                return;
            }

            currentCollisions.Add(collision);
            Debug.Log($"New contact with: {collision.gameObject.name}");
            Debug.Log($"Contact count: {currentCollisions.Count}");

            UpdateCollisionData(newContactDot, newContactNormal);
        }

        void OnCollisionExit(Collision collision) //other collision overwrites, so exit might not happen.
        {
            // TODO: re-evaluate currentDot and currentNormal.

            Debug.Log($"Exited contact with: {collision.gameObject.name}");
            currentCollisions.Remove(collision);
            Debug.Log($"Contact count: {currentCollisions.Count}");
            CheckCurrentCollisions();
        }

        void CheckCurrentCollisions()
        {
            if (currentCollisions.Count == 0) //no collisions, we are in the air.
            {
                currentContactDot = -1f;
                currentContactNormal = Vector3.zero;
                Game.m_player.m_onAir = true;
                Game.m_player.m_jumpAvailable = false;
                Game.m_player.m_onWall = false;
                return;
            }

            foreach (var collision in currentCollisions) //check all collisions to find the one with the highest dot product.
            {
                if (collision == null) return; //collision can be null if the object was destroyed.
                if (collision.contactCount == 0) return; //contact count can be zero.

                ContactPoint contact = collision.GetContact(0); //there shouldn't be multiple contacts in one collision.
                Vector3 newContactNormal = contact.normal;
                float newContactDot = Vector3.Dot(newContactNormal, Vector3.up);

                UpdateCollisionData(newContactDot, newContactNormal);
            }

        }

        void UpdateCollisionData(float contactDot, Vector3 contactNormal)
        {
            if (contactDot > currentContactDot) //we want to always use the contact with the highest dot product.
            {
                currentContactDot = contactDot;
                currentContactNormal = contactNormal;
            }
            else
            {
                return;
            }

            if (currentContactDot < 0.01f) //we are on a wall.
            {
                Game.m_player.m_onWall = true;
            }
            else if (currentContactDot >= 0.01f) //we are on the ground.
            {
                Game.m_player.m_onWall = false;
            }
            Game.m_player.m_onAir = false;
            Game.m_player.m_jumpAvailable = true;
            Game.m_player.m_surfaceDot = currentContactDot;
            Game.m_player.m_surfaceNormal = currentContactNormal;
        }
    }
}