using System.Collections.Generic;
using UnityEngine;

namespace AGGE.GameOfLife {
    public class GameManager : MonoBehaviour {
        [SerializeField]
        Transform grid = default;
        [SerializeField]
        GameObject cellPrefab = default;

        [SerializeField, Range(1, 100)]
        int width = 10;
        [SerializeField, Range(1, 100)]
        int height = 10;

        IEnumerable<Vector3> allPositionsWithin {
            get {
                for (int x = 0; x < width; x++) {
                    for (int y = 0; y < height; y++) {
                        yield return transform.position + new Vector3(x, 0, y);
                    }
                }
            }
        }

        protected void Start() {
            foreach (var position in allPositionsWithin) {
                var cell = Instantiate(cellPrefab, position, Quaternion.identity, grid);
                cell.name = $"Cell {position}";
            }
            transform.position -= new Vector3(width / 2, Mathf.Sqrt(width * height), height / 2);
        }
    }
}