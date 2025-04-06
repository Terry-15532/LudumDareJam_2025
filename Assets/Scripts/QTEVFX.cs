using UnityEngine;
using UnityEngine.Serialization;

public enum QTERanking{
	Perfect,
	Good,
	Failed
}

public class QTEVFX : MonoBehaviour{
	public ParticleSystem[] particles;
	public QTERanking rank;
	public ParticleSystem.MainModule main;

	public void Start(){
		particles = GetComponentsInChildren<ParticleSystem>();
		foreach (var p in particles){
			main = p.main;
			if (rank == QTERanking.Perfect){
				main.startColor = new Color(0.95f, 0.8f, 0.2f);
			}
			else if (rank == QTERanking.Good){
				main.startColor = new Color(0.4f, 0.7f, 0.9f);
			}
			else{
				main.startColor = new Color(0.8f, 0.1f, 0.1f);
			}
		}

		Tools.CallDelayed(() => { Destroy(gameObject); }, 1f);
	}

	public static QTEVFX Create(QTERanking ranking){
		var vfx = Instantiate(ResourceManager.LoadPrefab<QTEVFX>(), SceneInfo.canvasRectTransform);
		vfx.transform.localPosition = new Vector2(0, 0);
		// vfx.transform.localScale = new Vector3(1, 1, 1);
		vfx.rank = ranking;
		return vfx;
	}
}