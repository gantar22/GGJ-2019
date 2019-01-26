using System;
using System.Collections;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(Animator))]
public class DialogDisplay : MonoBehaviour
{
    [SerializeField]
    private DialogAsset _Dialog;

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

    private Animator _Animator;

    private bool _Displaying = false;
    private WaitUntil _ButtonPressWait;

    private readonly int _DisplayingHash = Animator.StringToHash("Displaying");
    private readonly int _ReadyForNextHash = Animator.StringToHash("ReadyForNext");

    [ContextMenu("Display (Test)")]
    public void Display()
    {
        Debug.Assert(Application.isPlaying);

        StartCoroutine(DisplayCR());
    }

    private IEnumerator DisplayCR()
    {
        if(_Displaying)
        {
            Debug.LogWarning("Tried to open dialog display when it is already opened.  Ignoring...");
            yield break;
        }

        _Displaying = true;
        _DisplayText.text = "";
        _Animator.SetBool(_DisplayingHash, true);
        yield return new WaitForSeconds(_StartTextCrawlDelay);

        for(int x = 0; x < _Dialog.DialogEntries.Length; x++)
        {
            _DisplayText.text = "";
            var entry = _Dialog.DialogEntries[x];

            var hold = Input.GetButton(_AdvanceDialogButton);
            var speed = entry.TextSpeed >= 0 ? entry.TextSpeed + 1 : 1.0f / (-entry.TextSpeed + 1);
            var wait = new WaitForSeconds(1.0f / (_DefaultCharactersPerSecond * (hold ? _HoldButtonSpeedup : 1.0f) * speed));

            for(int i = 0; i < entry.EntryText.Length; ++i)
            {
                char c = entry.EntryText[i];
                _DisplayText.text += c;

                if (c != ' ' && c != '\n')
                    yield return wait;
            }

            _Animator.SetBool(_ReadyForNextHash, true);
            yield return _ButtonPressWait;
            _Animator.SetBool(_ReadyForNextHash, false);
        }

        _Displaying = false;
        _Animator.SetBool(_DisplayingHash, false);
    }

    private void Start()
    {
        _Animator = GetComponent<Animator>();

        _ButtonPressWait = new WaitUntil(() => Input.GetButtonDown(_AdvanceDialogButton));
    }
}
