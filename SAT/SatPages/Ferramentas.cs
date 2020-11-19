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

namespace Gbot_XamarinAndroid.SAT.SatPages
{
    [Activity(Label = "Ferramentas")]
    public class Ferramentas : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ferramentas);
            // Create your application here
        }
    }
}