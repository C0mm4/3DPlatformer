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
        // 10 �� �� ��Ȱ��ȭ
        releaseCoroutine = StartCoroutine(ReleaseAfterDelay(10f));
    }

    private void OnDisable()
    {
        // ��Ȱ��ȭ �� �ڷ�ƾ �ʱ�ȭ
        if (releaseCoroutine != null)
        {
            StopCoroutine(releaseCoroutine);
            releaseCoroutine = null;
        }
    }

    private IEnumerator ReleaseAfterDelay(float delay)
    {
        // delay �� �� Release
        yield return new WaitForSeconds(delay);
        Release();
    }

    private void Release()
    {
        // Pool�� Release ��û
        Pool.Release(gameObject);
    }
}
