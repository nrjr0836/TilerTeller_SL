using UnityEngine;
using System.Collections;
using System.IO;
using System.Collections.Generic;

public class MetricManager : MonoBehaviour {

	public static MetricManager Instance;

	string createText = "";

	public struct LevelAndTime
	{
		public string LevelName;
		public float TimeInSeconds;

		public LevelAndTime(string theLevelName, float theTime){
			LevelName = theLevelName;
			TimeInSeconds = theTime;
		}
	}

	private List<LevelAndTime> _levelAndTimeMetric = new List<LevelAndTime> ();


	void Start () {}
	void Update () {}
	void Awake(){
//		DontDestroyOnLoad (transform.gameObject);
		if (Instance == null) {
			DontDestroyOnLoad (gameObject);
			Instance = this;
		} else if (Instance != this) {
			Destroy (gameObject);
		}
	}
	
	//When the game quits we'll actually write the file.
	void OnApplicationQuit(){
		GenerateMetricsString ();
		string time = System.DateTime.UtcNow.ToString ();string dateTime = System.DateTime.Now.ToString (); //Get the time to tack on to the file name
		time = time.Replace ("/", "-"); //Replace slashes with dashes, because Unity thinks they are directories..
		string reportFile = "TilerTeller_Metrics_" + time + ".txt"; 
		File.WriteAllText (reportFile, createText);
		//In Editor, this will show up in the project folder root (with Library, Assets, etc.)
		//In Standalone, this will show up in the same directory as your executable
	}

	void GenerateMetricsString(){
		foreach (LevelAndTime x in _levelAndTimeMetric) {
			createText += "Level Name: " + x.LevelName + "， " + x.TimeInSeconds +"s. \n";
		}

	}
		

	public void AddToLevelAndTimeMetric(string LevelName, float TimeInLevel)
	{
		_levelAndTimeMetric.Add(new LevelAndTime(LevelName, TimeInLevel));
	}

}
