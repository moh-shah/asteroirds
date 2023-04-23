using System;
using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Moshah.Asteroids.Base
{
    public class MonoBehaviourHelper : MonoBehaviour
    {
        
    }

    public static class MonoUtils
    {
        private static MonoBehaviourHelper _monoBehaviourHelper;
        public static void InjectMonoHelper(MonoBehaviourHelper monoBehaviourHelper)
        {
            _monoBehaviourHelper = monoBehaviourHelper;
        }

        public static void RunCounterRoutine(this TMP_Text tmpText, int to)
        {
            tmpText.StopAllCoroutines();
            int.TryParse(tmpText.text, out var from);
            _monoBehaviourHelper.StartCoroutine(CounterRoutine(tmpText, from, to));
        }

        private static IEnumerator CounterRoutine(TMP_Text tmpText, int from, int to)
        {
            var wait = new WaitForSeconds(.1f);
            tmpText.text = from.ToString();
            var counter = from;
            while (counter < to)
            {
                counter++;
                tmpText.text = counter.ToString();
                yield return wait;
            }

            tmpText.text = to.ToString();
        }
        
        public static void AnimateGraphicsFadeInAndOut(this Graphic tmpText)
        {
            var fadeSequence = DOTween.Sequence();
            fadeSequence.Append(tmpText.DOFade(0, 1));
            fadeSequence.Append(tmpText.DOFade(1, 1));
            fadeSequence.SetLoops(1000);
            fadeSequence.Play();
        }
    }
}