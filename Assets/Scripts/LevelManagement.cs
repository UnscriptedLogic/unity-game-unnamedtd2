using GridManagement;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameManagement
{
    public class LevelManagement : MonoBehaviour
    {
        [SerializeField] protected GridManager gridManager;

        private async void Start()
        {
            await gridManager.GenerateNodes();
        }
    }
}