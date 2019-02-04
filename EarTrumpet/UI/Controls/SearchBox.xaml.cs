﻿using EarTrumpet.UI.ViewModels;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace EarTrumpet.UI.Controls
{
    public partial class SearchBox : TextBox
    {
        public object ItemsSource
        {
            get { return (object)this.GetValue(ItemsSourceProperty); }
            set { this.SetValue(ItemsSourceProperty, value); }
        }
        public static readonly DependencyProperty ItemsSourceProperty = DependencyProperty.Register(
          "ItemsSource", typeof(object), typeof(SearchBox), new PropertyMetadata(null));


        public SearchBox()
        {
            InitializeComponent();
            TextChanged += SearchBox_TextChanged;

            GotKeyboardFocus += SearchBox_GotKeyboardFocus;
            GotFocus += SearchBox_GotFocus;
            PreviewKeyDown += SearchBox_PreviewKeyDown;
        }

        private void SearchBox_GotFocus(object sender, RoutedEventArgs e)
        {
            MaybeShowSearchPopup();
        }

        private void SearchBox_PreviewKeyDown(object sender, System.Windows.Input.KeyEventArgs e)
        {
            if (e.Key == System.Windows.Input.Key.Down)
            {
                var popup = ((Popup)GetTemplateChild("Popup"));
                if (popup.IsOpen)
                {
                    popup.Child.Focus();
                }
            }
        }

        private void SearchBox_GotKeyboardFocus(object sender, System.Windows.Input.KeyboardFocusChangedEventArgs e)
        {
            MaybeShowSearchPopup();
        }

        private void SearchBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            ((TextBlock)GetTemplateChild("PromptText")).Visibility = Text.Length > 0 ? System.Windows.Visibility.Collapsed : System.Windows.Visibility.Visible;


            MaybeShowSearchPopup();
        }

        private void MaybeShowSearchPopup()
        {
            if (Text.Length > 0)
            {
                var popup = ((Popup)GetTemplateChild("Popup"));
                popup.PlacementTarget = this;
                popup.Width = ActualWidth;
                popup.DataContext = new SettingsSearchBoxResultsViewModel((SettingsViewModel)ItemsSource, Text);

                popup.UpdateLayout();

                popup.IsOpen = true;
                popup.Focus();
            }
        }
    }
}
