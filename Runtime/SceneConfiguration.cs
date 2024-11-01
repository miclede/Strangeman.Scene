using Strangeman.Utils;
using Strangeman.Utils.Extensions;
using Strangeman.Utils.Service;
using UnityEngine;

namespace Strangeman.SceneHelper.Core
{
    [CreateAssetMenu(fileName = "SceneConfiguration", menuName = "Scene/Scene Configuration")]
    public class SceneConfiguration : ScriptableObject
    {
        [SerializeField] SceneField _scene;
        [SerializeField] LoadingGraphicController _loadingGraphic;
        [SerializeField] SceneTransition _sceneTransition;

        public SceneField Scene => _scene;

        public void StartLoad()
        {
            ServiceLocator.Global.GetMonoService(out SceneLoader sceneLoader);
            sceneLoader.StartLoad(this);
        }

        private void InitiateSceneLoad(SceneEventController eventController)
        {
            if (_loadingGraphic.InstantiateMono(out var loadingGraphic))
            {
                loadingGraphic.InitializeWith(eventController);
            }
        }

        private void InitiateSceneChange(SceneEventController eventController)
        {
            if (_sceneTransition.InstantiateMono(out var sceneTransition))
            {
                sceneTransition.InitializeWith(eventController);
            }
        }

        public void RegisterForLoad(SceneEventController eventController)
        {

            eventController.LoadingStarted += InitiateSceneLoad;
            eventController.LoadingCompleted += InitiateSceneChange;
        }

        public void DeregisterForLoad(SceneEventController eventController)
        {

            eventController.LoadingStarted -= InitiateSceneLoad;
            eventController.LoadingCompleted -= InitiateSceneChange;
        }
    }
}
