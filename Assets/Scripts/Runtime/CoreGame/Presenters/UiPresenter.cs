using System.Collections;
using Moshah.Asteroids.Base;
using Moshah.Asteroids.Gameplay;
using Moshah.Asteroids.Models;
using TMPro;
using UnityEngine;
using Zenject;

namespace Moshah.Asteroids.Presenters
{
    public class UiPresenter : MonoBehaviour
    {
        [SerializeField] private GameObject loadingParent;
        [SerializeField] private GameObject mainMenuParent;
        [SerializeField] private GameObject gameplayParent;
        [SerializeField] private GameObject resultScreenParent;
        [Space]
        [SerializeField] private TMP_Text loadingText;
        [SerializeField] private TMP_Text spaceToContinueText;
        [SerializeField] private TMP_Text scoreText;
        [SerializeField] private TMP_Text hpText;
        [SerializeField] private TMP_Text yourScoreText;
        [SerializeField] private TMP_Text highScoreText;
        
        [Inject] private GameStateManager _gameStateManager;
        [Inject] private CoreGameController _coreGameController;
        [Inject] private GameConfig _gameConfig;
        [Inject] private SpaceShipController _spaceShipController;
        [Inject] private IScoreDataPort _scoreDataPort;


        private void Start()
        {
            _gameStateManager.OnStateChanged += OnGameStateChange;
            _coreGameController.OnScoreIncreased += OnScoreIncreased;
            _spaceShipController.OnHpChanged += OnHpChanged;
            ResetUi();
            SetupLoading();
        }

        private void Update()
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                switch (_gameStateManager.CurrentState)
                {
                    case GameState.MainMenu:
                        _gameStateManager.ChangeState(GameState.Gameplay);
                        break;
                }
            }
        }

        private void OnGameStateChange(GameState gameState)
        {
            ResetUi();
            switch (gameState)
            {
                case GameState.MainMenu:
                    SetupMainMenu();
                    break;
                
                case GameState.Gameplay:
                    SetupGameplay();
                    break;
                
                case GameState.ResultScreen:
                    SetupResultScreen();
                    break;
            }
        }

        private void OnScoreIncreased(int prevScore, int newScore)
        {
            scoreText.RunCounterRoutine(newScore);
        }

        private void OnHpChanged(int hp)
        {
            hpText.text = hp.ToString();
        }
        
        private void ResetUi()
        {
            loadingParent.gameObject.SetActive(false);
            mainMenuParent.gameObject.SetActive(false);
            gameplayParent.SetActive(false);
            resultScreenParent.SetActive(false);
        }

        private void SetupLoading()
        {
            loadingParent.SetActive(true);
            loadingText.AnimateGraphicsFadeInAndOut();
        }

        private void SetupGameplay()
        {
            gameplayParent.SetActive(true);
            hpText.text = _gameConfig.spaceshipHp.ToString();
            scoreText.text = "0";
        }

        private void SetupMainMenu()
        {
            mainMenuParent.SetActive(true);
            spaceToContinueText.AnimateGraphicsFadeInAndOut();
        }

        private void SetupResultScreen()
        {
            resultScreenParent.SetActive(true);
            yourScoreText.text = _coreGameController.TotalScore.ToString();
            highScoreText.text = _scoreDataPort.GetHighScore().ToString();
            spaceToContinueText.AnimateGraphicsFadeInAndOut();
            StartCoroutine(ChangeStateWithDelay());
        }

        private IEnumerator ChangeStateWithDelay()
        {
            yield return new WaitForSeconds(5);
            _gameStateManager.ChangeState(GameState.MainMenu);
        }
    }
}