using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using StickerViewExample.StickerView;

namespace StickerViewExample.bs
{
	public class App : Application
	{

	private static App instance;

	public static App get()
	{
		return instance;
	}

		public override void OnCreate()
		{
			base.OnCreate();	
			instance = this;
			Lib.init(this);
		}
	}
}