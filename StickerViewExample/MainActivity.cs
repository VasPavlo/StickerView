using Android.App;
using Android.Widget;
using Android.OS;
using Android.Support.V7.App;


//////https://github.com/kencheung4/android-StickerView
//////https://github.com/niravkalola/Android-StickerView


namespace StickerViewExample
{
	[Activity(Label = "StickerViewExample", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity
	{
		

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.Main);

			FrameLayout canvas = (FrameLayout)FindViewById(Resource.Id.canvasView);

			StickerImageView iv_sticker = new StickerImageView(this);
			iv_sticker.SetImageDrawable(Resources.GetDrawable(Resource.Drawable.c10));
			canvas.AddView(iv_sticker);
		}
	}
}

