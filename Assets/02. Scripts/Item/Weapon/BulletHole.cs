using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

public class BulletHole : MonoBehaviour
{
    public IObjectPool<GameObject> Pool { get; set; }
    private Coroutine releaseCoroutine;

    private void OnEnable()
    {
        releaseCoroutine = StartCoroutine(ReleaseAfterDelay(10f));
    }

    private void OnDisable()
    {
        if (releaseCoroutine != null)
        {
            StopCoroutine(releaseCoroutine);
            releaseCoroutine = null;
        }
    }

    private IEnumerator ReleaseAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Release();
    }
    private void Release()
    {
        Pool.Release(gameObject);
    }
}
