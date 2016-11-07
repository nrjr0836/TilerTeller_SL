﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
//using Giverspace;


public class pageManager : MonoBehaviour {

	public SoundManager sound;

	public GameObject[] pages;
	public GameObject hintPage;

	public Animator hintDoor;


//	public MetricManager metric;


	private int bookLength;  //total number of pages
	private int pageCount;  //How many pages have been read
	private int currentPage;  //Current Page Number
	private bool isWaiting; //State: waiting for solving puzzle

	private int[] pageRead; //store the Page Numbers that has been read

	private GameObject lastBtn;
	private GameObject nextBtn;
	private GameObject homeBtn_1;
	private GameObject homeBtn_2;


	private int puzzleNum;  //Puzzle Number that has been solved
	private int lastPuzzleNum;
	private int pagetogo;

	private bool startAnim;

	private AudioSource[] hintSound;

	private float last_start_time;



	void Start () {
		

		lastBtn = GameObject.Find("/Canvas/last");
		nextBtn = GameObject.Find ("/Canvas/next");
		homeBtn_1 = GameObject.Find ("/Canvas/home1");
		homeBtn_2 = GameObject.Find ("/Canvas/home2");

		bookLength = pages.Length;
		pageCount = 0;
		currentPage = 0;
		isWaiting = false;

		pageRead = new int[bookLength];
		pageRead [0] = 0;

		puzzleNum = lastPuzzleNum = 0;

		startAnim = false;

		hintSound = hintPage.GetComponents<AudioSource> ();

		last_start_time = Time.time;



	}


	void Update () {

		lastBtn.SetActive (true);
		nextBtn.SetActive (true);
		homeBtn_1.SetActive (false);
		homeBtn_2.SetActive (false);




	
//		puzzleNum = datamanager.getPuzzleNum ();
		
		if (GameObject.Find ("BluetoothManager") != null) {
			puzzleNum = bluetoothManager.Instance.datamanager.getPuzzleNum();

		}

		/* waiting for players to solve puzzles */
		if (isWaiting) {
			/* Disabled buttons */
			nextBtn.SetActive (false);

			if (Input.GetKeyDown (KeyCode.Alpha1)) {
				startAnim = true;
				puzzleNum = 1;
			} 

			if (Input.GetKeyDown (KeyCode.Alpha2)) {
				startAnim = true;
				puzzleNum = 2;
			} 
			if (Input.GetKeyDown (KeyCode.Alpha3)) {
				startAnim = true;
				puzzleNum = 3;
			}


			if (puzzleNum != lastPuzzleNum && puzzleNum != 0) {
				pagetogo = puzzleNum;
				startAnim = true;	
				hintDoor.SetTrigger ("Open");
				if (!hintSound[0].isPlaying) {
					hintSound[0].Play ();
				}
			
			}

		

			if (startAnim) {

				Debug.Log (puzzleNum);
				
				if (hintDoor.GetCurrentAnimatorStateInfo (0).IsName ("Finished")) {

					startAnim = false;
					gotoPage (pagetogo);
					hintDoor.SetTrigger ("Close");

					string levelName = "Story1-Puzzle-Page" + currentPage;
					if (GameObject.Find ("MetricManager") != null) {
						MetricManager.Instance.AddToLevelAndTimeMetric (levelName, (Time.time - last_start_time));
					}
//					metric.AddToLevelAndTimeMetric (levelName, (Time.time - last_start_time));
					last_start_time = Time.time;
				}
			}


		}

		/* Display the current page */
		for (int i = 0; i < bookLength; i++) {
			if (i == currentPage && pages [i] != null && isWaiting != true) {
				pages [i].SetActive (true);
//				if (pages [i].GetComponent<AudioSource> () != null && pages [i].GetComponent<AudioSource> ().isPlaying != true) {
//					pages [i].GetComponent<AudioSource> ().Play ();}
			} else {
				pages[i].SetActive(false);
			}
		}
		if (currentPage == 0 && !isWaiting) {
			homeBtn_1.SetActive (true);
			lastBtn.SetActive (false);
		}
		if (currentPage == bookLength - 1) {
			homeBtn_2.SetActive (true);
			nextBtn.SetActive (false);
		}

	}

