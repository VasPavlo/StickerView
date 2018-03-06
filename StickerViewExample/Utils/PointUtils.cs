using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;

namespace StickerViewExample.Utils
{
	public class PointUtils
	{
		public static float[] GetBitmapPoints(Bitmap bitmap, Matrix matrix)
		{
			float[] dst = new float[8];
			float[] src = new float[]
			{
				0, 0,
				bitmap.Width, 0,
				0, bitmap.Height,
				bitmap.Width, bitmap.Height
			};

			matrix.MapPoints(dst, src);
			return dst;
		}
	}
}