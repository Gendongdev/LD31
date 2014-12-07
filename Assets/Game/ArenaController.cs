using UnityEngine;
using System.Collections.Generic;
using System;

public class WeaponInfo {
	public int counter;
	public int maxCounter;
	public int unlockedCounter;
	public string title;
	public Action spawnBlock;

	public WeaponInfo(string title, int maxCounter, int unlockedCounter, Action spawnBlock) {
		counter = 0;
		this.title = title;
		this.maxCounter = maxCounter;
		this.unlockedCounter = unlockedCounter;
		this.spawnBlock = spawnBlock;
	}

	public bool isUnlocked(int c){
		return unlockedCounter <= c;
	}
}

public class ArenaController : MonoBehaviour, IPUCode {

	public PUColor ChatContainer;
	public PUColor UnlockedContainer;

	public PUImage Heart0;
	public PUImage Heart1;
	public PUImage Heart2;
	public PUImage Heart3;
	public PUImage Heart4;
	public PUImage Heart5;
	public PUImage Heart6;
	public PUImage Heart7;
	public PUImage Heart8;
	public PUImage Heart9;
	public PUText Score;
	public PUText SpawnText;

	public PUImage Info;

	public PUColor RedDamage;

	public List<WeaponInfo> Weapons = new List<WeaponInfo>();

	public int spawnCounter = 0;
	public int playerScore = 0;

	void Start () {
		PlanetUnityGameObject.SetReferenceResolution (960, 600);

		RedDamage.CheckCanvasGroup();
		RedDamage.canvasGroup.alpha = 0.0f;

		NotificationCenter.addObserver (this, "ENEMY_KILLED", null, (args, name) => {
			int score = (int)args["score"];
			playerScore += score;
			Score.text.text = string.Format("Score: "+playerScore);

			CloseInfo();
		});

		NotificationCenter.addObserver (this, "PLAYER_LIFE_UPDATE", null, (args, name) => {
			int life = (int)args["life"];
			PUImage[] hearts = {Heart0, Heart1, Heart2, Heart3, Heart4, Heart5, Heart6, Heart7, Heart8, Heart9};

			RedDamage.canvasGroup.alpha = 0.5f;
			LeanTween.alpha(RedDamage.gameObject, 0, 0.5f).setEase(LeanTweenType.easeInCubic);

			for(int i = 0; i < 10; i++){
				PUImage heart = hearts[i];
				if(i >= life){
					if(heart.gameObject.activeSelf){
						LeanTween.scale(heart.rectTransform, Vector2.zero, 1.0f).setEase(LeanTweenType.easeInElastic).setOnComplete( () => {
							heart.gameObject.SetActive(false);
						});
					}
				}
			}

		});

		Weapons.Add (new WeaponInfo ("!rageaid", 15, 0, () => { SpawnAid(); } ));
		Weapons.Add (new WeaponInfo ("!chicken", 2, 0, () => { SpawnChicken(); } ));
		Weapons.Add (new WeaponInfo ("!knife", 5, 20, () => { SpawnKnife(); } ));
		Weapons.Add (new WeaponInfo ("!trap", 10, 60, () => { SpawnTrap(); } ));
		Weapons.Add (new WeaponInfo ("!boulder", 20, 120, () => { SpawnBoulder(); } ));

		if (TwitchController.isConnected == false) {
			TwitchController.BeginDemoPlay ();
		}
		TwitchController.onMessageReceived = (name, message) => {

			int priorSpawnCount = spawnCounter;

			foreach(WeaponInfo weapon in Weapons){
				if(weapon.isUnlocked(spawnCounter)) {
					if(message.Contains(weapon.title)) {
						weapon.counter++;

						UpdateUnlockables(weapon.title);

						if(weapon.counter >= weapon.maxCounter) {
							weapon.counter = 0;
							weapon.spawnBlock();
							spawnCounter++;
							break;
						}
					}
				}
			}

			foreach(WeaponInfo weapon in Weapons){
				if(weapon.isUnlocked(priorSpawnCount) == false &&
					weapon.isUnlocked(spawnCounter) == true ){
					UpdateUnlockables(weapon.title);
				}
			}
				
			UpdateUnlockables(null);
			ReportMessage(name, message);
		};
	}

	void CloseInfo() {
		if (Info.rectTransform.anchoredPosition.y != -100 && LeanTween.isTweening (Info.gameObject) == false) {
			LeanTween.moveLocalY (Info.rectTransform, -100, 2.0f).setEase (LeanTweenType.easeInOutCubic);
		}
	}

	public void ReportMessage(string name, string message){
	
		Rect r = ChatContainer.rectTransform.rect;
		float delta = 28;

		float y = r.height - ((ChatContainer.children.Count+1) * delta);

		PUText chatMessage = new PUText ();
		chatMessage.SetFrame (0, y, r.width, delta, 0, 0, "bottom,left");
		chatMessage.SetFontColor (new Color (1.0f, 1.0f, 1.0f, 1.0f));
		chatMessage.SetValue (string.Format("[h]{0}[/h] {1}", name, message));
		chatMessage.SetFontSize (12);
		chatMessage.SetAlignment (PlanetUnity2.TextAlignment.lowerLeft);
		chatMessage.LoadIntoPUGameObject (ChatContainer);

		// Move every chat message already in there up
		if (y < r.y) {
			foreach (PUText msg in ChatContainer.children) {
				Vector3 pos = msg.rectTransform.anchoredPosition;
				pos.y += delta;
				msg.rectTransform.anchoredPosition = pos;
			}

			PUText topMsg = ChatContainer.children [0] as PUText;
			topMsg.unload ();
		}

		ChatContainer.CheckCanvasGroup ();
		ChatContainer.canvasGroup.alpha = 0.4f;
	}

