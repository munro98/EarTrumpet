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
using EarTrumpet.SendInput;

namespace EarTrumpet.MidiControls

/*
 TODO

    implement play/pause prev next buttons using virtual key input
    
    finish creating volume groups in settings panel
    persist the settings in volume groups

 */


{
    class MidiControl
    {
        private DeviceCollectionViewModel _mainViewModel;

        MidiDeviceWatcher midiInDeviceWatcher;
        private List<MidiInPort> midiInPorts;

        private HashSet<string> browserApps = new HashSet<string>()
                    {
                        "Google Chrome",
                        "Mozilla FireFox",
                        "Microsoft Edge",
                    };

        private HashSet<string> musicApps = new HashSet<string>()
                    {
                        "Spotify",
                        "AIMP",
                        "VLC media player",
                    };

        private HashSet<string> gameApps = new HashSet<string>()
                    {
                        "Counter Strike",
                        "",
                    };


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

            if (onMsg.Note == 60 && _mainViewModel.Default != null) // master
            {
                _mainViewModel.Default.Volume = vol;

            }
            //return;
            if (onMsg.Note == 61 && _mainViewModel.Default != null) // music
            {
                foreach (var app in _mainViewModel.Default.Apps)
                {
                    if (musicApps.Contains(app.DisplayName))
                    {
                        app.Volume = vol;
                    }
                }

            }
            if (onMsg.Note == 62 && _mainViewModel.Default != null) // browser apps
            {
                foreach (var app in _mainViewModel.Default.Apps)
                {
                    if (browserApps.Contains(app.DisplayName))
                    {
                        app.Volume = vol;
                    }
                }
            }
            if (onMsg.Note == 63 && _mainViewModel.Default != null) // games
            {
                foreach (var app in _mainViewModel.Default.Apps)
                {
                    if (gameApps.Contains(app.DisplayName))
                    {
                        app.Volume = vol;
                    }
                }
            }


            if (onMsg.Note == 64) // play/pause
            {

                //VK_MEDIA_PLAY_PAUSE 0xB3
                InputSender.SendKeyboardInput(new InputSender.KeyboardInput[]
                {
                    new InputSender.KeyboardInput
                    {
                        wScan = 0x0,
                        wVk = 0xB3,
                        dwFlags = (uint)(InputSender.KeyEventF.KeyDown),
                    },
                    new InputSender.KeyboardInput
                    {
                        wScan = 0x0,
                        wVk = 0xB3,
                        dwFlags = (uint)(InputSender.KeyEventF.KeyUp),
                    }
                });
                
            }

            if (onMsg.Note == 65) // prev
            {
                //VK_MEDIA_PREV_TRACK
                //0xB1
                
            }

            if (onMsg.Note == 66) // next
            {
                //VK_MEDIA_NEXT_TRACK
                //0xB0

            }

            /*
            if (onMsg.Note == 67) // assign application to volume group?
            {
            }
            */


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

        }



    }
}
