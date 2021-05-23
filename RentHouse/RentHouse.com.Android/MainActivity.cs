	
using Android;
using Android.App;
using Android.Content.PM;
using Android.OS;
using Android.Runtime;
using Plugin.CurrentActivity;
using Xamarin.Forms.GoogleMaps.Android;

namespace RentHouse.com.Droid
{
	[Activity(Label = "RentHouse.com", Icon = "@mipmap/icon", Theme = "@style/MainTheme", MainLauncher = true, ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation | ConfigChanges.UiMode | ConfigChanges.ScreenLayout | ConfigChanges.SmallestScreenSize)]
	public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
	{
		protected override void OnCreate(Bundle savedInstanceState)
		{
			TabLayoutResource = Resource.Layout.Tabbar;
			ToolbarResource = Resource.Layout.Toolbar;
			SetStatusBarColor((Android.Graphics.Color.ParseColor("#ee3861")));

			var platformConfig = new PlatformConfig
			{
				BitmapDescriptorFactory = new BitmapConfig()
			};
			Xamarin.FormsGoogleMaps.Init(this, savedInstanceState, platformConfig);

			base.OnCreate(savedInstanceState);
			CrossCurrentActivity.Current.Init(this, savedInstanceState);


			Xamarin.Essentials.Platform.Init(this, savedInstanceState);
			global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
			Xamarin.FormsGoogleMaps.Init(this, savedInstanceState);
			FFImageLoading.Forms.Platform.CachedImageRenderer.Init(enableFastRenderer: true);
			LoadApplication(new App());
		}

		public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
		{

			if (requestCode == RequestLocationId)
			{
				if ((grantResults.Length == 1) && (grantResults[0] == (int)Permission.Granted))
				{

				}
			}
			else
			{
				base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
			}

			Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

		}

		const int RequestLocationId = 0;

		readonly string[] LocationPermissions =
		{
		Manifest.Permission.AccessCoarseLocation,
		Manifest.Permission.AccessFineLocation
		};

		protected override void OnStart()
		{
			base.OnStart();

			if ((int)Build.VERSION.SdkInt >= 23)
			{
				if (CheckSelfPermission(Manifest.Permission.AccessFineLocation) != Permission.Granted)
				{
					RequestPermissions(LocationPermissions, RequestLocationId);
				}
				else
				{
					// Permissions already granted - display a message.
				}
			}
		}

	}
}