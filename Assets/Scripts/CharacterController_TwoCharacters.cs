using UnityEngine;

public class CharacterController_TwoCharacters : MonoBehaviour{
	public GameObject otherCharacter;
	public LineRenderer line;
	public float maxDist = 5, speed = 1;
	public KeyCode[] keys;

	void Update(){
		var v = Vector3.zero;
		try{
			if (Input.GetKey(keys[0])){
				v += Vector3.forward;
			}

			if (Input.GetKey(keys[1])){
				v += Vector3.back;
			}

			if (Input.GetKey(keys[2])){
				v += Vector3.left;
			}

			if (Input.GetKey(keys[3])){
				v += Vector3.right;
			}

			v = (Quaternion.Euler(new Vector3(0, CameraController_TwoCharacters.instance.transform.eulerAngles.y, 0)) * v).normalized * (speed * Time.deltaTime);

			var newPos = transform.position + v;
			var dir = newPos - otherCharacter.transform.position;
			var dist = dir.magnitude;
			if (dist > maxDist){
				transform.position = otherCharacter.transform.position + dir.normalized * maxDist;
			}
			else{
				transform.position = newPos;
			}

			if (line){
				line.SetPosition(0, transform.position);
				line.SetPosition(1, otherCharacter.transform.position);
				line.startColor = Color.Lerp(Color.cyan, Color.red, dist / maxDist);
				line.endColor = line.startColor;
			}
		}
		finally{ }
	}
}