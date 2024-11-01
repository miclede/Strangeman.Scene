using Strangeman.SceneHelper.Core;
using UnityEngine;
using TMPro;
using UnityEngine.InputSystem;
using System.Collections;

namespace Strangeman.SceneHelper.Example
{
    public class LogoLoading : LoadingGraphicController
    {
        [SerializeField] TMP_Text _loadingLogo;
        [SerializeField] TMP_Text _anyButtonText;

        private void Awake()
        {
            _anyButtonText.enabled = false;
        }

        protected override void LoadingProgress(float progress)
        {
            Color currentColor = _loadingLogo.color;
            currentColor.a = Mathf.Lerp(0, 1, progress);
            _loadingLogo.color = currentColor;

            base.LoadingProgress(progress);
        }

        protected override IEnumerator AllowLoadCompletion()
        {
            _anyButtonText.enabled = true;

            yield return new WaitUntil(() => Keyboard.current.anyKey.wasPressedThisFrame);

            StartCoroutine(base.AllowLoadCompletion());
        }
    }
}