	/* When NextPage button is clicked */
	public void turnNextPage(){

		string levelName = "Story1-Page" + currentPage;
		if (GameObject.Find ("MetricManager") != null) {
			MetricManager.Instance.AddToLevelAndTimeMetric (levelName, (Time.time - last_start_time));
		}
//		metric.AddToLevelAndTimeMetric (levelName, (Time.time - last_start_time));

		sound.StopPlaying ();

		if (pageCount == bookLength - 2) {
			
			if (GameObject.Find ("BluetoothManager") != null) {
				bluetoothManager.Instance.ble.sendBluetooth("1");
			}
			currentPage = bookLength - 1;
			pageRead [bookLength - 1] = currentPage;

			sound.PlaySound ("EV_Story1_Opening_Music_Start");
			sound.PlaySound ("EV_Story1_Opening_DLG_Start");

		} else if (pageRead [pageCount + 1] != 0) {
			currentPage = pageRead [pageCount + 1];
			pageCount++;
			if (currentPage == 1) {
				sound.PlaySound ("EV_Story1_Aus_AMB_Start");
				sound.PlaySound ("EV_Story1_Aus_DLG_Start");
			}
			if (currentPage == 2) {
				sound.PlaySound ("EV_Story1_Sahara_AMB_Start");
			}
			if (currentPage == 3) {
				sound.PlaySound ("EV_Story1_Ice_AMB_Start");
			}
			return;
		}

		else {
			showHintPage ();
			sound.PlaySound ("EV_Story1_Hint_AMB_Start");
		}

		pageCount++;

	}

	public void turnLastPage(){

		sound.StopPlaying ();

		if (currentPage == 0) {
			if (isWaiting) {
				hideHintPage ();
				isWaiting = false;
				sound.PlaySound ("EV_Story1_Opening_Music_Start");
				sound.PlaySound ("EV_Story1_Opening_DLG_Start");
				pageCount--;
			} else {
				return;
			}
		} else {
			pageCount--;
			currentPage = pageRead [pageCount];
			hideHintPage ();
			isWaiting = false;

			switch (currentPage) {
			case 0:
				sound.PlaySound ("EV_Story1_Opening_Music_Start");
				sound.PlaySound ("EV_Story1_Opening_DLG_Start");
				break;
			case 1:
				sound.PlaySound ("EV_Story1_Aus_AMB_Start");
				sound.PlaySound ("EV_Story1_Aus_DLG_Start");
				break;
			case 2:
				sound.PlaySound ("EV_Story1_Sahara_AMB_Start");
				break;
			case 3:
				sound.PlaySound ("EV_Story1_Ice_AMB_Start");
				break;
			}

		}
		if (GameObject.Find ("MetricManager") != null) {
			MetricManager.Instance.AddToLevelAndTimeMetric ("Go back to last Page!", 0);
		}
//		metric.AddToLevelAndTimeMetric ("Go back to last Page!", 0);
		last_start_time = Time.time;
	}

	void showHintPage(){


		if (GameObject.Find ("BluetoothManager") != null) {
			bluetoothManager.Instance.ble.sendBluetooth("0");
		}
		
		if (hintPage != null) {
			hintPage.SetActive (true);
			hintSound [1].Play ();
			Debug.Log ("next page");
			isWaiting = true;
		}

		last_start_time = Time.time;

	}

	void hideHintPage(){
		sound.PlaySound ("EV_Story1_Hint_AMB_Stop");
		hintPage.SetActive (false);
	}

	/* Go to certain page */
	void gotoPage(int page){
		sound.StopPlaying ();
			pageRead [pageCount] = page;
			currentPage = page;
			hideHintPage ();
			isWaiting = false;
			lastPuzzleNum = puzzleNum;
		switch (page) {
		case 1:
			sound.PlaySound ("EV_Story1_Aus_AMB_Start");
			sound.PlaySound ("EV_Story1_Aus_DLG_Start");
			break;
		case 2:
			sound.PlaySound ("EV_Story1_Sahara_AMB_Start");
			break;
		case 3:
			sound.PlaySound ("EV_Story1_Ice_AMB_Start");
			break;
		}
	}

}