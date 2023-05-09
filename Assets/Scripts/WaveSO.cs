using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnscriptedLogic.WaveSystems.Asynchronous.Timed;

[CreateAssetMenu(fileName = "New Wave", menuName = "ScriptableObjects/Create New Wave")]
public class WaveSO : ScriptableObject
{
    [SerializeField] private List<Wave> waves;

    public List<Wave> Waves => waves;
}
