using RootStack.Demo.Services.Interfaces;
using RootStack.Demo.Views;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq.Expressions;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.Forms;

namespace RootStack.Demo.ViewModels
{
    public class BaseViewModel : INotifyPropertyChanged
    {
        protected async void ShowView(Page page)
        {
            var mainPage = Application.Current.MainPage;
            await mainPage.Navigation.PushAsync(page);
        }

        public virtual void ViewAppearing() { }

        protected void GoHome()
        {
            Application.Current.MainPage = new NavigationPage(new MainPage());
        }

        private bool isBusy;

        public bool IsBusy
        {
            get { return isBusy; }
            set { SetPropertyValue(ref isBusy, value); }
        }


        public event PropertyChangedEventHandler PropertyChanged;

        protected void RaiseAllPropertiesChanged()
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(string.Empty));
        }

        protected void RaisePropertyChanged<T>(Expression<Func<T>> propExpr)
        {
            PropertyInfo prop = (PropertyInfo)((MemberExpression)propExpr.Body).Member;
            RaisePropertyChanged(prop.Name);
        }

        protected void RaisePropertyChanged([CallerMemberName] string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        protected bool SetPropertyValue<T>(ref T storageField, T newValue, Expression<Func<T>> propExpr)
        {
            if (Equals(storageField, newValue))
                return false;

            storageField = newValue;
            PropertyInfo prop = (PropertyInfo)((MemberExpression)propExpr.Body).Member;
            RaisePropertyChanged(prop.Name);

            return true;
        }
        protected bool SetPropertyValue<T>(ref T storageField, T newValue, [CallerMemberName] string propertyName = "")
        {
            if (Equals(storageField, newValue))
                return false;

            storageField = newValue;
            RaisePropertyChanged(propertyName);
            return true;
        }
    }
}
