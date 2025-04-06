using UnityEngine;

// ReSharper disable Unity.PerformanceCriticalCodeInvocation

public class SceneInfo{
	public static string currScene;
	private static RectTransform _canvasRectTransform;

	public static RectTransform canvasRectTransform{
		get{
			if (!_canvasRectTransform){
				_canvasRectTransform = GameObject.Find("MainCanvas").GetComponent<RectTransform>();
				if (!_canvasRectTransform){
					_canvasRectTransform = GameObject.Find("Canvas").GetComponent<RectTransform>();
				}
			}

			return _canvasRectTransform;
		}
		set{ _canvasRectTransform = value; }
	}
}