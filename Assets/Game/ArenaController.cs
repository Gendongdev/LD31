using UnityEngine;
using UnityEditor.VersionControl;

public class ArenaController : MonoBehaviour {

	public Transform EnemyContainer;
	public Transform SpawnPoints;
	public Transform BulletContainer;

	public Transform chicken;


	float chickenSpawnTimer = 0;

	void Start () {
		TwitchController.onMessageReceived = (name, message) => {
			Debug.Log("message: "+message);
			if(message.Contains("!chicken")){
				SpawnChicken ();
			}
		};
	}
	
	void Update () {

	}


	public Vector3 GetSpawnLocation() {
		int childIndex = Random.Range(0, SpawnPoints.childCount);
		return SpawnPoints.GetChild (childIndex).localPosition;
	}

	public void SpawnChicken() {
		Transform spawn = (Transform)Instantiate (chicken, GetSpawnLocation(), Quaternion.identity);
		spawn.SetParent (EnemyContainer, false);
	}

}
