﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class PandaManager : MonoBehaviour {

	public notebookGenerator notebook;
	public SoundManager sound;
	public Text[] instructions;
	public Image nextIcon;

	public bool start = false;

	private int[] buttonCounter = { 0, 0, 0, 0 };
	private int[] lastButtonCounter = { 0, 0, 0, 0 };


	int count = 0;
	int[] pandaOrder = { 0, 0, 0, 0 };
	int[] pandaPressedOrder = { 0, 0, 0, 0 };

	public enum State{
		InstructionOne =0,
		Notebook = 1,
		Start = 2,
		PandaOne = 3,
		PandaTwo = 4, 
		PandaThree = 5,
		PandaFour = 6,
		End = 7,
	}

	private State m_state = State.InstructionOne;
	public State state
	{
		get{ return m_state;}
		set{

			if (value == State.InstructionOne) {
				instructions [1].DOFade (0, 0);
				instructions [0].DOFade (1, 1f).SetDelay (0.5f);
				nextIcon.DOFade (1, 0.5f).SetDelay (6f);
			}

			if (value == State.Notebook) {
				sound.PlaySound ("EV_GUI_ButtonClick");
				count = 0;
				nextIcon.DOFade (0, 0);
				instructions [0].DOFade (0, 0);
				instructions [1].DOFade (0, 0);
				notebook.GetComponent<CanvasGroup> ().DOFade (1, 0.5f);
				notebook.createPandas ();
			}

			if (value == State.PandaOne) {
				pandaOrder = (int[])notebook.getPandaOrder ().Clone();
				Debug.Log (pandaOrder[0]+" "+pandaOrder[1]+" "+pandaOrder[2]+" "+pandaOrder[3]+" ");
				nextIcon.DOFade (0, 0);
				instructions [1].DOFade (0, 0);
				instructions [1].DOFade (1, 1f).SetDelay (2f);
				state++;
			}
			if (value == State.End) {
				
			}
			m_state = value;
		}
	}

	void Awake(){
		for (int i = 0; i < 2; i++) {
			instructions [i].DOFade (0, 0);
		}

	}

	void Update(){
		
		if (start) {
			state = State.InstructionOne;
			start = false;
		}

		if (Input.GetMouseButtonDown (0)&&(int)m_state<1) {
			state++;
		}

		if (state == State.Start) {

			if (GameObject.Find ("BluetoothManager") != null) {
				int pandaNum;
				pandaNum = bluetoothManager.Instance.datamanager.getPandaPressed ();
				Debug.Log (pandaNum);
				if (pandaNum != -1) {
					switch (pandaNum) {
					case 0: //red
						if (pandaOrder [count] == 0) {
							sound.PlaySound ("EV_Story2_Rabbit_Correct");
							gameObject.GetComponent<Animator> ().SetTrigger ("red");
							count++;
						} else {
							sound.PlaySound ("EV_Story2_Rabbit_Wrong");
							notebook.destroyPandas ();
							state = State.Notebook;
						}
						break;
					case 1: //yellow
						if (pandaOrder [count] == 2) {
							sound.PlaySound ("EV_Story2_Rabbit_Correct");
							gameObject.GetComponent<Animator> ().SetTrigger ("yellow");
							count++;
						} else {
							sound.PlaySound ("EV_Story2_Rabbit_Wrong");
							notebook.destroyPandas ();
							state = State.Notebook;
						}
						break;
					case 2: //blue
						if (pandaOrder [count] == 1) {
							sound.PlaySound ("EV_Story2_Rabbit_Correct");
							gameObject.GetComponent<Animator> ().SetTrigger ("blue");
							count++;
						} else {
							sound.PlaySound ("EV_Story2_Rabbit_Wrong");
							notebook.destroyPandas ();
							state = State.Notebook;
						}
						break;
					case 3:
						if (pandaOrder [count] == 3) {
							sound.PlaySound ("EV_Story2_Rabbit_Correct");
							gameObject.GetComponent<Animator> ().SetTrigger ("green");
							count++;
						} else {
							sound.PlaySound ("EV_Story2_Rabbit_Wrong");
							notebook.destroyPandas ();
							state = State.Notebook;
						}
						break;
					}
					bluetoothManager.Instance.datamanager.setPandaPressed ();
				}
			}

			//1:red, 2:blue, 3:yellow, 4:green
			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				if (pandaOrder [count] == 0) {
					gameObject.GetComponent<Animator> ().SetTrigger ("red");
					count++;
				} else {
					notebook.destroyPandas ();
					state = State.Notebook;
				}

			}
			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				if (pandaOrder [count] == 1) {
					gameObject.GetComponent<Animator> ().SetTrigger ("blue");
					count++;
				} else {
					notebook.destroyPandas ();
					state = State.Notebook;
				}
			}
			if (Input.GetKeyDown (KeyCode.Alpha3)) {
				if (pandaOrder [count] == 2) {
					gameObject.GetComponent<Animator> ().SetTrigger ("yellow");
					count++;
				} else {
					notebook.destroyPandas ();
					state = State.Notebook;
				}
			}
			if (Input.GetKeyDown (KeyCode.Alpha4)) {
				if (pandaOrder [count] == 3) {
					gameObject.GetComponent<Animator> ().SetTrigger ("green");
					count++;
				} else {
					notebook.destroyPandas ();
					state = State.Notebook;
				}
			}
			if (count == 4) {
				state = State.End;
			}

		}

		Debug.Log (state);


	}

	public void startGame(){
		state = State.Start;
	}


}
