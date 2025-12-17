/*
  AnalogReadSerial
  Reads a potentiometer on analog input A0.
  Open the Serial Monitor to see the values (set baud rate to 9600).
*/
int lastPitchValue;
int lastSpeedValue;

void setup() {
  // Initialize serial communication at 9600 bits per second:
  Serial.begin(9600);
}

void loop() {
  // Read the value from the potentiometer on analog pin A0:
  int pitchValue = analogRead(A1);
  int speedValue = analogRead(A0);

  // Print the value to the Serial Monitor:
  if (lastPitchValue != pitchValue || lastPitchValue < pitchValue - 1 || lastPitchValue > pitchValue + 1)
    {Serial.println("Pitch: " + String(pitchValue));}
  if(lastSpeedValue != speedValue)
    {Serial.println("Speed: " + String(speedValue));}
  
  // Add a small delay for stable readings
  lastPitchValue = pitchValue;
  lastSpeedValue = speedValue;
  
  delay(10);
}