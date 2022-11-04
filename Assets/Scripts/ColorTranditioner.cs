using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorTranditioner : MonoBehaviour
{
    [SerializeField] Image _img = default;
    [SerializeField] Color _toColor = default;
    [SerializeField] float _duration = 1;

    Color _flomColor;
    float _elapsed = 0;

    public bool IsCompleted => _img is null ? false : _img.color == _toColor;

    void Start()
    {
        if (_img is null) return;
        _flomColor = _img.color;
    }

    void Update()
    {
        _elapsed += Time.deltaTime;
        if( _elapsed < _duration)
        {
            _img.color = Color.Lerp(_flomColor,_toColor,_elapsed/_duration);
        }
        else _img.color = _toColor;
    }

    public void Show(Color color)
    {
        if(_img is null) return;
        _flomColor = _img.color;
        _toColor = color;
        _elapsed = 0;
    }

    public void Skip()
    {
        _elapsed = _duration;
    }
}
