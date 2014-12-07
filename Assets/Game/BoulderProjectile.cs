using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class BoulderProjectile : MonoBehaviour {

	private Vector3 velocity = new Vector3 (0, -0.1f, 0);
	private float introAnim;

	private Transform shadow;
	private Vector3 startBoulderPosition;
	private Vector3 startShadowPosition;

	public virtual int Damage() {
		return 1;
	}

	void Start() {

		// we want to start the first second or two showing a growing shadow, then plop the bolder on it
		shadow = transform.GetChild (0);

		startBoulderPosition = transform.localPosition;
		startShadowPosition = shadow.transform.localPosition;

		transform.localPosition = new Vector3 (startBoulderPosition.x, startBoulderPosition.y + 10, startBoulderPosition.z);
		shadow.transform.localPosition = new Vector3 (startShadowPosition.x, startShadowPosition.y - 10, startShadowPosition.z);

		shadow.transform.localScale = Vector3.zero;
	}

	void Update () {
		const float delay = 3.5f;

		introAnim += Time.deltaTime;

		if (introAnim > delay) {
			transform.localPosition = transform.localPosition + (velocity * Time.deltaTime);

			if (Vector3.Magnitude (velocity) < 3.0f) {
				velocity *= 1.2f;
			}

			if (transform.localPosition.y < -5) {
				GameObject.Destroy (gameObject);
			}
		} else {
			float t = (delay - introAnim) / delay;

			t = LeanTween.easeInBounce (0, 1, t);

			transform.localPosition = new Vector3 (startBoulderPosition.x, startBoulderPosition.y + 10 * t, startBoulderPosition.z);
			shadow.transform.localPosition = new Vector3 (startShadowPosition.x, startShadowPosition.y - 10 * t, startShadowPosition.z);

			shadow.transform.localScale = new Vector3 (10.0f * t + 5.0f, 10.0f * t + 5.0f, 10.0f * t + 5.0f);
		}
	}
		
	void OnTriggerEnter2D(Collider2D coll) {
		//Debug.Log ("BULLET TRIGGER: "+coll.gameObject.tag);
		if (coll.gameObject.tag == "Enemy" || coll.gameObject.tag == "Player") {
			coll.gameObject.SendMessage("ApplyDamage", Damage());

			// Push whoever they are away from me...
			Vector3 diff = coll.gameObject.transform.localPosition - gameObject.transform.localPosition;
			coll.gameObject.rigidbody2D.AddForce (diff * 400.0f);
		}
	}
}
