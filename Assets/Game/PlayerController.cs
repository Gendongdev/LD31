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

	public AudioSource gunFire;

	public AudioClip rageAid;

	public AudioClip[] hurtSounds; 
	public AudioClip[] tauntSounds; 

	private int life = 10;
	public float movementForce = 20.0f;

	public Sprite spriteFace0;
	public Sprite spriteFace1;
	public Sprite spriteFace2;
	public Sprite spriteFace3;
	public Sprite spriteFace4;
	public Sprite spriteFace5;
	public Sprite spriteFace6;
	public Sprite spriteFace7;

	public Transform casing;

	public Transform bullet1;

	public ParticleSystem muzzleFlashParticleSystem;

	public float cameraShake = 0.0f;
	public float cameraWiggleT = 0.0f;

	public float ignoreDamageTimer;
	public float rageTimer;


	public void RageAid() {
		spriteRenderer.color = new Color (0.5f, 0.5f, 1, 1);
		LeanTween.color (gameObject, new Color (1, 1, 1, 1), 5.0f).setEase (LeanTweenType.easeInSine);
		rageTimer = 5.0f;

		gunFire.PlayOneShot (rageAid);
	}

	public void ApplyDamage (float dmg) {
		// ignore all damage while we are flashing red
		if (ignoreDamageTimer > 0.0f) {
			return;
		}

		if (life > 0) {
			life -= (int)dmg;
			cameraShake = 0.3f;

			spriteRenderer.color = new Color (1, 0, 0, 1);
			LeanTween.color (gameObject, new Color (1, 1, 1, 1), 0.5f).setEase (LeanTweenType.easeInOutBounce);
			ignoreDamageTimer = 0.5f;

			int r = Random.Range (0, hurtSounds.Length);
			gunFire.PlayOneShot (hurtSounds [r]);

			NotificationCenter.postNotification (null, "PLAYER_LIFE_UPDATE", NotificationCenter.Args("life", life));
		}
	}

	void Start () {
		player = this;

		muzzleFlashParticleSystem.renderer.sortingLayerName = "fore";

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
		if (ignoreDamageTimer > 0.0f) {
			ignoreDamageTimer -= Time.deltaTime;
		}
		if (rageTimer > 0.0f) {
			rageTimer -= Time.deltaTime;
		}
		GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(transform.position.y * 100f) * -1;
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

		cameraShake *= 0.98f;

		cameraWiggleT += Time.fixedDeltaTime * 20.0f;

		float wiggleX = (Wiggle.value (1.0f, cameraWiggleT, 32432.0f) - 0.5f)*0.2f*cameraShake;
		float wiggleY = (Wiggle.value (1.0f, cameraWiggleT, 32432.0f) - 0.5f)*0.2f*cameraShake;
		float wiggleZ = (Wiggle.value (1.0f, cameraWiggleT, 32432.0f) - 0.5f)*0.2f*cameraShake;

		Vector3 camPos = new Vector3 (wiggleX, wiggleY, -4.9f + wiggleZ);

		Camera.main.transform.localPosition = camPos;
	}

	public void CheckFireWeapon(float playerAngle) {
		if (reloadTime <= 0.0f) {

			Vector3[] muzzleOffset = { 
				new Vector3 (38.0f/100.0f, 14.0f/100.0f, 0.0f),
				new Vector3 (38.0f/100.0f, 41.0f/100.0f, 0.0f),
				new Vector3 (-11.0f/100.0f, 37.0f/100.0f, 0.0f),
				new Vector3 (-37.0f/100.0f, 40.0f/100.0f, 0.0f),
				new Vector3 (-37.0f/100.0f, 13.0f/100.0f, 0.0f),
				new Vector3 (-33.0f/100.0f, -9.0f/100.0f, 0.0f),
				new Vector3 (11.0f/100.0f, -11.0f/100.0f, 0.0f),
				new Vector3 (34.0f/100.0f, -9.0f/100.0f, 0.0f)
			};

			int fadeIdx = 0;
			if ((playerAngle >= -22 && playerAngle < 0.0f) || (playerAngle <= 22 && playerAngle >= 0.0f)) {
				fadeIdx = 0;
			} else if (playerAngle >= 22 && playerAngle <= 67) {
				fadeIdx = 1;
			} else if (playerAngle >= 67 && playerAngle <= 112) {
				fadeIdx = 2;
			} else if (playerAngle >= 112 && playerAngle <= 157) {
				fadeIdx = 3;
			} else if (playerAngle >= 157 || playerAngle <= -157) {
				fadeIdx = 4;
			} else if (playerAngle >= -67 && playerAngle <= -22) {
				fadeIdx = 7;
			} else if (playerAngle >= -112 && playerAngle <= -67) {
				fadeIdx = 6;
			} else if (playerAngle >= -157 && playerAngle <= -112) {
				fadeIdx = 5;
			}

			if (rageTimer > 0) {
				reloadTime = 0.06125f;
			} else {
				reloadTime = 0.25f;
			}

			Vector3 baseVector = new Vector3 (1, 0, 0);
			baseVector = baseVector.RotateZ (MathR.DegreeToRadian (playerAngle));

			Transform bulletClone = (Transform)Instantiate (bullet1, transform.localPosition + muzzleOffset[fadeIdx], Quaternion.Euler(new Vector3(0, 0, playerAngle)));
			PlayerProjectile projectile = bulletClone.GetComponent<PlayerProjectile> ();
			projectile.spawnDirection = new Vector3 (baseVector.x, baseVector.y, 0);
			bulletClone.SetParent (BulletContainer, false);

			rigidbody2D.AddForce(new Vector2(-baseVector.x*movementForce, -baseVector.y*movementForce));


			muzzleFlashParticleSystem.transform.localPosition = bulletClone.transform.localPosition;
			muzzleFlashParticleSystem.Emit (6);

			cameraShake = 0.1f;
			
			CreateShellCasing(transform.localPosition, baseVector);

			gunFire.Play ();


			if (Random.Range (0, 100) <= 2) {
				int r = Random.Range (0, tauntSounds.Length);
				gunFire.PlayOneShot (tauntSounds [r]);
			}
			
		}
	}

	public void CreateShellCasing(Vector3 pos, Vector3 direction) {
		Transform shell = (Transform)Instantiate (casing, transform.localPosition, Quaternion.Euler(new Vector3(0, 0, UnityEngine.Random.Range(0,360))));
		shell.SetParent (BulletContainer, false);
		Physics2D.IgnoreCollision (shell.collider2D, collider2D, true);

		if (direction.x < 0) {
			shell.rigidbody2D.AddForce (new Vector3 (0.4f, 0.4f, 0));
		} else {
			shell.rigidbody2D.AddForce (new Vector3 (-0.4f, 0.4f, 0));
		}
		shell.rigidbody2D.AddTorque (UnityEngine.Random.Range(0.0f, 0.4f));
	}
}
