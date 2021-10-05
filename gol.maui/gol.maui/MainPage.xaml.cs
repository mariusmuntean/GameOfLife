﻿using gol.maui.ViewModels;
using Microsoft.Maui.Controls;

namespace gol.maui
{
    public partial class MainPage : ContentPage
    {
        public MainPage(MainPageViewModel mainPageViewModel)
        {
            InitializeComponent();

            BindingContext = mainPageViewModel;
        }
    }
}
