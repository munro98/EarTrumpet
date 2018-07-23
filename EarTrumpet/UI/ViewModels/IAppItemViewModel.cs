﻿using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows;
using System.Windows.Media;

namespace EarTrumpet.UI.ViewModels
{
    public interface IAppItemViewModel : INotifyPropertyChanged
    {
        string Id { get; }
        bool IsMuted { get; set; }
        int Volume { get; set; }
        SolidColorBrush Background { get; }
        ObservableCollection<IAppItemViewModel> ChildApps { get; }
        string DisplayName { get; }
        string ExeName { get; }
        string AppId { get; }
        ImageSource Icon { get; }
        char IconText { get; }
        bool IsExpanded { get; }
        bool IsMovable { get; }
        float PeakValue1 { get; }
        float PeakValue2 { get; }
        string PersistedOutputDevice { get; }
        int ProcessId { get; }

        bool DoesGroupWith(IAppItemViewModel app);
        void MoveToDevice(string id, bool hide);
        void RefreshDisplayName();
        void UpdatePeakValueForeground();
        void UpdatePeakValueBackground();
        void OpenPopup(UIElement uIElement);
    }
}