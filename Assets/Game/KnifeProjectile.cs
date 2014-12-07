using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class KnifeProjectile : MonoBehaviour {

	public Vector3 velocity;

	public virtual int Damage() {
		return 1;
	}

	void Update () {
		float angle = Mathf.Atan2(velocity.y, velocity.x) * Mathf.Rad2Deg;
		transform.localRotation = Quaternion.Euler(new Vector3(0, 0, angle));
		transform.localPosition = transform.localPosition + (velocity * Time.deltaTime);

		if (Vector3.Magnitude (velocity) < 5.0f) {
			velocity *= 1.2f;
		}
	}

	void OnCollisionEnter2D(Collision2D coll) {
		//Debug.Log ("BULLET COLLISION: "+coll.gameObject.tag);
		if (coll.gameObject.tag == "Wall") {
			GameObject.Destroy (this);
		}
	}

	void OnTriggerEnter2D(Collider2D coll) {
		//Debug.Log ("BULLET TRIGGER: "+coll.gameObject.tag);
		if (coll.gameObject.tag == "Wall") {
			GameObject.Destroy (gameObject);
		}

		if (coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Player") {
			coll.gameObject.SendMessage("ApplyDamage", Damage());
			GameObject.Destroy (gameObject);
		}
	}
}
