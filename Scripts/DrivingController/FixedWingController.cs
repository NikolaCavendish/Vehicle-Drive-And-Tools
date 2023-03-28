using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//Fixed-wing plane
public class FixedWingController : MonoBehaviour
{

    Transform m_transform;

    /// <summary>
    /// 刚体
    /// </summary>
    Rigidbody m_rigidbody;

    /// <summary>
    /// 牵引力
    /// </summary>
    public float motorForce = 20f;

    /// <summary>
    /// 制动力
    /// </summary>
    public float brakeForce = 50f;

    /// <summary>
    /// 起飞速度
    /// </summary>
    public float takeOffSpeed = 10f;

    /// <summary>
    /// 最大移动速度
    /// </summary>
    public float maxMoveSpeed = 30f;
    public float yawSpeed = 5f;
    public float pitchSpeed = 2f;
    public float rollSpeed = 8f;

    // Start is called before the first frame update
    void Start()
    {
        m_transform = transform;
        m_rigidbody = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MoveForward();
        Yaw();
    }

    void MoveForward()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            return;
        }
        //速度够了可以起飞
        Debug.Log("m_rigidbody.velocity.magnitude" + m_rigidbody.velocity.magnitude);
        if (m_rigidbody.velocity.magnitude > takeOffSpeed)
        {
            m_rigidbody.useGravity = false;
            Pitch();
            Roll();
        }
        else
        {
            m_rigidbody.useGravity = true;
        }

        //使用W和S键控制飞机的前进和后退
        float vertical = Input.GetAxisRaw("Vertical");
        float move = vertical * motorForce * Time.deltaTime;
        move = Mathf.Clamp(move, -maxMoveSpeed / 2, maxMoveSpeed);
        m_rigidbody.AddForce(m_transform.forward * move, ForceMode.VelocityChange);

    }

    /// <summary>
    /// 偏航
    /// </summary>
    void Yaw()
    {
        //使用A和D键控制飞机的左右旋转（yaw）
        float horizontal = Input.GetAxisRaw("Horizontal");
        float yaw = horizontal * yawSpeed * Time.deltaTime;
        m_rigidbody.AddTorque(m_transform.up * yaw, ForceMode.VelocityChange);
    }

    /// <summary>
    /// 俯仰
    /// </summary>
    void Pitch()
    {
        //使用小键盘的8和5键控制飞机的上下旋转（pitch）
        float pitchDir = Input.GetAxisRaw("Pitch");
        float pitch = pitchDir * pitchSpeed * Time.deltaTime;
        m_rigidbody.AddTorque(m_transform.right * pitch, ForceMode.VelocityChange);
    }

    /// <summary>
    /// 横滚
    /// </summary>
    void Roll()
    {
        //使用小键盘的4和6键控制飞机的左右倾斜（roll）
        float rollDir = Input.GetAxisRaw("Roll");
        float roll = rollDir * rollSpeed * Time.deltaTime;
        m_rigidbody.AddTorque(m_transform.forward * -roll, ForceMode.VelocityChange);
    }
}

