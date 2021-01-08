using RootStack.Demo.Models;
using RootStack.Demo.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RootStack.Demo.ViewModels
{
    public class HomeViewModel : BaseViewModel
    {
        private IBackendRequestHandlerService backendRequestHandlerService;
        public HomeViewModel()
        {
            backendRequestHandlerService = DependencyService.Get<IBackendRequestHandlerService>();
            UserTreshold = 1;
            Users = new ObservableCollection<User>();
            TemporalyUsers = new ObservableCollection<User>();
            LoadUsers();
        }

        public ObservableCollection<User> TemporalyUsers { get; set; }
        public ICommand UserTresholdReachedCommand => new Command(UsersTresholdReached);
        public ICommand SearchCommand => new Command<string>(Search);

        private ObservableCollection<User> users;

        public ObservableCollection<User> Users
        {
            get { return users; }
            set { SetPropertyValue(ref users, value); }
        }


        private int userTreshold;
        public int UserTreshold
        {
            get { return userTreshold; }
            set { SetPropertyValue(ref userTreshold, value); }
        }

        private async void UsersTresholdReached()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                var items = await GetUsersFromServer(true, Users.Count);

                var previousLastItem = Users.Last();
                foreach (var item in items)
                {
                    Users.Add(item);
                }
                TemporalyUsers = Users;
                Debug.WriteLine($"{items.Count()} {Users.Count} ");
                if (items.Count() == 0)
                {
                    UserTreshold = -1;
                    return;
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private async void LoadUsers()
        {
            if (IsBusy)
                return;

            IsBusy = true;

            try
            {
                UserTreshold = 1;
                Users.Clear();
                var items = await GetUsersFromServer(true);
                foreach (var item in items)
                {
                    Users.Add(item);
                }
                TemporalyUsers = Users;
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
            }
            finally
            {
                IsBusy = false;
            }
        }

        private void Search(string searchTerm)
        {
            if (!string.IsNullOrEmpty(searchTerm))
            {
                var user = Users.Where(s => s.Email.Contains(searchTerm));
                UserTreshold = -1;
                Users = new ObservableCollection<User>(user);
            }
            else
            {
                Users = TemporalyUsers;
                UserTreshold = 1;
            }
        }


        public async Task<IEnumerable<User>> GetUsersFromServer(bool forceRefresh = false, int lastIndex = 1)
        {
            int numberOfItemsPerPage = 15;
            Dictionary<string, string> parameters = new Dictionary<string, string>
            {
                { "page", lastIndex.ToString() },
                { "results", numberOfItemsPerPage.ToString() },
            };
            BackendResponse response = await backendRequestHandlerService.GetRequest<BackendResponse>("api", parameters);
            return response.Results;
        }
    }
}
