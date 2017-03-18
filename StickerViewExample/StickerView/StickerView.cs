using System;
using Android.Content;
using Android.Content.Res;
using Android.Graphics;
using Android.Util;
using Android.Views;
using Android.Widget;

namespace StickerViewExample
{
	public abstract class StickerView : FrameLayout, View.IOnTouchListener
	{

		public static String TAG = "com.knef.stickerView";
		private BorderView iv_border;
		private ImageView iv_scale;
		private ImageView iv_delete;
		private ImageView iv_flip;

		// For scalling
		private float this_orgX = -1, this_orgY = -1;
		private float scale_orgX = -1, scale_orgY = -1;
		private double scale_orgWidth = -1, scale_orgHeight = -1;
		// For rotating
		private float rotate_orgX = -1, rotate_orgY = -1, rotate_newX = -1, rotate_newY = -1;
		// For moving
		private float move_orgX = -1, move_orgY = -1;

		private double centerX, centerY;

		private static int BUTTON_SIZE_DP = 30;
		private static int SELF_SIZE_DP = 100;


		public StickerView(Context context) : base(context)
		{
			Init(context);
		}

		public StickerView(Context context, IAttributeSet attrs) : base(context, attrs)
		{

			Init(context);
		}

		public StickerView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{
			Init(context);
		}

		void Init(Context context)
		{
			iv_border = new BorderView(context);
			iv_scale = new ImageView(context);
			iv_delete = new ImageView(context);
			iv_flip = new ImageView(context);

			iv_scale.SetImageResource(Resource.Drawable.zoominout);
			iv_delete.SetImageResource(Resource.Drawable.remove);
			iv_flip.SetImageResource(Resource.Drawable.flip);

			Tag = "DraggableViewGroup";
			iv_border.Tag = "iv_border";
			iv_scale.Tag = "iv_scale";
			iv_delete.Tag = "iv_delete";
			iv_flip.Tag = "iv_flip";

			int margin = ConvertDpToPixel(BUTTON_SIZE_DP, Context) / 2;
			int size = ConvertDpToPixel(SELF_SIZE_DP, Context);

			LayoutParams this_params =
			   new LayoutParams(
					   size,
					   size
			   );
			this_params.Gravity = GravityFlags.Center;

			LayoutParams iv_main_params =
			   new LayoutParams(
					   ViewGroup.LayoutParams.MatchParent,
					   ViewGroup.LayoutParams.MatchParent
			   );
			iv_main_params.SetMargins(margin, margin, margin, margin);



			LayoutParams iv_border_params =
			   new LayoutParams(
					   ViewGroup.LayoutParams.MatchParent,
					   ViewGroup.LayoutParams.MatchParent
			   );
			iv_border_params.SetMargins(margin, margin, margin, margin);

			LayoutParams iv_scale_params =
					new LayoutParams(
							ConvertDpToPixel(BUTTON_SIZE_DP, Context),
							ConvertDpToPixel(BUTTON_SIZE_DP, Context)
					);


			iv_scale_params.Gravity = GravityFlags.Bottom | GravityFlags.Right;

			LayoutParams iv_delete_params =
					new LayoutParams(
							ConvertDpToPixel(BUTTON_SIZE_DP, Context),
							ConvertDpToPixel(BUTTON_SIZE_DP, Context)
					);
			iv_delete_params.Gravity = GravityFlags.Top | GravityFlags.Right;

			LayoutParams iv_flip_params =
					new LayoutParams(
							ConvertDpToPixel(BUTTON_SIZE_DP, Context),
							ConvertDpToPixel(BUTTON_SIZE_DP, Context)
					);
			iv_flip_params.Gravity = GravityFlags.Top | GravityFlags.Left;


			this.LayoutParameters = this_params;
			this.AddView(GetMainView(), iv_main_params);
			this.AddView(iv_border, iv_border_params);
			this.AddView(iv_scale, iv_scale_params);
			this.AddView(iv_delete, iv_delete_params);
			this.AddView(iv_flip, iv_flip_params);
			this.SetOnTouchListener(this);
			this.iv_scale.SetOnTouchListener(this);
			this.iv_delete.Click+=(svm,e) =>
			{
				if (Parent != null)
				{
					ViewGroup myCanvas = ((ViewGroup)Parent);
					myCanvas.RemoveView(this);
				}
			};

			this.iv_flip.Click += (dwd, wd) => 
			{
				Log.Verbose(TAG, "flip the view");

				View mainView = GetMainView();
				mainView.RotationY = (mainView.RotationY == -180f ? 0f : -180f);
				mainView.Invalidate();
				RequestLayout();
			};
		}

