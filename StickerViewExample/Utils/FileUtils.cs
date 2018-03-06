

using Android.OS;
using Java.IO;
using StickerViewExample.StickerView;

namespace StickerViewExample.Utils
{
	public static class FileUtils
	{
		private static File mCacheFile;

		public static File getCacheFile()
		{
			File file = new File(getAppCacheDir(), "image");
			if (!file.Exists())
			{
				file.Mkdirs();
			}
			string fileName = "temp_" + Java.Lang.JavaSystem.CurrentTimeMillis() + ".jpg";
			return new File(file, fileName);
		}

		private static File getAppCacheDir()
		{
			if (Environment.MediaMounted.Equals(Environment.ExternalStorageState)
					|| !Environment.IsExternalStorageRemovable)
			{
				mCacheFile = Lib.getInstance().GetExternalCacheDirs()[0]; ///
			}
			if (mCacheFile == null)
			{
				mCacheFile = Lib.getInstance().CacheDir;
			}
			return mCacheFile;
		}
	}
}