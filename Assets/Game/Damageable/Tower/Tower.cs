using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FateGames.Core;
using UnityEngine.Events;
using DG.Tweening;

public partial class Tower : DamageableStructure
{
    [SerializeField] private Transform pointContainer;
    private readonly List<Transform> points = new();
    [SerializeField] private SaveDataVariable saveData;
    [SerializeField] private UnityEvent OnMergeAvailable;
    [SerializeField] private LevelManager levelManager;
    [SerializeField] private GameObject meshObject;
    [SerializeField] private Transform structurePartsContainer;
    [SerializeField] private BoolVariable isTowerFull;
    [SerializeField] private Transform mergePoint;
    private StructurePart[] structureParts = null;
    private void Awake()
    {
        InitializeStructureParts();
    }
    private void Start()
    {
        InputManager.GetKeyDownEvent(KeyCode.C).AddListener(Collapse);
        InputManager.GetKeyDownEvent(KeyCode.R).AddListener(Rewind);
        Initialize();
    }
    public void Initialize()
    {
        for (int i = 0; i < saveData.Value.SoldierTable.Length; i++)
        {
            for (int j = 0; j < saveData.Value.SoldierTable[i]; j++)
            {
                AddSoldier(i, false);
            }
        }
    }
    protected override void OnEnable()
    {
        base.OnEnable();
        SetPoints();
    }

    private Transform GetPoint(int index)
    {
        return points[index];
    }

    private void SetPoints()
    {
        for (int i = 0; i < pointContainer.childCount; i++)
            points.Add(pointContainer.GetChild(i));
    }

    public void PlaceSoldiers()
    {
        List<Soldier> soldiers = GetSortedSoldierList();

        for (int i = 0; i < soldiers.Count; i++)
        {
            Soldier soldier = soldiers[i];
            soldier.SetPosition(points[i].position);
        }
    }

    public override void Die()
    {
        Debug.Log("Die", this);
        Collapse();
        OnDied.Invoke();
        IEnumerator finishLevelAfterSeconds(float t)
        {
            yield return new WaitForSeconds(t);
            levelManager.FinishLevel(false);
        }
        StartCoroutine(finishLevelAfterSeconds(3));
    }

    public void Collapse()
    {
        meshObject.SetActive(false);
        structurePartsContainer.gameObject.SetActive(true);
        if (structureParts == null) InitializeStructureParts();
        for (int i = 0; i < structureParts.Length; i++)
        {
            StructurePart structurePart = structureParts[i];
            structurePart.Collapse();
        }
    }
    public void Rewind()
    {
        for (int i = 0; i < structureParts.Length; i++)
        {
            StructurePart structurePart = structureParts[i];
            structurePart.Rewind();
        }
    }
    private void InitializeStructureParts()
    {
        structureParts = new StructurePart[structurePartsContainer.childCount];
        for (int i = 0; i < structureParts.Length; i++)
        {
            Transform structurePartTransform = structurePartsContainer.GetChild(i);
            structureParts[i] = new(structurePartTransform);
        }
    }
    private class StructurePart
    {
        public Transform transform;
        public Rigidbody rigidbody;
        public MeshCollider meshCollider;
        public Vector3 originalPosition;
        public Quaternion originalRotation;

        public StructurePart(Transform transform)
        {
            this.transform = transform;
            originalPosition = transform.position;
            originalRotation = transform.rotation;
            InitializeRigidbody();
            InitializeMeshCollider();
            transform.gameObject.layer = 12;
        }
        private void InitializeRigidbody()
        {
            rigidbody = transform.gameObject.AddComponent<Rigidbody>();
            rigidbody.isKinematic = true;
        }
        private void InitializeMeshCollider()
        {
            meshCollider = transform.gameObject.AddComponent<MeshCollider>();
            meshCollider.convex = true;
        }

        public void Collapse()
        {
            rigidbody.isKinematic = false;
        }
        public void Rewind()
        {
            rigidbody.isKinematic = true;
            transform.DOMove(originalPosition, 3);
            transform.DORotateQuaternion(originalRotation, 3);
        }
    }
}

public partial class SaveData
{
    public int[] SoldierTable = new int[8] { 1, 0, 0, 0, 0, 0, 0, 0 };
}