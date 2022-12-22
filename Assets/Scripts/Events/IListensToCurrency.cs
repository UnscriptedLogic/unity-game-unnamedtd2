using GameManagement;
using UnityEngine;

public interface IListensToCurrency
{
    void OnCurrencyChanged(ModificationType modificationType, float modificationAmount, float currentTotal);
}
