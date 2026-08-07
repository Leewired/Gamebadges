using UnityEngine;
using System.Collections;
using Unity.VisualScripting;

namespace MazeGame.Core
{
	public class Input
	{

		public delegate void OnMoveHandler(Vector2 v);
		public event OnMoveHandler OnMove;
		public delegate void OnLookHandler(Vector2 v);
		public event OnLookHandler OnLook;
		public delegate void OnJumpHandler();
		public event OnJumpHandler OnJump;
		
		public delegate void OnAcceptHandler();
        public event OnAcceptHandler OnAccept;

		public delegate void OnPauseHandler();
		public event OnPauseHandler OnPause;

        public static Input instance = null;

		private Input() //this will be a singleton. Can't call the constructor as it's private.
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
		public void InputJump() //TODO: add float parameter for jump height if needed
        {
            OnJump?.Invoke();
        }
		public void InputAccept()
		{
            OnAccept?.Invoke();
        }
		public void InputPause()
		{
			OnPause?.Invoke();
		}

	}
}