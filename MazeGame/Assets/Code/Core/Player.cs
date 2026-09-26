using MazeGame.Core;
using UnityEngine;
using MazeGame.Components;

namespace MazeGame.Core
{
	public class Player: BaseCharacter
	{
		private Vector3 m_velocity = Vector3.zero;
		public bool m_onAir = false;
		public bool m_onWall = false;
        public bool m_jumpAvailable = true;
        public float m_jumpForce = 5f;
        public float m_moveForce = 1f;
        public float m_surfaceDot = 0;
		public Vector3 m_surfaceNormal = Vector3.zero;

        public Player(PlayerComponent comp)
		{
			this.m_characterInstance = comp.gameObject;
			this.m_rigidBody = this.m_characterInstance.GetComponent<Rigidbody>();

            this.m_jumpForce = comp.m_jumpForce;
            this.m_moveForce = comp.m_moveForce;
        }

		public void TurnPlayer(Vector2 mouseDelta)
		{
			m_characterInstance.transform.Rotate(0, mouseDelta.x * 0.2f, 0f); //TODO: add x rotation for looking up and down 
            Game.m_gameData.m_playerRotation = m_characterInstance.transform.rotation;
		}

		public void MovePlayer(Vector2 wasd)
		{
            Vector3 v = new Vector3(wasd.x, 0f, wasd.y) * 10; // * Time.deltaTime;
            Vector3 f = m_rigidBody.transform.TransformVector(v) * m_moveForce;

            Debug.Log($"Player is on wall: {m_onWall}");
           
			if (m_rigidBody.linearVelocity.sqrMagnitude < 100f) //100 is max speed, add only if we're under it
			{
                if (m_onWall)
                {
                    float transformDot = Vector3.Dot(f, m_surfaceNormal);
                    //We could glue player to wall, and separate only with input or new collision enter.
                    if (transformDot < 0f) //If negative dot product, we are pushing towards the wall.
                    {
                        m_rigidBody.AddForce(f - (m_surfaceNormal * transformDot), ForceMode.Force);
                    }
                    else
                    {
                        m_rigidBody.AddForce(f, ForceMode.Force);
                    }
                }
                else if (m_onAir)
                {
                    m_rigidBody.AddForce(f * 0.3f, ForceMode.Force); //Add less force on air.
                }
                else
                {
                    m_rigidBody.AddForce(f, ForceMode.Force);
                }
				Game.m_gameData.m_playerPosition = m_rigidBody.position;
			}
		}

		public void JumpPlayer()
        {
            if (m_jumpAvailable)
            {
				// TODO: Take slopes into account. Just calculate vectors from normals.
				// TODO: make sure end force is always upwards, even when falling.

				/*
                if ( 0.05 < m_rigidBody.linearVelocity.sqrMagnitude && m_rigidBody.linearVelocity.sqrMagnitude < 100f) //100 is max speed, add only if we're under it
                {
                    m_rigidBody.AddForce(m_rigidBody.transform.forward * ff, ForceMode.Impulse);
                }
				*/

                if (!m_onWall)
                {
					Debug.Log("Floor jump. Force: " + m_jumpForce);
                    m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
					return;
                }

                else
                {
                    Debug.Log("Wall jump");
                    //TODO: calculate force off the wall based on slope.
                    m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
                    m_rigidBody.AddForce(m_surfaceNormal * m_jumpForce * 0.5f, ForceMode.Impulse);
                    return;
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