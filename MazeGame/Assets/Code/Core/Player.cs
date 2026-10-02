using MazeGame.Core;
using UnityEngine;
using MazeGame.Components;

namespace MazeGame.Core
{
	public class Player: BaseCharacter
	{
        public bool m_hasKey = false;
        public bool m_playerBeatLevel = false;
        private Vector3 m_velocity = Vector3.zero;
		public bool m_onAir = false;
		public bool m_onWall = false;
        public bool m_jumpAvailable = true;
        public float m_jumpForce = 300f;
        public float m_moveForce = 10f;
        public float m_maxSpeed = 100f;
        public float m_airForceMultiplier = 0.3f;
        public float m_maxUpwardsSpeed = 20f;
        public float m_lowVelocityThreshold = 0.1f;
        public float m_lowVelocityForceMultiplier = 2f;
        public float m_counterForceMultiplier = 1.5f;
        public float m_wallJumpForceUpwardsMultiplier = 1f;
        public float m_wallJumpForceOutwardsMultiplier = 0.5f;
        public float m_wallJumpForceForwardsMultiplier = 0.2f;
        public float m_wallJumpForceUpwardsCounterMultiplier = 20f;
        public float m_rotationSpeedMultiplier = 0.2f;
        public float m_surfaceDot = 0;
		public Vector3 m_surfaceNormal = Vector3.zero;
        public Vector3 m_currentLinearVelocity = Vector3.zero;

        public Player(PlayerComponent comp)
		{
			this.m_characterInstance = comp.gameObject;
			this.m_rigidBody = this.m_characterInstance.GetComponent<Rigidbody>();

            this.m_jumpForce = comp.m_jumpForce;
            this.m_moveForce = comp.m_moveForce;
            this.m_maxSpeed = comp.m_maxSpeed;
            this.m_wallJumpForceUpwardsMultiplier = comp.m_wallJumpForceUpwardsMultiplier;
            this.m_wallJumpForceUpwardsCounterMultiplier = comp.m_wallJumpForceUpwardsCounterMultiplier;
            this.m_rotationSpeedMultiplier = comp.m_rotationSpeedMultiplier;
            this.m_airForceMultiplier = comp.m_airForceMultiplier;
            this.m_lowVelocityForceMultiplier = comp.m_lowVelocityForceMultiplier;
            this.m_lowVelocityThreshold = comp.m_lowVelocityThreshold;
            this.m_counterForceMultiplier = comp.m_counterForceMultiplier;
            this.m_maxUpwardsSpeed = comp.m_maxUpwardsSpeed;
            this.m_wallJumpForceForwardsMultiplier = comp.m_wallJumpForceForwardsMultiplier;
        }

		public void TurnPlayer(Vector2 mouseDelta)
		{
			m_characterInstance.transform.Rotate(0, mouseDelta.x * m_rotationSpeedMultiplier, 0f); //TODO: add x rotation for looking up and down 
            Game.m_gameData.m_playerRotation = m_characterInstance.transform.rotation;
		}

		public void MovePlayer(Vector2 wasd)
		{
            Vector3 v = new Vector3(wasd.x, 0f, wasd.y) * 10; // * Time.deltaTime;
            Vector3 f = m_rigidBody.transform.TransformVector(v) * m_moveForce;
            m_currentLinearVelocity = m_rigidBody.linearVelocity;

            //TODO: Clean           
            if (m_currentLinearVelocity.sqrMagnitude >= m_maxSpeed) return; //100 is max speed, add only if we're under it
			
            if (m_onAir)
            {
                f *= m_airForceMultiplier;
            }
            else if (m_currentLinearVelocity.sqrMagnitude < m_lowVelocityThreshold) //If player is almost stopped, add more force
            {
                f *= m_lowVelocityForceMultiplier;
            }
            else
            {
                f *= 1f;
            }
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
            else
            {
                //Check if we're pushing against our current direction and if so, add a multiplier to the force to make it easier to turn around.
                if(Vector3.Dot(f, m_currentLinearVelocity) < 0f)
                {
                    f *= m_counterForceMultiplier;
                }
                m_rigidBody.AddForce(f, ForceMode.Force);
            }

			Game.m_gameData.m_playerPosition = m_rigidBody.position; //save to gameData
		}

		public void JumpPlayer()
        {
            if (!m_jumpAvailable) return;

            // TODO: Take slopes into account. Just calculate vectors from normals.
            // TODO: add max velocity upwards

            // TODO: check if we're pushing w (or s) and if so, add more force forwards to the jump.
            // add force forwards
            if ( 0.05 < m_rigidBody.linearVelocity.sqrMagnitude && m_rigidBody.linearVelocity.sqrMagnitude < 100f) //100 is max speed, add only if we're under it
            {
                m_rigidBody.AddForce(m_rigidBody.transform.forward * m_wallJumpForceForwardsMultiplier, ForceMode.Impulse);
            }

            if (!m_onWall)
            {
                m_rigidBody.AddForce(Vector3.up * m_jumpForce, ForceMode.Impulse);
            }
            else //we are on the wall
            {
                //TODO: calculate force off the wall based on slope.
                //check if linear velocity is downwards, to add counter force
                if (m_currentLinearVelocity.y < 0f)
                {
                    //Add upwards force based on the current downward velocity to always jump upwards
                    m_rigidBody.AddForce(Vector3.up * m_jumpForce * m_wallJumpForceUpwardsMultiplier + (new Vector3(0f, -m_currentLinearVelocity.y, 0f) * m_wallJumpForceUpwardsCounterMultiplier), ForceMode.Impulse);
                }
                else
                {
                    m_rigidBody.AddForce(Vector3.up * m_jumpForce * m_wallJumpForceUpwardsMultiplier, ForceMode.Impulse);
                }
                //add force outwards of the wall
                m_rigidBody.AddForce(m_surfaceNormal * m_jumpForce * m_wallJumpForceOutwardsMultiplier, ForceMode.Impulse);
            }
        }

        public void Start()
		{
			m_rigidBody.linearVelocity = m_velocity;
		}

		public void Stop()
		{
            m_velocity = m_rigidBody.linearVelocity; //store velocity for continued action when resuming
            m_rigidBody.linearVelocity = Vector3.zero;
        }
		
    }
}