using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Devices.Enumeration;
using Windows.Devices.Midi;
using System.Diagnostics;

using EarTrumpet.UI.ViewModels;

using System.Windows.Threading;

namespace EarTrumpet.MidiControls

{
    class MidiControl
    {
        private DeviceCollectionViewModel _mainViewModel;

        MidiDeviceWatcher midiInDeviceWatcher;
        private List<MidiInPort> midiInPorts;


        public MidiControl(DeviceCollectionViewModel mainViewModel)
        {
            _mainViewModel = mainViewModel;

            // Initialize the list of active MIDI input devices
            this.midiInPorts = new List<MidiInPort>();

            // Set up the MIDI input device watcher
            this.midiInDeviceWatcher = new MidiDeviceWatcher(MidiInPort.GetDeviceSelector(), this);

            // Start watching for devices
            this.midiInDeviceWatcher.Start();




        }

        public async void selectMidiDevice(DeviceInformation devInfo)
        {
            var currentMidiInputDevice = await MidiInPort.FromIdAsync(devInfo.Id);
            if (currentMidiInputDevice == null)
            {
                //this.rootPage.NotifyUser("Unable to create MidiInPort from input device", NotifyType.ErrorMessage);
                return;
            }

            // We have successfully created a MidiInPort; add the device to the list of active devices, and set up message receiving
            if (!this.midiInPorts.Contains(currentMidiInputDevice))
            {
                this.midiInPorts.Add(currentMidiInputDevice);
                currentMidiInputDevice.MessageReceived += MidiInputDevice_MessageReceived;
            }

        }

        private void noteOn(IMidiMessage msg)
        {

            var onMsg = (MidiNoteOnMessage)msg;
            int vol = (int)((float)onMsg.Velocity * 100.0f / 127.0);
            Trace.WriteLine($"MidiControl noteOn {onMsg.Velocity}");

            if (onMsg.Note == 60 && _mainViewModel.Default != null)
            {
                _mainViewModel.Default.Volume = vol;
            }


            if (onMsg.Note == 64)
            {
                
            }

            if (onMsg.Note == 65)
            {

            }

            if (onMsg.Note == 66)
            {

            }

            if (onMsg.Note == 67)
            {

            }

        }


        private void noteOff(IMidiMessage msg)
        {

        }

