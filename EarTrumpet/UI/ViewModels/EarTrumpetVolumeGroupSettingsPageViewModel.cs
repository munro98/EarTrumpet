using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


using EarTrumpet.UI.Views;

namespace EarTrumpet.UI.ViewModels
{
    public class EarTrumpetVolumeGroupSettingsPageViewModel : SettingsPageViewModel
    {
        public bool UseLegacyIcon
        {
            get => _settings.UseLegacyIcon;
            set => _settings.UseLegacyIcon = value;
        }

        private readonly AppSettings _settings;

        public EarTrumpetVolumeGroupSettingsPageViewModel(AppSettings settings) : base(null)
        {
            _settings = settings;
            Title = "Volume Groups";
            Glyph = "\xE825";



            //SettingsWindow.


        }
    }
}