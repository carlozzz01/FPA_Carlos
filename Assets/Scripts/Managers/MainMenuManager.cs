using UnityEngine;

namespace Managers
{
    public class MainMenuManager : MonoBehaviour
    {
        // Play music when the game starts
        private void Start()
        {
            Time.timeScale = 1;

            AudioManager.Instance.RestoreMusicPitch();

            AudioManager.Instance.PlayMusic("mainMenu");
        }

        /// <summary>
        /// Calls SceneController to load Game scene
        /// </summary>
        public void Play()
        {
            SceneController.Instance.LoadScene("Game");
        }

        /// <summary>
        /// Quits the game
        /// </summary>
        public void Quit()
        {
            Application.Quit();
        }

        /// <summary>
        /// Calls the AudioManager to change the master volume
        /// </summary>
        /// <param name="sliderValue"></param>
        public void SetMasterVolume(float sliderValue)
        {
            AudioManager.Instance.SetMasterVolume(sliderValue);
        }

        /// <summary>
        /// Calls the AudioManager to change the effects volume
        /// </summary>
        /// <param name="sliderValue"></param>
        public void SetEffectsVolume(float sliderValue)
        {
            AudioManager.Instance.SetEffectsVolume(sliderValue);
        }

        /// <summary>
        /// Calls the AudioManager to change the music volume
        /// </summary>
        /// <param name="sliderValue"></param>
        public void SetMusicVolume(float sliderValue)
        {
            AudioManager.Instance.SetMusicVolume(sliderValue);
        }
    }
}
