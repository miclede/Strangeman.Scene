using Codice.CM.Common;
using Strangeman.SceneHelper.Core;
using System;
using System.Collections;
using UnityEngine;

namespace Strangeman.SceneHelper.Example
{
    // The commented out portion is a approach that does not invole using another class like: TransitionStrategy (Strategy pattern-like approach)
    public class TimedSceneTransition : SceneTransition
    {
        [SerializeField] private float _fromActivityDuration = 2f;
        [SerializeField] private float _intoActivityDuration = 2f;

        //TransitionStrategy _fromStrategy = new();
        //TransitionStrategy _intoStrategy = new();

        //bool _fromCondition;
        //bool _intoCondition;

        (float From, float Into) durationTuple;
        (TransitionStrategy From, TransitionStrategy Into) strategyTuple;

        private record TimedTransitionRecord(float Duration, TransitionStrategy Strategy);

        private void Awake()
        {
             durationTuple = (_fromActivityDuration, _intoActivityDuration);
             strategyTuple = (new(), new());
        }

        private void OnEnable()
        {
            LoadTransition += transitionType => TimedTransition(transitionType);
            FromSceneCondition = strategyTuple.From.TransitionState.Condition;
            IntoSceneCondition = strategyTuple.Into.TransitionState.Condition;

            //LoadTransition += TimedTransition;
            //FromSceneCondition = () => _fromCondition;
            //IntoSceneCondition = () => _intoCondition;
        }

        // Functional Approach
        private Coroutine TimedTransition(LoadTransitionType transitionType) => StartCoroutine(transitionType switch
        {
            LoadTransitionType.FromScene => 
                TimedTransitionActivity(new (durationTuple.From, strategyTuple.From)),
            LoadTransitionType.IntoScene => 
                TimedTransitionActivity(new(durationTuple.Into, strategyTuple.Into)),
            _ => throw new ArgumentOutOfRangeException(nameof(transitionType), transitionType, null)
        });


        private IEnumerator TimedTransitionActivity(TimedTransitionRecord timedRecord)
        {
            yield return new WaitForSeconds(timedRecord.Duration);

            timedRecord.Strategy.UpdateCondition(true);
        }

        // Traditional
        //private void TimedTransition(LoadTransitionType transitionType)
        //{
        //    // Approach 1
        //    //Action transitionAction = transitionType switch
        //    //{
        //    //    LoadTransitionType.FromScene => () => StartCoroutine(TimedTransitionActivity(_startActivityDuration, _fromStrategy)),
        //    //    LoadTransitionType.IntoScene => () => StartCoroutine(TimedTransitionActivity(_endActivityDuration, _intoStrategy)),
        //    //    _ => () => { }
        //    //};

        //    //transitionAction?.Invoke();


        //    // Approach 2
        //    //switch (transitionType)
        //    //{
        //    //    case LoadTransitionType.FromScene:
        //    //        StartCoroutine(TimedTransitionActivity(_startActivityDuration, () => _fromCondition = true));
        //    //        break;
        //    //    case LoadTransitionType.IntoScene:
        //    //        StartCoroutine(TimedTransitionActivity(_endActivityDuration, () => _intoCondition = true));
        //    //        break;
        //    //}
        //}

        //private IEnumerator TimedTransitionActivity(float time, TransitionStrategy strategy)
        //{
        //    yield return new WaitForSeconds(time);

        //    strategy.UpdateCondition(true);
        //}


        //private IEnumerator TimedTransitionActivity(float time, Action callback)
        //{
        //    yield return new WaitForSeconds(time);
        //    callback?.Invoke();
        //}
    }

    public class TransitionStrategy
    {
        //bool _condition;
        //public Func<bool> TransitionCondition;

        public (bool State, Func<bool> Condition) TransitionState;

        public TransitionStrategy()
        {
            //TransitionCondition = () => _condition;
            TransitionState = (false, () => TransitionState.State);
        }

        public void UpdateCondition(bool update) => TransitionState = (update, () => TransitionState.State);
    }
}