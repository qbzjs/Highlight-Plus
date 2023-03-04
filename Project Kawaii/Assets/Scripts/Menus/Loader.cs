using UnityEngine;
using UnityEngine.UI;
using TMPro;
using MikelW.Data;

namespace MikelW.Menus
{
    public class Loader : MonoBehaviour
    {
        private float timeSpentLoading = 0;

        [SerializeField]
        private int artificialLoadTime = 3;

        [SerializeField]
        private TMP_Text textPercentage;
        [SerializeField]
        private Image loadingBar;

        private void Start()
        {
            MenuFunctions.LoadScene(StaticData.sceneIndexToLoad);
            MenuFunctions.SetSceneActivation(false);
        }

        private void Update()
        {
            timeSpentLoading += Time.deltaTime;

            textPercentage.text = MenuFunctions.GetLoadPercentage();
            loadingBar.fillAmount = MenuFunctions.GetLoadProgress();
            if (loadingBar.fillAmount >= 0.99f && timeSpentLoading >= artificialLoadTime)
            {
                Debug.Log("Done loading");
                MenuFunctions.SetSceneActivation(true);
            }
        }
    }
}