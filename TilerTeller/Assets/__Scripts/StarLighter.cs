using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.UI;

public class StarLighter : MonoBehaviour {

	public Text[] instructions;
	public Image nextIcon;

	public Star[] stars;
	public Animator Eggy;
	public SoundManager sound;

	public bool start;
	public bool leave = false;


	private  int[] buttonCounter = { 0, 0, 0, 0 };
	private int[] lastButtonCounter = { 0, 0, 0, 0 };

	public enum State{
		InstructionOne = 0,
		InstructionTwo = 1,
		Start = 2,
		Completed = 3,
	}

	private State m_state = State.InstructionOne;

	public State state{
		get{ return m_state;}
		set{

			if (value == State.InstructionOne) {
				instructions [0].DOFade (1, 1f).SetDelay(1f);
				nextIcon.DOFade (1, 0.5f).SetDelay (5f);
			}
			if (value == State.InstructionTwo) {
				sound.PlaySound ("EV_GUI_ButtonClick");
				nextIcon.DOFade (0, 1f);
				instructions [0].DOFade (0, 1f);
				instructions [1].DOFade (0, 0);
				instructions [1].DOFade (1, 1f).SetDelay (1.5f);
				nextIcon.DOFade (1, 0.5f).SetDelay (5f);
				gameObject.GetComponent<Animator>().SetTrigger("P3start");

			}
			if (value == State.Start) {
				sound.PlaySound ("EV_GUI_ButtonClick");
				nextIcon.DOFade (0, 1f);
				instructions [1].DOFade (0, 1f);
				instructions [2].DOFade (0, 0);
				instructions [2].DOFade (1, 1f).SetDelay (1.5f);

			}
			if (value == State.Completed) {
				sound.PlaySound ("EV_Story2_Firefly_LevelComplete");
				Debug.Log ("Level Complete!");
			}

			m_state = value;

		}
	}

	void Awake(){
		for (int i = 0; i < 3; i++) {
			instructions [i].DOFade (0, 0);
		}
	}


	void Update () {

		Debug.Log (state);

		if (start) {
			state = State.InstructionOne;
			start = false;
		}

		if (Input.GetMouseButtonDown (0)&&(int)m_state<2) {
			state++;
		}

		if (state == State.Start) {

			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				buttonCounter [0]++;
			}
			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				buttonCounter [1]++;
			}
			if (Input.GetKeyDown (KeyCode.Alpha3)) {
				buttonCounter [2]++;
			}
			if (Input.GetKeyDown (KeyCode.Alpha4)) {
				buttonCounter [3]++;
			}

			if (GameObject.Find ("BluetoothManager") != null) {
				buttonCounter = bluetoothManager.Instance.datamanager.getButtonCounter ();
			}


			for (int i = 0; i < 4; i++) {
				if (buttonCounter [i] != lastButtonCounter [i]) {
					sound.PlaySound ("EV_Story2_Firefly_Release");
					Eggy.SetTrigger ("release");
					stars [i].transform.DOScale (0.85f, 0.2f);
					stars [i].transform.DOScale (0.75f, 0.2f).SetDelay (0.2f);
					StartCoroutine (lightUp (i,buttonCounter[i]));
					lastButtonCounter [i] = buttonCounter [i];
					break;
				}
			}
				
			if (stars [0].isCompleted && stars [1].isCompleted && stars [2].isCompleted && stars [3].isCompleted ) {
				state = State.Completed;
			}

		}



//		Debug.Log(buttonCounter[0]+","+buttonCounter[1]+","+buttonCounter[2]+","+buttonCounter[3]);
	}
		

	IEnumerator lightUp( int star, int num){
		stars [star].lightFirfly (num);
		yield return new WaitForSeconds (3f);
		stars [star].lightStar (num);
	}

}
