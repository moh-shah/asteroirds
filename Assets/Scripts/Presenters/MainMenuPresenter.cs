using System;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Moshah.Asteroids.Presenters
{
    public class MainMenuPresenter : MonoBehaviour
    {
        [SerializeField] private TMP_Text spaceToContinueText;

        private void Start()
        {
            SetupDescriptionTextAnimation();
        }

        private void SetupDescriptionTextAnimation()
        {
            var fadeSequence = DOTween.Sequence();
            fadeSequence.Append(spaceToContinueText.DOFade(0, 1));
            fadeSequence.Append(spaceToContinueText.DOFade(1, 1));
            fadeSequence.SetLoops(1000);
            fadeSequence.Play();
        }
    }
}