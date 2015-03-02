using CompletePasswordManager.Common;
using CompletePasswordManager.DataModel;
using System;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;
using SQLite;

// The Basic Page item template is documented at http://go.microsoft.com/fwlink/?LinkID=390556

namespace CompletePasswordManager.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class EditPage : Page
    {
        private NavigationHelper _navigationHelper;
        private ObservableDictionary _defaultViewModel = new ObservableDictionary();
        private string _initialName;
        private string _initialPassword;
        public EditPage()
        {
            this.InitializeComponent();

            this._navigationHelper = new NavigationHelper(this);
            this._navigationHelper.LoadState += this.NavigationHelper_LoadState;
            this._navigationHelper.SaveState += this.NavigationHelper_SaveState;
        }

        /// <summary>
        /// Gets the <see cref="NavigationHelper"/> associated with this <see cref="Page"/>.
        /// </summary>
        public NavigationHelper NavigationHelper
        {
            get { return this._navigationHelper; }
        }

        /// <summary>
        /// Gets the view model for this <see cref="Page"/>.
        /// This can be changed to a strongly typed view model.
        /// </summary>
        public ObservableDictionary DefaultViewModel
        {
            get { return this._defaultViewModel; }
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
            Entry entry = e.NavigationParameter as Entry;
            if (entry != null)
            {
                this.TxtName.Text = entry.Name;
                this._initialName = entry.Name;
                this.TxtPassword.Text = entry.Password;
                this._initialPassword = entry.Password;
            }
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
            this._navigationHelper.OnNavigatedTo(e);
           
        }

        protected override void OnNavigatedFrom(NavigationEventArgs e)
        {
            this._navigationHelper.OnNavigatedFrom(e);
        }

        #endregion

        private async void AppBarBtnSave_OnClick(object sender, RoutedEventArgs e)
        {
            string name = this.TxtName.Text;
            string password = this.TxtPassword.Text;
            if (string.IsNullOrEmpty(name.Trim()) || string.IsNullOrEmpty(password.Trim()))
            {
                MessageDialog messageDialog = new MessageDialog("Name and Password are mandatory", "Input error");
                await messageDialog.ShowAsync();
            }
            else if (name.Equals(_initialName) && password.Equals(_initialPassword))
            {
                MessageDialog messageDialog = new MessageDialog("Nothing to save. There were no modifications", "Save not fulfilled");
                await messageDialog.ShowAsync();
            }
            else
            {
                var allreadyInTheDatabase =
                    await App._connection.Table<Entry>().Where(k => k.Name.Equals(name)).FirstOrDefaultAsync();
                if (allreadyInTheDatabase != null && !name.Equals(_initialName))
                {
                    MessageDialog messageDialog =
                        new MessageDialog("There is already an account with this name. Please choose a different one :)");
                    await messageDialog.ShowAsync();
                }
                else
                {
                    string statement = "UPDATE Entry SET Name='" + name + "', Password='" + password + "' WHERE Name='" +
                                       _initialName + "'";
                    await App._connection.ExecuteAsync(statement, statement.Length);
                    this.TxtName.Text = String.Empty;
                    this.TxtPassword.Text = String.Empty;
                }


            }
        }

       
    }
}
