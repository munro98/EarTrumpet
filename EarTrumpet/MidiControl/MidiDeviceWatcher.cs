﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Windows.Devices.Midi;
using Windows.Devices.Enumeration;
using Windows.UI.Core;
using Windows.UI.Xaml.Controls;
using System.Windows.Threading;
//using Microsoft.CompilerServices.AsyncTargetingPack;

namespace EarTrumpet.MidiControls
{
    /// <summary>
    /// DeviceWatcher class to monitor adding/removing MIDI devices on the fly
    /// </summary>
    internal class MidiDeviceWatcher
    {
        internal DeviceWatcher deviceWatcher = null;
        internal DeviceInformationCollection deviceInformationCollection = null;
        bool enumerationCompleted = false;
        //ListBox portList = null;
        string midiSelector = string.Empty;
        Dispatcher coreDispatcher = Dispatcher.CurrentDispatcher;

        MidiControl midiControl;

        /// <summary>
        /// Constructor: Initialize and hook up Device Watcher events
        /// </summary>
        /// <param name="midiSelectorString">MIDI Device Selector</param>
        /// <param name="dispatcher">CoreDispatcher instance, to update UI thread</param>
        /// <param name="portListBox">The UI element to update with list of devices</param>
        internal MidiDeviceWatcher(string midiSelectorString, MidiControl midiControl)
        {
            this.midiControl = midiControl;
            this.deviceWatcher = DeviceInformation.CreateWatcher(midiSelectorString);
            //this.portList = portListBox;
            this.midiSelector = midiSelectorString;
            //this.coreDispatcher = Disp;

            this.deviceWatcher.Added += DeviceWatcher_Added;
            this.deviceWatcher.Removed += DeviceWatcher_Removed;
            this.deviceWatcher.Updated += DeviceWatcher_Updated;
            this.deviceWatcher.EnumerationCompleted += DeviceWatcher_EnumerationCompleted;
        }

        /// <summary>
        /// Destructor: Remove Device Watcher events
        /// </summary>
        ~MidiDeviceWatcher()
        {
            this.deviceWatcher.Added -= DeviceWatcher_Added;
            this.deviceWatcher.Removed -= DeviceWatcher_Removed;
            this.deviceWatcher.Updated -= DeviceWatcher_Updated;
            this.deviceWatcher.EnumerationCompleted -= DeviceWatcher_EnumerationCompleted;
        }

        /// <summary>
        /// Start the Device Watcher
        /// </summary>
        internal void Start()
        {
            if (this.deviceWatcher.Status != DeviceWatcherStatus.Started)
            {
                this.deviceWatcher.Start();
            }
        }

        /// <summary>
        /// Stop the Device Watcher
        /// </summary>
        internal void Stop()
        {
            if (this.deviceWatcher.Status != DeviceWatcherStatus.Stopped)
            {
                this.deviceWatcher.Stop();
            }
        }

        /// <summary>
        /// Get the DeviceInformationCollection
        /// </summary>
        /// <returns></returns>
        internal DeviceInformationCollection GetDeviceInformationCollection()
        {
            return this.deviceInformationCollection;
        }

        /// <summary>
        /// Add any connected MIDI devices to the list
        /// </summary>
        private async void UpdateDevices()
        {
            // Get a list of all MIDI devices
            this.deviceInformationCollection = await DeviceInformation.FindAllAsync(this.midiSelector); // https://stackoverflow.com/questions/44099401/frombluetoothaddressasync-iasyncoperation-does-not-contain-a-definition-for-get


            int i = 0;


            // If no devices are found, update the ListBox
            if ((this.deviceInformationCollection == null) || (this.deviceInformationCollection.Count == 0))
            {
                // Start with a clean list
                //this.portList.Items.Clear();

                //this.portList.Items.Add("No MIDI ports found");
                //this.portList.IsEnabled = false;
            }
            // If devices are found, enumerate them and add them to the list
            else
            {
                // Start with a clean list
                //this.portList.Items.Clear();

                foreach (var device in deviceInformationCollection)
                {
                    //this.portList.Items.Add(device.Name);
                    if (device.Name.IndexOf("Teensy MIDI") != -1)
                    {

                        //selectMidiDevice(device);
                        i = 10;
                        this.midiControl.selectMidiDevice(device);
                    }
                    
                }

                //this.portList.IsEnabled = true;

                

            }

            Console.Error.Write("" + i);
        }

        /// <summary>
        /// Update UI on device added
        /// </summary>
        /// <param name="sender">The active DeviceWatcher instance</param>
        /// <param name="args">Event arguments</param>
        private async void DeviceWatcher_Added(DeviceWatcher sender, DeviceInformation args)
        {
            // If all devices have been enumerated
            if (this.enumerationCompleted)
            {
                /*
                await coreDispatcher.RunAsync(CoreDispatcherPriority.High, () =>
                {
                    // Update the device list
                    UpdateDevices();
                });
                */
                await coreDispatcher.BeginInvoke((Action)(() =>
                 {
                     // Update the device list
                     UpdateDevices();
                 }));
            }
        }

        /// <summary>
        /// Update UI on device removed
        /// </summary>
        /// <param name="sender">The active DeviceWatcher instance</param>
        /// <param name="args">Event arguments</param>
        private async void DeviceWatcher_Removed(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            // If all devices have been enumerated
            if (this.enumerationCompleted)
            {
                await coreDispatcher.BeginInvoke((Action)(() =>
                {
                    // Update the device list
                    UpdateDevices();
                }));
            }
        }

        /// <summary>
        /// Update UI on device updated
        /// </summary>
        /// <param name="sender">The active DeviceWatcher instance</param>
        /// <param name="args">Event arguments</param>
        private async void DeviceWatcher_Updated(DeviceWatcher sender, DeviceInformationUpdate args)
        {
            // If all devices have been enumerated
            if (this.enumerationCompleted)
            {
                await coreDispatcher.BeginInvoke((Action)(() =>
                {
                    // Update the device list
                    UpdateDevices();
                }));
            }
        }

        /// <summary>
        /// Update UI on device enumeration completed.
        /// </summary>
        /// <param name="sender">The active DeviceWatcher instance</param>
        /// <param name="args">Event arguments</param>
        private async void DeviceWatcher_EnumerationCompleted(DeviceWatcher sender, object args)
        {
            this.enumerationCompleted = true;
            await coreDispatcher.BeginInvoke((Action)(() =>
            {
                // Update the device list
                UpdateDevices();
            }));
        }
    }
}
