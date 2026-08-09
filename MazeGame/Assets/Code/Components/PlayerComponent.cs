using UnityEngine;
using MazeGame.Core;
using Unity.VisualScripting;

namespace MazeGame.Components
{
    public class PlayerComponent : MonoBehaviour
    {

        void OnCollisionStay(Collision collision)
        {
            Debug.Log("Collision stay detected.");

            float dot = -1f;
            Vector3 normal = Vector3.zero;

            foreach (ContactPoint contact in collision.contacts)
            {
                float contactDot = Vector3.Dot(contact.normal, Vector3.up);
                if (contactDot > dot) // Prefer contact with highest dot product.
                {
                    dot = contactDot;
                    normal = contact.normal;
                }
            }
            if (dot >= 0f)
            {
                Game.m_player.m_onAir = false;
                Game.m_player.m_jumpAvailable = true;
                Game.m_player.m_surfaceDot = dot;
                Game.m_player.m_surfaceNormal = normal;
                return;
            }
        }

        void OnCollisionExit(Collision collision)
        {
            Debug.Log("Collision exit detected.");

            Game.m_player.m_onAir = true;
            Game.m_player.m_jumpAvailable = false;
        }
    }
}