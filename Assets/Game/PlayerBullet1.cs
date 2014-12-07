using UnityEngine;
using System.Collections;

public class PlayerBullet1 : PlayerProjectile {

	const float speed = 10.0f;

	public override int Damage() {
		return 1;
	}
	
	void Update () {
		transform.localPosition = transform.localPosition + (spawnDirection * speed * Time.deltaTime);
	}
}
