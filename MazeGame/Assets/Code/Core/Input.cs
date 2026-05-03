using UnityEngine;
using System.Collections;

namespace MazeGame.Core
{
	public class Input
	{

		public delegate void OnMoveHandler(Vector2 v);
		public event OnMoveHandler OnMove;
		public delegate void OnLookHandler(Vector2 v);
		public event OnMoveHandler OnLook;

		public static Input instance = null;

		private Input() //this will be a singleton. Can't call the constructor as its private.
        {
			Debug.Log("Input class created.");
		} 
		
		public static Input GetInstance()
		{
			if (instance == null)
			{
				instance = new Input();
			}
			return instance;
		}

		public void InputMove(Vector2 v)
		{
			OnMove?.Invoke(new Vector3(v.x, v.y, 0));
		}
		public void InputLook(Vector2 v)
		{
            OnLook?.Invoke(v);
        }

	}
}