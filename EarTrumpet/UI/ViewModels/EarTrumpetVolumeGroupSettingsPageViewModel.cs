using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Navigation;


using EarTrumpet.UI.Views;
using System.Collections.ObjectModel;

namespace EarTrumpet.UI.ViewModels
{
    public class EarTrumpetVolumeGroupSettingsPageViewModel : SettingsPageViewModel
    {
        public bool UseLegacyIcon
        {
            get => _settings.UseLegacyIcon;
            set => _settings.UseLegacyIcon = value;
        }

        public ObservableCollection<String> Apps{ get; set; }
        = new ObservableCollection<String>();

        /*
        public ObservableCollection<String> Apps { get {
           ObservableCollection<String> newApps = new ObservableCollection<String>();

            foreach (var app : collectionViewModel.Default.Apps) {
                if (!browser.contains(app) && !music.contains(app) && !game.contains(app))
                    Apps.Add(app.DisplayName);
            }

            returns newApps;

        } set{
        
        } }
        = new ObservableCollection<String>();

        // music
        public ObservableCollection<String> musicApps { get {
           ObservableCollection<String> newApps = new ObservableCollection<String>();

            foreach (var app : collectionViewModel.Default.Apps) {
                if (music.contains(app))
                    Apps.Add(app.DisplayName);
            }
            returns newApps;
        } set{
        } }
        = new ObservableCollection<String>();

        //browser
        public ObservableCollection<String> browserApps { get {
           ObservableCollection<String> newApps = new ObservableCollection<String>();

            foreach (var app : collectionViewModel.Default.Apps) {
                if (browserc.contains(app))
                    Apps.Add(app.DisplayName);
            }
            returns newApps;
        } set{
        } }
        = new ObservableCollection<String>();

        // Game
        public ObservableCollection<String> MusicApps { get {
           ObservableCollection<String> newApps = new ObservableCollection<String>();

            foreach (var app : collectionViewModel.Default.Apps) {
                if (game.contains(app))
                    Apps.Add(app.DisplayName);
            }
            returns newApps;
        } set{
        } }
        = new ObservableCollection<String>();

        */


        public String _SelectedApps;
        public String SelectedApps { get { return _SelectedApps; } 
            set
            {
                if (_SelectedApps == null || !_SelectedApps.Equals(value) )
                {
                    _SelectedApps = value;
                    Apps.Add("coolstuff");
                }
                //
            }
        }

        private readonly AppSettings _settings;
        private DeviceCollectionViewModel _collectionViewModel;

        public EarTrumpetVolumeGroupSettingsPageViewModel(AppSettings settings, DeviceCollectionViewModel collectionViewModel) : base(null)
        {
            _settings = settings;
            _collectionViewModel = collectionViewModel;


            Title = "Volume Groups";
            Glyph = "\xE825";

            foreach ( var dev in collectionViewModel.Default.Apps)
            {
                Apps.Add(dev.DisplayName);
            }

            //Apps.Add("coolstuff");
            //Apps.Add("coolstuff");
            //Apps.Add("coolstuffsasda");

            //SettingsWindow.


        }



        private void ListView1_SelectionChanged(object sender, SelectionChangedEventArgs e) //SelectionChanged="{Binding EarTrumpet.UI.ViewModels.EarTrumpetVolumeGroupSettingsPageViewModel.ListView1_SelectionChanged(SelectedItem)}"
        {
            Apps.Add("coolstuff");
            /*
            if (listView1.SelectedItem != null)
            {
                selectedItem.Text =
                    "Selected item: " + listView1.SelectedItem.ToString();
            }
            else
            {
                selectedItem.Text =
                    "Selected item: null";
            }
            selectedIndex.Text =
                "Selected index: " + listView1.SelectedIndex.ToString();
            selectedItemCount.Text =
                "Items selected: " + listView1.SelectedItems.Count.ToString();
            addedItems.Text =
                "Added: " + e.AddedItems.Count.ToString();
            removedItems.Text =
                "Removed: " + e.RemovedItems.Count.ToString();
            */
        }


    }
}