using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour {

	private int life = 2;
	private int damage = 1;
	private float speed = 0.02f;
	private int score = 10;

	public AudioSource audioHurt;
	public AudioSource audioDead;
	public AudioSource audioSpawn;

	public Transform DeadPrefab;

	private ParticleSystem onHitParticles;

	void Start () {
		if (OnHitParticleSystemName () != null) {
			onHitParticles = GameObject.Find (OnHitParticleSystemName ()).GetComponent<ParticleSystem> ();
		}
		if (onHitParticles != null) {
			onHitParticles.renderer.sortingLayerName = "fore";
		}

		EnemySpawn ();
	}

	void Update () {
		GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
	}

	void FixedUpdate () {
		EnemyUpdate ();
	}

	public void ApplyDamage (int dmg) {
		if (life > 0) {
			life -= dmg;
			SpriteRenderer spriteRenderer = GetComponent<SpriteRenderer>();
			spriteRenderer.color = new Color (1, 0, 0, 1);
			LeanTween.color (gameObject, new Color (1, 1, 1, 1), 0.5f).setEase (LeanTweenType.easeInOutBounce);

			if (onHitParticles != null) {
				onHitParticles.transform.localPosition = gameObject.transform.localPosition;
				onHitParticles.Emit (2);
			}

			if (audioHurt != null) {
				audioHurt.Play ();
			}

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


	protected virtual string OnHitParticleSystemName () {
		return null;
	}

	protected virtual void EnemySpawn () {

		if (audioSpawn != null && Random.Range(0, 100) < 25) {
			audioSpawn.Play ();
		}
	}

	protected virtual void EnemyDead () {

		if (DeadPrefab != null) {
			Transform spawn = (Transform)Instantiate (DeadPrefab, gameObject.transform.localPosition, Quaternion.identity);
			spawn.SetParent (ArenaControllerPrefabs.instance.EnemyContainer, false);
		}

		if (audioDead != null) {
			audioDead.Play ();
		}

		NotificationCenter.postNotification (null, "ENEMY_KILLED", NotificationCenter.Args("score", score));
		GameObject.Destroy (gameObject);
	}

	protected virtual void EnemyUpdate () {
		// by default, just move towards the player using the given speed
		float angle = MovementAngleToPlayer ();

		Vector3 baseVector = new Vector3 (1, 0, 0).RotateZ (MathR.DegreeToRadian (angle));
		transform.localPosition += baseVector * speed;

		float scaleX = Mathf.Abs(transform.localScale.x);
		if (baseVector.x < 0) {
			transform.localScale = new Vector3 (scaleX, transform.localScale.y, transform.localScale.z);
		} else {
			transform.localScale = new Vector3 (-scaleX, transform.localScale.y, transform.localScale.z);
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		//Debug.Log ("Enemy COLLISION: "+coll.gameObject.tag);
		if (coll.gameObject.tag == "Player") {
			coll.gameObject.SendMessage("ApplyDamage", damage);
		}
	}
		
}
