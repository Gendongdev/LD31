
using UnityEngine;

public class IntroController : MonoBehaviour, IPUCode {

	public GameObject Music;

	public PUGameObject LogoContainer;
	public PUImage Man;
	public PUImage Vs;
	public PUImage Twitch;

	public PUInputField TwitchChannelInput;
	public PUImageButton NoWebCover;
	public PUText Disclaimer;

	public PUGameObject StreamerArena;
	public PUGameObject SoloArena;

	public PUColor Black;

	bool introFinished = false;

	public void Start() {

		DontDestroyOnLoad(Music.gameObject);

		PlanetUnityGameObject.SetReferenceResolution (960, 600);

		Black.CheckCanvasGroup ();

		Man.rectTransform.localScale = Vector3.zero;
		Vs.rectTransform.localScale = Vector3.zero;
		Twitch.rectTransform.localScale = Vector3.zero;
		Twitch.rectTransform.localEulerAngles = new Vector3 (0, 0, -15);

		LeanTween.scale(Man.rectTransform, new Vector3(1,1,1), 0.5f).setDelay(0.0f+0.5f).setEase(LeanTweenType.easeOutCubic);
		LeanTween.scale(Vs.rectTransform, new Vector3(1,1,1), 0.5f).setDelay(0.5f+0.5f).setEase(LeanTweenType.easeOutCubic);
		LeanTween.scale(Twitch.rectTransform, new Vector3(1,1,1), 1.0f).setDelay(1.0f+0.5f).setEase(LeanTweenType.easeOutBounce);
		LeanTween.rotate(Twitch.gameObject, Vector3.zero, 1.0f).setDelay(1.0f+0.5f).setEase(LeanTweenType.easeOutCubic);

		LeanTween.alpha (Black.gameObject, 0.3f, 4.0f).setEase(LeanTweenType.easeInCubic).setOnComplete (() => {
		
			AudioSource audioSource = Music.GetComponent<AudioSource>();
			audioSource.Play();

			LeanTween.scale(LogoContainer.rectTransform, new Vector3(0.3f, 0.3f, 0.3f), 1.0f).setEase(LeanTweenType.easeOutBounce);
			LeanTween.moveLocalY(LogoContainer.rectTransform, 200, 1.0f).setEase(LeanTweenType.easeOutCubic).setOnComplete( () => {

				introFinished = true;

				LeanTween.moveLocalX(StreamerArena.rectTransform, 0, 1.0f).setEase(LeanTweenType.easeOutCirc);
				LeanTween.moveLocalX(SoloArena.rectTransform, 0, 1.0f).setEase(LeanTweenType.easeOutCirc);

			});
		});


		NoWebCover.gameObject.SetActive (false);
		if (Application.isWebPlayer) {
			NoWebCover.gameObject.SetActive (true);
		}
	}

	public void StartGameSolo() {
		if (introFinished) {
			TwitchController.BeginDemoPlay();
			Application.LoadLevel (1);
		}
	}

	public void StartGameStreamer() {
		if (introFinished) {

			TwitchController.onConnectedToServer = () => {
				TwitchController.SendRoomMessage("MAN vs TWITCH! Battle your streamer in the arena now by typing !chicken, !knife, !trap, !boulder, or !rageaid.");
				Application.LoadLevel (1);
			};

			TwitchController.ConnectToTwitchChat (TwitchChannelInput.text.text);
		}
	}



}
