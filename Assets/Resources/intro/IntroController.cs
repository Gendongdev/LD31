
using UnityEngine;

public class IntroController : MonoBehaviour, IPUCode {

	public PUGameObject LogoContainer;
	public PUImage Man;
	public PUImage Vs;
	public PUImage Twitch;

	public PUColor Black;

	public PUText ClickToBegin;

	bool introFinished = false;

	public void Start() {

		PlanetUnityGameObject.SetReferenceResolution (960, 600);

		Black.CheckCanvasGroup ();
		ClickToBegin.CheckCanvasGroup();
		ClickToBegin.canvasGroup.alpha = 0.0f;

		Man.rectTransform.localScale = Vector3.zero;
		Vs.rectTransform.localScale = Vector3.zero;
		Twitch.rectTransform.localScale = Vector3.zero;
		Twitch.rectTransform.localEulerAngles = new Vector3 (0, 0, -15);

		LeanTween.scale(Man.rectTransform, new Vector3(1,1,1), 0.5f).setDelay(0.0f+0.5f).setEase(LeanTweenType.easeOutCubic);
		LeanTween.scale(Vs.rectTransform, new Vector3(1,1,1), 0.5f).setDelay(0.5f+0.5f).setEase(LeanTweenType.easeOutCubic);
		LeanTween.scale(Twitch.rectTransform, new Vector3(1,1,1), 1.0f).setDelay(1.0f+0.5f).setEase(LeanTweenType.easeOutBounce);
		LeanTween.rotate(Twitch.gameObject, Vector3.zero, 1.0f).setDelay(1.0f+0.5f).setEase(LeanTweenType.easeOutCubic);

		LeanTween.alpha (Black.gameObject, 0.3f, 4.0f).setEase(LeanTweenType.easeInCubic).setOnComplete (() => {
			LeanTween.scale(LogoContainer.rectTransform, new Vector3(0.4f, 0.4f, 0.4f), 1.0f).setEase(LeanTweenType.easeOutBounce);
			LeanTween.moveLocalY(LogoContainer.rectTransform, 200, 1.0f).setEase(LeanTweenType.easeOutCubic).setOnComplete( () => {

				introFinished = true;

				ClickToBegin.canvasGroup.alpha = 0.6f;
				LeanTween.alpha ( ClickToBegin.gameObject, 1.0f, 1.0f).setLoopPingPong();

			});
		});

		TwitchController.ConnectToTwitchChat ("indygamedev");
	}

	public void StartGame() {
		if (introFinished) {
			Application.LoadLevel (1);
		}
	}



}
