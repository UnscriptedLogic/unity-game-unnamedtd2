using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitHealthBarSpawner : MonoBehaviour
{
    [SerializeField] private GameObject healthbarPrefab;
    [SerializeField] private float distanceFromHead = 0.5f;
    private void Start()
    {
        UnitBase.OnAnyUnitSpawned += UnitBase_OnAnyUnitSpawned;
    }

    private void UnitBase_OnAnyUnitSpawned(object sender, System.EventArgs e)
    {
        UnitBase entity = sender as UnitBase;
        BoxCollider boxCollider = entity.GetComponent<BoxCollider>();

        float height = boxCollider.size.y + distanceFromHead;
        GameObject healthbar = Instantiate(healthbarPrefab, entity.transform);

        healthbar.transform.localPosition = new Vector3(0f, height, 0f);
        healthbar.transform.localScale = entity.ID.Contains("boss") ? Vector3.one * 0.0015f : Vector3.one * 0.001f;

        healthbar.GetComponentInChildren<WorldSpaceCustomSlider>().SetUnit(entity);
    }
}
