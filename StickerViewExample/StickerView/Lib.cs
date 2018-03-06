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
using Java.Lang;

namespace StickerViewExample.StickerView
{
	public class Lib
	{
		private static Application app;

		public static void init(Application app)
		{
			Lib.app = app;
		}

		public static Application getInstance()
		{
			if (Lib.app == null)
			{
				throw new IllegalArgumentException("LBase application is null");
			}
			return Lib.app;
		}
	}
}