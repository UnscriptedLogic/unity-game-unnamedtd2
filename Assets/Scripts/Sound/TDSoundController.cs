using UnityEngine;

public class TDSoundController : MonoBehaviour
{
    private FXManager fxManager;
    private BuildManager buildManager;

    private void Start()
    {
        fxManager = FXManager.instance;

        SceneController.OnLevelFinishedLoading += SceneController_OnLevelFinishedLoading;

        UpgradeButton.OnAnyUpgradeButtonClicked += PlayUpgradeButtonClick;
    }

    private void SceneController_OnLevelFinishedLoading(object sender, LevelLoadEventArgs e)
    {
        Debug.Log("Called!", gameObject);
        if (e.scene == SceneIndexes.TITLE)
        {
            fxManager.PlayThemeStartScreenSound();
        } else
        {
            fxManager.PlayThemeAtmosphereSound();
        }

        buildManager = BuildManager.instance;
        if (buildManager != null)
        {
            buildManager.OnBuild += PlayTowerBuildSound;
        }
    }

    private void PlayUpgradeButtonClick(object sender, bool isBought)
    {
        if (isBought)
        {
            fxManager.PlaySound(fxManager.GlobalEffects.RandomUpgradeBought.audioSettings, Vector3.zero);
        }
    }

    private void PlayTowerBuildSound(object sender, OnBuildEventArgs e)
    {
        TowerBase tower = e.buildObject.GetComponent<TowerBase>();
        if (tower != null)
        {
            fxManager.PlayFXPair(fxManager.GlobalEffects.TowerPlaced, e.position);
        }
    }
}
