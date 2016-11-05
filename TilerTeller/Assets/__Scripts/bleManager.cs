using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;
using System.Collections; 
using System.Collections.Generic; 
using System.Text;


public class bleManager : MonoBehaviour {

	public static bleManager Instance;


	private string _FullUID;
	private string DeviceName = "HMSoft";
	private string _serviceUUID = "FFE0";
	private string _readCharacteristicUUID = "FFE1";
	private string _writeCharacteristicUUID = "FFE1";
	private string _deviceAddress;


	public bool isConnected = false;
	private bool _readFound = false;
	private bool _writeFound = false;
	private string _connectedID = null;

	private float _subscribingTimeout = 0f;
	private bool _scanning = false;
	private bool _connecting = false;

	private int devicesFound = 0;

	private string dataReceived;


	void Start () {
		BluetoothLEHardwareInterface.Initialize (asCentral: true, asPeripheral: false, action: () => {
			BluetoothLEHardwareInterface.Log ("Initialized!");
		}, errorAction: (error) => {
			BluetoothLEHardwareInterface.Log ("ERROR: " + error + "\n");
		});

		Invoke ("scan", 1);
	}

	void connectTo(string addr){
		if (_connecting == false) {
			_connecting = true;
			BluetoothLEHardwareInterface.StopScan ();
			_scanning = false;

			connectBluetooth (addr);
		}
	}

	void connectBluetooth(string addr){
		BluetoothLEHardwareInterface.ConnectToPeripheral (addr, (address) => {},
			(address, serviceUUID) => {},
			(address, serviceUUID, characteristicUUID) => {
				if(isEqual(serviceUUID,_serviceUUID)){
					_connectedID = address;
					isConnected = true;
					if(isEqual(characteristicUUID,_readCharacteristicUUID)){
						_readFound = true;
					}
					if(isEqual(characteristicUUID,_writeCharacteristicUUID)){
						_writeFound = true;
					}

					BluetoothLEHardwareInterface.Log("Characteristic found: " + address + " -> " + serviceUUID + " -> " + characteristicUUID + "\n");
				}
			}, (address) => {
				isConnected = false;
			});

		_connecting = false;
	}

	string FullUUID (string uuid)
	{
		return "0000" + uuid + "-0000-1000-8000-00805f9b34fb";
	}


	bool isEqual(string uuid1, string uuid2){
		if (uuid1.Length == 4) { 
			uuid1 = FullUUID (uuid1); 
		} 
		if (uuid2.Length == 4) { 
			uuid2 = FullUUID (uuid2); 
		} 
		return (uuid1.ToUpper().CompareTo(uuid2.ToUpper()) == 0); 
	}

	public void scan(){
		if (_scanning == true) {
			BluetoothLEHardwareInterface.StopScan (); 
			_scanning = false; 
		} else {
			BluetoothLEHardwareInterface.ScanForPeripheralsWithServices (null, (address, name) => {
				if (name.Contains (DeviceName)) {
					_deviceAddress = address;
					connectTo(_deviceAddress);

				}
			}, (address, name, rssi, advertisingInfo) => {
			});

			_scanning = true;
		}
	}

	void sendBytesBluetooth (byte[] data) { 
		BluetoothLEHardwareInterface.Log (string.Format ("data length: {0} data {1} uuid: {2}", data.Length.ToString (), ASCIIEncoding.UTF8.GetString (data) , _writeCharacteristicUUID)); 
		BluetoothLEHardwareInterface.WriteCharacteristic (_connectedID, _serviceUUID, _writeCharacteristicUUID, data, data.Length,false, (characteristicUUID) => {          
			BluetoothLEHardwareInterface.Log ("Write Succeeded"); 
		}); 
	} 

	void sendDataBluetooth(string sData){
		if (sData.Length > 0) { 
			byte[] bytes = ASCIIEncoding.UTF8.GetBytes (sData); 
			if (bytes.Length > 0) { 
				sendBytesBluetooth (bytes); 
			}       
		} 
	}

	string textBuffer;
	void receiveText(string s){
		//		dataReceived += s;
		textBuffer += s;

		string res = "";
		if (textBuffer.Contains ("*") && textBuffer.Contains ("|") && textBuffer.IndexOf("|") > textBuffer.IndexOf("*") + 1  ) {
			res = textBuffer.Substring (textBuffer.IndexOf ("*") + 1, textBuffer.IndexOf ("|") - textBuffer.IndexOf ("*") - 1);
			textBuffer = textBuffer.Substring (textBuffer.IndexOf ("|") + 1);
		}

		dataReceived = res;
		BluetoothLEHardwareInterface.Log ("res: " + dataReceived); 

	}

	public void sendBluetooth(string sData){
		sendDataBluetooth (sData);
	}

	public string getdataReceived(){
		return dataReceived;
	}

	void Update () {
		if (_readFound && _writeFound) {
			_readFound = false;
			_writeFound = false;
			_subscribingTimeout = 1f;

		}

		if (_subscribingTimeout > 0f) {
			_subscribingTimeout -= Time.deltaTime;
			if (_subscribingTimeout <= 0f) {
				_subscribingTimeout = 0f;
				BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_connectedID,_serviceUUID,_readCharacteristicUUID,
					(deviceAddress,notification)=>{},(deviceAddress2,characteristic,data) => {

						BluetoothLEHardwareInterface.Log("Subscribe! id"+_connectedID);

						if(deviceAddress2.CompareTo(_connectedID) == 0){

							BluetoothLEHardwareInterface.Log (string.Format ("data length: {0}", data.Length)); 
							if (data.Length == 0){ 
								// do nothing 
							} else { 
								string s = ASCIIEncoding.UTF8.GetString (data); 
								BluetoothLEHardwareInterface.Log ("data: " + s); 
								receiveText(s); 
							} 

						}

					});
			}
		}
	}
}
