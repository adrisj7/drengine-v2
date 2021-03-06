﻿using System;
using Microsoft.Xna.Framework;
using Math = GameEngine.Util.Math;

namespace GameEngine.Game.UI
{
    public class UIGridLayout : UIComponent
    {
        public enum FitTypes
        {
            Uniform,
            Width,
            Height,
            FixedRows,
            FixedColumns
        }

        public Vector2 CellSize;
        public int Columns;

        public FitTypes FitType = FitTypes.Uniform;

        /// <summary>
        ///     Set to false if you want manual control on the number of columns
        /// </summary>
        public bool FitX = true;

        /// <summary>
        ///     Set to false if you want manual control on the number of rows
        /// </summary>
        public bool FitY = true;

        public Padding Padding;

        public int Rows;

        public Vector2 Spacing;

        public UIGridLayout(GamePlus game, UIComponent parent = null) : base(game, parent)
        {
        }

        protected override void Draw(UIScreen screen, Rect targetRect)
        {
            // Layout our children.
            var childCount = ChildCount;
            if (FitType == FitTypes.Width || FitType == FitTypes.Height || FitType == FitTypes.Uniform)
            {
                var sqRt = Math.Sqrt(childCount);

                Rows = Math.CeilToInt(sqRt);
                Columns = Math.CeilToInt(sqRt);
            }

            switch (FitType)
            {
                case FitTypes.Uniform:
                    break;
                case FitTypes.FixedColumns:
                case FitTypes.Width:
                    Rows = Math.CeilToInt(childCount / (float) Columns);
                    break;
                case FitTypes.FixedRows:
                case FitTypes.Height:
                    Columns = Math.CeilToInt(childCount / (float) Rows);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }

            float parentWidth = targetRect.Width,
                parentHeight = targetRect.Height;

            float cellWidth =
                    parentWidth / Columns - Spacing.X / Columns * 2f - (Padding.Left + Padding.Right) / Columns,
                cellHeight = parentHeight / Rows - Spacing.Y / Rows * 2f - (Padding.Top + Padding.Bottom) / Rows;
            CellSize.X = FitX ? cellWidth : CellSize.X;
            CellSize.Y = FitY ? cellHeight : CellSize.Y;

            int columnCount = 0,
                rowCount = 0;
            var i = 0;
            foreach (var child in Children)
            {
                rowCount = i / Columns;
                columnCount = i % Columns;

                float xpos = CellSize.X * columnCount + Spacing.X * columnCount + Padding.Left,
                    ypos = CellSize.Y * rowCount + Spacing.Y * rowCount + Padding.Top;
                child.WithLayout(
                    Layout.CornerLayout(Layout.TopLeft, CellSize.X, CellSize.Y)
                        .OffsetBy(xpos, ypos)
                );
                ++i;
            }
        }
    }
}