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
            // �θ� ���� ���� ��ǥ ��ȯ
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
        collider.convex = true;      // Ʈ���� ��� ���� �ʼ�
        collider.isTrigger = true;   // Ʈ���� Ȱ��ȭ
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
