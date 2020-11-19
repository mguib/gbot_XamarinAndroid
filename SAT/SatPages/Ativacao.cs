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
    [Activity(Label = "Ativacao")]
    public class Ativacao : Activity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            SetContentView(Resource.Layout.ativacao);
            // Create your application here
        }
    }
}