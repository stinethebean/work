using LifeTrackerApp.Common;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Shapes;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.Storage;
using Windows.Storage.FileProperties;
using Windows.Storage.Pickers;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;
using Microsoft.WindowsAzure.MobileServices;
using System.Runtime.Serialization;



// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkId=234237

namespace LifeTrackerApp
{

    [DataContract]
    public class Item
    {
        [DataMember]
        public int Id { get; set; }
        [DataMember]
        public string UserID { get; set; }
        [DataMember]
        public string Title { get; set; }
        [DataMember]
        public string Date { get; set; }
        [DataMember]
        public string Description { get; set; }
        [DataMember]
        public string Files { get; set; }
        [DataMember]
        public string Favorite{ get; set; }
        }

    /// <summary>
    /// A basic page that provides characteristics common to most applications.
    /// </summary>
    public sealed partial class AddanItem : Page
    {

        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();

        /// <summary>
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// NavigationHelper is used on each page to aid in navigation and 
        /// process lifetime management
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }


        public AddanItem()
        {

            this.InitializeComponent();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += navigationHelper_LoadState;
            this.navigationHelper.SaveState += navigationHelper_SaveState;
            PickFilesButton.Click += new RoutedEventHandler(PickFilesButton_Click);
             
        }
        async void PickFilesButton_Click(object sender, RoutedEventArgs e)
        {
            // Clear any previously returned files between iterations of this scenario

            FileOpenPicker openPicker = new FileOpenPicker();
            openPicker.ViewMode = PickerViewMode.List;
            openPicker.SuggestedStartLocation = PickerLocationId.DocumentsLibrary;
            openPicker.FileTypeFilter.Add("*");
            IReadOnlyList<StorageFile> files = await openPicker.PickMultipleFilesAsync();
            if (files.Count > 0)
            {
                StringBuilder output = new StringBuilder("Chosen files:\n");
                StringBuilder filepaths = new StringBuilder();
                // Application now has read/write access to the picked file(s)
                foreach (StorageFile file in files)
                {
                    output.Append(file.Name + "\n");
                    filepaths.Append(file.Path + "\n");
                }
                OutputTextBlock.Text = output.ToString();
                FilePathBox.Text = filepaths.ToString();
            }
            else
            {
                OutputTextBlock.Text = "Operation cancelled.";
            }
        }

        
        async void InsertItem()
        {
            Item item = new Item { UserID = "Christine ", Title = TitleInput.Text, Date = DateInput.Date.ToString(), Description = DescriptionInput.Text, Files = FilePathBox.Text, Favorite=FavoriteCheck.IsChecked.ToString()};
            //Item title = new Item { Title = "Title" };
            // string date_number = DateInput.Date.ToString()
            //Item date = new Item { Date = "Date"};
            FavoriteCheck.IsChecked.ToString();

            await App.MobileService.GetTable<Item>().InsertAsync(item);
        }
        private void ButtonSave_Click(object sender, RoutedEventArgs e)
        {
            InsertItem();
            NavigationHelper.GoBack();
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="navigationParameter">The parameter value passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested.
        /// </param>
        /// <param name="pageState">A dictionary of state preserved by this page during an earlier
        /// session.  This will be null the first time a page is visited.</param>
        private void navigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="pageState">An empty dictionary to be populated with serializable state.</param>
        private void navigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// 
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="GridCS.Common.NavigationHelper.LoadState"/>
        /// and <see cref="GridCS.Common.NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            navigationHelper.OnNavigatedTo(e);
            //AppBarButton Save = new AppBarButton();
            //Save.Label = "Save";
            //Save.Icon = new SymbolIcon(Symbol.Save);
 
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {


            navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void ComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {

        }

        private void pageTitle_SelectionChanged(object sender, RoutedEventArgs e)
        {

        }


        
    }

}
