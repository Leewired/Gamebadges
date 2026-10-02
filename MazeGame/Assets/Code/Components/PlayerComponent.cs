using System.Collections.Generic;
using UnityEngine;
using MazeGame.Core;

namespace MazeGame.Components
{
    public class PlayerComponent : MonoBehaviour
    {
        public float m_jumpForce = 300f;
        public float m_moveForce = 10f;
        public float m_maxSpeed = 100f;
        public float m_maxUpwardsSpeed = 20f;
        public float m_airForceMultiplier = 0.3f;
        public float m_lowVelocityThreshold = 0.1f;
        public float m_lowVelocityForceMultiplier = 2f;
        public float m_counterForceMultiplier = 1.5f;
        public float m_wallJumpForceUpwardsMultiplier = 1f;
        public float m_wallJumpForceOutwardsMultiplier = 0.5f;
        public float m_wallJumpForceForwardsMultiplier = 0.2f;
        public float m_wallJumpForceUpwardsCounterMultiplier = 20f;
        public float m_rotationSpeedMultiplier = 0.2f;
        private float m_currentContactDot = -1f;
        private float m_lowestContactDot = 1f;
        private Vector3 m_currentContactNormal = Vector3.zero;
        private Vector3 m_lowestContactNormal = Vector3.zero;
        private Dictionary<GameObject, (float dot, Vector3 normal)> m_currentContactObjects = new Dictionary<GameObject, (float, Vector3)>();

        private void OnCollisionEnter(Collision collision)
        {
            //Player component is loaded before assignment to game, so we need to check if player is null.
            if (Game.m_player == null) return;

            if(collision.gameObject.CompareTag("Key"))
            {
                Game.m_player.m_hasKey = true;
                collision.gameObject.SetActive(false);
                Debug.Log("KEY COLLECTED");
                return;
            }

            if (collision.gameObject.CompareTag("Exit") && Game.m_player.m_hasKey)
            {
                Game.m_player.m_hasKey = false;
                Game.m_player.m_playerBeatLevel = true;
                Game.m_gameStateMachine.AddParameter("End", true); //TODO: clean logic
                return;
            }

            if (collision.gameObject.CompareTag("Enemy"))
            {
                Debug.Log("AHHH! You caught me!");
                Game.m_gameStateMachine.AddParameter("End", true); //TODO: clean logic
                return;
            }

            //ContactPoint includes both colliders, normal and point
            ContactPoint contact = collision.GetContact(0); //there shouldn't be multiple contacts in one collision.
            Vector3 newContactNormal = contact.normal;
            float newContactDot = Vector3.Dot(newContactNormal, Vector3.up);

            if (newContactDot < 0) //We don't care about collisions that point downwards for now.
            {
                Debug.Log($"Ignored new contact with: {collision.gameObject.name}");
                return;
            }

            m_currentContactObjects.Add(collision.gameObject, (newContactDot, newContactNormal));
            UpdateCollisionData();
        }

        void OnCollisionExit(Collision collision)
        {
            // Remove the contact object from the dictionary when the collision ends.
            if (m_currentContactObjects.ContainsKey(collision.gameObject))
            {
                m_currentContactObjects.Remove(collision.gameObject);
            }
            UpdateCollisionData();
        }

        (float dot, Vector3 normal) GetHighestContactDot() //return highest dot and matching normal.
        {
            float highestDot = -1f;
            Vector3 matchingNormal = Vector3.zero;
            foreach (var (dot, normal) in m_currentContactObjects.Values)
            {
                if (dot > highestDot)
                {
                    highestDot = dot;
                    matchingNormal = normal;
                }
            }
            return (highestDot, matchingNormal);
        }

        (float dot, Vector3 normal) GetLowestContactDot() //return lowest dot and matching normal.
        {
            float lowestDot = 1f;
            Vector3 matchingNormal = Vector3.zero;
            foreach (var (dot, normal) in m_currentContactObjects.Values)
            {
                if (dot < lowestDot)
                {
                    lowestDot = dot;
                    matchingNormal = normal;
                }
            }
            return (lowestDot, matchingNormal);
        }

        void UpdateCollisionData()
        {
            var (hDot, hNormal) = GetHighestContactDot();
            m_currentContactDot = hDot;
            m_currentContactNormal = hNormal;
            //GetHighest returns -1 as dot if there's no contacts.
            if (m_currentContactDot == -1f)
            {
                Debug.Log("In the air!");
                Game.m_player.m_onAir = true;
                Game.m_player.m_jumpAvailable = false;
                Game.m_player.m_surfaceNormal = Vector3.zero;
                return;
            }
            var (lDot, lNormal) = GetLowestContactDot();
            m_lowestContactDot = lDot;
            m_lowestContactNormal = lNormal;

            //TODO: move logic to player, to calculate from normal
            if (m_currentContactDot < 0.01f) //we are on a wall.
            {
                Game.m_player.m_onWall = true;
            }
            else if (m_currentContactDot >= 0.01f) //we are on the ground.
            {
                Game.m_player.m_onWall = false;
            }
            Game.m_player.m_onAir = false;
            Game.m_player.m_jumpAvailable = true;
            Game.m_player.m_surfaceDot = m_currentContactDot;
            Game.m_player.m_surfaceNormal = m_currentContactNormal;
            Game.m_player.m_lowestContactDot = m_lowestContactDot;
            Game.m_player.m_lowestContactNormal = m_lowestContactNormal;
        }
    }
}