		public bool isFlip()
		{
			return GetMainView().RotationY == -180f;
		}


		protected abstract View GetMainView();

		private static int ConvertDpToPixel(float dp, Context context)
		{
			Resources resources = context.Resources;
			DisplayMetrics metrics = resources.DisplayMetrics;
			float px = dp * ((int)metrics.DensityDpi / 160f);
			return (int)px;
		}

		private double GetLength(double x1, double y1, double x2, double y2)
		{
			return Math.Sqrt(Math.Pow(y2 - y1, 2) + Math.Pow(x2 - x1, 2));
		}


		public bool OnTouch(View view, MotionEvent e)
		{
			if (view.Tag.Equals("DraggableViewGroup"))
			{
				switch (e.Action)
				{
					case MotionEventActions.Down:
						Log.Verbose(TAG, "sticker view action down");
						move_orgX = e.RawX;
						move_orgY = e.RawY;
						break;
					case MotionEventActions.Move:
						Log.Verbose(TAG, "sticker view action move");

						float offsetX = e.RawX - move_orgX;

						float offsetY = e.RawY - move_orgY;
						this.SetX(this.GetX() + offsetX);
						this.SetY(this.GetY() + offsetY);
						move_orgX = e.RawX;
						move_orgY = e.RawY;
						break;
					case MotionEventActions.Up:
						Log.Verbose(TAG, "sticker view action up");
						break;
				}
			}
			else
				if (view.Tag.Equals("iv_scale"))
			{
				switch (e.Action)
				{
					case MotionEventActions.Down:
						Log.Verbose(TAG, "iv_scale action down");

						this_orgX = GetX();
						this_orgY = GetY();

						scale_orgX = e.RawX;
						scale_orgY = e.RawY;
						scale_orgWidth = this.LayoutParameters.Width;
						scale_orgHeight = this.LayoutParameters.Height;

						rotate_orgX = e.RawX;
						rotate_orgY = e.RawY;

						centerX = this.GetX() +
									  ((View)this.Parent).GetX() +
								(float)this.Width / 2;


						//double statusBarHeight = Math.ceil(25 * getContext().getResources().getDisplayMetrics().density);
						int result = 0;
						int resourceId = Resources.GetIdentifier("status_bar_height", "dimen", "android");
						if (resourceId > 0)
						{
							result = Resources.GetDimensionPixelSize(resourceId);
						}
						double statusBarHeight = result;
						centerY = this.GetY() +
							((View)Parent).GetY() +
							statusBarHeight +
									  (float)this.Height / 2;

						break;
					case MotionEventActions.Move:
						Log.Verbose(TAG, "iv_scale action move");

						rotate_newX = e.RawX;
						rotate_newY = e.RawY;

						double angle_diff = Math.Abs(
							Math.Atan2(e.RawY - scale_orgY, e.RawX - scale_orgX)
								- Math.Atan2(scale_orgY - centerY, scale_orgX - centerX)) * 180 / Math.PI;

						Log.Verbose(TAG, "angle_diff: " + angle_diff);

						double length1 = GetLength(centerX, centerY, scale_orgX, scale_orgY);
						double length2 = GetLength(centerX, centerY, e.RawX, e.RawY);

						int size = ConvertDpToPixel(SELF_SIZE_DP, Context);
						if (length2 > length1
							&& (angle_diff < 25 || Math.Abs(angle_diff - 180) < 25)
							)
						{
							//scale up
							double offsetX = Math.Abs(e.RawX - scale_orgX);
							double offsetY = Math.Abs(e.RawY - scale_orgY);
							double offset = Math.Max(offsetX, offsetY);
							offset = Math.Round(offset);
							this.LayoutParameters.Width += (int)offset;
							this.LayoutParameters.Height += (int)offset;
							OnScaling(true);
							//DraggableViewGroup.this.setX((float) (getX() - offset / 2));
							//DraggableViewGroup.this.setY((float) (getY() - offset / 2));
						}
						else if (length2 < length1
									   && (angle_diff < 25 || Math.Abs(angle_diff - 180) < 25)
								 && this.LayoutParameters.Width > size / 2
								 && this.LayoutParameters.Height > size / 2)
						{
							//scale down
							double offsetX = Math.Abs(e.RawX - scale_orgX);
							double offsetY = Math.Abs(e.RawY - scale_orgY);
							double offset = Math.Max(offsetX, offsetY);
							offset = Math.Round(offset);
							this.LayoutParameters.Width -= (int)offset;
							this.LayoutParameters.Height -= (int)offset;
							OnScaling(false);
						}

						//rotate

						double angle = Math.Atan2(e.RawY - centerY, e.RawX - centerX) * 180 / Math.PI;
						Log.Verbose(TAG, "log angle: " + angle);

						//setRotation((float) angle - 45);
						Rotation = ((float)angle - 45);
						Log.Verbose(TAG, "getRotation(): " + Rotation);

						OnRotating();

						rotate_orgX = rotate_newX;
						rotate_orgY = rotate_newY;

						scale_orgX = e.RawX;
						scale_orgY = e.RawY;

						PostInvalidate();
						RequestLayout();

						break;
					case MotionEventActions.Up:
						Log.Verbose(TAG, "iv_scale action up");
						break;
				}
			}
			return true;
		}

