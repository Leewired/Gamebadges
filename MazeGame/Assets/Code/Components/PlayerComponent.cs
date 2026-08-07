using UnityEngine;
using MazeGame.Core;

namespace MazeGame.Components
{
    public class PlayerComponent : MonoBehaviour
    {

        void OnCollisionStay(Collision collision)
        {
            Debug.Log("Collision stay detected.");

            Game.m_player.m_onAir = false;
            foreach (ContactPoint contact in collision.contacts)
            {
                Game.m_player.m_surfaceDot = Vector3.Dot(contact.normal, Vector3.up);
                if (Game.m_player.m_surfaceDot >= 0f)
                {
                    Game.m_player.m_onAir = false;
                    Game.m_player.m_jumpAvailable = true;
                    return;
                }
            }
        }

        void OnCollisionExit(Collision collision)
        {
            Game.m_player.m_onAir = true;
            Game.m_player.m_jumpAvailable = false;
        }
    }
}