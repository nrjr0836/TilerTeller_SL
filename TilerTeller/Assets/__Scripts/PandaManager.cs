using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using DG.Tweening;


public class PandaManager : MonoBehaviour {

	public notebookGenerator notebook;
	public SoundManager sound;
	public Text[] instructions;
	public Image nextIcon;

	public GameObject[] pandaDialogues;
	public GameObject[] pandaDialoguesTwo;
	public GameObject[] eggyDialogues;

	public bool start = false;

	private int[] buttonCounter = { 0, 0, 0, 0 };
	private int[] lastButtonCounter = { 0, 0, 0, 0 };


	int count = 0;
	int[] pandaOrder = { 0, 0, 0, 0 }; //red,blue,yellow,green
	int[] pandaPressedOrder = { 0, 0, 0, 0 };

	Animator anim;

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
				nextIcon.DOFade (1, 0.5f).SetDelay (2f);
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

			if (value == State.Start) {
				pandaOrder = (int[])notebook.getPandaOrder ().Clone();
				Debug.Log (pandaOrder[0]+" "+pandaOrder[1]+" "+pandaOrder[2]+" "+pandaOrder[3]+" ");
				nextIcon.DOFade (0, 0);
				instructions [1].DOFade (0, 0);
				instructions [1].DOFade (1, 0).SetDelay(1.5f);
				value++;
			}
			if (value == State.PandaOne) {
				
				anim.SetTrigger ("enter");
				showPandaDialogue (0,pandaDialogues);
			}
			if (value == State.PandaTwo) {
				anim.SetTrigger ("enter");
				showPandaDialogue (1,pandaDialogues);
			}
			if (value == State.PandaThree) {
				anim.SetTrigger ("enter");
				showPandaDialogue (2,pandaDialogues);
			}
			if (value == State.PandaFour) {
				anim.SetTrigger ("enter");
				showPandaDialogue (3,pandaDialogues);
			}
			if (value == State.End) {
				
			}
			m_state = value;
		}
	}

	void showPandaDialogue(int m, GameObject[] dialogues){
		for (int i = 0; i < 4; i++) {
			if (i == pandaOrder [m]) {
				dialogues [i].SetActive (true);
			} else {
				dialogues [i].SetActive (false);
			}
		}
	}

	void showEggyDialogue(int m){
		for (int i = 0; i < 4; i++) {
			if (i == m) {
				eggyDialogues [i].SetActive (true);
			} else {
				eggyDialogues [i].SetActive (false);
			}
		}
	}




	void Awake(){
		for (int i = 0; i < 2; i++) {
			instructions [i].DOFade (0, 0);
		}
		anim = gameObject.GetComponent<Animator> ();
	}

	string[] triggers= {"red","blue","yellow","green"};
	bool isCorrect = false;


	void Update(){
		
		if (start) {
			state = State.InstructionOne;
			start = false;
		}

		if (Input.GetMouseButtonDown (0)&&(int)m_state<1) {
			state++;
		}



		if ((int)state > 2 && (int) state< 7) {
			
				if (Input.GetKeyDown ((pandaOrder [(int)state - 3] + 1).ToString ())) {
//				Debug.Log (pandaOrder [(int)state - 2]);
				anim.SetBool("wrong",false);
					anim.SetTrigger ("jump");
					anim.SetTrigger (triggers [pandaOrder [(int)state - 3]]);
					isCorrect = true;
				} 


				else if (Input.GetKeyDown (KeyCode.Alpha1)) {
					anim.SetBool ("wrong",true);
					showEggyDialogue (0);
					showPandaDialogue ((int)state-3,pandaDialoguesTwo);
				}
				else if (Input.GetKeyDown (KeyCode.Alpha2)) {
					anim.SetBool ("wrong",true);	
					showEggyDialogue (1);
					showPandaDialogue ((int)state-3,pandaDialoguesTwo);
				}
				else if (Input.GetKeyDown (KeyCode.Alpha3)) {
					anim.SetBool ("wrong",true);
					showEggyDialogue (2);
					showPandaDialogue ((int)state-3,pandaDialoguesTwo);
				}
				else if (Input.GetKeyDown (KeyCode.Alpha4)) {
					anim.SetBool ("wrong",true);
					showEggyDialogue (3);
					showPandaDialogue ((int)state-3,pandaDialoguesTwo);
				}

					

				

				if (isCorrect && anim.GetCurrentAnimatorStateInfo (0).IsName ("S2_P5_Start")) {
					state++;
					isCorrect = false;
				}
		}

		if (state == State.End) {
			Debug.Log ("Complete!");
		}



//		if (state == State.Start) {
//
//			if (GameObject.Find ("BluetoothManager") != null) {
//				int pandaNum;
//				pandaNum = bluetoothManager.Instance.datamanager.getPandaPressed ();
//				Debug.Log (pandaNum);
//				if (pandaNum != -1) {
//					switch (pandaNum) {
//					case 0: //red
//						if (pandaOrder [count] == 0) {
//							sound.PlaySound ("EV_Story2_Rabbit_Correct");
//							gameObject.GetComponent<Animator> ().SetTrigger ("red");
//							count++;
//						} else {
//							sound.PlaySound ("EV_Story2_Rabbit_Wrong");
//							notebook.destroyPandas ();
//							state = State.Notebook;
//						}
//						break;
//					case 1: //yellow
//						if (pandaOrder [count] == 2) {
//							sound.PlaySound ("EV_Story2_Rabbit_Correct");
//							gameObject.GetComponent<Animator> ().SetTrigger ("yellow");
//							count++;
//						} else {
//							sound.PlaySound ("EV_Story2_Rabbit_Wrong");
//							notebook.destroyPandas ();
//							state = State.Notebook;
//						}
//						break;
//					case 2: //blue
//						if (pandaOrder [count] == 1) {
//							sound.PlaySound ("EV_Story2_Rabbit_Correct");
//							gameObject.GetComponent<Animator> ().SetTrigger ("blue");
//							count++;
//						} else {
//							sound.PlaySound ("EV_Story2_Rabbit_Wrong");
//							notebook.destroyPandas ();
//							state = State.Notebook;
//						}
//						break;
//					case 3:
//						if (pandaOrder [count] == 3) {
//							sound.PlaySound ("EV_Story2_Rabbit_Correct");
//							gameObject.GetComponent<Animator> ().SetTrigger ("green");
//							count++;
//						} else {
//							sound.PlaySound ("EV_Story2_Rabbit_Wrong");
//							notebook.destroyPandas ();
//							state = State.Notebook;
//						}
//						break;
//					}
//					bluetoothManager.Instance.datamanager.setPandaPressed ();
//				}
//			}
//
//			//1:red, 2:blue, 3:yellow, 4:green
//			if (Input.GetKeyDown (KeyCode.Alpha1)) {
//				if (pandaOrder [count] == 0) {
//					gameObject.GetComponent<Animator> ().SetTrigger ("red");
//					count++;
//				} else {
//					notebook.destroyPandas ();
//					state = State.Notebook;
//				}
//
//			}
//			if (Input.GetKeyDown (KeyCode.Alpha2)) {
//				if (pandaOrder [count] == 1) {
//					gameObject.GetComponent<Animator> ().SetTrigger ("blue");
//					count++;
//				} else {
//					notebook.destroyPandas ();
//					state = State.Notebook;
//				}
//			}
//			if (Input.GetKeyDown (KeyCode.Alpha3)) {
//				if (pandaOrder [count] == 2) {
//					gameObject.GetComponent<Animator> ().SetTrigger ("yellow");
//					count++;
//				} else {
//					notebook.destroyPandas ();
//					state = State.Notebook;
//				}
//			}
//			if (Input.GetKeyDown (KeyCode.Alpha4)) {
//				if (pandaOrder [count] == 3) {
//					gameObject.GetComponent<Animator> ().SetTrigger ("green");
//					count++;
//				} else {
//					notebook.destroyPandas ();
//					state = State.Notebook;
//				}
//			}
//			if (count == 4) {
//				state = State.End;
//			}
//
//		}



		Debug.Log (state);


	}

	public void startGame(){
		state = State.Start;
	}


}
