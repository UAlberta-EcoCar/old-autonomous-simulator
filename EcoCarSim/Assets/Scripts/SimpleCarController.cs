using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;


[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor;
    public bool steering;
}

public class SimpleCarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos;
    public float maxMotorTorque;
    public float maxSteeringAngle;

    public float AntiRoll = 20000.0f;
    public float idealRPM = 500f;
    public float maxRPM = 1000f;
    public float brakeMaxTorque = 25f;

    public Transform centerOfGravity;

    public Text speedText;
    public Stopwatch speedUpdate = Stopwatch.StartNew();



    public

    void Start()
    {
        GetComponent<Rigidbody>().centerOfMass = centerOfGravity.localPosition;
    }

    public float Speed(WheelCollider wheel)
    {
        return wheel.radius * Mathf.PI * wheel.rpm * 60f / 1000f;
    }

    // finds the corresponding visual wheel
    // correctly applies the transform
    public void ApplyLocalPositionToVisuals(WheelCollider collider)
    {
        if (collider.transform.childCount == 0)
        {
            return;
        }

        Transform visualWheel = collider.transform.GetChild(0);

        Vector3 position;
        Quaternion rotation;
        collider.GetWorldPose(out position, out rotation);

        visualWheel.transform.position = position;
        visualWheel.transform.rotation = rotation;
        visualWheel.transform.Rotate(0, 0, 90);
    }

    public void FixedUpdate()
    {
        float scaledTorque = Input.GetAxis("Vertical") * maxMotorTorque;
        float steering = maxSteeringAngle * Input.GetAxis("Horizontal");

        if (speedText != null && (speedUpdate.ElapsedMilliseconds%2==0)) {
            int dispSpeed = (int)Speed(axleInfos[0].leftWheel);
            speedText.text = "Speed: " + dispSpeed.ToString("f0") +" km/h";
        }


        foreach (AxleInfo axleInfo in axleInfos)
        {

            DoRollBar(axleInfo.leftWheel, axleInfo.rightWheel);
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                if (axleInfo.leftWheel.rpm < idealRPM)
                    scaledTorque = Mathf.Lerp(scaledTorque / 10f, scaledTorque, axleInfo.leftWheel.rpm / idealRPM);
                else
                    scaledTorque = Mathf.Lerp(scaledTorque, 0, (axleInfo.leftWheel.rpm - idealRPM) / (maxRPM - idealRPM));

                axleInfo.leftWheel.motorTorque = scaledTorque;
                axleInfo.rightWheel.motorTorque = scaledTorque;

                if (Input.GetButton("Fire1"))
                {
                    axleInfo.leftWheel.brakeTorque = brakeMaxTorque;
                    axleInfo.rightWheel.brakeTorque = brakeMaxTorque;
                }
                else
                {
                    axleInfo.leftWheel.brakeTorque = 0;
                    axleInfo.rightWheel.brakeTorque = 0;
                }
            }
            ApplyLocalPositionToVisuals(axleInfo.leftWheel);
            ApplyLocalPositionToVisuals(axleInfo.rightWheel);
        }
    }

    void DoRollBar(WheelCollider WheelL, WheelCollider WheelR)
    {
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