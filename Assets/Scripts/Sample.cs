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

    /*private IEnumerator RunAsync()
    {
        while (true)
        {
            Debug.Log("�}�E�X�̃{�^�����͂�҂��܂�");
            yield return WaitForMouseButtonDown(out var awaiter);

            // Awaiter �̏I����́A�K�����ʂ��ۏ؂���Ă���
            Debug.Log($"�}�E�X��{awaiter.Result}�{�^����������܂���");
            yield return null;
        }
    }

    public IEnumerator WaitForMouseButtonDown(out IAwaiter<int> awaiter)
    {
        var awaiterImpl = new Awaiter<int>();
        var e = WaitForMouseButtonDown(awaiterImpl);
        awaiter = awaiterImpl;
        return e;
    }

    private IEnumerator WaitForMouseButtonDown(Awaiter<int> awaiter)
    {
        // �ǂ̃}�E�X�{�^���������ꂽ�̂��A���ʂ�Ԃ������B
        while (true)
        {
            for (var i = 0; i < 3; i++)
            {
                if (Input.GetMouseButtonDown(i))
                {
                    awaiter.SetResult(i); // �������I���E���ʂ�ݒ�
                    yield break;
                }
            }

            yield return null;
        }
    }//*/
    public interface IAwaiter<T> // ���ʂ��󂯎�鑤�̂��߂̃C���^�[�t�F�C�X
    {
        /// <summary>
        /// �������I���������ǂ����B
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// �����̌��ʁB
        /// </summary>
        T Result { get; }
    }
}