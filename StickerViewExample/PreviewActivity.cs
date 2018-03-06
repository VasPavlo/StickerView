using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Text;
using Android.Views;
using Android.Widget;

namespace StickerViewExample
{
	public class PreviewActivity : Activity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);
			Intent intent = Intent;
			String path = intent.GetStringExtra("path");
			if (TextUtils.IsEmpty(path))
			{
				Toast.MakeText(this, "数据错误", ToastLength.Short).Show();
				Finish();
				return;
			}
			SetContentView(Resource.Layout.activity_preview);
			ImageView ivPreview = (ImageView)FindViewById(Resource.Id.iv_preview);
			ivPreview.SetImageURI(Android.Net.Uri.Parse(path));
		}
	}
}