using UnityEngine;
using UnityEngine.UI;

namespace AGGE.UI {
    public class FlexibleGridLayout : LayoutGroup {
        [SerializeField]
        FitType fitType = default;
        [SerializeField, Range(1, 10)]
        int rows = 1;
        [SerializeField, Range(1, 10)]
        int columns = 1;
        [SerializeField]
        Vector2 spacing = default;
        [SerializeField]
        Vector2 cellSize = default;
        [SerializeField]
        bool fitX = false;
        [SerializeField]
        bool fitY = false;

        public enum FitType {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns
        }

        public override void CalculateLayoutInputHorizontal() {
            base.CalculateLayoutInputHorizontal();
            if (transform.childCount == 0 || rows == 0 || columns == 0) {
                return;
            }

            if (fitType == FitType.Width || fitType == FitType.Height || fitType == FitType.Uniform) {
                fitX = true;
                fitY = true;
                float sqrt = Mathf.Sqrt(transform.childCount);
                rows = Mathf.CeilToInt(sqrt);
                columns = Mathf.CeilToInt(sqrt);
            }

            if (fitType == FitType.Width || fitType == FitType.FixedColumns) {
                rows = Mathf.CeilToInt(transform.childCount / (float)columns);
            }
            if (fitType == FitType.Height || fitType == FitType.FixedRows) {
                columns = Mathf.CeilToInt(transform.childCount / (float)rows);
            }

            float parentWidth = rectTransform.rect.width;
            float parentHeight = rectTransform.rect.height;

            //float cellWidth = parentWidth / columns - (spacing.x / columns * (columns - 1)) - (padding.left / columns) - (padding.right / columns);
            //float cellHeight = parentHeight / rows - (spacing.y / rows * (rows - 1)) - (padding.top / rows) - (padding.bottom / rows);

            float cellWidth = (parentWidth - padding.left - padding.right - spacing.x * (columns - 1)) / columns;
            float cellHeight = (parentHeight - padding.top - padding.bottom - spacing.y * (rows - 1)) / rows;

            cellSize.x = fitX ? cellWidth : cellSize.x;
            cellSize.y = fitY ? cellHeight : cellSize.y;

            int columnCount = 0;
            int rowCount = 0;

            for (int i = 0; i < rectChildren.Count; i++) {
                rowCount = i / columns;
                columnCount = i % columns;

                var item = rectChildren[i];

                float xPos = (cellSize.x * columnCount) + (spacing.x * columnCount) + padding.left;
                float yPos = (cellSize.y * rowCount) + (spacing.y * rowCount) + padding.top;

                SetChildAlongAxis(item, 0, xPos, cellSize.x);
                SetChildAlongAxis(item, 1, yPos, cellSize.y);

            }
        }
        public override void CalculateLayoutInputVertical() {

        }
        public override void SetLayoutHorizontal() {

        }
        public override void SetLayoutVertical() {

        }

    }
}
