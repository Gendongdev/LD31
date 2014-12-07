using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class SpikeTrap : MonoBehaviour {

	public Vector3 velocity;

	public AudioSource trapDamage;

	public virtual int Damage() {
		return 1;
	}

	void Start() {
		trapDamage.Play ();

		LeanTween.alpha (gameObject, 0, 0.5f).setDelay (10).setOnComplete (() => {
			GameObject.Destroy(gameObject);
		});
	}

	void OnCollisionEnter2D(Collision2D coll) {
	}

	void OnTriggerEnter2D(Collider2D coll) {
		if (coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Player") {
			coll.gameObject.SendMessage("ApplyDamage", Damage());

			// Push whoever they are away from me...
			Vector3 diff = coll.gameObject.transform.localPosition - gameObject.transform.localPosition;
			coll.gameObject.rigidbody2D.AddForce (diff * 400.0f);

			trapDamage.Play ();
		}
	}
}