	public void UpdateUnlockables(string weaponName) {

		int nextUnlock = 99999;

		int index = 0;
		foreach(WeaponInfo weapon in Weapons){

			if (weapon.isUnlocked (spawnCounter) == false && weapon.unlockedCounter < nextUnlock) {
				nextUnlock = weapon.unlockedCounter;
			}

			// Do we need to create a label for this?
			if (index >= UnlockedContainer.children.Count) {
				Rect r = UnlockedContainer.rectTransform.rect;
				float y = r.height - ((index + 1) * 24);

				PUText weaponText = new PUText ();
				weaponText.SetFrame (0, y, r.width, 24, 0, 0, "bottom,left");

				if (weapon.isUnlocked (spawnCounter)) {
					weaponText.SetFontColor (new Color (1.0f, 1.0f, 1.0f, 1.0f));
					weaponText.SetValue (string.Format ("[h]{0}[/h]   {1} / {2}", weapon.title, weapon.counter, weapon.maxCounter));
				} else {
					weaponText.SetFontColor (new Color (1.0f, 1.0f, 1.0f, 0.6f));
					weaponText.SetValue (string.Format ("[h2]{0}[/h2] @ {1}", weapon.title, weapon.unlockedCounter));
				}
				weaponText.SetFontSize (18);
				weaponText.SetAlignment (PlanetUnity2.TextAlignment.lowerLeft);
				weaponText.LoadIntoPUGameObject (UnlockedContainer);
			} else {
				if (weaponName != null && weaponName.Equals (weapon.title)) {
					PUText weaponText = UnlockedContainer.children [index] as PUText;
					string newValue = string.Format ("[h]{0}[/h]   {1} / {2}", weapon.title, weapon.counter, weapon.maxCounter);
					weaponText.text.text = PlanetUnityStyle.ReplaceStyleTags (newValue);
				}
			}
			index++;
		}

		SpawnText.text.text = PlanetUnityStyle.ReplaceStyleTags(string.Format ("[h]Next Power Unlock[/h]\n         {0} / {1}", spawnCounter, nextUnlock));

	}

	public Vector3 GetSpawnLocation() {
		int childIndex = UnityEngine.Random.Range(0, ArenaControllerPrefabs.instance.SpawnPoints.childCount);
		return ArenaControllerPrefabs.instance.SpawnPoints.GetChild (childIndex).localPosition;
	}

	public void SpawnChicken() {
		Transform spawn = (Transform)Instantiate (ArenaControllerPrefabs.instance.chicken, GetSpawnLocation(), Quaternion.identity);
		spawn.SetParent (ArenaControllerPrefabs.instance.EnemyContainer, false);
	}

	public void SpawnKnife() {
		Vector3[] spawnLocs = {
			new Vector3 (4.2f, 0, 0),
			new Vector3 (-4.1f, 0, 0),
			new Vector3 (0, 2.1f, 0),
			new Vector3 (0, -2.2f, 0)
		};

		float angle = 0;
		int randLoc = UnityEngine.Random.Range (0, spawnLocs.Length);
		Vector3 spawnLoc = spawnLocs [randLoc];

		for (int i = 0; i < 8; i++) {

			Vector3 baseVector = new Vector3 (1, 0, 0);
			baseVector = baseVector.RotateZ (MathR.DegreeToRadian (angle));



			Transform spawn = (Transform)Instantiate (ArenaControllerPrefabs.instance.knife, spawnLoc, Quaternion.identity);
			spawn.SetParent (ArenaControllerPrefabs.instance.EnemyContainer, false);


			KnifeProjectile script = spawn.GetComponent<KnifeProjectile> ();
			script.velocity = baseVector;

			angle += 360 / 8;
		}

		ArenaControllerPrefabs.instance.knifeSound.gameObject.transform.localPosition = spawnLoc;
		ArenaControllerPrefabs.instance.knifeSound.Play ();

	}

	public void SpawnTrap() {

		Vector3 spawnLoc = new Vector3 (UnityEngine.Random.Range (-4.1f, 4.2f), UnityEngine.Random.Range (2.1f, -2.2f));

		Transform spawn = (Transform)Instantiate (ArenaControllerPrefabs.instance.trap, spawnLoc, Quaternion.identity);
		spawn.SetParent (ArenaControllerPrefabs.instance.EnemyContainer, false);
	}

	public void SpawnBoulder() {
		Vector3 spawnLoc = new Vector3 (UnityEngine.Random.Range (-2.1f, 2.2f), 2.1f);

		Transform spawn = (Transform)Instantiate (ArenaControllerPrefabs.instance.boulder, spawnLoc, Quaternion.identity);
		spawn.SetParent (ArenaControllerPrefabs.instance.EnemyContainer, false);
	}

	public void SpawnAid() {

		Vector3 spawnLoc = new Vector3 (UnityEngine.Random.Range (-4.1f, 4.2f), UnityEngine.Random.Range (2.1f, -2.2f));

		Transform spawn = (Transform)Instantiate (ArenaControllerPrefabs.instance.rageaid, spawnLoc, Quaternion.identity);
		spawn.SetParent (ArenaControllerPrefabs.instance.EnemyContainer, false);
	}

}
