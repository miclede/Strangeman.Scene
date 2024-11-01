using Strangeman.SceneHelper.Core;
using Strangeman.Utils.Service;
using UnityEngine;

namespace Strangeman.SceneHelper.Example
{
    public class SceneDirector : MonoBehaviour
    {
        private enum LoadType { SceneLoader, SceneConfig }

        [SerializeField] SceneConfiguration _targetSceneConfiguration;
        [SerializeField] bool _loadOnStart;

        [SerializeField] LoadType _loadType;

        SceneLoader _sceneLoader;

        private void Awake()
        {
            ServiceLocator.Global.GetMonoService(out _sceneLoader);
        }

        private void Start()
        {
            if (_loadOnStart)
                StartLoad();
        }

        //Both load the scene using the same system
        public void StartLoad()
        {
            switch (_loadType)
            {
                case LoadType.SceneLoader:
                    _sceneLoader.StartLoad(_targetSceneConfiguration);
                    break;
                case LoadType.SceneConfig:
                    _targetSceneConfiguration.StartLoad();
                    break;
            }
        }
    }
}
