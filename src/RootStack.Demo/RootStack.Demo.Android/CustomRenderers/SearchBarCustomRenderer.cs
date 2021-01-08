using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using RootStack.Demo.Droid.CustomRenderers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(SearchBar), typeof(SearchBarCustomRenderer))]
namespace RootStack.Demo.Droid.CustomRenderers
{
    public class SearchBarCustomRenderer : SearchBarRenderer
    {
        public SearchBarCustomRenderer(Context context) : base(context)
        {
        }
        protected override void OnElementChanged(ElementChangedEventArgs<SearchBar> e)
        {
            base.OnElementChanged(e);
            if (Control != null)
            {
                var searchView = Control;
                searchView.Iconified = true;
                searchView.SetIconifiedByDefault(false);
                int searchIconId = Context.Resources.GetIdentifier("android:id/search_mag_icon", null, null);
                var icon = searchView.FindViewById(searchIconId);
                icon.Visibility = Android.Views.ViewStates.Gone;


                ImageView searchViewIcon = (ImageView)searchView.FindViewById<ImageView>(searchIconId);
                searchViewIcon.SetImageDrawable(null);

                var plateId = Resources.GetIdentifier("android:id/search_plate", null, null);
                var plate = Control.FindViewById(plateId);
                plate.SetBackgroundColor(Android.Graphics.Color.Transparent);
            }
        }
    }
}