


///// Designed for the teensy 2.0 micro computer

#include <Bounce.h>  // Bounce library makes button change detection easy

int ledPin = 11;
#define EP 2
const int channel = 1;

Bounce button1 = Bounce(1, 5);  // 5 = 5 ms debounce time
Bounce button2 = Bounce(2, 5);  // which is appropriate for good
Bounce button3 = Bounce(3, 5);  // quality mechanical pushbuttons
Bounce button4 = Bounce(4, 5);
void setup() {
  pinMode(1, INPUT_PULLUP);
  pinMode(2, INPUT_PULLUP);
  pinMode(3, INPUT_PULLUP);
  pinMode(4, INPUT_PULLUP);

  // put your setup code here, to run once:
  pinMode(ledPin, OUTPUT);
  Serial.begin(38400);
}

// Our volume knobs
int knob0;
int knob0_prev;
int knob1;
int knob1_prev;
int knob2;
int knob2_prev;
int knob3;
int knob3_prev;
int knob4;
int knob4_prev;

// Our button controls
int butt7;
int butt8;
int butt9;
int butt10;

int butt7_prev;
int butt8_prev;
int butt9_prev;
int butt10_prev;


void loop() {

  button1.update();
  button2.update();
  button3.update();
  button4.update();

  butt7 = digitalRead(7);
  butt8 = digitalRead(8);
  butt9 = digitalRead(9);
  butt10 = digitalRead(10);

  if (butt7 != butt7_prev) { // play/pause
    if (butt7) {
      usbMIDI.sendNoteOn(64, 255, channel);
    } 
    //else {
    //  usbMIDI.sendNoteOff(64, 0, channel);
    //}
  }

  if (butt8 != butt8_prev) { // prev
    if (butt8) {
      usbMIDI.sendNoteOn(65, 255, channel);
    } 
    //else {
    //  usbMIDI.sendNoteOff(65, 0, channel);
    //}
  }

  if (butt9 != butt9_prev) { // next
    if (butt9) {
      usbMIDI.sendNoteOn(66, 255, channel);
    } 
    //else {
    //  usbMIDI.sendNoteOff(66, 0, channel);
    //}
  }

  if (butt10 != butt10_prev) { // mark current app
    if (butt10) {
      usbMIDI.sendNoteOn(67, 255, channel);
    } 
    //else {
    //  usbMIDI.sendNoteOff(67, 0, channel);
    //}
  }

  butt7 = butt7_prev;
  butt8 = butt8_prev;
  butt9 = butt9_prev;
  butt10 = butt10_prev;
  
  
  knob0 = analogRead(0);
  knob0 = map(knob0, 0, 1022, 0, 127);
  
  knob1 = analogRead(1);
  knob1 = map(knob1, 0, 1022, 0, 127);
  
  knob2 = analogRead(2);
  knob2 = map(knob2, 0, 1022, 0, 127);
  
  knob3 = analogRead(3);
  knob3 = map(knob3, 0, 1022, 0, 127);
  
  if (abs(knob0 - knob0_prev) > EP) { // master
    Serial.print("analog 0 is: ");
    Serial.println(knob0);
    knob0_prev =  knob0;
    usbMIDI.sendNoteOn(60, knob0, channel); 
  }

  if (abs(knob1 - knob1_prev) > EP) { // music
    //Serial.print("analog 1 is: ");
    //Serial.println(knob1);
    knob1_prev =  knob1;
    usbMIDI.sendNoteOn(61, knob1, channel);
  }

  if (abs(knob2 - knob2_prev) > EP) { // browser
    //Serial.print("analog 2 is: ");
    //Serial.println(knob2);
    knob2_prev =  knob2;
    //usbMIDI.sendNoteOn(62, knob2, channel);
  }

  if (abs(knob3 - knob3_prev) > EP) { // game
    //Serial.print("analog 3 is: ");
    //Serial.println(knob3);
    knob3_prev =  knob3;
    //usbMIDI.sendNoteOn(63, knob3, channel);
  }

  // MIDI Controllers should discard incoming MIDI messages.
  while (usbMIDI.read()) {
  }
  
  delay(100);
}
