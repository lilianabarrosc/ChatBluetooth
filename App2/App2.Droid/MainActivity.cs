using System;

using Android.App;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;

namespace App2.Droid
{
	[Activity (Label = "App2.Droid", MainLauncher = true, Icon = "@drawable/icon")]
	public class MainActivity : Activity
	{
		int count = 1;
        private string data = null;

        protected override void OnCreate (Bundle bundle)
		{
			base.OnCreate (bundle);

			// Set our view from the "main" layout resource
			SetContentView (Resource.Layout.Main);

			// Get our button from the layout resource,
			// and attach an event to it
			Button button = FindViewById<Button> (Resource.Id.myButton);
			
			button.Click += delegate {
                button.Text = data;//string.Format ("{0} clicks!", count++);
			};

            //bluetooth configuracion
            bluetoothManager btmanager = new bluetoothManager();
            btmanager.getDevices();

            System.Threading.Thread thread = new System.Threading.Thread(() =>
            {
                while (true)
                {

                    data =btmanager.getMessaje();
                }
            });
            thread.IsBackground = true;
            thread.Start();

            //tab host
            this.ActionBar.NavigationMode = ActionBarNavigationMode.Tabs;

            var frag1 = new SampleTabFragment();
            frag1.texto(data);

            var frag2 = new SampleTabFragment2();
            frag1.texto("texto2");

            AddTab("Enviados", Resource.Drawable.Icon, frag1);
            AddTab("Recibidos", Resource.Drawable.Icon, frag2);

            if (bundle != null)
                this.ActionBar.SelectTab(this.ActionBar.GetTabAt(bundle.GetInt("tab")));

        }

        protected override void OnSaveInstanceState(Bundle outState)
        {
            outState.PutInt("tab", this.ActionBar.SelectedNavigationIndex);

            base.OnSaveInstanceState(outState);
        }

        void AddTab(string tabText, int iconResourceId, Fragment view)
        {
            var tab = this.ActionBar.NewTab();
            tab.SetText(tabText);
            tab.SetIcon(Resource.Drawable.Icon);

            // must set event handler before adding tab
            tab.TabSelected += delegate (object sender, ActionBar.TabEventArgs e)
            {
                var fragment = this.FragmentManager.FindFragmentById(Resource.Id.fragmentContainer);
                if (fragment != null)
                    e.FragmentTransaction.Remove(fragment);
                e.FragmentTransaction.Add(Resource.Id.fragmentContainer, view);
            };
            tab.TabUnselected += delegate (object sender, ActionBar.TabEventArgs e) {
                e.FragmentTransaction.Remove(view);
            };
            this.ActionBar.AddTab(tab);
        }

        class SampleTabFragment : Fragment
        {
            TextView sampleTextView;
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                base.OnCreateView(inflater, container, savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.Tab, container, false);
                sampleTextView = view.FindViewById<TextView>(Resource.Id.sampleTextView);
                return view;
            }

            public void texto(string text)
            {
                sampleTextView.Text = text;
            }
        }

        class SampleTabFragment2 : Fragment
        {
            TextView sampleTextView;
            public override View OnCreateView(LayoutInflater inflater, ViewGroup container, Bundle savedInstanceState)
            {
                base.OnCreateView(inflater, container, savedInstanceState);

                var view = inflater.Inflate(Resource.Layout.Tab, container, false);
                sampleTextView = view.FindViewById<TextView>(Resource.Id.sampleTextView);

                return view;
            }

            public void texto(string text)
            {
                sampleTextView.Text = text;
            }
        }
    }
}


