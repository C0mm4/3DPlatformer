using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapMeshCollider : TriggerInteractor
{
    public void Awake()
    {
        CombineAllMeshes();
    }

    public void CombineAllMeshes()
    {
        MeshFilter[] meshFilters = GetComponentsInChildren<MeshFilter>();
        List<CombineInstance> combineInstances = new List<CombineInstance>();

        foreach (MeshFilter mf in meshFilters)
        {
            if (mf.sharedMesh == null) continue;

            CombineInstance ci = new CombineInstance();
            ci.mesh = mf.sharedMesh;
            // 부모 로컬 기준 좌표 변환
            ci.transform = transform.worldToLocalMatrix * mf.transform.localToWorldMatrix;
            combineInstances.Add(ci);
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.indexFormat = UnityEngine.Rendering.IndexFormat.UInt32;
        combinedMesh.CombineMeshes(combineInstances.ToArray());

        MeshFilter newFilter = gameObject.AddComponent<MeshFilter>();
        newFilter.sharedMesh = combinedMesh;

        MeshRenderer newRenderer = gameObject.AddComponent<MeshRenderer>();
        newRenderer.sharedMaterial = meshFilters[0].GetComponent<MeshRenderer>().sharedMaterial;

        MeshCollider collider = gameObject.AddComponent<MeshCollider>();
        collider.sharedMesh = combinedMesh;
        collider.convex = true;      // 트리거 사용 위해 필수
        collider.isTrigger = true;   // 트리거 활성화
    }

    protected override void OnTriggerEvent(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Hit");
            PlayerManager.Instance.Player.condition.DecreaseHealth(1);
        }
    }
}
