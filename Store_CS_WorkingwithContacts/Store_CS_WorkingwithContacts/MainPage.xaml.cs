using System;
using System.Collections.Generic;
using Windows.ApplicationModel.Contacts;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Media.Imaging;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234238

namespace Store_CS_WorkingwithContacts
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {

        string selectionType = "";

        /// <summary>
        /// The Selected Contacts
        /// </summary>
        IReadOnlyList<ContactDetails> _ContactDetails;

        public IReadOnlyList<ContactDetails> ContactDetails
        {
            get { return _ContactDetails; }
            set { _ContactDetails = value; }
        }


         /// <summary>
         /// List of Contact Details use to display in UI
         /// </summary>
      List<ContactDetails> _SingleContactDetails;

      public List<ContactDetails> SingleContactDetails
        {
            get { return _SingleContactDetails; }
            set { _SingleContactDetails = value; }
        }

        public MainPage()
        {
            this.InitializeComponent();
        }

        /// <summary>
        /// Invoked when this page is about to be displayed in a Frame.
        /// </summary>
        /// <param name="e">Event data that describes how this page was reached.  The Parameter
        /// property is typically used to configure the page.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
        }

        /// <summary>
        /// The click Event. This event will decide wheather to allow Single contact pick or allow used to select multiple
        /// contacts.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnCallContact_Click(object sender, RoutedEventArgs e)
        {
            SingleContactDetails = new List<Store_CS_WorkingwithContacts.ContactDetails>(); 
            var contactSelector = new Windows.ApplicationModel.Contacts.ContactPicker();
            //Set the text for the commit button of the ContactPicker UI
            contactSelector.CommitButtonText = "Pick Contact";


            if (selectionType == "Single")
            {
                //Select Single Contact
                var selectedContact = await contactSelector.PickSingleContactAsync();
               SingleContactDetails.Add(new ContactDetails(selectedContact));
            }
            else
            {
                if (selectionType == "Multiple")
                {
                    //Select Multiple Contacts
                    var selectedContact = await contactSelector.PickMultipleContactsAsync();

                    foreach (var item in selectedContact)
                    {
                        SingleContactDetails.Add(new ContactDetails(item));
                    }
                }
                else
                {
                    txtmessage.Text = "Please Select the option";
                }
               
            }
            lstcontactsdetails.ItemsSource = SingleContactDetails;

        }

        private void rdsingleview_Click(object sender, RoutedEventArgs e)
        {
            selectionType = "Single";
        }

        private void rdmultipleview_Click(object sender, RoutedEventArgs e)
        {
            selectionType = "Multiple";
        }
    }

    /// <summary>
    /// Class to Display Contact Details
    /// This class is used to read the imformation of the selectded contact(s)
    /// </summary>
    public class ContactDetails
    {
        public string ContactName { get; private set; }
        public BitmapImage ContactImage { get; private set; }
        //The boolean property used to decide wheather to display the PhoneNo and Email List
        //based upon the availabe infiormation in the contact
        public Visibility CanShow { get; set; }

        public List<string> PhoneNumbers { get; set; }

        public List<string> ContactEmails { get; set; }

        public ContactDetails(ContactInformation c)
        {
            PhoneNumbers = new List<string>();
            ContactEmails = new List<string>(); 

            CanShow = Visibility.Visible;
            ContactName = c.Name;
            if (c.PhoneNumbers.Count > 0)
            {
                foreach (var item in c.PhoneNumbers)
                {
                    PhoneNumbers.Add(item.Value);
                }
            }
            else
            {
                CanShow = Visibility.Collapsed;
            }
            if (c.Emails.Count > 0)
            {
                foreach (var item in c.Emails)
                {
                    ContactEmails.Add(item.Value);
                }
            }
            else
            {
                CanShow = Visibility.Collapsed;
            }
            GetContactImage(c);
         
        }
        /// <summary>
        /// The method whihc read the Contact Image
        /// </summary>
        /// <param name="c"></param>
        async void GetContactImage(ContactInformation c)
        {
            var imgStream = await c.GetThumbnailAsync();
            ContactImage = new BitmapImage(); 
            if (imgStream != null && imgStream.Size > 0)
            {
                ContactImage.SetSource(imgStream);
            }
        }
    }
}
