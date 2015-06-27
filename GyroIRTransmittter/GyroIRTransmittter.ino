// initialize the library with the numbers of the interface pins
// Used Pins
/*
0 : Reserved for Serial Rx
1 : Reserved for Serial Tx
2 : NC
3 : Vibration/jolt
4 : IR LED Gun Trigger
5 : Buzzer
6 : S/W serial Rx
7 : S/W serial Tx
8 : IR LED Signal
9 : NC
10 : Bluetooth serial Tx
11 : Bluetooth serial Rx
12 : NC
13 : NC

A0 : X-Axis Accelerometer
A1 : Y-Axis Accelerometer
A2 : Z-Axis Accelerometer
A3 : NC
A4 : NC
A5 : NC
*/

#include <SoftwareSerial.h>
#include <OneWire.h>

//Pin Definations
#define BluetoothTX 10
#define BluetoothRX 11
#define IRReciever A4
#define buzzer 5 
#define vibration 3
//#define trigger 4
//#define ledPin 8

//Variable Definations
int value;
int isPress = 0;
int flagGame = 0;
int flagHealth = 0;
int flagTemp = 0;
char ch;

//Bluetooth
SoftwareSerial bluetooth(BluetoothTX,BluetoothRX);

//OneWire
int DS18S20_Pin = 2; //DS18S20 Signal pin on digital 2
//Temperature chip i/o
OneWire ds(DS18S20_Pin);  // on digital pin 2
float Temperature=0.0;

//Accelerometer
#define PIN_ACCEL_X A0
#define PIN_ACCEL_Y A1
#define PIN_ACCEL_Z A2
#define DELAY_S 0.1
int xval, yval, zval;
int Status = 0;
int fallProbe = 0;
int fallCount = 0;

float getTemperature()
{
  //returns the temperature from one DS18S20 in DEG Celsius

  byte data[12];
  byte addr[8];

  if ( !ds.search(addr)) {
      //no more sensors on chain, reset search
      ds.reset_search();
      return -1000;
  }

  if ( OneWire::crc8( addr, 7) != addr[7]) {
      Serial.println("CRC is not valid!");
      return -1000;
  }

  if ( addr[0] != 0x10 && addr[0] != 0x28) {
      Serial.print("Device is not recognized");
      return -1000;
  }

  ds.reset();
  ds.select(addr);
  ds.write(0x44,1); // start conversion, with parasite power on at the end

  byte present = ds.reset();
  ds.select(addr);    
  ds.write(0xBE); // Read Scratchpad

  
  for (int i = 0; i < 9; i++) { // we need 9 bytes
    data[i] = ds.read();
  }
  
  ds.reset_search();
  
  byte MSB = data[1];
  byte LSB = data[0];

  float tempRead = ((MSB << 8) | LSB); //using two's compliment
  float TemperatureSum = tempRead / 16;
  
  return TemperatureSum;
  
}

void updateTemperature()
{
  int count=0;
  float sum=0;
  float tempValue;
            
  for(count=0; count < 10; )
  {
    tempValue = getTemperature();
    if (tempValue > 0 && tempValue < 170)
    {
      sum +=tempValue;
      count++;
    }
  }
  
  Temperature = sum/count+1.5;
}

//void sendIRPulse()
//{
//  //value = analogRead(IRReciever);
//  digitalWrite(ledPin, HIGH);
//  delayMicroseconds(13);
//  digitalWrite(ledPin, LOW);
//  delayMicroseconds(13);
//  Serial.println(value);
//}

void vibrateBuzz(int a)
{
  if(a < 600)
  {
    digitalWrite(buzzer, HIGH);
    digitalWrite(vibration, HIGH);
    delay(1000);
    digitalWrite(buzzer, LOW);
    digitalWrite(vibration, LOW);
    sendMessage("s");
  }
}

void fallDetected()
{
  //GPS location has to be sent from here
  digitalWrite(buzzer, HIGH);
  delay(500);
  digitalWrite(buzzer, LOW);
  sendMessage("f");
}

//Send a message back to the Windows Phone.
void sendMessage(String message) {
  //int messageLen = strlen(message);
  int messageLen = message.length();
   bluetooth.write(messageLen);
   bluetooth.print(message);
}

void getStatus()
{
  // Read Accelerometer values:
  xval = analogRead(PIN_ACCEL_X);
  yval = analogRead(PIN_ACCEL_Y);
  zval = analogRead(PIN_ACCEL_Z); 

  if( ((xval<453 || xval>604) && yval>475) || ((zval<350 || zval>610) && yval>480) )
  { 
    //delay(250);
    fallCount++; 
    Status = 1;
    if(fallCount >= 80)
    {
      fallCount = 0;
      fallProbe = 1;
    }
    else
    {
      fallProbe = 0;
    }
  }
  else
  {
    fallProbe = 0;
    fallCount = 0;   
    Status = 0;
  }
}

void setup()
{
//  pinMode(ledPin, OUTPUT);
//  pinMode(trigger, INPUT);
//  digitalWrite(trigger, LOW);
  pinMode(IRReciever, INPUT);
  digitalWrite(IRReciever, LOW);
  pinMode(buzzer,OUTPUT);
  pinMode(vibration,OUTPUT);
  pinMode(2, INPUT);
  Serial.begin(9600);
  bluetooth.begin(115200);
}

void loop()
{ 
//    updateTemperature();
//    Serial.println(Temperature);
  while(bluetooth.available())
  {
    ch = bluetooth.read();
    Serial.print(ch);
  }
  
  if(ch == 'g')
  {
    flagGame = 1;
    flagHealth = 0;
    flagTemp = 0;
  }
  else if(ch == 'h')
  {
     flagGame = 0;
     flagHealth = 1; 
     flagTemp = 0;
  }
  else if(ch == 't')
  {
     flagGame = 0;
     flagHealth = 0; 
     flagTemp = 1;
  } 
  
  if(flagGame == 1)
  {
    //  isPress = digitalRead(trigger);
    //  if(!isPress)
    //  {
    //    sendIRPulse();
    //  }
    value = analogRead(IRReciever);
    Serial.println(value);
    vibrateBuzz(value);
  }
  else if(flagHealth == 1)
  {
    fallProbe=0;
    getStatus();
    Serial.print(" X = ");
    Serial.print(xval);
    Serial.print(" Y = ");
    Serial.print(yval);
    Serial.print(" Z = ");
    Serial.println(zval);
    if(fallProbe==1)
    {
      fallDetected();
    }
  }
  else if(flagTemp == 1)
  {
    updateTemperature();
     Serial.println(Temperature);
     //char charBuf[4];
     //int tempINT = Temperature;
     String tempString = String(Temperature);
     //char* myString = tempString.toCharArray(charBuf, 4) 
     sendMessage(tempString);
     ch=';';
     flagTemp = 0;
  }   
}
