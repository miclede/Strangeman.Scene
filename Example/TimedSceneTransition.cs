using Strangeman.SceneHelper.Core;
using System;
using System.Collections;
using UnityEngine;

namespace Strangeman.SceneHelper.Example
{
    public class TimedSceneTransition : SceneTransition
    {
        [SerializeField] private float _fromActivityDuration = 2f;
        [SerializeField] private float _intoActivityDuration = 2f;

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
        }
        
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
    }

    public class TransitionStrategy
    {

        public (bool State, Func<bool> Condition) TransitionState;

        public TransitionStrategy()
        {
            TransitionState = (false, () => TransitionState.State);
        }

        public void UpdateCondition(bool update) => TransitionState = (update, () => TransitionState.State);
    }
}