// This file has been autogenerated from parsing an Objective-C header file added in Xcode.

using System;

using MonoTouch.Foundation;
using MonoTouch.UIKit;
using System.Drawing;
using XamarinEvolveSSLibrary;

namespace XamarinEvolveIOS
{
	public partial class ProfileViewController : UITableViewController
	{
		private User _currentUser;

		public ProfileViewController (User user) : base (UITableViewStyle.Grouped)
		{
			CurrentUser = user;
		}

		public override void LoadView ()
		{
			this.Title = CurrentUser.IsLocalUser ? "My Profile" : "Profile";

			base.LoadView ();
			TableView.DataSource = new LocalUserProfileDataSource (
				this, CurrentUser);
			TableView.Delegate = new LocalUserProfileDelegate ();
			SetupEditButton ();
		}

		void SetupEditButton ()
		{
			if (!CurrentUser.IsLocalUser)
				return;

			UIBarButtonItem editButton = new UIBarButtonItem ("Edit", UIBarButtonItemStyle.Done, delegate {
				this.SetEditing (!this.Editing, true);
			});
			this.NavigationItem.RightBarButtonItem = editButton;
		}

		public override void SetEditing (bool editing, bool animated)
		{
			base.SetEditing (editing, animated);

			if (this.Editing) {
				this.NavigationItem.RightBarButtonItem.Title = "Done";
				this.NavigationItem.RightBarButtonItem.Style = UIBarButtonItemStyle.Done;
			}
			else {
				this.NavigationItem.RightBarButtonItem.Title = "Edit";
				this.NavigationItem.RightBarButtonItem.Style = UIBarButtonItemStyle.Plain;
				RefreshHeaderCell ();
			}
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

		protected User CurrentUser {
			get{
				return _currentUser;
			}
			set{
				_currentUser = value;

				LocalUserProfileDataSource dataSrc = TableView.DataSource as LocalUserProfileDataSource;
				if (dataSrc != null)
					dataSrc.UserProfile = value;

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

		public static bool canUsePhone ()
		{
			return UIApplication.SharedApplication.CanOpenUrl(
				new NSUrl ("tel://412-867-5309"));
		}

		public static bool canUseEMail ()
		{
			return UIApplication.SharedApplication.CanOpenUrl(
				new NSUrl ("mailto:?to=fun@xamarin.com"));
		}

		bool validEMail (string value)
		{
			if (string.IsNullOrWhiteSpace (value))
				return false;

			return true;
		}

		bool validPhone (string value)
		{
			if (string.IsNullOrWhiteSpace (value))
				return false;

			return true;
		}

		public override void RowSelected (UITableView tableView, NSIndexPath indexPath)
		{
			if (indexPath.Section == 2 && indexPath.Row == 1)
			{
				if (!canUsePhone ())
					return;

				CustomUITableViewCell cell = tableView.CellAt (indexPath) as CustomUITableViewCell;
				NameValueCell nameCell = cell.CustomView as NameValueCell;

				cell.Selected = false;
				string value = nameCell.ValueLabel.Text;

				if (!validPhone (value))
					return;

				UIAlertView view = new UIAlertView ("Call Number?", value, 
				                                    null, null, new string [] {"Yes", "No"}); 
				view.CancelButtonIndex = 1;

				view.Clicked += (object sender, UIButtonEventArgs e) => {
					if (e.ButtonIndex == 0)
					UIApplication.SharedApplication.OpenUrl (
							new NSUrl (string.Format ("tel://{0}", value)));
				};

				view.Show ();
			}
			else if (indexPath.Section == 2 && indexPath.Row == 0)
			{
				if (!canUseEMail ())
					return;

				CustomUITableViewCell cell = tableView.CellAt (indexPath) as CustomUITableViewCell;
				NameValueCell nameCell = cell.CustomView as NameValueCell;

				cell.Selected = false;
				string value = nameCell.ValueLabel.Text;
				
				if (!validEMail (value))
					return;

				UIAlertView view = new UIAlertView ("Create E-Mail?", value, 
				                                    null, null, new string [] {"Yes", "No"}); 
				view.CancelButtonIndex = 1;
				
				view.Clicked += (object sender, UIButtonEventArgs e) => {
					if (e.ButtonIndex == 0)
						UIApplication.SharedApplication.OpenUrl (
							new NSUrl (string.Format ("mailto:?to={0}", value)));
				};
				
				view.Show ();
			}
		}
	}

	public class LocalUserProfileDataSource : UITableViewDataSource
	{
		UITableViewController _controller;

		public LocalUserProfileDataSource (UITableViewController controller, User profile)
		{
			UserProfile = profile;
			_controller = controller;
		}
		
		public User UserProfile {get; set;}
		
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

			headerCell.OnImageChangeRequest += (originalImage) => {
				AvatarSelectorController ctrl;
				this._controller.NavigationController.PushViewController (ctrl = new AvatarSelectorController (originalImage), true);
				ctrl.SelectorView.ImageApplied += (image) => {
					headerCell.ImageView.Image = image; 
					PostNewAvatar (image);
				};
			};

			return headerCell.LoadCell (tableView);
		}

		void PostNewAvatar (UIImage image)
		{
			NSData data = image.AsPNG ();

			byte[] dataBytes = new byte[data.Length];
			System.Runtime.InteropServices.Marshal.Copy(data.Bytes, dataBytes, 0, Convert.ToInt32(data.Length));

			Engine.Instance.AvatarAccess.PostNewAvatar (dataBytes, (result) => {
				if (!string.IsNullOrEmpty (result.URL))
				{
					UIImageCache.ReloadImage (result.URL, null, image);
				}
			});
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
			UITableViewCell cell;
			NameValueCell nameValueCell;
			
			switch (indexPath.Row)
			{
			case 0:
				nameValueCell = new NameValueCell ("e-mail", () => UserProfile.Email, v => UserProfile.Email= v);
				nameValueCell.ValueTextField.KeyboardType = UIKeyboardType.EmailAddress;
				nameValueCell.ValueTextField.AutocapitalizationType = UITextAutocapitalizationType.None;
				nameValueCell.ValueTextField.AutocorrectionType = UITextAutocorrectionType.No;
				cell = nameValueCell.LoadCell (tableView);
				if (LocalUserProfileDelegate.canUsePhone ())
					cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
				break;
			case 1:
				nameValueCell = new NameValueCell ("phone", () => UserProfile.Phone, v => UserProfile.Phone= v);
				nameValueCell.ValueTextField.KeyboardType = UIKeyboardType.PhonePad;
				nameValueCell.ValueTextField.AutocorrectionType = UITextAutocorrectionType.No;
				cell = nameValueCell.LoadCell (tableView);
				if (LocalUserProfileDelegate.canUseEMail ())
					cell.SelectionStyle = UITableViewCellSelectionStyle.Blue;
				break;
				
			default:
				throw new IndexOutOfRangeException ("indexPath.Row out of range.");
			}
			
			return cell;
		}
	}
}
