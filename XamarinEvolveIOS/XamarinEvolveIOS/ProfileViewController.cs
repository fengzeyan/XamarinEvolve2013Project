// This file has been autogenerated from parsing an Objective-C header file added in Xcode.

using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;

namespace XamarinEvolveIOS
{
	public partial class ProfileViewController : UITableViewController
	{
		public ProfileViewController (IntPtr handle) : base (handle)
		{

		}

		public override void LoadView ()
		{
			base.LoadView ();
			TableView.DataSource = new LocalUserProfileDataSource (
				this, new LocalUserProfile () {
				UserName = "billholmes54",
				FullName = "Bill Holmes",
				City = "Pittsburgh, PA",
				Company = "Slap Holmes Productions",
				Title = "CEO",
				EMail = "bill@mobillholmes.com",
				Phone = "(555)-555-5555",
			});
			TableView.Delegate = new LocalUserProfileDelegate ();
			UIBarButtonItem editButton = new UIBarButtonItem ("Edit", UIBarButtonItemStyle.Done, delegate {

				this.SetEditing (!this.Editing, true);

				if (this.Editing)
				{
					this.NavigationItem.RightBarButtonItem.Title = "Done";
					this.NavigationItem.RightBarButtonItem.Style = UIBarButtonItemStyle.Done;
				}
				else
				{
					this.NavigationItem.RightBarButtonItem.Title = "Edit";
					this.NavigationItem.RightBarButtonItem.Style = UIBarButtonItemStyle.Plain;

					RefreshHeaderCell ();
				}
			});
			this.NavigationItem.RightBarButtonItem = editButton;
		}

		void RefreshHeaderCell ()
		{
			CustomUITableViewCell headerCell = 
				this.TableView.CellAt (NSIndexPath.FromRowSection (0, 0)) as CustomUITableViewCell;

			if (headerCell != null) {
				UserProfileHeaderCell headerInnerCell = headerCell.CustomView as UserProfileHeaderCell;
				if (headerInnerCell != null) {
					MonoTouch.UIKit.UIApplication.SharedApplication.BeginInvokeOnMainThread (delegate {
						headerInnerCell.RefreshImageFromData ();
					});
				}
			}
		}
	}

	public class LocalUserProfileDelegate : UITableViewDelegate
	{
		public override float GetHeightForRow (UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Section == 0 && indexPath.Row == 0)
				return 100;

			return 44;
		}

		public override float GetHeightForFooter (UITableView tableView, int section)
		{
			if (section == 2)
				return 60;

			return 0;
		}

		public override UIView GetViewForFooter (UITableView tableView, int section)
		{
//			if (section == 2)
//			{
//				UIButton button = UIButton.FromType (UIButtonType.RoundedRect);
//				button.SetTitle ("click", UIControlState.Normal);
//				button.Frame = new RectangleF (10, 15, 140, 44);
//
//				UIView newView = new UIView (new RectangleF (0, 0, 1000, 60));
//				newView.Add (button);
//
//				return newView;
//			}

			return null;
		}

		public override UITableViewCellEditingStyle EditingStyleForRow (UITableView tableView, NSIndexPath indexPath)
		{
			return UITableViewCellEditingStyle.None;
		}

		public override bool ShouldIndentWhileEditing (UITableView tableView, NSIndexPath indexPath)
		{
			return false;
		}
	}

	public class LocalUserProfileDataSource : UITableViewDataSource
	{
		UITableViewController _controller;

		public LocalUserProfileDataSource (UITableViewController controller, LocalUserProfile profile)
		{
			UserProfile = profile;
			_controller = controller;
		}
		
		public LocalUserProfile UserProfile {get; private set;}
		
		#region UITableViewDataSource	

		public override bool CanEditRow (UITableView tableView, NSIndexPath indexPath)
		{
			return true;
		}

		public override int NumberOfSections (UITableView tableView)
		{
			return 3;
		}
		
		public override int RowsInSection (UITableView tableView, int section)
		{
			if (section == 0)
				return 1;

			if (section == 1)
				return 2;

			if (section == 2)
				return 2;

			throw new IndexOutOfRangeException ("section out of range");
		}		
		
		public override UITableViewCell GetCell (UITableView tableView, NSIndexPath indexPath)
		{
			UITableViewCell cell;

			switch (indexPath.Section)
			{
			case 0:
				cell = GetCellSectionOne (tableView, indexPath);
				break;
			case 1:
				cell = GetCellSectionTwo (tableView, indexPath);
				break;
			case 2:
				cell = GetCellSectionThree (tableView, indexPath);
				break;
				
			default:
				throw new IndexOutOfRangeException ("indexPath.Section out of range.");
			}

			return cell;
		}
		
		#endregion

		private UITableViewCell GetCellSectionOne (UITableView tableView, NSIndexPath indexPath)
		{
			UserProfileHeaderCell headerCell = new UserProfileHeaderCell (UserProfile);

			headerCell.OnImageChangeRequest += delegate {
				this._controller.NavigationController.PushViewController (new UIViewController (), true);
			};

			return headerCell.LoadCell (tableView);
		}

		private UITableViewCell GetCellSectionTwo (UITableView tableView, NSIndexPath indexPath)
		{
			NameValueCell cell;
			
			switch (indexPath.Row)
			{
			case 0:
				cell = new NameValueCell ("company", () => UserProfile.Company, v => UserProfile.Company= v);
				cell.ValueTextField.AutocapitalizationType = UITextAutocapitalizationType.Words;
				cell.ValueTextField.AutocorrectionType = UITextAutocorrectionType.No;
				break;
			case 1:
				cell = new NameValueCell ("title", () => UserProfile.Title, v => UserProfile.Title= v);
				cell.ValueTextField.AutocapitalizationType = UITextAutocapitalizationType.Words;
				cell.ValueTextField.AutocorrectionType = UITextAutocorrectionType.No;
				break;
				
			default:
				throw new IndexOutOfRangeException ("indexPath.Row out of range.");
			}

			return cell.LoadCell (tableView);
		}

		private UITableViewCell GetCellSectionThree (UITableView tableView, NSIndexPath indexPath)
		{
			NameValueCell cell;
			
			switch (indexPath.Row)
			{
			case 0:
				cell = new NameValueCell ("e-mail", () => UserProfile.EMail, v => UserProfile.EMail= v);
				cell.ValueTextField.KeyboardType = UIKeyboardType.EmailAddress;
				cell.ValueTextField.AutocorrectionType = UITextAutocorrectionType.No;
				break;
			case 1:
				cell = new NameValueCell ("phone", () => UserProfile.Phone, v => UserProfile.Phone= v);
				cell.ValueTextField.KeyboardType = UIKeyboardType.PhonePad;
				cell.ValueTextField.AutocorrectionType = UITextAutocorrectionType.No;
				break;
				
			default:
				throw new IndexOutOfRangeException ("indexPath.Row out of range.");
			}
			
			return cell.LoadCell (tableView);
		}
	}
	
	public class UserProfile
	{
		public string UserName {get; set;}
		public string FullName {get; set;}
		public string EMail {get; set;}
		public string City {get;set;}
		public string Company {get; set;}
		public string Title {get; set;}
		public string Phone {get;set;}
		public string AvatarURL {get;set;}
	}

	public class LocalUserProfile : UserProfile
	{
		
	}

}
