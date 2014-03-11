﻿using System.Globalization;
using System.Windows;
using System.Windows.Media;
using FiresecAPI;
using FiresecAPI.SKD.PassCardLibrary;
using Infrustructure.Plans.Designer;
using Infrustructure.Plans.Painters;

namespace SKDModule.PassCard.Painter
{
	public class PassCardImagePropertyPainter : RectanglePainter
	{
		private GeometryDrawing _textDrawing;
		private ScaleTransform _scaleTransform;

		public PassCardImagePropertyPainter(CommonDesignerCanvas designerCanvas, ElementPassCardImageProperty element)
			: base(designerCanvas, element)
		{
		}

		protected override void InnerDraw(DrawingContext drawingContext)
		{
			base.InnerDraw(drawingContext);
			drawingContext.PushTransform(_scaleTransform);
			drawingContext.DrawDrawing(_textDrawing);
			drawingContext.Pop();
		}
		public override void Transform()
		{
			base.Transform();
			var height = Rect.Height > Element.BorderThickness ? Rect.Height - Element.BorderThickness : 0;
			var width = Rect.Width > Element.BorderThickness ? Rect.Width - Element.BorderThickness : 0;
			var bound = new Rect(Rect.Left + Element.BorderThickness / 2, Rect.Top + Element.BorderThickness / 2, width, height);
			var typeface = new Typeface(SystemFonts.CaptionFontFamily, FontStyles.Normal, FontWeights.Normal, new FontStretch());
			var formattedText = new FormattedText(((ElementPassCardImageProperty)Element).PropertyType.ToDescription(), CultureInfo.InvariantCulture, FlowDirection.LeftToRight, typeface, SystemFonts.CaptionFontSize, PainterCache.BlackBrush);
			formattedText.TextAlignment = TextAlignment.Center;
			Point point = bound.TopLeft;
			switch (formattedText.TextAlignment)
			{
				case TextAlignment.Right:
					point = bound.TopRight;
					break;
				case TextAlignment.Center:
					point = new Point(bound.Left + bound.Width / 2, bound.Top);
					break;
			}
			if (_scaleTransform != null)
			{
				_scaleTransform.CenterX = point.X;
				_scaleTransform.CenterY = point.Y;
				_scaleTransform.ScaleX = bound.Width / formattedText.Width;
				_scaleTransform.ScaleY = bound.Height / formattedText.Height;
			}
			_textDrawing.Geometry = formattedText.BuildGeometry(point);
		}
		public override void Invalidate()
		{
			_textDrawing = new GeometryDrawing(PainterCache.BlackBrush, null, null);
			_scaleTransform = new ScaleTransform();
			base.Invalidate();
		}
	}
}