using UnityEngine;

namespace MazeGame.Core
{
	public class Player: BaseCharacter
	{

		public Player(PlayerComponent comp)
		{
			this.m_characterInstance = comp.gameObject;
			this.m_rigidBody = this.m_characterInstance.GetComponent<Rigidbody>();
		}

		public void TurnPlayer(Vector2 mouseDelta)
		{
			m_characterInstance.transform.Rotate(0f, mouseDelta.x * 0.2f, 0f);
		}

		public void MovePlayer(Vector2 wasd)
		{
			Vector3 v = new Vector3(wasd.x, 0f, wasd.y) * 1000f * Time.deltaTime;
			Vector3 f = m_rigidBody.transform.TransformVector(v);
			if (m_rigidBody.linearVelocity.sqrMagnitude < 100f) //100 is max speed, add only if we're under it
			{
				m_rigidBody.AddForce(f, ForceMode.Force);
			}
		}

	}
}