		protected override void OnDraw(Canvas canvas)
		{
			base.OnDraw(canvas);
		}

		private float[] GetRelativePos(float absX, float absY)
		{
			Log.Verbose("ken", "getRelativePos getX:" + ((View)Parent).GetX());
			Log.Verbose("ken", "getRelativePos getY:" + ((View)Parent).GetY());
			float[] pos = new float[]{
				absX-((View)Parent).GetX(),
				absY-((View)Parent).GetY()
		};
			Log.Verbose(TAG, "getRelativePos absY:" + absY);
			Log.Verbose(TAG, "getRelativePos relativeY:" + pos[1]);
			return pos;
		}

		public void SetControlItemsHidden(bool isHidden)
		{
			if (isHidden)
			{
				iv_border.Visibility = ViewStates.Invisible;
				iv_scale.Visibility = ViewStates.Invisible;
				iv_delete.Visibility = ViewStates.Invisible;
				iv_flip.Visibility = ViewStates.Invisible;
			}
			else
			{
				iv_border.Visibility = ViewStates.Visible;
				iv_scale.Visibility = ViewStates.Visible;
				iv_delete.Visibility = ViewStates.Visible;
				iv_flip.Visibility = ViewStates.Visible;
			}
		}

		protected View GetImageViewFlip()
		{
			return iv_flip;
		}
		protected void OnScaling(bool scaleUp) { }

		protected void OnRotating() { }
	}




	internal class BorderView : View
	{

		public BorderView(Context context) : base(context)
		{

		}

		public BorderView(Context context, IAttributeSet attrs) : base(context, attrs)
		{

		}

		public BorderView(Context context, IAttributeSet attrs, int defStyle) : base(context, attrs, defStyle)
		{

		}

		protected override void OnDraw(Android.Graphics.Canvas canvas)
		{
			base.OnDraw(canvas);

			var pars = (FrameLayout.LayoutParams)this.LayoutParameters;

			Log.Verbose("com.knef.stickerView", "params.leftMargin: " + pars.LeftMargin);

			Rect border = new Rect();
			border.Left = (int)this.Left - pars.LeftMargin;
			border.Top = (int)this.Top - pars.TopMargin;
			border.Right = (int)this.Right - pars.RightMargin;
			border.Bottom = (int)this.Bottom - pars.BottomMargin;
			Paint borderPaint = new Paint();
			borderPaint.StrokeWidth = 6;
			borderPaint.Color = Color.White;
			borderPaint.SetStyle(Paint.Style.Stroke);
			canvas.DrawRect(border, borderPaint);
		}

	}

}
