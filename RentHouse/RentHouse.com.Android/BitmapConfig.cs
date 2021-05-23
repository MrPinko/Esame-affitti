using Xamarin.Forms.GoogleMaps;
using Xamarin.Forms.GoogleMaps.Android.Factories;
using AndroidBitmapDescriptor = Android.Gms.Maps.Model.BitmapDescriptor;
using AndroidBitmapDescriptorFactory = Android.Gms.Maps.Model.BitmapDescriptorFactory;

namespace RentHouse.com.Droid
{
    public sealed class BitmapConfig : IBitmapDescriptorFactory
    {
        public AndroidBitmapDescriptor ToNative(BitmapDescriptor descriptor)
        {
            int iconId = 0;
            switch (descriptor.Id)
            {
                case "tourist":
                    iconId = Resource.Drawable.touristPin;
                    break;
                case "home":
                    iconId = Resource.Drawable.homePin;
                    break;
            }
            return AndroidBitmapDescriptorFactory.FromResource(iconId);
        }
    }
}