using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;



public class LogoAnim : MonoBehaviour {

	// Use this for initialization
	void Start () {
		gameObject.GetComponent<Image> ().DOFade (1, 2).SetDelay (3);
		StartCoroutine (logoFadeout());
	}
	
	IEnumerator logoFadeout(){
		yield return new WaitForSeconds (8);
		gameObject.GetComponent<Image> ().DOFade (0, 2);
		GameObject.Find ("whitemask").GetComponent<Image> ().DOFade (0, 2).SetDelay(0.5f);
		yield return new WaitForSeconds (1);
		GameObject.Find ("Title Screen").GetComponent<Animator> ().SetTrigger ("start");
		yield return new WaitForSeconds (0.8f);
		gameObject.GetComponent<SoundManager> ().PlaySound ("EV_Title_Music_Start");
	}
}
