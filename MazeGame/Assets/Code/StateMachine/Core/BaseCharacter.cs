using UnityEngine;

namespace MazeGame.Core
{
	public class BaseCharacter
	{

		public GameObject m_characterInstance = null;
		public Rigidbody m_rigidBody = null;

		public BaseCharacter()
		{
			Debug.Log(string.Format("Class {0} created.", this.GetType().ToString()));
		}
		
	}
}