using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public float life = 1;
	public float damage = 1;
	public float speed = 0.2f;


	void Start () {

	}
	
	void FixedUpdate () {
		EnemyUpdate ();
	}

	public void ApplyDamage (float dmg) {
		if (life > 0) {
			life -= dmg;
			if (life <= 0) {
				EnemyDead ();
			}
		}
	}

	float MovementAngleToPlayer () {
		Vector3 worldPos = PlayerController.player.gameObject.transform.localPosition;
		worldPos.x = worldPos.x - transform.localPosition.x;
		worldPos.y = worldPos.y - transform.localPosition.y;
		return Mathf.Atan2(worldPos.y, worldPos.x) * Mathf.Rad2Deg;
	}


	protected virtual void EnemySpawn () {

	}

	protected virtual void EnemyDead () {
		GameObject.Destroy (gameObject);
	}

	protected virtual void EnemyUpdate () {
		// by default, just move towards the player using the given speed
		float angle = MovementAngleToPlayer ();

		Vector3 baseVector = new Vector3 (1, 0, 0).RotateZ (MathR.DegreeToRadian (angle));
		transform.localPosition += baseVector * speed;
	}
}
