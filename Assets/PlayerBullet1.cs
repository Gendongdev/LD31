using UnityEngine;
using System.Collections;

public class PlayerBullet1 : PlayerProjectile {

	const float speed = 3.0f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.localPosition = transform.localPosition + (spawnDirection * speed * Time.deltaTime);
	}
}
