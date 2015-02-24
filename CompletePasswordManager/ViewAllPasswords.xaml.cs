using CompletePasswordManager.Common;
using CompletePasswordManager.DataModel;
using CompletePasswordManager.DataSource;
using CompletePasswordManager.Repository;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Graphics.Display;
using Windows.UI.Popups;
using Windows.UI.ViewManagement;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace CompletePasswordManager
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewAllPasswords : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private List<GroupInfoList<object>> dataLetter = null;
        public ViewAllPasswords()
        {
            this.InitializeComponent();
            EntryRepository entryRepository = new EntryRepository();
            AddEntryToRepository(entryRepository.Collection);
            dataLetter = entryRepository.GetGroupsByLetter;
            cvs.Source = dataLetter;

            this.lvZoomedInPasswords.SelectionChanged -= lvZoomedIn_SelectionChanged;
            this.lvZoomedInPasswords.SelectedItem = null;

            (this.semanticZoom.ZoomedOutView as ListViewBase).ItemsSource = entryRepository.PasswordHeaders;
            this.lvZoomedInPasswords.SelectionChanged += lvZoomedIn_SelectionChanged;

            this.semanticZoom.ViewChangeStarted -= semanticZoom_ViewChangeStarted;
            this.semanticZoom.ViewChangeStarted += semanticZoom_ViewChangeStarted;


            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        private void semanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            if (e.SourceItem == null || e.SourceItem.Item == null) return;

            if (e.SourceItem.Item.GetType() == typeof(HeaderItem))
            {
                HeaderItem headerItem = (HeaderItem)e.SourceItem.Item;

                var group = dataLetter.Find(d => ((char)d.Key) == headerItem.HeaderName);
                if (group != null)
                {
                    e.DestinationItem = new SemanticZoomLocation() { Item = group };
                }
             }
        }
        private async  void lvZoomedIn_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (this.lvZoomedInPasswords.SelectedItem != null)
            {
                Entry entry = this.lvZoomedInPasswords.SelectedItem as Entry;
                MessageDialog md = new MessageDialog(String.Format("You selected: {0} with Psw {1}", entry.Name, entry.Password));

                await md.ShowAsync();
                this.lvZoomedInPasswords.SelectedItem = null;
            }
        }

        private void AddEntryToRepository(ObservableCollection<Entry> observableCollection)
        {
            observableCollection.Add(new Entry { Name = "9GAG", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "500PX", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Facebook", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Facebook", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "facebook", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Twitter", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Google", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Yahoo", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Microsoft", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "DOna", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Hotmail", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "GMAIL", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Outlook", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Facebook", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Facebook", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Twitter", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Google", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Yahoo", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Microsoft", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "DOna", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Hotmail", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "GMAIL", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Twitter", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Google", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Yahoo", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Microsoft", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "DOna", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "Hotmail", Password = "34234234234sdsdf" });
            observableCollection.Add(new Entry { Name = "1GMAIL", Password = "34234234234sdsdf" });
        }

        private List<Entry> CreateEntries()
        {
            List<Entry> entries = new List<Entry>();
            entries.Add(new Entry { Name = "Facebook", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Twitter", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Google", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Yahoo", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Microsoft", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "DOna", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Hotmail", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "GMAIL", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Outlook", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Facebook", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Facebook", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Twitter", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Google", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Yahoo", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Microsoft", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "DOna", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Hotmail", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "GMAIL", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Twitter", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Google", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Yahoo", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Microsoft", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "DOna", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "Hotmail", Password = "34234234234sdsdf" });
            entries.Add(new Entry { Name = "GMAIL", Password = "34234234234sdsdf" });
            return entries;
        }

        private void Grid_Tapped(object sender, TappedRoutedEventArgs e)
        {
            this.semanticZoom.IsZoomedInViewActive = false;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this.navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this.defaultViewModel; }
        }

        /// <summary>
        /// Populates the page with content passed during navigation.  Any saved state is also
        /// provided when recreating a page from a prior session.
        /// </summary>
        /// <param name="sender">
        /// The source of the event; typically <see cref="NavigationHelper"/>
        /// </param>
        /// <param name="e">Event data that provides both the navigation parameter passed to
        /// <see cref="Frame.Navigate(Type, Object)"/> when this page was initially requested and
        /// a dictionary of state preserved by this page during an earlier
        /// session.  The state will be null the first time a page is visited.</param>
        private void NavigationHelper_LoadState(object sender, LoadStateEventArgs e)
        {
        }

        /// <summary>
        /// Preserves state associated with this page in case the application is suspended or the
        /// page is discarded from the navigation cache.  Values must conform to the serialization
        /// requirements of <see cref="SuspensionManager.SessionState"/>.
        /// </summary>
        /// <param name="sender">The source of the event; typically <see cref="NavigationHelper"/></param>
        /// <param name="e">Event data that provides an empty dictionary to be populated with
        /// serializable state.</param>
        private void NavigationHelper_SaveState(object sender, SaveStateEventArgs e)
        {
        }

        #region NavigationHelper registration

        /// <summary>
        /// The methods provided in this section are simply used to allow
        /// NavigationHelper to respond to the page's navigation methods.
        /// <para>
        /// Page specific logic should be placed in event handlers for the  
        /// <see cref="NavigationHelper.LoadState"/>
        /// and <see cref="NavigationHelper.SaveState"/>.
        /// The navigation parameter is available in the LoadState method 
        /// in addition to page state preserved during an earlier session.
        /// </para>
        /// </summary>
        /// <param name="e">Provides data for navigation methods and event
        /// handlers that cannot cancel the navigation request.</param>
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion
    }
}
