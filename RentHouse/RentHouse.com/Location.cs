using System;
using Xamarin.Essentials;


namespace RentHouse.com
{
	class Location
	{

		public async System.Threading.Tasks.Task<Xamarin.Essentials.Location> GetLocation()
		{
			try
			{
				var location = await Geolocation.GetLastKnownLocationAsync();

				if (location != null)
				{
					Console.WriteLine($"Latitude: {location.Latitude}, Longitude: {location.Longitude}, Altitude: {location.Altitude}");
				}

				return location;

			}
			catch (FeatureNotSupportedException fnsEx)
			{
				Console.WriteLine(fnsEx.Message);
				// Handle not supported on device exception
			}
			catch (FeatureNotEnabledException fneEx)
			{
				Console.WriteLine(fneEx.Message);
				// Handle not enabled on device exception
			}
			catch (PermissionException pEx)
			{
				Console.WriteLine(pEx.Message);
				// Handle permission exception
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
				// Unable to get location
			}

			return null;

		}

	}
}
