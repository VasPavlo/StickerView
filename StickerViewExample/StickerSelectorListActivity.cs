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
using static Android.Resource;

namespace StickerViewExample
{
	[Activity(Label = "Activity2")]
	public class StickerSelectorListActivity : Activity, AdapterView.IOnItemClickListener
	{
		private GridView gv;
		private List<int> data;
		protected override void OnCreate(Bundle savedInstanceState)
		{
			base.OnCreate(savedInstanceState);

			SetContentView(Resource.Layout.activity_sticker_selector);
			gv = (GridView)FindViewById(Resource.Id.gv);
			gv.OnItemClickListener = this;
			data = new List<int>();
			data.Add(Resource.Drawable.ic_avatar_1);
			data.Add(Resource.Drawable.ic_avatar_2);
			data.Add(Resource.Drawable.ic_avatar_3);

			StickerAdapter adapter = new StickerAdapter(this, data);
			gv.SetAdapter(adapter);
		}
		public void OnItemClick(AdapterView parent, View view, int position, long id)
		{
			int resource = (int)parent.GetItemAtPosition(position);
			if (resource > 0)
			{
				Intent intent = Intent;
				intent.PutExtra("res", resource);
				SetResult(Result.Ok, intent);
				Finish();
			}
		}
	
	private class StickerAdapter : BaseAdapter
	{
		private Context context;
		private List<int> data;

	public StickerAdapter(Context context, List<int> data)
	{
		this.context = context;
		this.data = data;
	}

			public override int Count
			{
				get { return data.Count; }
			}

			public override Java.Lang.Object GetItem(int position)
			{
				return data[position];
			}

			public override long GetItemId(int position)
			{
				return position;
			}

			public override View GetView(int position, View convertView, ViewGroup parent)
			{
				if (convertView == null)
				{
					convertView = LayoutInflater.From(context).Inflate(Resource.Layout.item_sticker, null);
				}
				ImageView ivSticker = (ImageView)convertView.FindViewById(Resource.Id.iv_sticker);
				ivSticker.SetImageResource(data[position]);
				return convertView;
			}			
		}
	}
}