        private async void MidiInputDevice_MessageReceived(MidiInPort sender, MidiMessageReceivedEventArgs args)
        {
            ///*
            IMidiMessage receivedMidiMessage = args.Message;

            // Build the received MIDI message into a readable format
            StringBuilder outputMessage = new StringBuilder();
            outputMessage.Append(receivedMidiMessage.Timestamp.ToString()).Append(", Type: ").Append(receivedMidiMessage.Type);

            // Add MIDI message parameters to the output, depending on the type of message
            switch (receivedMidiMessage.Type)
            {
                case MidiMessageType.NoteOff:
                    var noteOffMessage = (MidiNoteOffMessage)receivedMidiMessage;
                    outputMessage.Append(", Channel: ").Append(noteOffMessage.Channel).Append(", Note: ").Append(noteOffMessage.Note).Append(", Velocity: ").Append(noteOffMessage.Velocity);

                    noteOff(receivedMidiMessage);

                    break;
                case MidiMessageType.NoteOn:
                    var noteOnMessage = (MidiNoteOnMessage)receivedMidiMessage;
                    outputMessage.Append(", Channel: ").Append(noteOnMessage.Channel).Append(", Note: ").Append(noteOnMessage.Note).Append(", Velocity: ").Append(noteOnMessage.Velocity);

                    noteOn(receivedMidiMessage);

                    break;
                case MidiMessageType.PolyphonicKeyPressure:
                    var polyphonicKeyPressureMessage = (MidiPolyphonicKeyPressureMessage)receivedMidiMessage;
                    outputMessage.Append(", Channel: ").Append(polyphonicKeyPressureMessage.Channel).Append(", Note: ").Append(polyphonicKeyPressureMessage.Note).Append(", Pressure: ").Append(polyphonicKeyPressureMessage.Pressure);
                    break;
                case MidiMessageType.ControlChange:
                    var controlChangeMessage = (MidiControlChangeMessage)receivedMidiMessage;
                    outputMessage.Append(", Channel: ").Append(controlChangeMessage.Channel).Append(", Controller: ").Append(controlChangeMessage.Controller).Append(", Value: ").Append(controlChangeMessage.ControlValue);
                    break;
                case MidiMessageType.ProgramChange:
                    var programChangeMessage = (MidiProgramChangeMessage)receivedMidiMessage;
                    outputMessage.Append(", Channel: ").Append(programChangeMessage.Channel).Append(", Program: ").Append(programChangeMessage.Program);
                    break;
                case MidiMessageType.ChannelPressure:
                    var channelPressureMessage = (MidiChannelPressureMessage)receivedMidiMessage;
                    outputMessage.Append(", Channel: ").Append(channelPressureMessage.Channel).Append(", Pressure: ").Append(channelPressureMessage.Pressure);
                    break;
                case MidiMessageType.PitchBendChange:
                    var pitchBendChangeMessage = (MidiPitchBendChangeMessage)receivedMidiMessage;
                    outputMessage.Append(", Channel: ").Append(pitchBendChangeMessage.Channel).Append(", Bend: ").Append(pitchBendChangeMessage.Bend);
                    break;
                case MidiMessageType.SystemExclusive:
                    break;
                case MidiMessageType.MidiTimeCode:
                    var timeCodeMessage = (MidiTimeCodeMessage)receivedMidiMessage;
                    outputMessage.Append(", FrameType: ").Append(timeCodeMessage.FrameType).Append(", Values: ").Append(timeCodeMessage.Values);
                    break;
                case MidiMessageType.SongPositionPointer:
                    var songPositionPointerMessage = (MidiSongPositionPointerMessage)receivedMidiMessage;
                    outputMessage.Append(", Beats: ").Append(songPositionPointerMessage.Beats);
                    break;
                case MidiMessageType.SongSelect:
                    var songSelectMessage = (MidiSongSelectMessage)receivedMidiMessage;
                    outputMessage.Append(", Song: ").Append(songSelectMessage.Song);
                    break;
                case MidiMessageType.TuneRequest:
                    var tuneRequestMessage = (MidiTuneRequestMessage)receivedMidiMessage;
                    break;
                case MidiMessageType.TimingClock:
                    var timingClockMessage = (MidiTimingClockMessage)receivedMidiMessage;
                    break;
                case MidiMessageType.Start:
                    var startMessage = (MidiStartMessage)receivedMidiMessage;
                    break;
                case MidiMessageType.Continue:
                    var continueMessage = (MidiContinueMessage)receivedMidiMessage;
                    break;
                case MidiMessageType.Stop:
                    var stopMessage = (MidiStopMessage)receivedMidiMessage;
                    break;
                case MidiMessageType.ActiveSensing:
                    var activeSensingMessage = (MidiActiveSensingMessage)receivedMidiMessage;
                    break;
                case MidiMessageType.SystemReset:
                    var systemResetMessage = (MidiSystemResetMessage)receivedMidiMessage;
                    break;
                case MidiMessageType.None:
                    throw new InvalidOperationException();
                default:
                    break;
            }
            //*/

            await Dispatcher.CurrentDispatcher.BeginInvoke((Action)(() =>
            {
                if ((receivedMidiMessage.Type != MidiMessageType.TimingClock) && (receivedMidiMessage.Type != MidiMessageType.ActiveSensing))
                {
                    //this.inputDeviceMessages.Items.Add(outputMessage + "\n");
                    //this.inputDeviceMessages.ScrollIntoView(this.inputDeviceMessages.Items[this.inputDeviceMessages.Items.Count - 1]);
                    //this.rootPage.NotifyUser("Message received successfully!", NotifyType.StatusMessage);
                }
            }));

            /*
            // Use the Dispatcher to update the messages on the UI thread
            await Dispatcher.RunAsync(CoreDispatcherPriority.High, () =>
            {
                // Skip TimingClock and ActiveSensing messages to avoid overcrowding the list. Commment this check out to see all messages
                if ((receivedMidiMessage.Type != MidiMessageType.TimingClock) && (receivedMidiMessage.Type != MidiMessageType.ActiveSensing))
                {
                    //this.inputDeviceMessages.Items.Add(outputMessage + "\n");
                    //this.inputDeviceMessages.ScrollIntoView(this.inputDeviceMessages.Items[this.inputDeviceMessages.Items.Count - 1]);
                    //this.rootPage.NotifyUser("Message received successfully!", NotifyType.StatusMessage);
                }
            });
            */

        }



    }
}
