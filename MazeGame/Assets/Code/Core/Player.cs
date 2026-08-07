using MazeGame.Core;
using UnityEngine;
using MazeGame.Components;
using Unity.VisualScripting;

namespace MazeGame.Core
{
	public class Player: BaseCharacter
	{
		private Vector3 m_velocity = Vector3.zero;
		public bool m_onAir = false;
        public bool m_jumpAvailable = true;
		public float m_surfaceDot = 0;

        public Player(PlayerComponent comp)
		{
			this.m_characterInstance = comp.gameObject;
			this.m_rigidBody = this.m_characterInstance.GetComponent<Rigidbody>();
		}

		public void TurnPlayer(Vector2 mouseDelta)
		{
			m_characterInstance.transform.Rotate(0, mouseDelta.x * 0.2f, 0f); //TODO: add x rotation for looking up and down 
            Game.m_gameData.m_playerRotation = m_characterInstance.transform.rotation;
		}

		public void MovePlayer(Vector2 wasd)
		{
			//TODO: less force on air
			Vector3 v = new Vector3(wasd.x, 0f, wasd.y) * 1000f * Time.deltaTime;
            Vector3 f = m_rigidBody.transform.TransformVector(v);
            if (m_onAir)
			{
                f *= 0.3f;
            }
			if (m_rigidBody.linearVelocity.sqrMagnitude < 100f) //100 is max speed, add only if we're under it
			{
				Debug.Log($"Adding force: {f}");
				m_rigidBody.AddForce(f, ForceMode.Force);
				Game.m_gameData.m_playerPosition = m_rigidBody.position;
			}
		}

		public void JumpPlayer()
        {
            if (m_jumpAvailable)
            {
                if (m_surfaceDot > 0.5f)
                {
                    m_rigidBody.AddForce(Vector3.up * 5f, ForceMode.Impulse);
					return;
                }
				if (m_surfaceDot >= 0f)
				{
                    m_rigidBody.AddForce(Vector3.up * 3f, ForceMode.Impulse);
                    m_rigidBody.AddForce(Vector3.forward * 2f, ForceMode.Impulse);
                }
            }
        }

        public void Start()
		{
			m_rigidBody.linearVelocity = m_velocity;
		}

		public void Stop()
		{
			Debug.Log($"Rigidbody velocity: {m_rigidBody.linearVelocity}");
            m_velocity = m_rigidBody.linearVelocity; //store velocity for continued action when resuming
            m_rigidBody.linearVelocity = Vector3.zero;
        }
		
    }
}