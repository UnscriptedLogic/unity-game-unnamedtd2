using GameManagement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UnitManagement
{
    public class UnitEventHandler : MonoBehaviour
    {
        public Action<string> OnUnitDeath;
        public Action<ModificationType, string, float, float> OnUnitHealthModified;

        public void UnitHealthModified(ModificationType modificationType, string id, float amount, float currentHealth)
        {
            OnUnitHealthModified?.Invoke(modificationType, id, amount, currentHealth);
        }
    }
}