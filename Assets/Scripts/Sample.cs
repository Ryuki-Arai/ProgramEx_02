using System.Collections;
using UnityEngine;

public class SkipRequestSource
{
    /// <summary>
    /// スキップ判定用のトークンを返す。
    /// </summary>
    public SkipRequestToken Token
        => new SkipRequestToken(this);

    /// <summary>
    /// スキップを要求されている場合は true。
    /// </summary>
    public bool IsSkipRequested { get; private set; }

    /// <summary>
    /// スキップを要求する。
    /// </summary>
    public void Skip() { IsSkipRequested = true; }
}

public struct SkipRequestToken
{
    private SkipRequestSource _source;

    public SkipRequestToken(SkipRequestSource source)
        => _source = source;

    /// <summary>
    /// スキップを要求されている場合は true。
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
            yield return _actor.FadeOut(2, skipSource.Token); // 2秒かけてフェードアウト

            yield return WaitClick(); // クリックを待つ
            yield return null; // 直前の GetMouseButtonDown が連続しないように1フレーム待つ

            skipSource = new SkipRequestSource();
            StartCoroutine(SkipIfClicked(skipSource));
            yield return _actor.FadeIn(2, skipSource.Token); // ２秒かけてフェードイン

            yield return WaitClick(); // クリックを待つ
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