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
using Java.Lang;

namespace StickerViewExample.StickerView
{
	public class StickerActionIcon
	{
		private Context context;		
		private Bitmap srcIcon;
		private Rect rect;

		public StickerActionIcon(Context context)
		{
			this.context = context;
			rect = new Rect();
		}

		public void setSrcIcon(int resource)
		{
			try
			{
				srcIcon = BitmapFactory.DecodeResource(context.Resources, resource);
			}
			catch (OutOfMemoryError em)
			{
				//Toast toast = Toast.MakeText(this.context, em.Message, ToastLength.Long);
				//toast.Show();

				////AlertDialog.Builder adb = new AlertDialog.Builder(this.context);
				////AlertDialog ad = adb.Create();
				////ad.SetMessage(em.Message);
				////ad.Show();

			}
			catch (System.Exception ex)
			{

			}
			
		}

		public void draw(Canvas canvas, float x, float y)
		{			
			rect.Left = (int)(x - srcIcon.Width / 2);
			rect.Right = (int)(x + srcIcon.Width / 2);
			rect.Top = (int)(y - srcIcon.Height / 2);
			rect.Bottom = (int)(y + srcIcon.Height / 2);
			canvas.DrawBitmap(srcIcon, null, rect, null);
		}

		public bool isInActionCheck(MotionEvent evt)
		{
			int left = rect.Left;
			int right = rect.Right;
			int top = rect.Top;
			int bottom = rect.Bottom;
			return evt.GetX(0) >= left && evt.GetX(0) <= right && evt.GetY(0) >= top && evt.GetY(0) <= bottom;
		}
    }
}
