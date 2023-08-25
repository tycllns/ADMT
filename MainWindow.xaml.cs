using ADMT___WPF.Utilities;
using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ADMT_Revised
{
    public partial class MainWindow : Window
    {
        // Initialize utility classes

        AD_Utilities AD_UT = new AD_Utilities("Domain.com");
        UI_Utilities UI_UT = new UI_Utilities();

        // Constructor

        public MainWindow()
        {
            InitializeComponent();
            CloseAllPanels();

        }

//
// UI METHODS*************************
//
        //Close all window panels
        public void CloseAllPanels()
        {
            rctAddRemove.Visibility = Visibility.Hidden;
            rctComputerMan.Visibility = Visibility.Hidden;
            rctEnableDisable.Visibility = Visibility.Hidden;
            rctPwrdReset.Visibility = Visibility.Hidden;
            rctSettings.Visibility = Visibility.Hidden;
            rctUserCreate.Visibility = Visibility.Hidden;
        }

        // Reset menu button background colors
        public void ChangeBackToDefaultColor()
        {
            BrushConverter bc = new BrushConverter();
            btnUserCreate.Background = (Brush)bc.ConvertFrom("#FF3C3C3C");
            btnAddRemoveGroups.Background = (Brush)bc.ConvertFrom("#FF3C3C3C");
            btnEnable_Disable.Background = (Brush)bc.ConvertFrom("#FF3C3C3C");
            btnComputerMan.Background = (Brush)bc.ConvertFrom("#FF3C3C3C");
            btnPwrdReset.Background = (Brush)bc.ConvertFrom("#FF3C3C3C");
            btnSettings.Background = (Brush)bc.ConvertFrom("#FF3C3C3C");
        }

        //Clear all elements in User Create Screen
        public void UCClear()
        {
            txtUCUserName.Text = string.Empty;
            txtUCPassword.Password = string.Empty;
            txtUCConfirm.Password = string.Empty;
            txtUCFirstName.Text = string.Empty;
            txtUCLastName.Text = string.Empty;
            txtUCEmployeeID.Text = string.Empty;
            txtUCDepartment.Text = string.Empty;
            txtUCJobDescription.Text = string.Empty;
            chkbxUCMCP.IsChecked = false;
            chkbxUCCCP.IsChecked = false;
            chkbxUCPNE.IsChecked = false;
            chkbxUCAccountDisabled.IsChecked = false;
            txtUCFullName.Text = string.Empty;
            cmbxUCTargetOU.SelectedIndex = 0;
            //lstbxUCGroups.Items.Clear();
            //lstbxUCGroupsByLocation.Items.Clear();
            foreach (CheckBox item in lstbxUCGroupsByLocation.Items)
            {
                item.IsChecked = false;
            }
            foreach (CheckBox item in lstbxUCGroups.Items)
            {
                item.IsChecked = false;
            }


        }

        //Opens the requested Panel and animates it using storyboards
        private void OpenPanelAndAnimate(Panel panel, string openStoryboardKey, string buttonActivateStoryboardKey)
        {
            ChangeBackToDefaultColor();
            CloseAllPanels();
            panel.Visibility = Visibility.Visible;
            Storyboard sb = (this.FindResource(openStoryboardKey) as Storyboard);
            Storyboard sb2 = (this.FindResource(buttonActivateStoryboardKey) as Storyboard);
            sb.Begin();
            sb2.Begin();
            sb.Stop();
        }

//
// FUNCTIONAL METHODS*************************
//

        //Gather all elements from User Create Screen and put them in a messagebox
        private void CollectAndDisplayTextFromElements()
        {

            Dictionary<string, string> detailsDict = new Dictionary<string, string>();

            int itemCount = Math.Min(stkpnlUserDetails.Children.Count, stkpnlUserdetailLabels.Children.Count);

            for (int i = 0; i < itemCount; i++)
            {
                if (stkpnlUserdetailLabels.Children[i] is Label keyItem && stkpnlUserDetails.Children[i] is TextBox valueItem)
                {
                    detailsDict.Add(keyItem.Content.ToString(), valueItem.Text);
                }
            }

            detailsDict.Add("Full Name", txtUCFullName.Text);
            detailsDict.Add(lblUCDescription.Content.ToString(), txtUCJobDescription.Text);
            detailsDict.Add(chkbxUCMCP.Content.ToString(), chkbxUCMCP.IsChecked.ToString());
            detailsDict.Add(chkbxUCCCP.Content.ToString(), chkbxUCCCP.IsChecked.ToString());
            detailsDict.Add(chkbxUCPNE.Content.ToString(), chkbxUCPNE.IsChecked.ToString());
            detailsDict.Add(chkbxUCAccountDisabled.Content.ToString(), chkbxUCAccountDisabled.IsChecked.ToString());

            detailsDict.Add("Target OU", cmbxUCTargetOU.Text);

            List<string> Groups = new List<string>();
            foreach (CheckBox group in lstbxUCGroups.Items)
            {
                Groups.Add(group.Content.ToString());
            }
            List<string> GroupsByLocation = new List<string>();
            foreach (CheckBox group in lstbxUCGroupsByLocation.Items)
            {
                GroupsByLocation.Add(group.Content.ToString());
            }

            detailsDict.Add("Groups", string.Join(", ", Groups));
            detailsDict.Add("Groups By Location", string.Join(", ", GroupsByLocation));

            string detailsOutput = "You have created user with the following details:\n\n";
            foreach (var kvp in detailsDict)
            {
                detailsOutput += $"{kvp.Key}: {kvp.Value}\n\n";
            }

            MessageBox.Show(detailsOutput);
        }


        // Panel Open Method



//
// EVENTS*************************
//

        // Validation Events

        private void txtUCPassword_TextChanged(object sender, TextChangedEventArgs e)
        {
            // Password validation logic
            // Update UI elements based on password validity
        }

        private void txtUCPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (UI_UT.PasswordValidation(txtUCPassword.Password))
            {
                imgUCGoodPassword.Visibility = Visibility.Visible;
                imgUCBadPassword.Visibility = Visibility.Collapsed;
                txtUCConfirm.IsEnabled = true;
            }
            else
            {
                imgUCGoodPassword.Visibility = Visibility.Collapsed;
                imgUCBadPassword.Visibility = Visibility.Visible;
                txtUCConfirm.IsEnabled = false;
            }
        }

        private void txtCMComputerName_TextChanged(object sender, TextChangedEventArgs e)
        {
            if (txtCMComputerName.Text != "")
            {
                lstbxCMCurrentGroups.Items.Clear();
                lstbxCMCurrentGroups.Items.Add("NYC");
                lstbxCMCurrentGroups.Items.Add("OKC");
                cmbxCMTargetOU.Items.Clear();
                cmbxCMTargetOU.Items.Add("OKC");
                cmbxCMTargetOU.Items.Add("NYC");
            }
            else
            {
                lstbxCMCurrentGroups.Items.Clear();
                cmbxCMTargetOU.Items.Clear();
            }
        }

        private void txtPRNewPassword_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (UI_UT.PasswordValidation(txtPRNewPassword.Password))
            {
                imgPRGoodPassword_Copy.Visibility = Visibility.Visible;
                imgPRBadPassword_Copy.Visibility = Visibility.Collapsed;
                txtUCConfirm.IsEnabled = true;
            }
            else
            {
                imgPRGoodPassword_Copy.Visibility = Visibility.Collapsed;
                imgPRBadPassword_Copy.Visibility = Visibility.Visible;
                txtUCConfirm.IsEnabled = false;
            }
        }

        private void txtPRUserName_TextChanged(object sender, TextChangedEventArgs e)
        {

        }

        private void txtUCConfirm_PasswordChanged(object sender, RoutedEventArgs e)
        {
            if (txtUCConfirm.Password == txtUCPassword.Password)
            {
                imgUCGoodPassword_Copy.Visibility = Visibility.Visible;
                imgUCBadPassword_Copy.Visibility = Visibility.Collapsed;
            }
            else
            {
                imgUCGoodPassword_Copy.Visibility = Visibility.Collapsed;
                imgUCBadPassword_Copy.Visibility = Visibility.Visible;
            }
        }


        //Button Events

        private void btnUserCreate_Click(object sender, RoutedEventArgs e)
        {
            OpenPanelAndAnimate(rctUserCreate, "UCOpen", "UserCreateButtonActivate");
        }

        private void btnAddRemoveGroups_Click(object sender, RoutedEventArgs e)
        {
            OpenPanelAndAnimate(rctAddRemove, "ARGOpen", "AddRemoveButtonActivate");
        }

        private void btnEnable_Disable_Click(object sender, RoutedEventArgs e)
        {
            OpenPanelAndAnimate(rctEnableDisable, "EDOpen", "EnableDisableUserButtonActivate");
        }

        private void btnComputerMan_Click(object sender, RoutedEventArgs e)
        {
            OpenPanelAndAnimate(rctComputerMan, "CMOpen", "ComputerManagementButtonActivate");
        }

        private void btnPwrdReset_Click(object sender, RoutedEventArgs e)
        {
            OpenPanelAndAnimate(rctPwrdReset, "PROpen", "PwrdResetButtonActivate");
        }

        private void btnSettings_Click(object sender, RoutedEventArgs e)
        {
            
        }

        private void btnCreateUserConfirm_Click(object sender, RoutedEventArgs e)
        {
            if (txtUCConfirm.Password == txtUCPassword.Password)
            {
                CollectAndDisplayTextFromElements();
            }
            else
            {
                MessageBox.Show("Passwords do not match");
            }
        }

        private void btnAddRemoveFromGroups_Click(object sender, RoutedEventArgs e)
        {
            if (txtARGUserName1.Text != "")
            {
                List<string> RemovedGroups = new List<string>();
                List<string> AddedGroups = new List<string>();
                List<string> GroupCollections = new List<string>();

                foreach (CheckBox item in lstbxARGRemoveGroups.Items)
                {
                    if (item.IsChecked == true)
                    {
                        RemovedGroups.Add(item.Content.ToString());
                    }
                }
                foreach (CheckBox item in lstbxARGAddGroup.Items)
                {
                    if (item.IsChecked == true)
                    {
                        AddedGroups.Add(item.Content.ToString());
                    }
                }
                foreach (CheckBox item in lstbxARGAddGroupCollections.Items)
                {
                    if (item.IsChecked == true)
                    {
                        GroupCollections.Add(item.Content.ToString());
                    }
                }

                // Construct the message for the MessageBox
                string message = "User" + txtARGUserName1.Text + " has been:\n\n";
                if (RemovedGroups.Count > 0)
                {
                    message += "Removed from groups:\n";
                    message += string.Join(", ", RemovedGroups) + "\n\n";
                }
                if (AddedGroups.Count > 0)
                {
                    message += "Added to groups:\n";
                    message += string.Join(", ", AddedGroups) + "\n\n";
                }
                if (GroupCollections.Count > 0)
                {
                    message += "Added to group collections:\n";
                    message += string.Join(", ", GroupCollections);
                }

                // Display the MessageBox
                MessageBox.Show(message, "Groups Information", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else { MessageBox.Show("No Username Entered!"); }
        }

        private void btnEDEnable_Click(object sender, RoutedEventArgs e)
        {
            if(txtEDUserName2.Text == "" || dtpEDDate.Text == "")
            {
                MessageBox.Show("Please Enter Username and Date");
                return;
            }
            string message = txtEDUserName2.Text + "\n\nHas been Enabled \n\n" + dtpEDDate.Text;
            MessageBox.Show(message);
        }

        private void btnEDDisable_Click(object sender, RoutedEventArgs e)
        {
            if (txtEDUserName2.Text == "" || dtpEDDate.Text == "")
            {
                MessageBox.Show("Please Enter Username and Date");
                return;
            }
            string message = txtEDUserName2.Text + "\n\nHas been Disabled \n\n" + dtpEDDate.Text;
            MessageBox.Show(message);
        }

        private void btnCMAddToList_Click(object sender, RoutedEventArgs e)
        {
            lstbxCMComputersToMove.Items.Add(txtCMComputerName.Text);
            txtCMComputerName.Clear();
        }

        private void btnManageComputer_Click(object sender, RoutedEventArgs e)
        {
            if (lstbxCMComputersToMove.Items.Count > 0)
            {
                string message = "The Following Computers Have Been Added Or Moved:";
                foreach (string item in lstbxCMComputersToMove.Items)
                {
                    message += "\n\n" + item;
                }
                MessageBox.Show(message);
            }
            else
            {
                MessageBox.Show("No Computers To Move");
            }
        }

        private void btnCMClearList_Click(object sender, RoutedEventArgs e)
        {
            lstbxCMComputersToMove.Items.Clear();
        }

        private void btnPRReset_Click(object sender, RoutedEventArgs e)
        {
            if(imgPRBadPassword.Visibility == Visibility.Visible || imgPRBadPassword_Copy.Visibility == Visibility.Visible)
            {
                MessageBox.Show("Please enter a correct user name or password");
            }
            else 
            { 
                MessageBox.Show(txtPRUserName + " Password has been reset"); 
            }
            
        }

        private void btnUCClear_Click(object sender, RoutedEventArgs e)
        {
            UCClear();
        }

        
    }
}
