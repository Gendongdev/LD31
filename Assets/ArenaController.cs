using UnityEngine;

public class ArenaController : MonoBehaviour {

	public Transform EnemyContainer;
	public Transform SpawnPoints;
	public Transform BulletContainer;

	public Transform chicken;


	float chickenSpawnTimer = 0;

	void Start () {
	
	}
	
	void Update () {
		chickenSpawnTimer -= Time.deltaTime;
		if (chickenSpawnTimer < 0) {
			chickenSpawnTimer = 3.0f;

			SpawnObject ("!chicken");
		}
	}


	public Vector3 GetSpawnLocation() {
		int childIndex = Random.Range(0, SpawnPoints.childCount);
		return SpawnPoints.GetChild (childIndex).localPosition;
	}

	public void SpawnChicken() {
		Transform spawn = (Transform)Instantiate (chicken, GetSpawnLocation(), Quaternion.identity);
		spawn.SetParent (EnemyContainer, false);
	}

	public void SpawnObject(string objectName) {
		if (objectName.Equals ("!chicken")) {
			SpawnChicken ();
		}
	}
}
