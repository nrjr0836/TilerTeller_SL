using UnityEngine;
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
		End = 3,
	}

	private State m_state = State.InstructionOne;
	public State state
	{
		get{ return m_state;}
		set{

			if (value == State.InstructionOne) {
				instructions [0].DOFade (1, 1.5f).SetDelay (0.5f);
				nextIcon.DOFade (1, 0.5f).SetDelay (6f);
			}

			if (value == State.Notebook) {
				count = 0;
				nextIcon.DOFade (0, 0);
				instructions [0].DOFade (0, 0);
				instructions [1].DOFade (0, 0);
				notebook.GetComponent<CanvasGroup> ().DOFade (1, 0.5f);
				notebook.createPandas ();
			}

			if (value == State.Start) {
				pandaOrder = (int[])notebook.getPandaOrder ().Clone();
				Debug.Log (pandaOrder[0]+" "+pandaOrder[1]+" "+pandaOrder[2]+" "+pandaOrder[3]+" ");
				instructions [1].DOFade (0, 0);
				instructions [1].DOFade (1, 1.5f).SetDelay (2f);

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
				switch (pandaNum) {
				case 0: //red
					if (pandaOrder [count] == 0) {
						gameObject.GetComponent<Animator> ().SetTrigger ("red");
						count++;
					}else {
						notebook.destroyPandas ();
						state = State.Notebook;
					}
						break;
				case 1: //yellow
					if (pandaOrder [count] == 2) {
						gameObject.GetComponent<Animator> ().SetTrigger ("yellow");
						count++;
					}else {
						notebook.destroyPandas ();
						state = State.Notebook;
					}
					break;
				case 2: //blue
					if (pandaOrder [count] == 1) {
						gameObject.GetComponent<Animator> ().SetTrigger ("blue");
						count++;
					}else {
						notebook.destroyPandas ();
						state = State.Notebook;
					}
					break;
				case 3:
					if (pandaOrder [count] == 3) {
						gameObject.GetComponent<Animator> ().SetTrigger ("green");
						count++;
					} else {
						notebook.destroyPandas ();
						state = State.Notebook;
					}
					break;
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
			Debug.Log (count);

		}

		Debug.Log (state);


	}

	public void startGame(){
		state = State.Start;
	}


}
