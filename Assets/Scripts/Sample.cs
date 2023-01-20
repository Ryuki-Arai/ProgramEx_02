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

    /*private IEnumerator RunAsync()
    {
        while (true)
        {
            Debug.Log("マウスのボタン入力を待ちます");
            yield return WaitForMouseButtonDown(out var awaiter);

            // Awaiter の終了後は、必ず結果が保証されている
            Debug.Log($"マウスの{awaiter.Result}ボタンが押されました");
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
        // どのマウスボタンが押されたのか、結果を返したい。
        while (true)
        {
            for (var i = 0; i < 3; i++)
            {
                if (Input.GetMouseButtonDown(i))
                {
                    awaiter.SetResult(i); // 処理を終了・結果を設定
                    yield break;
                }
            }

            yield return null;
        }
    }//*/
    public interface IAwaiter<T> // 結果を受け取る側のためのインターフェイス
    {
        /// <summary>
        /// 処理が終了したかどうか。
        /// </summary>
        bool IsCompleted { get; }

        /// <summary>
        /// 処理の結果。
        /// </summary>
        T Result { get; }
    }
}