// This file has been autogenerated from parsing an Objective-C header file added in Xcode.

using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using XamarinEvolveSSLibrary;

namespace XamarinEvolveIOS
{

	public partial class WelcomeTableViewController : UITableViewController
	{
		public WelcomeTableViewController () : base ()
		{

		}

		public override void ViewDidLoad ()
		{
			this.Title = "Xamarin Evolve 2013";
			this.NavigationItem.BackBarButtonItem =  new UIBarButtonItem (
				"Back", UIBarButtonItemStyle.Bordered, null, null);

			this.TableView = new UITableView (this.View.Bounds, UITableViewStyle.Grouped);
			this.TableView.DataSource = new WelcomeTableViewDataSource ();
			this.TableView.Delegate = new WelcomeTableViewDelegate (NavigationController);	
		}

		public override void ViewDidAppear (bool animated)
		{
			base.ViewDidAppear (animated);

			TableView.ReloadData ();
		}

		private class  WelcomeTableViewDataSource : UITableViewDataSource
		{
			#region implemented abstract members of UITableViewDataSource	

			public override int NumberOfSections (UITableView tableView)
			{
				return 2;
			}

			public override int RowsInSection (UITableView tableView, int section)
			{
				if (section == 1)
				{
					if (Engine.Instance.UserAccess.GetCurrentUser ().IsAnonymousUser)
						return 1;
				}

				return 2;
			}			

			public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
			{
				UITableViewCell cell = tableView.DequeueReusableCell ("WelcomeTableViewCell");

				if (cell == null)
				{
					cell = new UITableViewCell (UITableViewCellStyle.Default, "WelcomeTableViewCell");
				}

				if (indexPath.Section == 0)
				{
					switch (indexPath.Row)
					{
					case 0:
						cell.TextLabel.Text = "Attendees";
						break;
					case 1:
						cell.TextLabel.Text = "Attendee Check-ins";
						break;

					default:
						throw new NotImplementedException ();
					}
				}
				else
				{
					switch (indexPath.Row)
					{
					case 0:
						if (!Engine.Instance.UserAccess.GetCurrentUser ().IsAnonymousUser)
							cell.TextLabel.Text = "Check-in";
						else
							cell.TextLabel.Text = "My Profile";
						break;
					case 1:
						cell.TextLabel.Text = "My Profile";
						break;
						
					default:
						throw new NotImplementedException ();
					}
				}

				cell.Accessory = UITableViewCellAccessory.DisclosureIndicator;
				return cell;
			}

			#endregion
		}

		private class WelcomeTableViewDelegate : UITableViewDelegate
		{
			UINavigationController _navigationController;

			public WelcomeTableViewDelegate (UINavigationController navigationController)
			{
				_navigationController = navigationController;
			}

			public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
			{
				if (indexPath.Section == 0)
				{
					switch (indexPath.Row)
					{
					case 0:
						_navigationController.PushViewController (new UsersViewController (), true);
						break;
					case 1:
						_navigationController.PushViewController (new MeetUpViewController (), true);
						break;
						
					default:
						throw new NotImplementedException ();
					}
				}
				else
				{
					switch (indexPath.Row)
					{
					case 0:
						if (!Engine.Instance.UserAccess.GetCurrentUser ().IsAnonymousUser)
							_navigationController.PushViewController (new CheckInViewController (), true);
						else
							_navigationController.PushViewController (
								new LocalProfileViewController (), true);
						break;	
					case 1:
						_navigationController.PushViewController (
							new LocalProfileViewController (), true);
						break;
						
					default:
						throw new NotImplementedException ();
					}
				}
			}
		}
	}


}
