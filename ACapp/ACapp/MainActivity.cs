using Android.App;
using Android.OS;
using Android.Runtime;
using Android.Widget;
using AndroidX.AppCompat.App;

namespace ACapp
{
    [Activity(Label = "@string/app_name", Theme = "@style/AppTheme", MainLauncher = true)]
    public class MainActivity : AppCompatActivity
    {
        Button btnMinus, btnPlus, btnSettings, btnDone;
        TextView tvTemp, tvMin, tvMax;
        EditText etMin, etMax;
        int minTemp, maxTemp, currentTemp;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);

            minTemp = Xamarin.Essentials.Preferences.Get("min", 16);
            maxTemp = Xamarin.Essentials.Preferences.Get("max", 32);
            currentTemp = Xamarin.Essentials.Preferences.Get("current", 20);

            OpenMain();
        }

        protected void OpenMain() {
            SetContentView(Resource.Layout.activity_main);

            btnMinus = FindViewById<Button>(Resource.Id.btnMinus);
            btnPlus = FindViewById<Button>(Resource.Id.btnPlus);
            btnSettings = FindViewById<Button>(Resource.Id.btnSettings);


            tvTemp = FindViewById<TextView>(Resource.Id.tvTemp);
            tvMin = FindViewById<TextView>(Resource.Id.tvMin);
            tvMax = FindViewById<TextView>(Resource.Id.tvMax);

            tvTemp.Text = IntToCelsius(currentTemp);
            tvMin.Text = IntToCelsius(minTemp);
            tvMax.Text = IntToCelsius(maxTemp);

            btnMinus.Click += (sender, e) => SetTemperature(currentTemp - 1);
            btnPlus.Click += (sender, e) => SetTemperature(currentTemp + 1);

            btnSettings.Click += (sender, e) => OpenSettings();
        }

        protected void OpenSettings() {
            SetContentView(Resource.Layout.settings_main);

            btnDone = FindViewById<Button>(Resource.Id.btnDone);

            etMin = FindViewById<EditText>(Resource.Id.etMin);
            etMax = FindViewById<EditText>(Resource.Id.etMax);

            etMin.Text = minTemp.ToString();
            etMax.Text = maxTemp.ToString();

            btnDone.Click += (sender, e) => {
                int _min = int.Parse(etMin.Text), _max = int.Parse(etMax.Text);
                if (_min >= _max) {
                    Toast.MakeText(this, "Minimum must be smaller than Maximum!", ToastLength.Long).Show();
                    return;
                }
                minTemp = _min;
                maxTemp = _max;

                Xamarin.Essentials.Preferences.Set("min", minTemp);
                Xamarin.Essentials.Preferences.Set("max", maxTemp);

                OpenMain();
            };
        }

        protected void SetTemperature(int temp) {
            if (temp < minTemp || temp > maxTemp) return;
            currentTemp = temp;
            Xamarin.Essentials.Preferences.Set("current", currentTemp);
            tvTemp.Text = IntToCelsius(currentTemp);
        }

        protected string IntToCelsius(int temp) => $"{temp}°C";

        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }
    }
}