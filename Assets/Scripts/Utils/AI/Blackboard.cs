using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Utils.AI;

public class Blackboard
{
    private Dictionary<string, IBlackboardType> data = new();

    public bool AddData<T>(string key, T value)
    {
        if (!data.ContainsKey(key))
        {
            BlackboardType<T> blackboardType = new BlackboardType<T>(value);
            data[key] = blackboardType;
            return true;
        }
        return false;
    }

    public bool ChangeData<T>(string key, T newValue)
    {
        if (data.TryGetValue(key, out var currentValue))
        {
            BlackboardType<T> blackboardType = (BlackboardType<T>)currentValue;
            blackboardType.SetValue(newValue);
            return true;
        }

        return false;
    }
    
    public T GetData<T>(string key)
    {
        if (data.TryGetValue(key, out var value))
        {
            BlackboardType<T> blackboardType = (BlackboardType<T>)value;
            return blackboardType.GetValue();
        }

        return default;
    }
}
