using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoSingleton<T> : MonoBehaviour
{
    private static Lazy<T> instance;

    public static T Instance => instance.Value;

    protected virtual void Awake()
    {
        instance = new Lazy<T>(this);
    }
}

public class GameManager : MonoSingleton<GameManager>
{
    protected override void Awake()
    {
        base.Awake();
    }
}

public class Test
{
    private GameManager _gameManager = GameManager.Instance;
}