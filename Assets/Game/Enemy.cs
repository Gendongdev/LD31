using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	public int life = 1;
	public int damage = 1;
	public float speed = 0.2f;
	public int score = 10;


	void Start () {

	}
	
	void FixedUpdate () {
		EnemyUpdate ();
	}

	public void ApplyDamage (int dmg) {
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
		NotificationCenter.postNotification (null, "ENEMY_KILLED", NotificationCenter.Args("score", score));
		GameObject.Destroy (gameObject);
	}

	protected virtual void EnemyUpdate () {
		// by default, just move towards the player using the given speed
		float angle = MovementAngleToPlayer ();

		Vector3 baseVector = new Vector3 (1, 0, 0).RotateZ (MathR.DegreeToRadian (angle));
		transform.localPosition += baseVector * speed;
	}

	void OnCollisionEnter2D(Collision2D coll) {
		//Debug.Log ("Enemy COLLISION: "+coll.gameObject.tag);
		if (coll.gameObject.tag == "Player") {
			coll.gameObject.SendMessage("ApplyDamage", damage);
		}
	}
		
}
