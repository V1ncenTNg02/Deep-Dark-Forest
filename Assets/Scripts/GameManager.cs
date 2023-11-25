using UnityEngine;
using UnityEngine.SceneManagement;

namespace ORZ
{
    public class GameManager : MonoBehaviour
    {
        public GameObject gameOverUI;
        public WinGameEventTimeLine WETL;

            private const int MainMenu = 0;
        private const int Game = 1;

        public void PlayGame()
        {
            GetComponent<LoadingScene>().LoadGame();
        }

        public void GameOver()
        {
            ObjectGetter.player.SetActive(false);
            Time.timeScale = 0;
            gameOverUI.SetActive(true);
            SoundController.Instance.PlaySpecBGM("BGM2");
            SoundController.Instance.PlaySpecSound("Kill");
        }

        public void Restart()
        {
            SceneManager.LoadScene(Game);
            Time.timeScale = 1;
        }

        public void Menu()
        {
            SceneManager.LoadScene(MainMenu);
            Time.timeScale = 1;
        }

        public void Quit(){
            Application.Quit();
        }

        public void Win(){
            WETL.Win();
        }

        
    }
}
