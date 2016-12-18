/**
 * HC-SR04 Demo
 * Demonstration of the HC-SR04 Ultrasonic Sensor
 * Date: August 3, 2016
 * 
 * Description:
 *  Connect the ultrasonic sensor to the Arduino as per the
 *  hardware connections below. Run the sketch and open a serial
 *  monitor. The distance read from the sensor will be displayed
 *  in centimeters and inches.
 * 
 * Hardware Connections:
 *  Arduino | HC-SR04 
 *  -------------------
 *    5V    |   VCC     
 *    7     |   Trig     
 *    8     |   Echo     
 *    GND   |   GND
 *  
 * License:
 *  Public Domain
 */

#include <NewPing.h>

#define SLAVE_ADDRESS 0x40
#include <Wire.h>

// Pins
const int TRIG_PIN = 7;
const int ECHO_PIN = 8;
//const int OUT_PIN = 11;

// Anything over 400 cm (23200 us pulse) is "out of range"
const unsigned int MAX_DIST = 23200;

void setup() {

  // The Trigger pin will tell the sensor to range find
  pinMode(TRIG_PIN, OUTPUT);
  digitalWrite(TRIG_PIN, LOW);
  pinMode(LED_BUILTIN, OUTPUT);
  //pinMode(OUT_PIN, OUTPUT);

  Wire.begin(SLAVE_ADDRESS);

  // We'll use the serial monitor to view the sensor output
  Serial.begin(9600);
}

void loop() {

  unsigned long t1;
  unsigned long t2;
  unsigned long pulse_width;
  float cm;

  // Hold the trigger pin high for at least 10 us
  digitalWrite(TRIG_PIN, HIGH);
  delayMicroseconds(10);
  digitalWrite(TRIG_PIN, LOW);

  // Wait for pulse on echo pin
  while ( digitalRead(ECHO_PIN) == 0 );

  // Measure how long the echo pin was held high (pulse width)
  // Note: the micros() counter will overflow after ~70 min
  t1 = micros();
  while ( digitalRead(ECHO_PIN) == 1);
  t2 = micros();
  pulse_width = t2 - t1;

  // Calculate distance in centimeters and inches. The constants
  // are found in the datasheet, and calculated from the assumed speed 
  //of sound in air at sea level (~340 m/s).
  cm = pulse_width / 58.0;

  // Print out results
  if ( pulse_width > MAX_DIST ) {
    
    Serial.println("Out of range");
    digitalWrite(LED_BUILTIN, LOW);
    //digitalWrite(OUT_PIN, LOW);
    Wire.onRequest(send0);
    
  } else {

    if(cm < 30) {
      digitalWrite(LED_BUILTIN, HIGH);
      //digitalWrite(OUT_PIN, HIGH);
      Wire.onRequest(send1);
    }
    else {
      digitalWrite(LED_BUILTIN, LOW);
      //digitalWrite(OUT_PIN, LOW);
      Wire.onRequest(send0);
    }
    
    Serial.print(cm);
    Serial.print(" cm \t");
    Serial.print("\n");
  }
  
  // Wait at least 60ms before next measurement
  delay(100);
}
void send1(){
  
      Wire.write('1');
}

void send0(){
  
      Wire.write('0');
}
