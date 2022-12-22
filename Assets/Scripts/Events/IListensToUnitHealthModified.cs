using GameManagement;
using UnityEngine;

public interface IListensToUnitHealthModified
{
    void OnUnitHealthDeducted(ModificationType modificationType, string id, float modAmount, float currHealth);
}
