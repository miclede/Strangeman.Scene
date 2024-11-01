using Strangeman.Utils;
using Strangeman.Utils.Service;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Strangeman.SceneHelper.Core
{
    public enum LoadTransitionType { FromScene, IntoScene }

    public abstract class SceneTransition : MonoBehaviour, IInitializeWith<SceneEventController>
    {
        //Going from a Scene, into another Scene: Load, From, Into
        protected Action<LoadTransitionType> LoadTransition;
        protected Func<bool> FromSceneCondition;
        protected Func<bool> IntoSceneCondition;

        #region Functionality
        Action AllowLoad;
        Action TransitionEnded;

        public void InitializeWith(SceneEventController eventController)
        {
            ServiceLocator.Global.GetMonoService(out SceneLoader sceneLoader);
            transform.SetParent(sceneLoader.transform);

            SceneManager.sceneLoaded += OnBeginIntoTransition;

            AllowLoad += eventController.OnAllowLoad;
            TransitionEnded += FinishedTransitionCycle;
            TransitionEnded += eventController.OnTransitionEnded;

            eventController.OnTransitionStart();

            LoadTransition += OnBeginFromTransition;
            LoadTransition?.Invoke(LoadTransitionType.FromScene);
        }

        private void OnBeginFromTransition(LoadTransitionType transitionType)
        {
            if (transitionType is LoadTransitionType.FromScene)
                StartCoroutine(TransitionActivity(FromSceneCondition, AllowLoad));
        }

        private void OnBeginIntoTransition(Scene sceneLoaded, LoadSceneMode sceneLoadMode)
        {
            LoadTransition?.Invoke(LoadTransitionType.IntoScene);
            StartCoroutine(TransitionActivity(IntoSceneCondition, TransitionEnded));
        }

        private IEnumerator TransitionActivity(Func<bool> activityCondition, Action callback)
        {
            yield return new WaitUntil(activityCondition);
            callback?.Invoke();
        }

        private void FinishedTransitionCycle() => Destroy(gameObject);

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnBeginIntoTransition;
            AllowLoad = null;
            TransitionEnded = null;
            LoadTransition = null;
            FromSceneCondition = null;
            IntoSceneCondition = null;
        }
        #endregion
    }
}