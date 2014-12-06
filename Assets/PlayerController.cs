using UnityEngine;

public class PlayerController : MonoBehaviour {

	private Vector2 velocity;
	private float reloadTime;
	private float currentWeaponReload = 3.0f;

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
	
	void FixedUpdate () {

		// Movement by AWSD keyboard
		float speed = 0.01f;
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
		} else if (angle >= 180 && angle <= -180) {
			spriteRenderer.sprite = spriteFace4;
		} else if (angle >= -67 && angle <= -22) {
			spriteRenderer.sprite = spriteFace7;
		} else if (angle >= -112 && angle <= -67) {
			spriteRenderer.sprite = spriteFace6;
		} else if (angle >= -157 && angle <= -112) {
			spriteRenderer.sprite = spriteFace5;
		}

		Debug.Log ("angle: " + angle);

		if (Input.GetMouseButton (0)) {
			CheckFireWeapon ();
		}

	}

	public void Update() {
		reloadTime -= Time.deltaTime;
	}

	public void CheckFireWeapon() {
		if (reloadTime <= 0.0f) {
			reloadTime = currentWeaponReload;

			
			Instantiate (bullet1, transform.localPosition, transform.rotation);

		}
	}
}
