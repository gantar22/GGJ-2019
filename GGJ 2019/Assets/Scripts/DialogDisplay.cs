using System;
using System.Text.RegularExpressions;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class DialogDisplay : MonoBehaviour
{
    [SerializeField]
    private List<DialogAsset> _Dialog;

    [SerializeField]
    int_event_object play_index;


    [Header("Scene")]
    [SerializeField]
    private TextMeshProUGUI _DisplayText;

    [Header("Input")]
    [SerializeField]
    private string _AdvanceDialogButton;

    [Header("Gameplay")]
    [SerializeField]
    private float _StartTextCrawlDelay = 0.25f;
    [SerializeField]
    private float _DefaultCharactersPerSecond = 20;
    [SerializeField]
    private float _HoldButtonSpeedup = 2.0f;

    [SerializeField]
    private event_object _TestEvent;

    private List<Vector2Int> wobbleRanges = new List<Vector2Int>();

    private Animator _Animator;

    private int _CurrentCharacter = 0;

    private bool _Displaying = false;
    private WaitUntil _ButtonPressWait;

    private float _StartWobbleTime;
    private bool _Wobbling = false;

    private readonly int _DisplayingHash = Animator.StringToHash("Displaying");
    private readonly int _ReadyForNextHash = Animator.StringToHash("ReadyForNext");

    private readonly Regex _WobbleRegex = new Regex(@"(\[wobble\]).*?(\[\/wobble\])", RegexOptions.Compiled | RegexOptions.IgnoreCase);

    public void SetDialog(List<DialogAsset> d)
    {
        _Dialog = d;
    }

    [ContextMenu("Display (Test)")]
    public void Display(int i)
    {
        if (!Application.isPlaying)
            return;

        StartCoroutine(DisplayCR(i));
    }

    private IEnumerator DisplayCR(int i)
    {
        if(_Displaying)
        {
            Debug.LogWarning("Tried to open dialog display when it is already opened.  Ignoring...");
            yield break;
        }

        _Displaying = true;
        _DisplayText.text = "";
        _Animator.SetBool(_DisplayingHash, true);
        wobbleRanges.Clear();
        yield return new WaitForSeconds(_StartTextCrawlDelay);

        for (int x = 0; x < _Dialog[i].DialogEntries.Length; x++)
        {
            _DisplayText.text = "";
            _CurrentCharacter = 0;
            wobbleRanges.Clear();
            var entry = _Dialog[i].DialogEntries[x];

            if(entry.ExecuteOnEnter)
                entry.ExecuteOnEnter.Invoke();

            int lastIdx = 0;
            int skipped = 0;
            StringBuilder sb = new StringBuilder();
            Match match = _WobbleRegex.Match(entry.EntryText);
            while (match.Success)
            {
                int idx1s = match.Groups[1].Index;
                int idx1e = match.Groups[1].Index + match.Groups[1].Length;
                int idx2s = match.Groups[2].Index;
                int idx2e = match.Groups[2].Index + match.Groups[2].Length;
                string s = "";
                foreach (Group g in match.Groups)
                    s += g.Index + " " + (g.Index + g.Length) + " ; ";
                Debug.Log(s);

                if(idx1e > idx2s)
                {
                    int t1 = idx1s; int t2 = idx1e;
                    idx1s = idx2s;
                    idx1e = idx2e;
                    idx2s = t1;
                    idx2e = t2;
                }

                skipped += match.Groups[1].Length;
                wobbleRanges.Add(new Vector2Int(idx1e - skipped, idx2s - skipped));
                skipped += match.Groups[2].Length;

                sb.Append(entry.EntryText.Substring(lastIdx, idx1s - lastIdx));
                sb.Append(entry.EntryText.Substring(idx1e, idx2s - idx1e));
                lastIdx = idx2e;

                match = match.NextMatch();
            }
            sb.Append(entry.EntryText.Substring(lastIdx));
            string entryText = sb.ToString();

            var speed = entry.TextSpeed >= 0 ? entry.TextSpeed + 1 : 1.0f / (-entry.TextSpeed + 1);
            var waitSlow = new WaitForSeconds(1.0f / (_DefaultCharactersPerSecond * speed));
            var waitFast = new WaitForSeconds(1.0f / (_DefaultCharactersPerSecond * speed * _HoldButtonSpeedup));

            bool hasStoppedHolding = false; // so that you need to let go of mouse before speeding up text

            for (i = 0; i < entryText.Length; ++i)
            {
                var hold = Input.GetButton(_AdvanceDialogButton);
                hasStoppedHolding = hasStoppedHolding || !hold;

                char c = entryText[i];
                _DisplayText.text += c;
                _CurrentCharacter++;

                if (c != ' ' && c != '\n')
                    yield return (hasStoppedHolding && hold) ? waitFast : waitSlow;
            }

            _Animator.SetBool(_ReadyForNextHash, true);
            _StartWobbleTime = Time.time;
            _Wobbling = true;
            yield return _ButtonPressWait;
            _Wobbling = false;
            _Animator.SetBool(_ReadyForNextHash, false);

            if (entry.ExecuteOnExit)
                entry.ExecuteOnExit.Invoke();
        }

        _Displaying = false;
        _Animator.SetBool(_DisplayingHash, false);
        wobbleRanges.Clear();
    }

    public void UpdateTextMesh()
    {
        Func<int, int, int, bool> inside = (int a, int b, int x) => x >= a && x < b;
        var textInfo = _DisplayText.textInfo;

        var meshInfo = textInfo.meshInfo;
        int count = textInfo.characterCount;

        _DisplayText.ForceMeshUpdate(); // reset positions

        int cur_range = 0;
        for (int i = 0; i < count; ++i)
        {
            bool wobble = false;
            if(cur_range < wobbleRanges.Count)
            {
                Vector2Int r = wobbleRanges[cur_range];
                wobble = inside(r.x, r.y, i);
                if(i >= r.y)
                    cur_range++;
            }

            int materialIndex = textInfo.characterInfo[i].materialReferenceIndex;
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;
            for (int j = 0; j < 4; ++j)
            {
                if(_Wobbling && wobble)
                    meshInfo[materialIndex].vertices[vertexIndex + j] += Vector3.up * Mathf.Clamp01((Time.time - _StartWobbleTime) / 0.2f) * Mathf.Sin(i * 0.2f + Time.time * 5f) * 3;
            }
        }

        _DisplayText.UpdateVertexData(TMP_VertexDataUpdateFlags.Vertices);
        _DisplayText.RecalculateMasking();
        _DisplayText.RecalculateClipping();
        _DisplayText.CalculateLayoutInputHorizontal();
    }

    private void Start()
    {
        _Animator = GetComponent<Animator>();

        _ButtonPressWait = new WaitUntil(() => Input.GetButtonDown(_AdvanceDialogButton));

        play_index.e.AddListener(Display);

        _TestEvent.addListener(() => Debug.Log("Just finished!"));
    }

    private void Update()
    {
        UpdateTextMesh();
    }
}
