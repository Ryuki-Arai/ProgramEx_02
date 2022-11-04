using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackGroundSequencer : MonoBehaviour
{
    [SerializeField]
    private ColorTranditioner _cd = default;

    [SerializeField]
    private Color[] _colors;

    private int _currentIndex = -1;

    void Start()
    {
        MoveNext();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (_cd is { IsCompleted: false })
            {
                _cd.Skip();
            }
            else  MoveNext(); 
        }
    }

    private void MoveNext()
    {
        if (_colors is null or { Length: 0 }) { return; }

        if (_currentIndex + 1 < _colors.Length)
        {
            _currentIndex++;
            _cd?.Show(_colors[_currentIndex]);
        }
    }
}
