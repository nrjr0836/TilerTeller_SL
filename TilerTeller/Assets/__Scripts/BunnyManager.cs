using UnityEngine;
using System.Collections;

public class BunnyManager : MonoBehaviour {

	public GameObject whiteBunny;
	public GameObject whiteBunnyRed;
	public GameObject brownBunny;
	public float duration;

	public float timeInterval = 2;

	public bool start = false;

	private bool isfirstTime = true;


	void Update(){
		if (start) {
			StartCoroutine (m_Spawn ());
			start = false;
		}
	}
	


	IEnumerator m_Spawn(){
		int i = 0;
//		if (isfirstTime) {
//			yield return new WaitForSeconds (12);
//			isfirstTime = false;
//		}
		yield return new WaitForSeconds(15);
		while (true) {
			if (timeInterval > 4) {
				timeInterval = timeInterval - 0.1f * i;
			}
			yield return new WaitForSeconds (timeInterval);

			SpawnBunny ( duration - i*0.1f);
			i++;
		}
	}

	void SpawnBunny( float duration ){
		
		int bunnyType = Random.Range(0,6);
	
		if (bunnyType < 1) {
			GameObject brownBunnyClone = (GameObject)Instantiate (brownBunny, brownBunny.transform.position, brownBunny.transform.rotation);
			brownBunnyClone.transform.parent = gameObject.transform;
			brownBunnyClone.GetComponent<BunnyMov> ().duration = duration;
		}
		if (bunnyType >= 1 && bunnyType < 3) {
			GameObject whiteBunnyClone = (GameObject)Instantiate (whiteBunny, whiteBunny.transform.position, whiteBunny.transform.rotation);
			whiteBunnyClone.transform.parent = gameObject.transform;
			whiteBunnyClone.GetComponent<BunnyMov> ().duration = duration;
		}
		if (bunnyType >= 3) {
			GameObject whiteBunnyRedClone = (GameObject)Instantiate (whiteBunnyRed, whiteBunnyRed.transform.position, whiteBunnyRed.transform.rotation);
			whiteBunnyRedClone.transform.parent = gameObject.transform;
			whiteBunnyRedClone.GetComponent<BunnyMov> ().duration = duration;
		}

//		yield return new WaitForSeconds (timeInterval);

	
	}

}	
