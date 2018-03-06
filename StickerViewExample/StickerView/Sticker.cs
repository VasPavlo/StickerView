using System;
using Android.Graphics;
using Android.Views;
using StickerViewExample.Utils;

namespace StickerViewExample
{
	public class Sticker
	{
		private Matrix matrix;
		private Bitmap srcImage;

		public Sticker(Bitmap bitmap)
		{
			this.srcImage = bitmap;
			matrix = new Matrix();
		}

		public void draw(Canvas canvas)
		{
			canvas.DrawBitmap(srcImage, matrix, null);
		}

		public PointF getMidPoint(MotionEvent evnt)
		{
			PointF point = new PointF();
			float x = evnt.GetX(0) + evnt.GetX(1);
			float y = evnt.GetY(0) + evnt.GetY(1);
			point.Set(x / 2, y / 2);
			return point;
		}

		public PointF getImageMidPoint(Matrix matrix)
		{
			PointF point = new PointF();
			float[] points = PointUtils.GetBitmapPoints(srcImage, matrix);
			float x1 = points[0];
			float x2 = points[2];
			float y2 = points[3];
			float y4 = points[7];
			point.Set((x1 + x2) / 2, (y2 + y4) / 2);
			return point;
		}

		public float getSpaceRotation(MotionEvent ev, PointF imageMidPoint)
		{
			double deltaX = ev.GetX(0) - imageMidPoint.X;
			double deltaY = ev.GetY(0) - imageMidPoint.Y;
			double radians = Math.Atan2(deltaY, deltaX);
			return (float)RadianToDegree(radians);
		}

		public float getMultiTouchDistance(MotionEvent ent)
		{
			float x = ent.GetX(0) - ent.GetX(1);
			float y = ent.GetY(0) - ent.GetY(1);
			return (float) Math.Sqrt(x* x + y* y);
		}

		public float getSingleTouchDistance(MotionEvent et, PointF imageMidPoint) 
		{
			float x = et.GetX(0) - imageMidPoint.X;
			float y = et.GetY(0) - imageMidPoint.Y;
			return (float) Math.Sqrt(x* x + y* y);
		}


		public RectF getSrcImageBound()
		{
			RectF dst = new RectF();
			matrix.MapRect(dst, new RectF(0, 0, getStickerWidth(), getStickerHeight()));
			return dst;
		}

		public int getStickerWidth()
		{
			return srcImage == null ? 0 : srcImage.Width;
		}

		public int getStickerHeight()
		{
			return srcImage == null ? 0 : srcImage.Height;
		}

		public Matrix getMatrix()
		{
			return matrix;
		}

		public Bitmap getSrcImage()
		{
			return srcImage;
		}


		private double RadianToDegree(double angle)
		{
			return angle * (180.0 / Math.PI);
		}

		private double DegreeToRadian(double angle)
		{
			return Math.PI * angle / 180.0;
		}
	}
}
