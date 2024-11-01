using System;

namespace Strangeman.SceneHelper.Core
{
    public class SceneEventController
    {
        public Action<SceneEventController> LoadingStarted;
        public Action<float> LoadingProgressed;
        public Action<SceneEventController> LoadingCompleted;

        public Action TransitionStarted;
        public Action TransitionEnded;

        public Action LoadAllowed;

        public void OnTransitionEnded() => TransitionEnded?.Invoke();

        public void OnAllowLoad() => LoadAllowed?.Invoke();

        public void OnLoadingStarted() => LoadingStarted?.Invoke(this);

        public void OnLoadingProgressed(float progress) => LoadingProgressed?.Invoke(progress);

        public void OnLoadingCompleted() => LoadingCompleted?.Invoke(this);

        public void OnTransitionStart() => TransitionStarted?.Invoke();
    }
}
