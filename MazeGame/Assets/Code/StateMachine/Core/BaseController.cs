using UnityEngine;
using System.Collections;
using UnityEditor.Search;

namespace MazeGame.Core
{
	public class BaseController: MonoBehaviour
	{

		public GameObject m_visualRoot;
		public GameObject m_logicRoot;
		public GameObject m_renderingRoot;

		virtual public void Start()
		{
			m_visualRoot = Game.GetGameObject(this.gameObject.scene, "Visual");
			m_logicRoot = Game.GetGameObject(this.gameObject.scene, "Logic");
			m_renderingRoot = Game.GetGameObject(this.gameObject.scene, "Rendering");
		}

		virtual public void Activate()
		{
			if (m_logicRoot != null)
			{
				m_logicRoot.SetActive(true);
			}
			Show();
		}

		virtual public void Deactivate()
		{

			if (m_logicRoot != null)
			{
				m_logicRoot.SetActive(false);
			}
			Hide();
		}

		virtual public void Hide()
		{
			if (m_visualRoot != null)
			{
				m_visualRoot.SetActive(false);
			}
			
			if (m_renderingRoot != null)
			{
				m_visualRoot.SetActive(false);
			}
		}

		virtual public void Show()
		{
			if (m_visualRoot != null)
			{
				m_visualRoot.SetActive(true);
			}
			
			if (m_renderingRoot != null)
			{
				m_visualRoot.SetActive(true);
			}
		}
		
	}
}