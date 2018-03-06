using Android.App;
using Android.Widget;
using Android.OS;
using Android.Views;
using Android.Support.V7.App;
using StickerViewExample.StickerView;
using Java.Lang;
using Android.Graphics;
using StickerViewExample.Utils;
using Java.IO;
using Android.Content;
using System;
using Android.Runtime;


//'/https://github.com/Kaka252/StickerView


namespace StickerViewExample
{
	[Activity(Label = "StickerViewExample", MainLauncher = true, Icon = "@mipmap/icon")]
	public class MainActivity : Activity, View.IOnClickListener
	{

		private StickerLayout stickerLayout;
		//private CompressTask task;
		private Thread t;

		public void OnClick(View v)
		{
			switch (v.Id)
			{
				case Resource.Id.tv_get_preview:
					stickerLayout.getPreview();
					break;
				case Resource.Id.tv_add_sticker:
					Intent intent = new Intent(this, typeof(StickerSelectorListActivity));
					StartActivityForResult(intent, 200);
					break;
				case Resource.Id.tv_generate_preview:
					Bitmap dstBitmap = stickerLayout.generateCombinedBitmap();

					//File successFile = FileUtils.getCacheFile();
					//task = new CompressTask(dstBitmap, this);
					//t = new Thread(task);
					//t.Start();

					break;
				default:
					break;
			}
		}

		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			// Set our view from the "main" layout resource
			SetContentView(Resource.Layout.activity_sticker_view);
			stickerLayout = (StickerLayout)FindViewById(Resource.Id.sticker_layout);
			stickerLayout.setBackgroundImage(Resource.Drawable.ic_avatar_1);
			stickerLayout.setZoomRes(Resource.Drawable.ic_resize);
			stickerLayout.setRemoveRes(Resource.Drawable.ic_remove);
			stickerLayout.setRotateRes(Resource.Drawable.ic_rotate);
			FindViewById(Resource.Id.tv_add_sticker).SetOnClickListener(this);
			FindViewById(Resource.Id.tv_generate_preview).SetOnClickListener(this);
			FindViewById(Resource.Id.tv_get_preview).SetOnClickListener(this);
		}


		protected override void OnActivityResult(int requestCode, [GeneratedEnum] Result resultCode, Intent data)
		{
			base.OnActivityResult(requestCode, resultCode, data);

			var fdfff = data.GetIntExtra("res",0);
			stickerLayout.addSticker(fdfff);
		}
	}
}


