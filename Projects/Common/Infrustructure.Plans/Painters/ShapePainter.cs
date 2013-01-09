﻿using System.Windows;
using System.Windows.Shapes;
using Infrustructure.Plans.Elements;
using System.Windows.Media;

namespace Infrustructure.Plans.Painters
{
	public abstract class ShapePainter : IPainter
	{
		protected abstract Geometry CreateShape(ElementBase element);

		protected virtual Brush GetBrush(ElementBase element)
		{
			return PainterCache.GetBrush(element.BackgroundColor, element.BackgroundPixels);
		}
		protected virtual Pen GetPen(ElementBase element)
		{
			return PainterCache.GetPen(element.BorderColor, element.BorderThickness);
		}
		protected Geometry Geometry { get; private set; }
		protected Rect Rect { get; private set; }
		public virtual void Draw(DrawingContext drawingContext, ElementBase element, Rect rect)
		{
			Rect = rect;
			Geometry = CreateShape(element);
			Geometry.Freeze();
			drawingContext.DrawGeometry(GetBrush(element), GetPen(element), Geometry);
		}
	}
}
