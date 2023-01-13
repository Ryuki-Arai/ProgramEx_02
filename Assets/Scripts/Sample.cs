using System.Collections;
using UnityEngine;

public class SkipRequestSource
{
    /// <summary>
    /// �X�L�b�v����p�̃g�[�N����Ԃ��B
    /// </summary>
    public SkipRequestToken Token
        => new SkipRequestToken(this);

    /// <summary>
    /// �X�L�b�v��v������Ă���ꍇ�� true�B
    /// </summary>
    public bool IsSkipRequested { get; private set; }

    /// <summary>
    /// �X�L�b�v��v������B
    /// </summary>
    public void Skip() { IsSkipRequested = true; }
}

public struct SkipRequestToken
{
    private SkipRequestSource _source;

    public SkipRequestToken(SkipRequestSource source)
        => _source = source;

    /// <summary>
    /// �X�L�b�v��v������Ă���ꍇ�� true�B
    /// </summary>
    public bool IsSkipRequested => _source.IsSkipRequested;
}

public class Sample : MonoBehaviour
{
    [SerializeField]
    private Actor _actor = default;

    private void Start()
    {
        StartCoroutine(RunAsync());
    }

    private IEnumerator RunAsync()
    {
        while (true)
        {
            var skipSource = new SkipRequestSource();
            StartCoroutine(SkipIfClicked(skipSource));
            yield return _actor.FadeOut(2, skipSource.Token); // 2�b�����ăt�F�[�h�A�E�g

            yield return WaitClick(); // �N���b�N��҂�
            yield return null; // ���O�� GetMouseButtonDown ���A�����Ȃ��悤��1�t���[���҂�

            skipSource = new SkipRequestSource();
            StartCoroutine(SkipIfClicked(skipSource));
            yield return _actor.FadeIn(2, skipSource.Token); // �Q�b�����ăt�F�[�h�C��

            yield return WaitClick(); // �N���b�N��҂�
            yield return null;
        }
    }

    private IEnumerator SkipIfClicked(SkipRequestSource skipSource)
    {
        while (!IsSkipRequested()) { yield return null; }
        skipSource.Skip();
    }

    private IEnumerator WaitClick()
    {
        while (!IsSkipRequested()) { yield return null; }
    }

    private static bool IsSkipRequested()
    {
        return Input.GetMouseButtonDown(0);
    }
}