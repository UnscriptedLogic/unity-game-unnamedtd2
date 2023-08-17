using UnityEngine;

public class TowerComponent : MonoBehaviour
{
    protected TowerController tower;

    protected virtual void Awake()
    {
        tower = GetComponent<TowerController>();

        tower.OnControllerInitialized += OnControllerInitialized;
        tower.OnTowerInitialized += OnTowerInitialized;
    }

    protected virtual void OnTowerInitialized(object sender, System.EventArgs e) { }
    protected virtual void OnControllerInitialized(object sender, System.EventArgs e) { }
}
