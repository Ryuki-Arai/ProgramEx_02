using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TextManager : MonoBehaviour
{
    [SerializeField] string[] text;
    [SerializeField] TextMeshProUGUI caption;
    [SerializeField] TextMeshProUGUI containt;
    [SerializeField] float textSpeed;
    int i;
    float time;
    void Start()
    {
        containt.GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        TextWrite(text[0]);
    }

    private void TextWrite(string _text)
    {
        if (_text.Length <= i) return;
        time += Time.deltaTime;
        if (time >= textSpeed)
        {
            containt.text += _text[i];
            time = 0f;
            i++;
        }
    }
}
