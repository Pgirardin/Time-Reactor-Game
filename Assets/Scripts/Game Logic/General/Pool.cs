using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���������� ���� �������� (��� ����������� ������� � ������� ����������� ����������� ��������)
/// </summary>
public class Pool : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField] private int amount;
    private List<GameObject> objects;

    private void Awake()
    {
        objects = new List<GameObject>();
        GameProperties.GeneralPool.Add(gameObject.name, this);
    }

    private void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            var poolObject = Instantiate(prefab);
            poolObject.SetActive(false);
            objects.Add(poolObject);
            poolObject.transform.SetParent(gameObject.transform);
        }
    }

    /// <summary>
    /// �������� ���� �� ��������� ��� ������������� ��������
    /// </summary>
    public GameObject GetObject()
    {
        for (int i = 0; i < amount; i++)
        {
            if (!objects[i].activeInHierarchy)
            {
                objects[i].SetActive(true);
                return objects[i];
            }
        }

        return null;
    }

    /// <summary>
    /// ������� ������ � ���
    /// </summary>
    public void ReturnObject(GameObject poolObject)
    {
        poolObject.SetActive(false);
    }
}