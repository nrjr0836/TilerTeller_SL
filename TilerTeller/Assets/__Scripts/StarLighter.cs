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

	public string[] soundevents;


	private  int[] buttonCounter = { 0, 0, 0, 0 };
	private int[] lastButtonCounter = { 0, 0, 0, 0 };

	public enum State{
		InstructionOne = 0,
		InstructionTwo = 3,
		Start = 4,
		Completed = 5,
		Reset = 6,
	}

	private State m_state = State.InstructionOne;

	public State state{
		get{ return m_state;}
		set{

			if (value == State.InstructionOne) {
				instructions [3].DOFade (0, 0);
				sound.PlaySound (soundevents [0] + "Start");
				instructions [0].DOFade (1, 1f).SetDelay(0.5f);
				nextIcon.DOFade (1, 0.5f).SetDelay (1f);
			}
			if (value == State.InstructionTwo) {
				sound.PlaySound ("EV_GUI_ButtonClick");
				nextIcon.DOFade (0, 0);
				nextIcon.DOFade (1, 0.5f).SetDelay(1);
				instructions [0].DOFade (0, 0);
				instructions [1].DOFade (0, 0);
				instructions [1].DOFade (1, 0);
				nextIcon.DOFade (1, 0.5f).SetDelay (0.5f);
				gameObject.GetComponent<Animator>().SetTrigger("P3start");

			}
			if (value == State.Start) {
				if (GameObject.Find ("BluetoothManager") != null) {
					bluetoothManager.Instance.datamanager.setButtonCounter ();
				}
				sound.PlaySound ("EV_GUI_ButtonClick");
				nextIcon.DOFade (0, 1f);
				instructions [1].DOFade (0, 1f);
				instructions [2].DOFade (0, 0);
				instructions [2].DOFade (1, 1f);

			}
			if (value == State.Completed) {
				instructions [2].DOFade (0, 1f);
				instructions [3].DOFade (0, 0);
				instructions [3].DOFade (1, 1f);
				sound.PlaySound ("EV_Story2_Firefly_LevelComplete");
				gameObject.GetComponent<Animator> ().SetTrigger ("P3end");
			}
			if (value == State.Reset) {
				gameObject.GetComponent<Animator> ().SetTrigger ("restart");
			}

			int textIndex = (int)value;
			if (textIndex < textList.Length) {
				sound.PlaySound ("EV_GUI_ButtonClick");
				instructions[0].text = textList [textIndex];
				nextIcon.DOFade (0, 0f);
				nextIcon.DOFade (1, 0.5f).SetDelay(1);
			}


			if ((int)value > 0 && (int)value<4) {
				sound.PlaySound (soundevents [(int)value - 1] + "Stop");
				sound.PlaySound (soundevents [(int)value] + "Start");
			}
			m_state = value;

		}
	}

	[SerializeField] string[] textList;

	void Awake(){
		for (int i = 0; i < 4; i++) {
			instructions [i].DOFade (0, 0);
		}
	}


	void Update () {


		if (start) {
			state = State.InstructionOne;
			start = false;
		}

		if (Input.GetMouseButtonDown (0)&&(int)m_state<4) {
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
					if (!stars [i].isCompleted) {
						sound.PlaySound ("EV_Story2_Firefly_Release");
						Eggy.SetTrigger ("release" + i);
						stars [i].transform.DOScale (0.85f, 0.2f);
						stars [i].transform.DOScale (0.75f, 0.2f).SetDelay (0.2f);
						StartCoroutine (lightUp (i, buttonCounter [i]));
					} else {
						Eggy.SetTrigger ("releasenone");
					}
					lastButtonCounter [i] = buttonCounter [i];
					break;
				}
			}
				
			if (stars [0].isCompleted && stars [1].isCompleted && stars [2].isCompleted && stars [3].isCompleted ) {
				state = State.Completed;
			}

		}

		Debug.Log (state);

//		Debug.Log(buttonCounter[0]+","+buttonCounter[1]+","+buttonCounter[2]+","+buttonCounter[3]);
	}
		

	IEnumerator lightUp( int star, int num){
		stars [star].lightFirfly (num);
		yield return new WaitForSeconds (2f);
		stars [star].lightStar (num);
	}

	public void reset(){
		state = State.Reset;
	}
}
