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

	public List<WeaponInfo> Weapons = new List<WeaponInfo>();

	public int spawnCounter = 0;


	void Start () {
		PlanetUnityGameObject.SetReferenceResolution (960, 600);

		Weapons.Add (new WeaponInfo ("!chicken", 2, 0, () => { SpawnChicken(); } ));
		Weapons.Add (new WeaponInfo ("!knife", 4, 100, () => { SpawnChicken(); } ));
		Weapons.Add (new WeaponInfo ("!moo", 10, 250, () => { SpawnChicken(); } ));
		Weapons.Add (new WeaponInfo ("!boulder", 10, 500, () => { SpawnChicken(); } ));
		Weapons.Add (new WeaponInfo ("!trap", 10, 1000, () => { SpawnChicken(); } ));

		if (TwitchController.isConnected == false) {
			TwitchController.BeginDemoPlay ();
		}
		TwitchController.onMessageReceived = (name, message) => {

			foreach(WeaponInfo weapon in Weapons){
				if(weapon.isUnlocked(spawnCounter)) {
					if(message.Contains(weapon.title)) {
						weapon.counter++;

						UpdateUnlockables(weapon.title);

						if(weapon.counter >= weapon.maxCounter) {
							weapon.counter = 0;
							weapon.spawnBlock();
							spawnCounter++;
						}
					}
				}
			}
				
			UpdateUnlockables(null);
			ReportMessage(name, message);
		};
	}
	
	void Update () {

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

		int index = 0;
		foreach(WeaponInfo weapon in Weapons){

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
					weaponText.SetValue (string.Format ("[h2]{0}[/h2]  unlocks @ {1}", weapon.title, weapon.unlockedCounter));
				}
				weaponText.SetFontSize (18);
				weaponText.SetAlignment (PlanetUnity2.TextAlignment.lowerLeft);
				weaponText.LoadIntoPUGameObject (UnlockedContainer);
			} else {
				Debug.Log("weaponName: "+weaponName);
				if (weaponName != null && weaponName.Equals (weapon.title)) {
					PUText weaponText = UnlockedContainer.children [index] as PUText;
					string newValue = string.Format ("[h]{0}[/h]   {1} / {2}", weapon.title, weapon.counter, weapon.maxCounter);
					weaponText.text.text = PlanetUnityStyle.ReplaceStyleTags (newValue);
				}
			}
			index++;
		}


	}

	public Vector3 GetSpawnLocation() {
		int childIndex = UnityEngine.Random.Range(0, ArenaControllerPrefabs.instance.SpawnPoints.childCount);
		return ArenaControllerPrefabs.instance.SpawnPoints.GetChild (childIndex).localPosition;
	}

	public void SpawnChicken() {
		Transform spawn = (Transform)Instantiate (ArenaControllerPrefabs.instance.chicken, GetSpawnLocation(), Quaternion.identity);
		spawn.SetParent (ArenaControllerPrefabs.instance.EnemyContainer, false);
	}

}
