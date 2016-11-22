using UnityEngine;
using System.Collections;
using DG.Tweening;
using UnityEngine.UI;

public class CameraControl : MonoBehaviour {

	[SerializeField] float moveTime = 1f;
//	[SerializeField] float defaultFOV = 60f;
	[SerializeField] float zoomInFOV = 30f;
	[SerializeField] Canvas dialogue;
	[SerializeField] Animator  cubeAnim;
	public Text text;
	
	public MetricManager metric;
	public string[] wwiseEvent;
	public SoundManager sound;



	public enum State
	{
		None = 0,
		SquareOne = 1,
		SquareTwo = 2,
		SquareThree = 3,
		SquareFour = 4,
		Fold = 5,
		Folded = 6,
		Cube = 7,
	}
	private State m_state = State.None;
	public State state
	{
		get { return m_state; }
		set { 
			if (((int)value) >= 1 && ((int)value) <= 4) {
				int index = (int)value - 1;
				if (focusList.Length > index) {
					Vector3 toPosition = focusList [index].position - new Vector3(0,0.8f,0);
					toPosition.z = transform.position.z;

					transform.DOMove (toPosition, moveTime);
					if (m_camera != null)
						m_camera.DOFieldOfView (zoomInFOV, moveTime);
				}
						
			}
			if ( value == State.Fold )
			{
				m_camera.DOFieldOfView( defaultFOV,moveTime);
				transform.DOMove (defaultPos , moveTime);
			}

			if (value == State.Folded) 
			{
				if (cubeAnim != null) {
					if (cubeAnim.GetCurrentAnimatorStateInfo (0).IsName ("Default")) {
						cubeAnim.SetTrigger ("Start");
					}
					
				}
			}

			if (value == State.Cube)
			{
				if (GameObject.Find ("BluetoothManager") != null) {
					bluetoothManager.Instance.ble.sendBluetooth("5");
				}
					for (int i = 0; i < squares.Length; i++) {
						squares [i].GetComponent<SpriteRenderer> ().DOFade (0, 0.5f);
					}
					GameObject[] dialogues = GameObject.FindGameObjectsWithTag ("UI");
					foreach (GameObject dialogue in dialogues) {
						dialogue.GetComponent<RectTransform> ().DOScale (0, 0.5f);
					}

				cube.SetActive (true);
			}



			nextIcon.DOFade (0, 0);

			int textIndex = (int)value - 1;
			if (textIndex < textList.Length) {
				text.text = textList [textIndex];
				text.DOFade (0, 0);
				text.DOFade (1, 1.5f);
				nextIcon.DOFade (1, 0.5f).SetDelay(2);
			}


				
			m_state = value;
		}
	}

	private Camera m_camera;
	private float defaultFOV;
	private Vector3 defaultPos;
	private GameObject cube;
	private Image nextIcon;
	private float last_start_time;
	private AudioSource[] audio;



	[SerializeField] GameObject[] squares;
	[SerializeField] Transform[] focusList;
	[SerializeField] string[] textList;


	void Awake()
	{
		m_camera = GetComponent<Camera> ();	
		if (m_camera != null)
			defaultFOV = m_camera.fieldOfView;
		defaultPos = transform.position;
		cube = GameObject.Find ("CUBE");
		cube.SetActive (false);
		audio = gameObject.GetComponents<AudioSource> ();

		nextIcon = GameObject.Find ("next").GetComponent<Image>();
		nextIcon.DOFade (0, 0);
	}

	void Start(){

		for (int i = 0; i < squares.Length; i++) {
			squares [i].GetComponent<SpriteRenderer> ().DOFade (1, 2);
		}
		GameObject[] dialogues = GameObject.FindGameObjectsWithTag ("UI");
		foreach (GameObject dialogue in dialogues) {
			dialogue.GetComponent<RectTransform> ().DOScale (1, 0.5f).SetDelay(1f);
		}
		nextIcon.DOFade (1, 0.5f).SetDelay(2);
		last_start_time = Time.time;
		StartCoroutine (playVoiceOver (2));
	}

	void Update()
	{
		
		if (Input.GetMouseButtonDown(0) ) {
			if (((int)m_state) < 5) {
				sound.PlaySound (wwiseEvent [0]);
			}
			if (((int)m_state) > 4 && ((int)m_state) < 7) {
				sound.PlaySound (wwiseEvent [1]);
			}

			string levelName = "Tutorial-Dialogue" + state;
			metric.AddToLevelAndTimeMetric (levelName, (Time.time - last_start_time));
			last_start_time = Time.time;
			state++;

			if ((int)state <= 7) {
				StartCoroutine (playVoiceOver ((int)state + 2));
			}

			
		}



	}

	IEnumerator playVoiceOver(int num){
		sound.PlaySound (wwiseEvent [num - 1] + "Stop");
		yield return new WaitForSeconds (1f);
		sound.PlaySound (wwiseEvent [num]+"Start");
	}


}
