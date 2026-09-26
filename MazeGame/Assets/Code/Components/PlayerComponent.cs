using System.Collections.Generic;
using UnityEngine;
using MazeGame.Core;

namespace MazeGame.Components
{
    public class PlayerComponent : MonoBehaviour
    {
        public float m_jumpForce = 5f;
        public float m_moveForce = 1f;
        public float currentContactDot = -1f;
        public Vector3 currentContactNormal = Vector3.zero;
        public Dictionary<GameObject, (float dot, Vector3 normal)> currentContactObjects = new Dictionary<GameObject, (float, Vector3)>();

        private void OnCollisionEnter(Collision collision)
        {
            //Player component is loaded before assignment to game, so we need to check if player is null.
            if (Game.m_player == null) return;

            //ContactPoint includes both colliders, normal and point
            ContactPoint contact = collision.GetContact(0); //there shouldn't be multiple contacts in one collision.
            Vector3 newContactNormal = contact.normal;
            float newContactDot = Vector3.Dot(newContactNormal, Vector3.up);

            if (newContactDot < 0) //We don't care about collisions that point downwards for now.
            {
                Debug.Log($"Ignored new contact with: {collision.gameObject.name}");
                return;
            }

            currentContactObjects.Add(collision.gameObject, (newContactDot, newContactNormal));
            UpdateCollisionData();
        }

        void OnCollisionExit(Collision collision)
        {
            // Remove the contact object from the dictionary when the collision ends.
            if (currentContactObjects.ContainsKey(collision.gameObject))
            {
                currentContactObjects.Remove(collision.gameObject);
            }
            UpdateCollisionData();
        }

        (float dot, Vector3 normal) GetHighestContactDot() //return highest dot and matching normal.
        {
            float highestDot = -1f;
            Vector3 matchingNormal = Vector3.zero;
            foreach (var (dot, normal) in currentContactObjects.Values)
            {
                if (dot > highestDot)
                {
                    highestDot = dot;
                    matchingNormal = normal;
                }
            }
            return (highestDot, matchingNormal);
        }

        void UpdateCollisionData()
        {
            var (dot, normal) = GetHighestContactDot();
            currentContactDot = dot;
            currentContactNormal = normal;
            //GetHighest returns -1 as dot if there's no contacts.
            if (currentContactDot == -1f)
            {
                Game.m_player.m_onAir = true;
                Game.m_player.m_jumpAvailable = false;
                Game.m_player.m_surfaceNormal = Vector3.zero;
                return;
            }

            //TODO: move logic to player, to calculate from normal
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