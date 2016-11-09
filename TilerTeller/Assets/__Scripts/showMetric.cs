using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;

public class showMetric : MonoBehaviour {

	public Text metric_content;
	public Image mask;
	public bool isShow = false;

	public void _showmetric(){
		Debug.Log ("???");
		if (isShow == false) {
			mask.gameObject.SetActive (true);
			mask.DOFade (1, 1);
			if (GameObject.Find ("MetricManager") != null) {
				Debug.Log("??");
				metric_content.text = MetricManager.Instance.getContent ();
			}
			metric_content.gameObject.SetActive (true);
			metric_content.DOFade (1, 1);
			isShow = true;
			return;
		}
			
		if (isShow == true) {
			Debug.Log ("!!");
			mask.DOFade (0, 1);
			mask.gameObject.SetActive (false);
			metric_content.DOFade (0, 1);
			metric_content.gameObject.SetActive (false);
			isShow = false;
		}
	}
	
}
