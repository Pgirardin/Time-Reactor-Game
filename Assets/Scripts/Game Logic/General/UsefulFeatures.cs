using UnityEngine;

/// <summary>
/// �������� �������, ������������ � ������ ����
/// </summary>
public static class UsefulFeatures
{
    /// <summary>
    /// ���������, ��������� ������ ������ ������ � ��������� ����������� (������������� �����) �� ����������� ���� �� ����� ������ �� ����� �����
    /// (����� ������ � ����� ������ ������������ �������; ��������, ������ � ������ �������, � ����� � ���������� ����)
    /// </summary>
    public static float CalculateDepthOfObjectEntryIntoNearestSurface(Vector3 startPoint, Vector3 endPoint, LayerMask layersToCheck)
    {
        RaycastHit hit;
        var objectStartToEndRay = new Ray(startPoint, (endPoint - startPoint).normalized);
        var distanceBetweenObjectStartAndEnd = Vector3.Distance(startPoint, endPoint);

        if (Physics.Raycast(objectStartToEndRay, out hit, distanceBetweenObjectStartAndEnd, layersToCheck))
        {
            return Vector3.Distance(hit.point, endPoint);
        }

        return 0f;
    }
}