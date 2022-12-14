using System;
using UnityEngine;

public class Fader : MonoBehaviour
{
    private static Fader _instance;
    public static Fader Instance
    {
        get
        {
            if (_instance == null)
            {
                var prefab = Resources.Load<Fader>("Fader/Fader");
                _instance = Instantiate(prefab);
                DontDestroyOnLoad(_instance.gameObject);
            }

            return _instance;
        }
    }
    
    public bool IsFading { get; private set; }

    private Action _fadedInCallback;
    private Action _fadedOutCallback;
    
    [SerializeField] private Animator _animator;
    private const string ANIMATOR_BOOL_NAME = "IsFaded";
    private static readonly int IsFaded = Animator.StringToHash(ANIMATOR_BOOL_NAME);


    public void FadeIn(Action fadedInCallback)
    {
        if (IsFading) return;

        IsFading = true;
        _fadedInCallback = fadedInCallback;
        _animator.SetBool(IsFaded, true);
    }
    
    public void FadeOut(Action fadedOutCallback)
    {
        if (IsFading) return;

        IsFading = true;
        _fadedOutCallback = fadedOutCallback;
        _animator.SetBool(IsFaded, false);
    }

    private void Handle_FadeInAnimationOver()
    {
        _fadedInCallback?.Invoke();
        _fadedInCallback = null;
        IsFading = false;
    }
    
    private void Handle_FadeOutAnimationOver()
    {
        _fadedOutCallback?.Invoke();
        _fadedOutCallback = null;
        IsFading = false;
    }
}
