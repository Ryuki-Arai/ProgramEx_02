using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample : MonoBehaviour
{
    void Start()
    {
        RotateAsync();
    }

    IEnumerator RotateAsync()
    {
        Debug.Log("RotateAsync");
        while (true)
        {
            transform.Rotate(0, 0, 1);
            yield return null;
        }
    }
}
