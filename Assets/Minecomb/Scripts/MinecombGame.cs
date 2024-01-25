using TMPro;
using UnityEngine;

public class MinecombGame : MonoBehaviour
{
    [SerializeField]
    TextMeshPro mineAmount;

    [SerializeField, Min(1)]
    int
        rowsAmount = 8,
        columnAmount = 21;
}