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
        // 10 초 후 비활성화
        releaseCoroutine = StartCoroutine(ReleaseAfterDelay(10f));
    }

    private void OnDisable()
    {
        // 비활성화 시 코루틴 초기화
        if (releaseCoroutine != null)
        {
            StopCoroutine(releaseCoroutine);
            releaseCoroutine = null;
        }
    }

    private IEnumerator ReleaseAfterDelay(float delay)
    {
        // delay 초 후 Release
        yield return new WaitForSeconds(delay);
        Release();
    }

    private void Release()
    {
        // Pool에 Release 요청
        Pool.Release(gameObject);
    }
}
