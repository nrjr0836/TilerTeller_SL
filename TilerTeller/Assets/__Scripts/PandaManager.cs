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

	public string[] soundEvents;


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
		Reset = 8,
	}

	private State m_state = State.InstructionOne;
	public State state
	{
		get{ return m_state;}
		set{

			if (value == State.InstructionOne) {
				
				sound.PlaySound ("EV_Story2_Panda_Intro_DLG_Start");
				showPandaDialogue (-1, pandaDialogues);
				showPandaDialogue (-1, pandaDialoguesTwo);
				showEggyDialogue (-1);
				instructions [2].DOFade (0, 0);
				instructions [1].DOFade (0, 0);
				instructions [0].DOFade (1, 1f).SetDelay (0.5f);
				nextIcon.DOFade (1, 0.5f).SetDelay (2f);
			}

			if (value == State.Notebook) {
				sound.PlaySound ("EV_GUI_ButtonClick");
				sound.PlaySound ("EV_Story2_Panda_Intro_DLG_Stop");
				sound.PlaySound ("EV_Story2_Panda_Instruct_DLG_Start");
				count = 0;
				nextIcon.DOFade (0, 0);
				instructions [2].DOFade (0, 0);
				instructions [0].DOFade (0, 0);
				instructions [1].DOFade (0, 0);
				notebook.gameObject.SetActive (true);
				notebook.GetComponent<CanvasGroup> ().DOFade (1, 0.5f);
				notebook.createPandas ();

			}

			if (value == State.Start) {
				if (GameObject.Find ("BluetoothManager") != null) {
					bluetoothManager.Instance.datamanager.setPandaPressed ();
				}
				sound.PlaySound("EV_Story2_Panda_Instruct_DLG_Stop");
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
				StartCoroutine (playPandaDialogues (pandaOrder [0]));

			}
			if (value == State.PandaTwo) {
				anim.SetTrigger ("enter");
				showPandaDialogue (1,pandaDialogues);
				StartCoroutine (playPandaDialogues (pandaOrder [1]));
			}
			if (value == State.PandaThree) {
				anim.SetTrigger ("enter");
				showPandaDialogue (2,pandaDialogues);
				StartCoroutine (playPandaDialogues (pandaOrder [2]));
			}
			if (value == State.PandaFour) {
				anim.SetTrigger ("enter");
				showPandaDialogue (3,pandaDialogues);
				StartCoroutine (playPandaDialogues (pandaOrder [3]));
			}
			if (value == State.End) {
				sound.PlaySound ("EV_Story2_Panda_NextReady_DLG");
				anim.SetTrigger ("complete");
				instructions [1].DOFade (0, 0);
				instructions [2].DOFade (1, 0).SetDelay (0.5f);
				nextIcon.DOFade (0, 0);
				nextIcon.DOFade (1, 0.5f).SetDelay (1f);
			}
			if (value == State.Reset) {
				for (int i = 0; i < 4; i++) {
					pandaOrder [i] = 0;
				}
			}

			m_state = value;
		}
	}

	IEnumerator playPandaDialogues(int num){
		yield return new WaitForSeconds (1f);
		sound.PlaySound ("EV_Story2_Panda_Walk");
		yield return new WaitForSeconds (0.5f);
		sound.PlaySound(soundEvents [num]);
	}

	void showPandaDialogue(int m, GameObject[] dialogues){
		if (m == -1) {
			foreach (GameObject dialogue in dialogues) {
				dialogue.SetActive (false);
			}
		} else {
			for (int i = 0; i < 4; i++) {
				if (i == pandaOrder [m]) {
					dialogues [i].SetActive (true);
				} else {
					dialogues [i].SetActive (false);
				}
			}
		}
	}

	void showEggyDialogue(int m){
		if (m == -1) {
			foreach (GameObject dialogue in eggyDialogues) {
				dialogue.SetActive (false);
			}
		}else{
		for (int i = 0; i < 4; i++) {
			if (i == m) {
				eggyDialogues [i].SetActive (true);
			} else {
				eggyDialogues [i].SetActive (false);
			}
			}
		}
	}




	void Awake(){
		for (int i = 0; i <3; i++) {
			instructions [i].DOFade (0, 0);
		}
		anim = gameObject.GetComponent<Animator> ();
	}

	string[] triggers= {"red","blue","yellow","green"};
	bool isCorrect = false;


	IEnumerator playPandaWrong(int num){
		sound.PlaySound (soundEvents [num + 4]);
		yield return new WaitForSeconds (1.5f);
		sound.PlaySound ("EV_Story2_Panda_UhOh_DLG");
	}

	IEnumerator playPandaRight(){
		sound.PlaySound ("EV_Story2_Panda_ThankYou_DLG");
		yield return new WaitForSeconds (0.3f);
		sound.PlaySound ("EV_Story2_Panda_Jump");
		yield return new WaitForSeconds (0.8f);
		sound.PlaySound ("EV_Story2_Panda_Perm");
	}

	void Update(){
		
		if (start) {
			state = State.InstructionOne;
			start = false;
		}

		if (Input.GetMouseButtonDown (0)&&(int)m_state<1) {
			state++;
		}
		
		if(Input.GetMouseButtonDown (0) && (int)m_state == 7){
			notebook.destroyPandas ();
			state = State.Notebook;	
			reset ();
		}



		if ((int)state > 2 && (int) state< 7) {

			if (GameObject.Find ("BluetoothManager") != null) {
				int panda_pressed;
				panda_pressed = bluetoothManager.Instance.datamanager.getPandaPressed ();
				bluetoothManager.Instance.datamanager.setPandaPressed ();
				Debug.Log (panda_pressed);

				if (panda_pressed == pandaOrder [(int)state - 3]) {
					anim.SetBool ("wrong", false);
					anim.SetTrigger ("jump");
					anim.SetTrigger (triggers [pandaOrder [(int)state - 3]]);
					isCorrect = true;
					StartCoroutine (playPandaRight ());
				} else if (panda_pressed == 0) {
					StartCoroutine (playPandaWrong (0));
					anim.SetBool ("wrong", true);
					showEggyDialogue (0);
					showPandaDialogue ((int)state - 3, pandaDialoguesTwo);
				} else if (panda_pressed == 1) {
					StartCoroutine (playPandaWrong (1));
					anim.SetBool ("wrong", true);	
					showEggyDialogue (1);
					showPandaDialogue ((int)state - 3, pandaDialoguesTwo);
				} else if (panda_pressed == 2) {
					StartCoroutine (playPandaWrong (2));
					anim.SetBool ("wrong", true);
					showEggyDialogue (2);
					showPandaDialogue ((int)state - 3, pandaDialoguesTwo);
				} else if (panda_pressed == 3) {
					StartCoroutine (playPandaWrong (3));
					anim.SetBool ("wrong",true);
					showEggyDialogue (3);
					showPandaDialogue ((int)state-3,pandaDialoguesTwo);
				}


			}
			
				if (Input.GetKeyDown ((pandaOrder [(int)state - 3] + 1).ToString ())) {
//				Debug.Log (pandaOrder [(int)state - 2]);
				anim.SetBool("wrong",false);
					anim.SetTrigger ("jump");
					anim.SetTrigger (triggers [pandaOrder [(int)state - 3]]);
					isCorrect = true;
					StartCoroutine (playPandaRight ());
				} 


				else if (Input.GetKeyDown (KeyCode.Alpha1)) {
					StartCoroutine (playPandaWrong (0));
					anim.SetBool ("wrong",true);
					showEggyDialogue (0);
					showPandaDialogue ((int)state-3,pandaDialoguesTwo);
				}
				else if (Input.GetKeyDown (KeyCode.Alpha2)) {
				StartCoroutine (playPandaWrong (1));
					anim.SetBool ("wrong",true);	
					showEggyDialogue (1);
					showPandaDialogue ((int)state-3,pandaDialoguesTwo);
				}
				else if (Input.GetKeyDown (KeyCode.Alpha3)) {
				StartCoroutine (playPandaWrong (2));
					anim.SetBool ("wrong",true);
					showEggyDialogue (2);
					showPandaDialogue ((int)state-3,pandaDialoguesTwo);
				}
				else if (Input.GetKeyDown (KeyCode.Alpha4)) {
				StartCoroutine (playPandaWrong (3));
					anim.SetBool ("wrong",true);
					showEggyDialogue (3);
					showPandaDialogue ((int)state-3,pandaDialoguesTwo);
				}
				
				if (isCorrect && anim.GetCurrentAnimatorStateInfo (0).IsName ("S2_P5_Start")) {
					state++;
					isCorrect = false;
					
				}
		}


		Debug.Log (state);

	}

	public void reset(){
		anim.SetTrigger ("restart");
		state = State.Reset;
	}

	public void startGame(){
		state = State.Start;
	}


}
