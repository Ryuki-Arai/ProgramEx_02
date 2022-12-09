using System;
using System.Collections;
using UnityEngine;

public class Sample : MonoBehaviour
{
    private void Start()
    {
        StartCoroutine(ExecuteAsync());
    }

    private IEnumerator ExecuteAsync()
    {
        Debug.Log("ExecuteAsync: Begin");

        while (true)
        {
            // X���̉�]��2�b��
            yield return RotateAsync(Vector3.right, 2);

            // 5�b�ԑҋ@����E�܂��̓N���b�N�����̂�҂�
            yield return WhenAny(new WaitForSeconds(5), StartCoroutine(WaitMouseButton(0)));

            // Y���̉�]��2�b��
            yield return RotateAsync(Vector3.up, 2);

            // 5�b�ԑҋ@����E�܂��̓N���b�N�����̂�҂�
            yield return WhenAny(new WaitForSeconds(5), StartCoroutine(WaitMouseButton(0)));

            // Z���̉�]��2�b��
            yield return RotateAsync(Vector3.forward, 2);

            // 5�b�ԑҋ@����E�܂��̓N���b�N�����̂�҂�
            yield return WhenAny(new WaitForSeconds(5), StartCoroutine(WaitMouseButton(0)));
        }

        // Debug.Log("ExecuteAsync: End");
    }

    private IEnumerator RotateAsync(Vector3 eulers, float duration)
    {
        Debug.Log($"RotateAsync: Begin eulers={eulers}, duration={duration}");
        var t = 0F;
        while (t < duration)
        {
            t += Time.deltaTime;
            transform.Rotate(eulers);
            yield return null;
        }
        Debug.Log("ExecuteAsync: End");
    }

    private IEnumerator WaitClick(float timeout)
    {
        Debug.Log($"WaitClick: Begin timeout={timeout}");
        var t = 0F;
        while (t < timeout)
        {
            t += Time.deltaTime;
            yield return null;
            if (Input.GetMouseButton(0)) { break; }
        }
        Debug.Log($"WaitClick: End");
    }

    private IEnumerator WaitMouseButton(int button)
    {
        while (!Input.GetMouseButtonDown(button)) { yield return null; }
    }
    private IEnumerator Then(YieldInstruction c, Action completed)
    {
        yield return c;
        completed();
    }

    private IEnumerator WhenAny(YieldInstruction c1, YieldInstruction c2)
    {
        var result = false; // c1 �܂��� c2 ���I���� true �ɂȂ�
        StartCoroutine(Then(c1, () => result = true));
        StartCoroutine(Then(c2, () => result = true));
        while (!result) { yield return null; }
    }
}