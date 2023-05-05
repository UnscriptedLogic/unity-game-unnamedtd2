using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExperienceLevel
{
    public int amount;
    public int totalSumOfBefore;
    public int difference { get; private set; }

    public void SetDifference(int value) => difference = value;
}

[CreateAssetMenu(fileName = "New Experience Levels", menuName = "ScriptableObjects/Create New Experience Levels")]
public class ExperienceLevelsSO : ScriptableObject
{
    [SerializeField] private List<ExperienceLevel> experienceLevels = new List<ExperienceLevel>();
    public List<ExperienceLevel> ExpList => experienceLevels;

    [Header("Debug")]
    [SerializeField] private bool memorizeDifference;
    [SerializeField] private int modifiedStartIndex;
    [SerializeField] private bool applyDifference;

    private void OnValidate()
    {
        for (int i = 0; i < experienceLevels.Count; i++)
        {
            if (i == 0) continue;
            experienceLevels[i].totalSumOfBefore = experienceLevels[i-1].amount + experienceLevels[i].amount;

            if (memorizeDifference)
            {
                experienceLevels[i].SetDifference(experienceLevels[i].amount - experienceLevels[i-1].amount);
            }

            if (applyDifference)
            {
                if (i <= modifiedStartIndex) continue;
                experienceLevels[i].amount = experienceLevels[i - 1].amount + experienceLevels[i].difference;
            }
        }

        if (memorizeDifference)
        {
            Debug.Log("Differences memorized");
            memorizeDifference = false;
        }

        if (applyDifference)
        {
            Debug.Log("Differences Applied");
            applyDifference = false;
        }
    }
}
