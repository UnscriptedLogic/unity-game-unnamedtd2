using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Debri
{
    [Tooltip("The prefab of the debri to use")]    
    public GameObject prefab;
    
    [Tooltip("The size in radius to avoid instantiating objects too close to the path")]    
    public float size;
}

[CreateAssetMenu(fileName = "New Debri Theme", menuName = "ScriptableObjects/Create New Debri Theme")]
public class DebriThemeSO : ScriptableObject
{
    [SerializeField] private List<Debri> debris = new List<Debri>();

    public List<Debri> Debris => debris;
}
