using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace StickerViewExample.StickerView
{
	public class StickerLayout : FrameLayout
	{
		private Context context;
		public List<StickerView> stickerViews;
		private LayoutParams stickerParams;
		private ImageView ivImage;

		private int rotateRes;
		private int zoomRes;
		private int removeRes;

		public StickerLayout(Context context) : this(context, null)
		{
		}
		public StickerLayout(Context context, IAttributeSet attrs) : this(context, attrs, 0)
		{
		}
		public StickerLayout(Context context, IAttributeSet attrs, int defStyleAttr) : base(context, attrs, defStyleAttr)
		{	
			Init(context);
		}

		private void Init(Context context)
		{
			this.context = context;
			stickerViews = new List<StickerView>();
			stickerParams = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
			addBackgroundImage();
		}

		private void addBackgroundImage()
		{
			LayoutParams bgParams = new LayoutParams(LayoutParams.MatchParent, LayoutParams.MatchParent);
			ivImage = new ImageView(context);
			ivImage.SetScaleType(ImageView.ScaleType.FitXy);
			ivImage.LayoutParameters = bgParams;
			AddView(ivImage);
		}

		public void setBackgroundImage(int resource)
		{
			ivImage.SetImageResource(resource);
		}

		public void addSticker(int resource)
		{
			Bitmap bitmap = BitmapFactory.DecodeResource(context.Resources, resource);
			addSticker(bitmap);
		}


		public void addSticker(Bitmap bitmap)
		{
			StickerView sv = new StickerView(context);
			sv.SetImageBitmap(bitmap);
			sv.LayoutParameters = stickerParams;
			sv.setOnStickerActionListener(new OnStickerActionListener(sv, this, stickerViews));

			AddView(sv);
			stickerViews.Add(sv);
			redraw();
		}

		public void getPreview()
		{
			foreach (var item in stickerViews)
			{				
				item.setEdit(false);
			}			
		}
		public void redraw()
		{
			redraw(true);
		}
		private void redraw(bool isNotGenerate)
		{
			int size = stickerViews.Count();
			if (size <= 0) return;

			for (int i = size - 1; i >= 0; i--)
			{
				StickerView item = stickerViews[i];
				if (item == null) continue;
				item.setZoomRes(zoomRes);
				item.setRotateRes(rotateRes);
				item.setRemoveRes(removeRes);
				if (i == size - 1)
				{
					item.setEdit(isNotGenerate);
				}
				else
				{
					item.setEdit(false);
				}
				stickerViews.Insert(i, item);
			}
		}

		public Bitmap generateCombinedBitmap()
		{
			redraw(false);
			Bitmap dst = Bitmap.CreateBitmap(Width, Height, Bitmap.Config.Argb8888);
			Canvas canvas = new Canvas(dst);
			Draw(canvas);
			return dst;
		}

		public void setRotateRes(int rotateRes)
		{
			this.rotateRes = rotateRes;
		}

		public void setZoomRes(int zoomRes)
		{
			this.zoomRes = zoomRes;
		}

		public void setRemoveRes(int removeRes)
		{
			this.removeRes = removeRes;
		}
	}


	public class OnStickerActionListener : IStickerActionListener
	{
		private StickerView _stickerView;
		private StickerLayout _stickerLayout;
		private List<StickerView> _stickerViews;
		public OnStickerActionListener(StickerView sv, StickerLayout sl, List<StickerView> stVs)
		{
			_stickerView = sv;
			_stickerLayout = sl;
			_stickerViews = stVs;
		}
		public void onDelete(StickerView stickerView)
		{
			
			_stickerLayout.RemoveView(stickerView);
			//_stickerViews.Remove(_stickerView);
			_stickerLayout.redraw();
		}

		public void onEdit(StickerView stickerView)
		{			
			stickerView.setEdit(true);
			stickerView.BringToFront();			

			foreach (var item in _stickerViews)
			{
				if (stickerView != item)
				{
					item.setEdit(false);
				}
			}			
		}
	}

}