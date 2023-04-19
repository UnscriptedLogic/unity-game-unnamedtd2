using System.Collections;
using UnityEngine;

public class DamageFlashSkinned : MonoBehaviour
{
    [SerializeField] private Material flashMat;
    [SerializeField] private SkinnedMeshRenderer skinnedMeshRenderer;
    [SerializeField] private float blinkDuration = 0.075f;

    private Material[] defaultMaterials;

    private void Start()
    {
        defaultMaterials = skinnedMeshRenderer.materials;
    }

    public void Flash()
    {
        StopAllCoroutines();
        StartCoroutine(FadeDelay());
    }

    private IEnumerator FadeDelay()
    {
        Material[] flashMats = new Material[skinnedMeshRenderer.materials.Length];
        for (int i = 0; i < skinnedMeshRenderer.materials.Length; i++)
        {
            flashMats[i] = flashMat;
        }

        skinnedMeshRenderer.materials = flashMats;

        yield return new WaitForSeconds(blinkDuration);
        
        skinnedMeshRenderer.materials = defaultMaterials;
    }
}