using System;
using MonoTouch.UIKit;
using XamarinEvolveSSLibrary.GoogleAPI;
using System.Collections.Generic;
using XamarinEvolveSSLibrary;
using System.Drawing;
using MonoTouch.CoreGraphics;

namespace XamarinEvolveIOS
{
	public class CheckInViewController : UIViewController
	{
		List<Place> _placeList = new List<Place> ();
		GeolocationHelper _geolocationHelper = new GeolocationHelper ();

		UITableView TableView {get;set;}
		BusyView _busyView;

		public CheckInViewController ()
		{
			this.Title = "Tap item to check-in";
		}

		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();

			TableView = new UITableView (this.View.Bounds, UITableViewStyle.Grouped);
			TableView.AutoresizingMask = UIViewAutoresizing.All;
			this.View.Add (TableView);

			_busyView = new BusyView (View.Bounds);
			this.View.Add (_busyView);

			TableView.DataSource = new CheckInTableViewDataSource (this);
			TableView.Delegate = new CheckInTableViewDelegate (this);

			_geolocationHelper.GetLocation ((result) => {
				LoadPlaceList (result);
			});


		}

		private void LoadPlaceList (GeolocationResult result)
		{
			Func<int> func = delegate {
				List<Place> lPlaceList = new List<Place> ();

				if (result.Position != null)
				{
					lPlaceList = Engine.Instance.PlaceLocator.GetNearbyPlaces (
						(float)result.Position.Latitude, (float)result.Position.Longitude);
				}

				this.BeginInvokeOnMainThread (delegate {
					if (lPlaceList.Count > 0)
					{
						_placeList = lPlaceList;
						((CheckInTableViewDataSource)(TableView.DataSource)).PlaceList = _placeList;
						TableView.ReloadData ();
						_busyView.Busy = false;
					}
					else
						ShowLocatinError (lPlaceList, result);
				});

				return 0;
			};

			func.BeginInvoke (null, null);
		}

		private void ShowLocatinError (List<Place> placeList, GeolocationResult result)
		{
			string message;

			if (result.Canceled)
				message = "The location request timed out.";
			else if (result.GeolocationNotAvailable)
				message = "Location services are not available on this device.";
			else if (result.GeolocationDisabled)
				message = "Location services are not enabled for the Evolve application.";
			else 
				message = "Did not find any places near your location.";

			_busyView.Busy = false;

			UIAlertView alertNew = new UIAlertView ("Error", message, null, "OK", null);
			alertNew.Show ();
		}

		private class CheckInTableViewDataSource : UITableViewDataSource
		{
			public List<Place> PlaceList {get;set;}

			public CheckInTableViewDataSource (CheckInViewController viewController)
			{
				PlaceList = new List<Place> ();
			}

			#region implemented abstract members of UITableViewDataSource
			public override int RowsInSection (UITableView tableView, int section)
			{
				return PlaceList.Count;
			}			

			public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell ("PlaceTableItem");

				if (cell == null)
				{
					cell = new UITableViewCell (UITableViewCellStyle.Subtitle, "PlaceTableItem");
				}

				Place place = PlaceList[indexPath.Row];
				cell.TextLabel.Text = place.name;
				cell.DetailTextLabel.Text = place.vicinity;

				return cell;
			}		
			#endregion
		}

		private class CheckInTableViewDelegate : UITableViewDelegate
		{
			public List<Place> PlaceList {get;set;}
			
			public CheckInTableViewDelegate (CheckInViewController viewController)
			{
				PlaceList = new List<Place> ();
			}
		}
	}
}
