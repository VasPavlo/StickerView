using System;
using Android.Content;
using Android.Graphics;
using Android.Graphics.Drawables;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace StickerViewExample
{
	public class StickerImageView : StickerView
	{
		private String owner_id;
		private ImageView iv_main;
		public StickerImageView(Context context) : base(context)
		{

		}

		public StickerImageView(Context context, IAttributeSet attrs) : base(context, attrs)
		{

		}

		public StickerImageView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{

		}


		public void SetOwnerId(String owner_id)
		{
			this.owner_id = owner_id;
		}

		public String GetOwnerId()
		{
			return this.owner_id;
		}

		public void SetImageResource(int res_id)
		{
			this.iv_main.SetImageResource(res_id);
		}

		public void SetImageDrawable(Drawable drawable) { iv_main.SetImageDrawable(drawable); }

		public Bitmap GetImageBitmap() { return ((BitmapDrawable)this.iv_main.Drawable).Bitmap; }

		protected override View GetMainView()
		{
			if (this.iv_main == null)
			{
				this.iv_main = new ImageView(Context);
				this.iv_main.SetScaleType(ImageView.ScaleType.FitXy);
			}
			return iv_main;
		}
	}
}
