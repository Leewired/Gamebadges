using MazeGame.Core;
using UnityEngine;
using MazeGame.Components;
using System;

namespace MazeGame.Core
{
	public class Player: BaseCharacter
	{
        //Game state
        public bool m_hasKey = false;
        public bool m_playerBeatLevel = false;

        //Movement state
        private Vector3 m_velocity = Vector3.zero;
        private Vector3 m_currentMovementVector = Vector3.zero;
		public bool m_onAir = false;
		public bool m_onWall = false;
        public bool m_jumpAvailable = true;
        public bool m_sliding = false;

        //Movement data from component
        //Movement
        public float m_rotationSpeedMultiplier = 0.2f;
        public float m_moveForce = 10f;
        public float m_maxSpeed = 100f;
        public float m_lowVelocityThreshold = 0.1f;
        public float m_lowVelocityForceMultiplier = 2f;
        public float m_counterForceMultiplier = 1.5f;

        //Jumping
        public float m_jumpForce = 300f;
        public float m_maxUpwardsSpeed = 20f;
        public float m_airForceMultiplier = 0.3f;

        //Wall
        public float m_wallJumpForceUpwardsMultiplier = 1f;
        public float m_wallJumpForceOutwardsMultiplier = 0.5f;
        public float m_wallJumpForceForwardsMultiplier = 0.2f;
        public float m_wallJumpForceUpwardsCounterMultiplier = 20f;

        //Slide
        public float m_slideForce = 150f;
        public float m_slideMovementForceMultiplier = 0.6f;

        //Contacts
        public float m_surfaceDot = 0;
        public Vector3 m_surfaceNormal = Vector3.zero;
        public float m_lowestContactDot = 1f;
        public Vector3 m_lowestContactNormal = Vector3.zero;
        public Vector3 m_currentLinearVelocity = Vector3.zero;

        public Player(PlayerComponent comp)
		{
			this.m_characterInstance = comp.gameObject;
			this.m_rigidBody = this.m_characterInstance.GetComponent<Rigidbody>();

            this.m_jumpForce = comp.m_jumpForce;
            this.m_slideForce = comp.m_slideForce;
            this.m_slideMovementForceMultiplier = comp.m_slideMovementForceMultiplier;
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
            m_currentMovementVector = new Vector3(wasd.x, 0f, wasd.y) * 10; // * Time.deltaTime;
            Vector3 f = m_rigidBody.transform.TransformVector(m_currentMovementVector) * m_moveForce;
            m_currentLinearVelocity = m_rigidBody.linearVelocity;

            //TODO: Clean           
            if (m_currentLinearVelocity.sqrMagnitude >= m_maxSpeed) return; //100 is max speed, add only if we're under it
			
            if (m_onAir)
            {
                f *= m_airForceMultiplier;
            }
            else if (m_sliding)
            {
                f *= m_slideMovementForceMultiplier; //swap later
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
                Debug.Log("On the wall.");
                float transformDot = Vector3.Dot(f, m_surfaceNormal);
                //We could glue player to wall, and separate only with input or new collision enter.
                //TODO: remove outwards force unless pushing wasd outwards, jumping (and later getting hit)
                //TODO: add force to push along wall
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
                //check if we're pushing against our current direction and if so, add a multiplier to the force to make it easier to turn around.
                if(Vector3.Dot(f, m_currentLinearVelocity) < 0f)
                {
                    f *= m_counterForceMultiplier;
                }
                if (m_lowestContactDot == 1f || m_lowestContactDot == m_surfaceDot)//there's no other contact
                {
                    m_rigidBody.AddForce(f, ForceMode.Force);
                    return;
                }
                float secondColliderDot = Vector3.Dot(f, m_lowestContactNormal);
                if (secondColliderDot < 0f) //If negative dot product, we are pushing towards the wall.
                {
                    m_rigidBody.AddForce((f - (m_lowestContactNormal * secondColliderDot)) * 2f, ForceMode.Force); //TODO: add multiplier based on the dot product.
                    Debug.Log($"Force: {f - (m_lowestContactNormal * secondColliderDot) * 2f}");
                }
                else
                {
                    m_rigidBody.AddForce(f, ForceMode.Force);
                }
            }

			Game.m_gameData.m_playerPosition = m_rigidBody.position; //save to gameData
		}

		public void JumpPlayer()
        {
            if (!m_jumpAvailable) return;
            if (m_sliding)
            {
                m_sliding = false;
            }

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
                    //add upwards force based on the current downward velocity to always jump upwards
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

        public void SlidePlayer()
        {
            if (!m_sliding && !m_onAir)
            {
                m_sliding = true;
                Vector3 f = m_rigidBody.transform.TransformVector(m_currentMovementVector) * m_slideForce;
                m_rigidBody.AddForce(f, ForceMode.Impulse);
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