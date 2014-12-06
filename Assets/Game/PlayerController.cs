using UnityEngine;

public static class Vector3RotationExtension
{
	public static Vector3 RotateLeft(this Vector3 v)
	{
		float x = -v.y;
		float y = v.x;
		v.x = x;
		v.y = y;
		return v;
	}

	public static Vector3 RotateRight(this Vector3 v)
	{
		float x = v.y;
		float y = v.x;
		v.x = x;
		v.y = y;
		return v;
	}

	public static Vector3 RotateLeftAboutY(this Vector3 v)
	{
		float x = -v.z;
		float z = v.x;
		v.x = x;
		v.z = z;
		return v;
	}

	public static Vector3 RotateRightAboutY(this Vector3 v)
	{
		float x = v.z;
		float z = -v.x;
		v.x = x;
		v.z = z;
		return v;
	}

	public static Vector3 RotateZ(this Vector3 v, float radians)
	{
		float x = v.x * Mathf.Cos (radians) - v.y * Mathf.Sin (radians);
		float y = v.x * Mathf.Sin (radians) + v.y * Mathf.Cos (radians);
		v.x = x;
		v.y = y;
		return v;
	}
}

public static class Vector2RotationExtension
{
	public static Vector2 RotateLeft(this Vector2 v)
	{
		float x = -v.y;
		float y = v.x;
		return new Vector2(x, y);
	}

	public static Vector2 RotateZ(this Vector2 v, float radians)
	{
		float x = v.x * Mathf.Cos (radians) - v.y * Mathf.Sin (radians);
		float y = v.x * Mathf.Sin (radians) + v.y * Mathf.Cos (radians);
		return new Vector2(x, y);
	}
}


public class PlayerController : MonoBehaviour {

	public static PlayerController player;

	private float reloadTime;
	private float currentWeaponReload = 0.25f;

	private SpriteRenderer spriteRenderer; 

	public Transform BulletContainer;

	public float movementForce = 20.0f;

	public Sprite spriteFace0;
	public Sprite spriteFace1;
	public Sprite spriteFace2;
	public Sprite spriteFace3;
	public Sprite spriteFace4;
	public Sprite spriteFace5;
	public Sprite spriteFace6;
	public Sprite spriteFace7;

	public Transform bullet1;



	void Start () {
		player = this;

		spriteRenderer = GetComponent<SpriteRenderer>();
		if (spriteRenderer.sprite == null)
			spriteRenderer.sprite = spriteFace0;
	}

	float FiringAngle () {
		Vector2 mousePos = Input.mousePosition;
		Camera camera = Camera.main;

		Vector3 worldPos = camera.ScreenToWorldPoint(new Vector3(mousePos.x, mousePos.y, Camera.main.transform.position.z));
		worldPos.x = (-worldPos.x) - transform.localPosition.x;
		worldPos.y = (-worldPos.y) - transform.localPosition.y;
		return Mathf.Atan2(worldPos.y, worldPos.x) * Mathf.Rad2Deg;
	}
	
	void FixedUpdate () {

		// Movement by AWSD keyboard
		if (Input.GetKey ("w")) {
			rigidbody2D.AddForce(new Vector2(0, movementForce));
		}
		if (Input.GetKey ("a")) {
			rigidbody2D.AddForce(new Vector2(-movementForce, 0));
		}
		if (Input.GetKey ("s")) {
			rigidbody2D.AddForce(new Vector2(0, -movementForce));
		}
		if (Input.GetKey ("d")) {
			rigidbody2D.AddForce(new Vector2(movementForce, 0));
		}

		// Fire gun by mouse
		float angle = FiringAngle ();

		if ((angle >= -22 && angle < 0.0f) || (angle <= 22 && angle >= 0.0f)) {
			spriteRenderer.sprite = spriteFace0;
		} else if (angle >= 22 && angle <= 67) {
			spriteRenderer.sprite = spriteFace1;
		} else if (angle >= 67 && angle <= 112) {
			spriteRenderer.sprite = spriteFace2;
		} else if (angle >= 112 && angle <= 157) {
			spriteRenderer.sprite = spriteFace3;
		} else if (angle >= 157 || angle <= -157) {
			spriteRenderer.sprite = spriteFace4;
		} else if (angle >= -67 && angle <= -22) {
			spriteRenderer.sprite = spriteFace7;
		} else if (angle >= -112 && angle <= -67) {
			spriteRenderer.sprite = spriteFace6;
		} else if (angle >= -157 && angle <= -112) {
			spriteRenderer.sprite = spriteFace5;
		}


		reloadTime -= Time.deltaTime;
		if (Input.GetMouseButton (0)) {
			CheckFireWeapon (angle);
		}

	}

	public void CheckFireWeapon(float playerAngle) {
		if (reloadTime <= 0.0f) {
			reloadTime = currentWeaponReload;

			Vector3 baseVector = new Vector3 (1, 0, 0);
			baseVector = baseVector.RotateZ (MathR.DegreeToRadian (playerAngle));

			Transform bulletClone = (Transform)Instantiate (bullet1, transform.localPosition + (baseVector * 0.4f), Quaternion.Euler(new Vector3(0, 0, playerAngle)));
			PlayerProjectile projectile = bulletClone.GetComponent<PlayerProjectile> ();
			projectile.spawnDirection = new Vector3 (baseVector.x, baseVector.y, 0);
			bulletClone.SetParent (BulletContainer, false);
		}
	}
}
