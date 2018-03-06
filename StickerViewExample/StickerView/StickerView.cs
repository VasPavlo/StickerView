using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.View;
using Android.Util;
using Android.Views;
using Android.Widget;
using StickerViewExample.Utils;

namespace StickerViewExample.StickerView
{
	public class StickerView : ImageView
	{
		private Context context;		
		private Sticker sticker;		
		private Matrix downMatrix = new Matrix();		
		private Matrix moveMatrix = new Matrix();		
		private PointF midPoint = new PointF();		
		private PointF imageMidPoint = new PointF();		
		private StickerActionIcon rotateIcon;		
		private StickerActionIcon zoomIcon;		
		private StickerActionIcon removeIcon;		
		private Paint paintEdge;
		
		private int mode;		
		private bool isEdit = true;		
		private OnStickerActionListener listener;


		public void setOnStickerActionListener(OnStickerActionListener listener)
		{
			this.listener = listener;
		}

		public StickerView(Context context): this(context, null)
		{		 
		}

		public StickerView(Context context, IAttributeSet attrs) : this(context, attrs, 0)
		{			
		}

		public StickerView(Context context, IAttributeSet attrs, int defStyleAttr ) : base (context, attrs, defStyleAttr)
		{			
			Init(context);
		}

		private void Init(Context context)
		{
			this.context = context;
			SetScaleType(ScaleType.Matrix);
			rotateIcon = new StickerActionIcon(context);
			zoomIcon = new StickerActionIcon(context);
			removeIcon = new StickerActionIcon(context);
			paintEdge = new Paint();
			paintEdge.Color =Color.Black;
			paintEdge.Alpha =170;
			paintEdge.AntiAlias = true;
		}


		protected override void OnLayout(bool changed, int left, int top, int right, int bottom)
		{
			base.OnLayout(changed, left, top, right, bottom);

			if (changed)
			{
				sticker.getMatrix().PostTranslate((Width - sticker.getStickerWidth()) / 2, (Height - sticker.getStickerHeight()) / 2);
			}
		}

		private bool isInStickerArea(Sticker sticker, MotionEvent e) 
		{
			RectF dst = sticker.getSrcImageBound();
			return dst.Contains(e.GetX(), e.GetY());
		}

	protected override void OnDraw(Canvas canvas)
		{
			if (sticker == null) return;
			sticker.draw(canvas);
			float[] points = PointUtils.GetBitmapPoints(sticker.getSrcImage(), sticker.getMatrix());
			float x1 = points[0];
			float y1 = points[1];
			float x2 = points[2];
			float y2 = points[3];
			float x3 = points[4];
			float y3 = points[5];
			float x4 = points[6];
			float y4 = points[7];
			if (isEdit)
			{
				canvas.DrawLine(x1, y1, x2, y2, paintEdge);
				canvas.DrawLine(x2, y2, x4, y4, paintEdge);
				canvas.DrawLine(x4, y4, x3, y3, paintEdge);
				canvas.DrawLine(x3, y3, x1, y1, paintEdge);
				
				rotateIcon.draw(canvas, x2, y2);
				zoomIcon.draw(canvas, x3, y3);
				removeIcon.draw(canvas, x1, y1);
			}
		}
		
		private float downX;
		private float downY;
		private float oldDistance;
		private float oldRotation;

