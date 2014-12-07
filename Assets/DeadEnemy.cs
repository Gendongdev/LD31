using UnityEngine;
using System.Collections;

public class DeadEnemy : MonoBehaviour {
	void Start () {

		foreach (Transform child in transform) {
			LeanTween.alpha (child.gameObject, 0, 1.0f).setDelay (5.0f);
		}

		LeanTween.alpha (gameObject, 0, 1.0f).setDelay (5.0f).setOnComplete (() => {
			GameObject.Destroy(gameObject);
		});
	}
}
