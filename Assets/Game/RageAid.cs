using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class RageAid : MonoBehaviour {

	void Update () {
		GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
	}

	void OnTriggerEnter2D(Collider2D coll) {

		if (coll.gameObject.tag == "Player") {
			coll.gameObject.SendMessage("RageAid");
			GameObject.Destroy (gameObject);
		}
	}
}
