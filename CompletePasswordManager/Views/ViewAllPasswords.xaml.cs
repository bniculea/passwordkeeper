using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Navigation;
using CompletePasswordManager.Common;
using CompletePasswordManager.DataModel;
using CompletePasswordManager.DataSource;
using CompletePasswordManager.DataStructures;
using CompletePasswordManager.Repository;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace CompletePasswordManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class ViewAllPasswords : Page
    {
        private NavigationHelper navigationHelper;
        private ObservableDictionary defaultViewModel = new ObservableDictionary();
        private ObservableRangeCollection<GroupInfoList<object>> _dataLetter = null;
        private EntryRepository entryRepository;
        private ListViewBase _listViewBase;
        private Border _groupHeaderBorder = null;
        private int _totalEntries = 0;
        public ViewAllPasswords()
        {
            this.InitializeComponent();
            entryRepository = new EntryRepository();
            this.navigationHelper = new NavigationHelper(this);
            this.navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this.navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        private async Task<List<Entry>> AddDataToRepository()
        {
            List<Entry> passwords = await App._connection.Table<Entry>().ToListAsync();
            return passwords;
        }
        private void semanticZoom_ViewChangeStarted(object sender, SemanticZoomViewChangedEventArgs e)
        {
            if (e.SourceItem == null || e.SourceItem.Item == null) return;

            if (e.SourceItem.Item.GetType() == typeof(HeaderItem))
            {
                HeaderItem headerItem = (HeaderItem)e.SourceItem.Item;

                var group = _dataLetter.First(d => ((char)d.Key) == headerItem.HeaderName);
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
        protected override async void OnNavigatedTo(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedTo(e);
            var list = await AddDataToRepository();
            entryRepository.Collection.AddRange(list);
            _totalEntries = entryRepository.Collection.Count;
            _dataLetter=  entryRepository.GetGroupsByLetter;
            cvs.Source = _dataLetter;
            
            _listViewBase = this.semanticZoom.ZoomedOutView as ListViewBase;
            if (_listViewBase != null)
                _listViewBase.ItemsSource = entryRepository.PasswordHeaders;


            this.lvZoomedInPasswords.SelectionChanged -= lvZoomedIn_SelectionChanged;
            this.lvZoomedInPasswords.SelectedItem = null;

            this.lvZoomedInPasswords.SelectionChanged += lvZoomedIn_SelectionChanged;

            this.semanticZoom.ViewChangeStarted -= semanticZoom_ViewChangeStarted;
            this.semanticZoom.ViewChangeStarted += semanticZoom_ViewChangeStarted;


        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this.navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private void ListViewItem_Holding(object sender, HoldingRoutedEventArgs e)
        {
            FrameworkElement senderElement = sender as FrameworkElement;
            FlyoutBase flyoutBase = FlyoutBase.GetAttachedFlyout(senderElement);
            flyoutBase.ShowAt(senderElement);

            //To get the clicked item
            // var datacontext = (e.OriginalSource as FrameworkElement).DataContext;
        }

        private async void FlyoutItemDelete_Click(object sender, RoutedEventArgs e)
        {
            MenuFlyoutItem menuFlyoutItem = sender as MenuFlyoutItem;
            if (menuFlyoutItem != null)
            {
                string tag = menuFlyoutItem.Tag.ToString();
                Entry entryToDelete = await App._connection.Table<Entry>().Where(k => k.Name.Equals(tag)).FirstAsync();
                await App._connection.DeleteAsync(entryToDelete);
                var group = _dataLetter.First(k => k.Contains(entryToDelete));
                int entryIndexInGroup = group.IndexOf(entryToDelete);
                group.RemoveAt(entryIndexInGroup);
                _totalEntries--;
                if (_groupHeaderBorder != null)
                {
                    EnableDisableGroupHeader(_groupHeaderBorder);
                }
            }
        }

        private void Border_Loaded(object sender, RoutedEventArgs e)
        {
            _groupHeaderBorder = sender as Border;
            if (_groupHeaderBorder != null)
            {
                EnableDisableGroupHeader(_groupHeaderBorder);
            }
        }

        private void EnableDisableGroupHeader(Border border)
        {
            if (_totalEntries < 10)
            {
                border.Width = 0;
                border.Height = 0;
                border.Opacity = 0;
                //Opacity = 0;
            }
        }

        private async void EditFlyoutItem_OnClick(object sender, RoutedEventArgs e)
        {
             MenuFlyoutItem menuFlyoutItem = sender as MenuFlyoutItem;
            if (menuFlyoutItem != null)
            {
                string tag = menuFlyoutItem.Tag.ToString();
                Entry entry = await App._connection.Table<Entry>().Where(k => k.Name.Equals(tag)).FirstAsync();
                Frame.Navigate(typeof (EditPage), entry);
            }
        }
    }
}
