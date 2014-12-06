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

	private Vector2 velocity;
	private float reloadTime;
	private float currentWeaponReload = 0.25f;

	private SpriteRenderer spriteRenderer; 

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
	
	void Update () {

		// Movement by AWSD keyboard
		float speed = Time.deltaTime * 0.5f;
		if (Input.GetKey ("w")) {
			velocity += new Vector2 (0, speed);
		}
		if (Input.GetKey ("a")) {
			velocity += new Vector2 (-speed, 0);
		}
		if (Input.GetKey ("s")) {
			velocity += new Vector2 (0, -speed);
		}
		if (Input.GetKey ("d")) {
			velocity += new Vector2 (speed, 0);
		}

		velocity *= 0.7f;

		Vector3 pos = gameObject.transform.localPosition;
		pos.x += velocity.x;
		pos.y += velocity.y;
		transform.localPosition = pos;

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


		if (Input.GetMouseButton (0)) {
			CheckFireWeapon (angle);
		}

	}

	public void CheckFireWeapon(float playerAngle) {
		reloadTime -= Time.deltaTime;

		if (reloadTime <= 0.0f) {
			reloadTime = currentWeaponReload;

			Vector3 baseVector = new Vector3 (1, 0, 0);
			baseVector = baseVector.RotateZ (MathR.DegreeToRadian (playerAngle));

			Instantiate (bullet1, transform.localPosition + (baseVector * 0.4f), Quaternion.Euler(new Vector3(0, 0, playerAngle)));
			PlayerProjectile projectile = bullet1.GetComponent<PlayerProjectile> ();
			projectile.spawnDirection = baseVector;

		}
	}
}
