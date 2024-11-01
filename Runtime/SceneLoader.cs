using Strangeman.Utils.Service;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Strangeman.SceneHelper.Core
{
    public class SceneLoader : GlobalMonoService<SceneLoader>
    {
        SceneEventController _sceneEventController;

        SceneConfiguration _targetSceneConfig;

        AsyncOperation _loader;

        private void OnEnable()
        {
            if (_sceneEventController is null)
            {
                _sceneEventController = new SceneEventController();
                _sceneEventController.LoadAllowed += AllowLoad;
            }
            else _sceneEventController.LoadAllowed += AllowLoad;
        }

        protected override void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        public async void StartLoad(SceneConfiguration sceneConfig)
        {
            _targetSceneConfig = sceneConfig;
            _targetSceneConfig.RegisterForLoad(_sceneEventController);

            await LoadAsync(_targetSceneConfig.Scene);
        }

        private async Awaitable LoadAsync(string toLoad)
        {
            float lastProgress = 0;

            _sceneEventController.OnLoadingStarted();

            _loader = SceneManager.LoadSceneAsync(toLoad);
            _loader.allowSceneActivation = false;

            _sceneEventController.LoadingProgressed(_loader.progress);

            while (_loader.progress < 0.9f)
            {
                UpdateProgress(ref lastProgress);

                await Awaitable.NextFrameAsync();
            }

            UpdateProgress(ref lastProgress);

            while (!_loader.isDone)
            {
                await Awaitable.NextFrameAsync();
            }

            _targetSceneConfig.DeregisterForLoad(_sceneEventController);
        }

        private void UpdateProgress(ref float lastProgress)
        {
            float progress = Mathf.Clamp01(_loader.progress / 0.9f);
            var needsUpdate = lastProgress != progress ? true : false;

            if (needsUpdate)
            {
                lastProgress = progress;
                _sceneEventController.LoadingProgressed(lastProgress);
            }
        }

        private void AllowLoad() => _loader.allowSceneActivation = true;

        private void OnDisable()
        {
            _sceneEventController.LoadAllowed -= AllowLoad;
        }
    }
}
