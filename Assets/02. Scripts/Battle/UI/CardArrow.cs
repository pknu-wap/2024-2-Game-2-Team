using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class CardArrow : MonoBehaviour
{
    #region �̱���
    public static CardArrow Instance { get; private set; }
    void Awake() => Instance = this;
    #endregion �̱���

    #region ȭ��ǥ ����
    public GameObject[] points;

    [Header("ȭ��ǥ ����")]
    // ȭ��ǥ �� ���� ����
    public int numberOfPoints;
    // ���� ����
    public float space;
    // ���� ������
    public GameObject point;
    // ���κ� ������
    public GameObject arrow;
    // ������ � �߰� ��ǥ
    public Vector3 startPosition;
    // ������ � �߰� ��ǥ
    public Transform middlePosition;

    // ���콺 �̺�Ʈ�� ���� ������Ʈ
    public GameObject mouseEventBlocker;

    // �θ� ȭ��ǥ ������Ʈ, ���� ��ġ�� transform�� �������� �Ѵ�.

    public void Start()
    {
        startPosition = transform.position;

        points = new GameObject[numberOfPoints + 1];

        // ������ numberOfPoints �� �����ϰ�
        for(int i = 0; i < numberOfPoints; ++i)
        {
            points[i] = Instantiate(point, transform);
        }

        // ���κе� �����Ѵ�.
        points[numberOfPoints] = Instantiate(arrow, transform);

        // ���� ������ ���� ���´�.
        HideArrow();
    }

    public void ShowArrow()
    {
        ShowBlocker();
        gameObject.SetActive(true);
    }

    public void MoveStartPosition(Vector2 position)
    {
        startPosition = position;
    }

    public void MoveArrow(Vector2 targetPosition)
    {
        // for�� �ȿ� ���� ó���� ���� ������ ������ ����. (ȸ���� �� �ֱ� ����
        points[0].transform.position = startPosition;

        for(int i = 1; i < numberOfPoints + 1; ++i)
        {
            // ������ � ��, �ڽ��� ��ġ�� ã�� �̵��Ѵ�.
            points[i].transform.position = GetBezierLerp(startPosition, middlePosition.position, targetPosition, (float)i / numberOfPoints);

            // ������ �ڱ� �ڽ��� ��ġ - ���� ����Ʈ�� ��ġ�� �����Ѵ�.
            Vector2 delta = points[i].transform.position - points[i - 1].transform.position;
            float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

            points[i].transform.rotation = Quaternion.Euler(0, 0, angle - 90);
        }
    }

    public void HideArrow()
    {
        gameObject.SetActive(false);
        HideBlocker();
    }

    Vector2 GetBezierLerp(Vector2 start, Vector2 middle, Vector2 end, float t)
    {
        float oneMinusT = 1f - t;
        return oneMinusT * oneMinusT * start
            + 2f * oneMinusT * t * middle
            + t * t * end;
    }
    #endregion ȭ��ǥ ����

    #region �̺�Ʈ ����Ŀ
    public void ShowBlocker()
    {
        mouseEventBlocker.SetActive(true);
    }

    public void HideBlocker()
    {
        mouseEventBlocker.SetActive(false);
    }
    #endregion �̺�Ʈ ����Ŀ
}