		public override bool OnTouchEvent(MotionEvent e)
		{
			//return base.OnTouchEvent(e);
			int action = MotionEventCompat.GetActionMasked(e);
			bool isStickerOnEdit = true;
        switch (action) {
            case (int)MotionEventActions.Down:
                downX = e.GetX();
                downY = e.GetY();
                if (sticker == null) return false;                
                if (removeIcon.isInActionCheck(e))
					{
						if (listener != null)
						{
							listener.onDelete(this);
						}
					}                
					else if (rotateIcon.isInActionCheck(e))
					{
						mode = (int)ActionMode.ROTATE;
						downMatrix.Set(sticker.getMatrix());
						imageMidPoint = sticker.getImageMidPoint(downMatrix);
						oldRotation = sticker.getSpaceRotation(e, imageMidPoint);
						Log.Debug("onTouchEvent", "Rotate gesture");
					}                
                else if (zoomIcon.isInActionCheck(e))
					{
						mode = (int)ActionMode.ZOOM_SINGLE;
						downMatrix.Set(sticker.getMatrix());
						imageMidPoint = sticker.getImageMidPoint(downMatrix);
						oldDistance = sticker.getSingleTouchDistance(e, imageMidPoint);
						Log.Debug("onTouchEvent", "Single point zoom gesture");
					}
                
                else if (isInStickerArea(sticker, e))
					{
						mode = (int)ActionMode.TRANS;
						downMatrix.Set(sticker.getMatrix());
						Log.Debug("onTouchEvent", "Pan gesture");
					}
				else
					{
						isStickerOnEdit = false;
					}
					break;
            case (int)MotionEventActions.PointerDown: 
                mode = (int)ActionMode.ZOOM_MULTI;
				oldDistance = sticker.getMultiTouchDistance(e);
				midPoint = sticker.getMidPoint(e);
				downMatrix.Set(sticker.getMatrix());
				break;
            case (int) MotionEventActions.Move:                
                if (mode == (int)ActionMode.ROTATE)
					{
						moveMatrix.Set(downMatrix);
						float deltaRotation = sticker.getSpaceRotation(e, imageMidPoint) - oldRotation;
						moveMatrix.PostRotate(deltaRotation, imageMidPoint.X, imageMidPoint.Y);
						sticker.getMatrix().Set(moveMatrix);
						Invalidate();					
					}	
			else if (mode == (int)ActionMode.ZOOM_SINGLE)
					{
						moveMatrix.Set(downMatrix);
						float scale = sticker.getSingleTouchDistance(e, imageMidPoint) / oldDistance;
						moveMatrix.PostScale(scale, scale, imageMidPoint.X, imageMidPoint.Y);
						sticker.getMatrix().Set(moveMatrix);
						Invalidate();
					}
			else if (mode == (int)ActionMode.ZOOM_MULTI)
					{
						moveMatrix.Set(downMatrix);
						float scale = sticker.getMultiTouchDistance(e) / oldDistance;
						moveMatrix.PostScale(scale, scale, midPoint.X, midPoint.Y);
						sticker.getMatrix().Set(moveMatrix);
						Invalidate();
					}
			else if (mode == (int) ActionMode.TRANS)
					{
						moveMatrix.Set(downMatrix);
						moveMatrix.PostTranslate(e.GetX() - downX, e.GetY() - downY);
						sticker.getMatrix().Set(moveMatrix);
						Invalidate();
					}
					break;
            case (int) MotionEventActions.PointerUp:
            case (int) MotionEventActions.Up:
                mode = (int) ActionMode.NONE;
				midPoint = null;
				imageMidPoint = null;
				break;
			default:
                break;
			}
			if (isStickerOnEdit && listener != null)
			{
				listener.onEdit(this);
			}
				return isStickerOnEdit;
		}

		public override void SetImageResource(int resId)
		{			
			sticker = new Sticker(BitmapFactory.DecodeResource(context.Resources, resId));
		}

		public Sticker getSticker()
		{
			return sticker;
		}

		public override void SetImageBitmap(Bitmap bm)
		{			
			sticker = new Sticker(bm);
		}

		public void setEdit(bool edit)
		{
			isEdit = edit;
			PostInvalidate();
		}
		public void setRotateRes(int rotateRes)
		{
			rotateIcon.setSrcIcon(rotateRes);
		}

		public void setZoomRes(int zoomRes)
		{
			zoomIcon.setSrcIcon(zoomRes);
		}

		public void setRemoveRes(int removeRes)
		{
			removeIcon.setSrcIcon(removeRes);
		}

	}
}