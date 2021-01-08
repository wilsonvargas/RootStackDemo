using RootStack.Demo.Services;
using RootStack.Demo.Services.Interfaces;
using RootStack.Demo.Views;
using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace RootStack.Demo
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            MainPage = new MainPage();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}
