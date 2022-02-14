using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyModel : MonoBehaviour
{
    public MeshRenderer[] colorMeshes;
    public Transform gemLocation;

    void Start()
    {
        GetComponent<Animator>().SetFloat("Speed", GetComponentInParent<EnemyAI>().speed);
    }
}
