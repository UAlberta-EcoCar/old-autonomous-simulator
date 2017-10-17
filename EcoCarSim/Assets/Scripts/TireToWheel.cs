﻿using UnityEngine;
using System.Collections;

public class TireToWheel : MonoBehaviour {

	public WheelCollider wheelCollider;

	void Start() {
		wheelCollider.GetComponent<ParticleSystem>().emissionRate = 500;
	}

	void FixedUpdate () {
	//	transform.position = wheelCollider.su

		UpdateWheelHeight(this.transform, wheelCollider);
	}


	void UpdateWheelHeight(Transform wheelTransform, WheelCollider collider) {
		
		Vector3 localPosition = wheelTransform.localPosition;
		
		WheelHit hit = new WheelHit();
		
		// see if we have contact with ground
		
		if (collider.GetGroundHit(out hit)) {

			float hitY = collider.transform.InverseTransformPoint(hit.point).y;

			localPosition.y = hitY + collider.radius;

			//wheelCollider.GetComponent<ParticleSystem>().enableEmission = true;
			if(
					Mathf.Abs(hit.forwardSlip) >= wheelCollider.forwardFriction.extremumSlip || 
					Mathf.Abs(hit.sidewaysSlip) >= wheelCollider.sidewaysFriction.extremumSlip
				) {
#pragma warning disable CS0618 // Type or member is obsolete
                wheelCollider.GetComponent<ParticleSystem>().enableEmission = true;
#pragma warning restore CS0618 // Type or member is obsolete
            }
			else {
#pragma warning disable CS0618 // Type or member is obsolete
                wheelCollider.GetComponent<ParticleSystem>().enableEmission = false;
#pragma warning restore CS0618 // Type or member is obsolete
            }


		} else {
			
			// no contact with ground, just extend wheel position with suspension distance
			
			localPosition = Vector3.Lerp (localPosition, -Vector3.up * collider.suspensionDistance, .05f);
#pragma warning disable CS0618 // Type or member is obsolete
            wheelCollider.GetComponent<ParticleSystem>().enableEmission = false;
#pragma warning restore CS0618 // Type or member is obsolete

        }
		
		// actually update the position
		
		wheelTransform.localPosition = localPosition;

		wheelTransform.localRotation = Quaternion.Euler(0, collider.steerAngle, 90);
		
	}


}
