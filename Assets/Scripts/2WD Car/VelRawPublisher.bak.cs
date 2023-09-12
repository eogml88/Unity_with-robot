using System;
using UnityEngine;
using Unity.Robotics.ROSTCPConnector;
using RosMessageTypes.Geometry;
using RosMessageTypes.BuiltinInterfaces;
using Unity.Robotics.Core;

public class VelRawPublisher : MonoBehaviour
{
    [SerializeField] private string topicName = "vel_raw";
    [SerializeField] private float publishFrequency = 0.1f;

    ROSConnection ros;
    private float timeElapsed = 0.0f;

    private TwistStampedMsg vel_raw;

    private void Start()
    {
        ros = ROSConnection.GetOrCreateInstance();
        ros.RegisterPublisher<TwistStampedMsg>(topicName);

        vel_raw = new TwistStampedMsg();
    }

    private void FixedUpdate()
    {
        timeElapsed += Time.deltaTime;

        if (timeElapsed >= publishFrequency)
        {
            // now timestamp
            var now = Clock.Now;
            var stamp = new TimeMsg
            {
                sec = (int)now,
                nanosec = (uint)((now - Math.Floor(now)) * Clock.k_NanoSecondsInSeconds)
            };
            // cal vel_raw
            vel_raw.twist.linear.x = 0;
            vel_raw.twist.linear.y = 0;
            vel_raw.twist.angular.z = 0;
            // init timeElapsed
            timeElapsed = 0;
            // publish ros topic
            vel_raw.header.stamp = stamp;
            ros.Publish(topicName, vel_raw);
        }
    }
}