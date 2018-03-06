using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Java.IO;

namespace StickerViewExample.Utils
{
	public class BitmapUtils
	{		
		public static bool saveBitmap(Bitmap bitmap, Java.IO.File file)
		{
			if (bitmap == null) return false;
			FileStream fos = null;
			try
			{
				fos = new FileStream(path:file.Path.ToString(), mode:FileMode.Create);
				
				bitmap.Compress(Bitmap.CompressFormat.Jpeg, 100, fos);
				fos.Flush();
				return true;
			}
			catch (Exception e)
			{
				//e.StackTrace();
			}
			finally
			{
				if (fos != null)
				{
					try
					{
						fos.Close();
					}
					catch (Exception er)
					{
						//e.printStackTrace();
					}
				}
			}
			return false;
		}

		public static Bitmap getSmallBitmap(String filePath, int reqWidth)
		{
			BitmapFactory.Options options = new BitmapFactory.Options();
			options.InJustDecodeBounds = true;//inJustDecodeBounds设置为true，可以不把图片读到内存中,但依然可以计算出图片的大小
			BitmapFactory.DecodeFile(filePath, options);
			float ow = options.OutWidth;
			float oh = options.OutHeight;
			float bl = 1;
			if (ow > reqWidth)
			{
				bl = reqWidth / ow;
			}
			ow = ow * bl;
			oh = oh * bl;
			int inSampleSize = computeSampleSize(options, -1, (int)(ow * oh));
			if (inSampleSize <= 0)
			{
				inSampleSize = 1;
			}
			options.InSampleSize = inSampleSize;
			options.InPreferredConfig = Bitmap.Config.Rgb565;
			options.InJustDecodeBounds = false;//重新读入图片，注意这次要把options.inJustDecodeBounds 设为 false
			return BitmapFactory.DecodeFile(filePath, options);// BitmapFactory.decodeFile()按指定大小取得图片缩略图
		}

		public static int computeSampleSize(BitmapFactory.Options options, int minSideLength, int maxNumOfPixels)
		{
			int initialSize = computeInitialSampleSize(options, minSideLength, maxNumOfPixels);
			int roundedSize;
			if (initialSize <= 8)
			{
				roundedSize = 1;
				while (roundedSize < initialSize)
				{
					roundedSize <<= 1;
				}
			}
			else
			{
				roundedSize = (initialSize + 7) / 8 * 8;
			}
			return roundedSize;
		}

		private static int computeInitialSampleSize(BitmapFactory.Options options, int minSideLength, int maxNumOfPixels)
		{
			double w = options.OutWidth;
			double h = options.OutHeight;
			int lowerBound = (maxNumOfPixels == -1) ? 1 : (int)Math.Ceiling(Math.Sqrt(w * h / maxNumOfPixels));
			int upperBound = (minSideLength == -1) ? 128 : (int)Math.Min(Math.Floor(w / minSideLength), Math.Floor(h / minSideLength));
			if (upperBound < lowerBound)
			{
				// return the larger one when there is no overlapping zone.
				return lowerBound;
			}
			if ((maxNumOfPixels == -1) && (minSideLength == -1))
			{
				return 1;
			}
			else if (minSideLength == -1)
			{
				return lowerBound;
			}
			else
			{
				return upperBound;
			}
		}

		public static void recycle(Bitmap bitmap)
		{
			if (bitmap != null && !bitmap.IsRecycled)
			{
				bitmap.Recycle();
			}
		}
	}

}