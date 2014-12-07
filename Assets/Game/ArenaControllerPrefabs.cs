using UnityEngine;

public class ArenaControllerPrefabs : MonoBehaviour {
	public Transform EnemyContainer;
	public Transform SpawnPoints;
	public Transform BulletContainer;

	public Transform chicken;
	public Transform knife;
	public Transform trap;
	public Transform boulder;

	static public ArenaControllerPrefabs instance;

	public void Start() {
		instance = this;
	}
}

