﻿using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MysteryLocation.View
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class TermsView : ContentPage
    {
        Boolean _isTapped = false;
        public TermsView()
        {
            InitializeComponent();
        }

        private async void agreeButton_Clicked(object sender, EventArgs e)
        {
            if (_isTapped)
                return;

            _isTapped = true;
            App.user.termsFlag = 1;
            await Navigation.PopModalAsync(true);

            _isTapped = false;
        }

       
    }
}