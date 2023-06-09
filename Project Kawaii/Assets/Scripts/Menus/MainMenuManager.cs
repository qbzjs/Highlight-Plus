using UnityEngine;
using System.Collections;

namespace MikelW.Menus
{
    public class MainMenuManager : MonoBehaviour
    {
        [SerializeField]
        private CustomizerManager customManager;

        [SerializeField]
        private Fader fadeObj;

        private void Awake()
        {
            customManager.LoadCustomization();
        }

        public void NewGame()
        {
            fadeObj.FadeOut();
            StartCoroutine(FadeDelay(2));
        }

        public void LoadGame()
        {
            //Add in loading functions and assign to StaticData.sceneIndexToLoad
            fadeObj.FadeOut();
            StartCoroutine(FadeDelay(2));
        }

        public void ExitGame()
        {
            MenuFunctions.ExitProject();
        }

        private IEnumerator FadeDelay(float time)
        {
            yield return new WaitForSeconds(time);
            MenuFunctions.LoadScene("LoadingScene");
        }
    }
}