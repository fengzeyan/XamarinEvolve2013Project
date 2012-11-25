using System;
using MonoTouch.UIKit;
using XamarinEvolveSSLibrary;

namespace XamarinEvolveIOS
{
	public class MeetUpViewController : PlaceListViewController
	{
		UIBarButtonItem _sortButton;
		
		public MeetUpViewController () : base ()
		{
			this.Title = "Attendee Check-ins";
			SortMethod = PlaceSortMethod.Popularity;
		}
		
		public override void ViewDidLoad ()
		{
			base.ViewDidLoad ();
			
			TableView.DataSource = new MeetUpViewDataDource (this);
			TableView.Delegate = new PlaceListViewDelegate (this);
			
			_sortButton = new UIBarButtonItem ("Sort", UIBarButtonItemStyle.Plain, delegate {
				SortButtonTouched ();
			});
			this.NavigationItem.RightBarButtonItem = _sortButton;
			
			SortMethod = PlaceSortMethod.Popularity;
			
			ChangeSortMethod (PlaceSortMethod.Popularity);
		}
		
		private void SortButtonTouched ()
		{
			UIActionSheet actionSheet = new UIActionSheet ("Select Sort Priority");
			actionSheet.AddButton ("Popular Places");
			actionSheet.AddButton ("Near Convention Center");
			
			actionSheet.AddButton ("Cancel");
			actionSheet.CancelButtonIndex = 2;
			
			actionSheet.Clicked += (sender, e) => {
				switch (e.ButtonIndex)
				{
				case 0:
					ChangeSortMethod (PlaceSortMethod.Popularity);
					break;
				case 1:
					ChangeSortMethod (PlaceSortMethod.NearConventionCenter);
					break;
				}
			};
			
			actionSheet.ShowInView (this.View);
		}
		
		private void ChangeSortMethod (PlaceSortMethod sortMethod)
		{
			NavigationItem.SetRightBarButtonItem (null, true);
			BusyView.Busy = true;
			
			Func<int> func = delegate {
				PlaceList list = Engine.Instance.CheckInAccess.GetPlaceList ();
				
				if (sortMethod == PlaceSortMethod.Popularity)
				list = list.SortByPopularity ();
				
				else if (sortMethod == PlaceSortMethod.NearConventionCenter)
				{
					list = list.SortByDistance (SystemConstants.DefaultPlace.Latitude,
					                            SystemConstants.DefaultPlace.Longitude);
				}
				
				this.BeginInvokeOnMainThread (delegate {
					PlaceList = list;
					TableView.ReloadData ();
					BusyView.Busy = false;
					NavigationItem.SetRightBarButtonItem (_sortButton, true);
				});
				return 0;
			};
			func.BeginInvoke (null, null);
		}
		
		private PlaceSortMethod _sortMethod = PlaceSortMethod.Popularity;
		private PlaceSortMethod SortMethod 
		{
			get
			{
				return _sortMethod;
			}
			
			set
			{
				
				if (TableView == null)
					return;
				
				if (TableView.DataSource != null)
					((MeetUpViewDataDource)(TableView.DataSource)).SortMethod = value;
				
				_sortMethod = value;
			}
		}
		
		protected class MeetUpViewDataDource : PlaceListViewDataSource
		{
			public PlaceSortMethod SortMethod {get;set;}
			
			public MeetUpViewDataDource (MeetUpViewController viewController) : base (viewController)
			{
				
			}
			
			public override UITableViewCell GetCell (UITableView tableView, MonoTouch.Foundation.NSIndexPath indexPath)
			{
				UITableViewCell cell = base.GetCell (tableView, indexPath);
				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				return cell;
			}
			
			public override string TitleForHeader (UITableView tableView, int section)
			{
				if (PlaceList.Count == 0)
					return string.Empty;
				
				switch (SortMethod)
				{
				case PlaceSortMethod.Popularity :
					return "Popular Places";
				case PlaceSortMethod.NearConventionCenter :
					return "Near Convention Center";
				}
				
				return string.Empty;
			}
		}
	}
}
