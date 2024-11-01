using Strangeman.Utils;
using System;
using System.Collections;
using UnityEngine;

namespace Strangeman.SceneHelper.Core
{
    public abstract class LoadingGraphicController : MonoBehaviour, IInitializeWith<SceneEventController>
    {
        SceneEventController _eventController;

        private Action OnCompleteLoad;

        public void InitializeWith(SceneEventController eventController)
        {
            _eventController = eventController;

            LoadingProgress(0);

            _eventController.LoadingProgressed += LoadingProgress;
            _eventController.TransitionStarted += CleanupLoadGraphics;
            OnCompleteLoad += _eventController.OnLoadingCompleted;
        }

        protected virtual void CleanupLoadGraphics()
        {
            Destroy(gameObject);
        }

        protected virtual void LoadingProgress(float progress)
        {
            if (progress == 1f) StartCoroutine(AllowLoadCompletion());
        }

        protected virtual IEnumerator AllowLoadCompletion()
        {
            OnCompleteLoad?.Invoke();

            yield break;
        }

        private void OnDisable()
        {
            if (_eventController is null) return;

            _eventController.LoadingProgressed -= LoadingProgress;
            _eventController.TransitionStarted -= CleanupLoadGraphics;
        }
    }
}
