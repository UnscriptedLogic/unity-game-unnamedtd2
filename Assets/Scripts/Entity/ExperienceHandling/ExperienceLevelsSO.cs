using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ExperienceLevel
{
    public int amount;
    public int totalSumOfBefore;
}

[CreateAssetMenu(fileName = "New Experience Levels", menuName = "ScriptableObjects/Create New Experience Levels")]
public class ExperienceLevelsSO : ScriptableObject
{
    [SerializeField] private List<ExperienceLevel> experienceLevels = new List<ExperienceLevel>();
    public List<ExperienceLevel> ExpList => experienceLevels;

    private void OnValidate()
    {
        for (int i = 0; i < experienceLevels.Count; i++)
        {
            if (i == 0) continue;
            experienceLevels[i].totalSumOfBefore = experienceLevels[i-1].amount + experienceLevels[i].amount;
        }
    }
}
