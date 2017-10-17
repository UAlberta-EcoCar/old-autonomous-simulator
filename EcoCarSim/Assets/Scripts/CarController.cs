using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class CarController : MonoBehaviour {

	public float idealRPM = 500f;
	public float maxRPM = 1000f;

	public Transform centerOfGravity;

	public WheelCollider FR;
	public WheelCollider FL;
	public WheelCollider BR;
	public WheelCollider BL;

	public float turnRadius = 6f;
	public float torque = 25f;
	public float brakeTorque = 100f;

	public float AntiRoll = 20000.0f;

	public enum DriveMode { Front, Rear, All };
	public DriveMode driveMode = DriveMode.Rear;

	//public Text speedText;

	void Start() {
		GetComponent<Rigidbody>().centerOfMass = centerOfGravity.localPosition;
	}

	public float Speed() {
		return BR.radius * Mathf.PI * BR.rpm * 60f / 1000f;
	}

	public float Rpm() {
		return BR.rpm;
	}

	void FixedUpdate () {

		//if(speedText!=null)
			//speedText.text = "Speed: " + Speed().ToString("f0") + " km/h";

		//Debug.Log ("Speed: " + (BR.radius * Mathf.PI * BR.rpm * 60f / 1000f) + "km/h    RPM: " + BR.rpm);

		float scaledTorque = Input.GetAxis("Vertical") * torque;

		if(BR.rpm < idealRPM)
			scaledTorque = Mathf.Lerp(scaledTorque/10f, scaledTorque, BR.rpm / idealRPM );
		else 
			scaledTorque = Mathf.Lerp(scaledTorque, 0,  (BR.rpm-idealRPM) / (maxRPM-idealRPM) );

		DoRollBar(FR, FL);
		DoRollBar(BR, BR);

		FR.steerAngle = Input.GetAxis("Horizontal") * turnRadius;
		FL.steerAngle = Input.GetAxis("Horizontal") * turnRadius;

		FR.motorTorque = driveMode==DriveMode.Rear  ? 0 : scaledTorque;
		FL.motorTorque = driveMode==DriveMode.Rear  ? 0 : scaledTorque;
		BR.motorTorque = driveMode==DriveMode.Front ? 0 : scaledTorque;
		BR.motorTorque = driveMode==DriveMode.Front ? 0 : scaledTorque;

		if(Input.GetButton("Fire1")) {
			FR.brakeTorque = brakeTorque;
			FL.brakeTorque = brakeTorque;
			BR.brakeTorque = brakeTorque;
			BR.brakeTorque = brakeTorque;
		}
		else {
			FR.brakeTorque = 0;
			FL.brakeTorque = 0;
			BR.brakeTorque = 0;
			BR.brakeTorque = 0;
		}
	}


	void DoRollBar(WheelCollider WheelL, WheelCollider WheelR) {
		WheelHit hit;
		float travelL = 1.0f;
		float travelR = 1.0f;
		
		bool groundedL = WheelL.GetGroundHit(out hit);
		if (groundedL)
			travelL = (-WheelL.transform.InverseTransformPoint(hit.point).y - WheelL.radius) / WheelL.suspensionDistance;
		
		bool groundedR = WheelR.GetGroundHit(out hit);
		if (groundedR)
			travelR = (-WheelR.transform.InverseTransformPoint(hit.point).y - WheelR.radius) / WheelR.suspensionDistance;
		
		float antiRollForce = (travelL - travelR) * AntiRoll;
		
		if (groundedL)
			GetComponent<Rigidbody>().AddForceAtPosition(WheelL.transform.up * -antiRollForce,
			                             WheelL.transform.position); 
		if (groundedR)
            GetComponent<Rigidbody>().AddForceAtPosition(WheelR.transform.up * antiRollForce,
			                             WheelR.transform.position); 
	}

}
