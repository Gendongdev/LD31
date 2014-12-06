using UnityEngine;
using System.Collections;

public class PlayerBullet1 : PlayerProjectile {

	const float speed = 10.0f;

	void Start () {
	
	}
	
	void Update () {
		transform.localPosition = transform.localPosition + (spawnDirection * speed * Time.deltaTime);
	}
}
