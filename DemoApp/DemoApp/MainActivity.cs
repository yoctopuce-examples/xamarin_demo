using Android.App;
using Android.OS;
using Android.Support.V7.App;
using Android.Runtime;
using Android.Widget;
using Com.Yoctopuce.YoctoAPI;

namespace DemoApp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.activity_main);
            var button = FindViewById<Button>(Resource.Id.ButtonRefresh);
            button.Click += (sender, e) => { this.RefreshInventory(); };

        }

        private void RefreshInventory()
        {

            LinearLayout layout =FindViewById<LinearLayout>(Resource.Id.InventoryList);
            layout.RemoveAllViews();

            YAPI.UpdateDeviceList();
            YModule module = YModule.FirstModule();
            while (module != null)
            {
                string line = module.SerialNumber + " (" + module.ProductName + ")";
                TextView tx = new TextView(this);
                tx.Text = line;
                layout.AddView(tx);
                module = module.NextModule();
            }

        }

        protected override void OnStart()
        {
            base.OnStart();
            YAPI.EnableUSBHost(this);
            YAPI.RegisterHub("usb");
        }

        protected override void OnStop()
        {
            YAPI.FreeAPI();
            base.OnStop();
        }

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}