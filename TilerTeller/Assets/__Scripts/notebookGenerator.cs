using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class notebookGenerator : MonoBehaviour {


	int[] pandaOrder = { 0,1,2,3};

	public GameObject[] pandas;
	private GameObject[] pandasCreated = new GameObject[4];

	public void createPandas () {

		ShuffleArray (pandaOrder);
		for (int i = 0; i < 4; i++) {
			pandasCreated [i] = (GameObject)Instantiate (pandas [pandaOrder [i]], pandas [i].transform.position, pandas [i].transform.rotation);
			pandasCreated[i].transform.SetParent(gameObject.transform,false);
		}
		popupPandas ();
	}

	public void onReady(){
		Image mask = GameObject.Find ("BlackMask").GetComponent<Image> ();
		mask.DOFade (0, 1f);
		gameObject.GetComponent<CanvasGroup> ().DOFade (0, 1).SetDelay (0.5f);


	}

	public void popupPandas(){
		pandasCreated [0].GetComponent<RectTransform> ().DOScale (0.68f, 0.5f);
		pandasCreated [1].GetComponent<RectTransform> ().DOScale (0.68f, 0.5f).SetDelay(0.8f);
		pandasCreated [2].GetComponent<RectTransform> ().DOScale (0.68f, 0.5f).SetDelay(1.6f);
		pandasCreated [3].GetComponent<RectTransform> ().DOScale (0.68f, 0.5f).SetDelay(2.4f);
	}


	int[] ShuffleArray(int[] array){


		for (int i = array.Length-1; i > 0; i--) {
			int rnd = Random.Range (0, i);
			int temp = array [i];
			array [i] = array [rnd];
			array [rnd] = temp;
		}

		return array;

	}


	public void destroyPandas(){
		foreach (GameObject panda in pandasCreated) {
			Destroy (panda);
		}
	}



}
