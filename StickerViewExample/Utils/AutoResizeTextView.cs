//using System;
//using Android.Content;
//using Android.Graphics;
//using Android.Text;
//using Android.Util;
//using Android.Widget;

//namespace StickerViewExample
//{
//	public class AutoResizeTextView:TextView
//	{

//		private interface SizeTester
//		{
//			/**
//			 *
//			 * @param suggestedSize
//			 *            Size of text to be tested
//			 * @param availableSpace
//			 *            available space in which text must fit
//			 * @return an integer < 0 if after applying {@code suggestedSize} to
//			 *         text, it takes less space than {@code availableSpace}, > 0
//			 *         otherwise
//			 */
//			int OnTestSize(int suggestedSize, RectF availableSpace);
//		}


//		private RectF mTextRect = new RectF();

//		private RectF mAvailableSpaceRect;

//		private SparseIntArray mTextCachedSizes;

//		private TextPaint mPaint;

//		private float mMaxTextSize;

//		private float mSpacingMult = 1.0f;

//		private float mSpacingAdd = 0.0f;

//		private float mMinTextSize = 20;

//		private int mWidthLimit;

//		private static int NO_LINE_LIMIT = -1;
//		private int mMaxLines;

//		private bool mEnableSizeCache = true;
//		private bool mInitiallized;


//		private SizeTester mSizeTester;


//		public AutoResizeTextView(Context context) : base(context)
//		{
//			Initialize();
//		}


//		public AutoResizeTextView(Context context, IAttributeSet attrs): base (context, attrs)
//		{
//			Initialize();
//		}

//		public AutoResizeTextView(Context context, IAttributeSet attrs, int defStyle) : base (context, attrs, defStyle)
//		{
//			Initialize();
//		}


//		void Initialize()
//		{
//			mPaint = new TextPaint(Paint);
//			mMaxTextSize = TextSize;
//			mAvailableSpaceRect = new RectF();
//			mTextCachedSizes = new SparseIntArray();
//			if (mMaxLines == 0)
//			{
//				// no value was assigned during construction
//				mMaxLines = NO_LINE_LIMIT;
//			}
//			mInitiallized = true;
//		}

//		public override void SetTextSize(ComplexUnitType unit, float size)
//		{
//			base.SetTextSize(unit, size);

//			mMaxTextSize = size;
//			mTextCachedSizes.Clear();
//			adjustTextSize(Text);
//		}

//		private void reAdjust()
//		{
//			adjustTextSize(Text);
//		}

//		private void adjustTextSize(string str)
//		{
//			if (!mInitiallized)
//			{
//				return;
//			}
//			int startSize = (int)mMinTextSize;
//			int heightLimit = MeasuredHeight- CompoundPaddingBottom- CompoundPaddingTop;
//			mWidthLimit =  MeasuredWidth -CompoundPaddingLeft - CompoundPaddingRight;
//			mAvailableSpaceRect.Right = mWidthLimit;
//			mAvailableSpaceRect.Bottom = heightLimit;

//			base.SetTextSize( TypedValue.ComplexToDimensionPixelSize,
//					efficientTextSizeSearch(startSize, (int)mMaxTextSize,
//							mSizeTester, mAvailableSpaceRect));
//		}

//		private int efficientTextSizeSearch(int start, int end,
//									   SizeTester sizeTester, RectF availableSpace)
//		{
//			if (!mEnableSizeCache)
//			{
//				return binarySearch(start, end, sizeTester, availableSpace);
//			}
//			string text = Text;
//			int key = text == null ? 0 : text.Length;
//			int size = mTextCachedSizes.Get(key);
//			if (size != 0)
//			{
//				return size;
//			}
//			size = binarySearch(start, end, sizeTester, availableSpace);
//			mTextCachedSizes.Put(key, size);
//			return size;
//		}

//		private static int binarySearch(int start, int end, SizeTester sizeTester,
//									RectF availableSpace)
//		{
//			int lastBest = start;
//			int lo = start;
//			int hi = end - 1;
//			int mid = 0;
//			while (lo <= hi)
//			{
//				mid = (lo + hi) >> 1;
//				int midValCmp = sizeTester.OnTestSize(mid, availableSpace);
//				if (midValCmp < 0)
//				{
//					lastBest = lo;
//					lo = mid + 1;
//				}
//				else if (midValCmp > 0)
//				{
//					hi = mid - 1;
//					lastBest = hi;
//				}
//				else
//				{
//					return mid;
//				}
//			}
//			// make sure to return last best
//			// this is what should always be returned
//			return lastBest;
//		}


		 

//}
//}
