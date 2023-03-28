using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class CarController : MonoBehaviour
{
    public float maxAngle = 35;
    public float AngleSpeed = 50;

    [Tooltip("牵引扭矩")]
    public float motorTorque = 3000;
    [Tooltip("制动扭矩")]
    public float brakeTorque = 5000;

    //注意这八个变量，四个是获取车轮碰撞器的，四个是获取车轮模型的
    public WheelCollider leftF;
    public WheelCollider leftB;
    public WheelCollider rightF;
    public WheelCollider rightB;

    public Transform model_leftF;
    public Transform model_leftB;
    public Transform model_rightF;
    public Transform model_rightB;

    /// <summary>
    /// 车辆刚体
    /// </summary>
    Rigidbody _rigidbody;

    private void Start()
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.centerOfMass = new Vector3(0, -0.6f, 0);

    }

    void Update()
    {
        WheelsControl_Update();
    }

    //控制移动 转向
    void WheelsControl_Update()
    {
        //垂直轴和水平轴
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        //前轮角度，后轮驱动
        //steerAngle:转向角度，总是围绕自身Y轴，转向

        //修改成逐渐增加到最大值
        leftF.steerAngle += horizontal * AngleSpeed * Time.deltaTime;
        leftF.steerAngle = Mathf.Clamp(leftF.steerAngle, -maxAngle, maxAngle);

        rightF.steerAngle += horizontal * AngleSpeed * Time.deltaTime;
        rightF.steerAngle = Mathf.Clamp(rightF.steerAngle, -maxAngle, maxAngle);

        if (horizontal == 0)
            leftF.steerAngle = rightF.steerAngle = 0;

        //motorTorque:电机转矩，驱动车轮
        leftB.motorTorque = vertical * motorTorque;
        rightB.motorTorque = vertical * motorTorque;

        if (Input.GetKey(KeyCode.Space)) // Input.GetKey(KeyCode.Space)
        {
            leftB.brakeTorque = brakeTorque;
            rightB.brakeTorque = brakeTorque;
        }


        //当车轮碰撞器位置角度改变，随之也变更车轮模型的位置角度
        WheelsModel_Update(model_leftF, leftF);
        WheelsModel_Update(model_leftB, leftB);
        WheelsModel_Update(model_rightF, rightF);
        WheelsModel_Update(model_rightB, rightB);
    }

    //控制车轮模型移动 转向
    void WheelsModel_Update(Transform t, WheelCollider wheel)
    {
        Vector3 pos = t.position;
        Quaternion rot = t.rotation;

        wheel.GetWorldPose(out pos, out rot);

        t.position = pos;
        t.rotation = rot;
    }

}

