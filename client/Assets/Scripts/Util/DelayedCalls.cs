using System;
using System.Linq;
using UnityEngine;
using System.Collections.Generic;

public class DelayedCalls : MonoBehaviour
{
    class Call
    {
        public Action action;
        public float time;
        public bool isDisable = false;
    }

    private static DelayedCalls _instance;
    private static DelayedCalls instance
    {
        get
        {
            if (_instance == null)
            {
                var loader = GameObject.Find("System");
                if (loader == null)
                    loader = new GameObject("System");

                if (loader != null)
                {
                    _instance = loader.GetComponent<DelayedCalls>();
                    if (_instance == null)
                    {
                        _instance = loader.AddComponent<DelayedCalls>();
                    }
                }
            }
            return _instance;
        }
    }

    List<Call> callList = new List<Call>();
    public static void Add(Action ac, float deltaTime)
    {
        instance.callList.Add(new Call { action = ac, time = deltaTime, isDisable = false });
    }

    void Update()
    {
        bool called = false;
        foreach (var call in callList)
        {
            call.time -= Time.unscaledDeltaTime;
            if (call.time < 0
                && call.isDisable == false)
            {
                call.action();
                call.isDisable = true;
                called = true;
            }
        }

        if (false == called
            && callList.Count > 0)
        {
            callList.RemoveAll(call => call.isDisable);
        }
    }
}
