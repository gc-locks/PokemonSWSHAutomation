#include <SPI.h>
#include <Ethernet.h>
#include "config.h"
#include "util.h"

EthernetServer server(80); // http

void setup() {
  // You can use Ethernet.init(pin) to configure the CS pin
  //Ethernet.init(10);  // Most Arduino shields
  //Ethernet.init(5);   // MKR ETH shield
  //Ethernet.init(0);   // Teensy 2.0
  //Ethernet.init(20);  // Teensy++ 2.0
  //Ethernet.init(15);  // ESP8266 with Adafruit Featherwing Ethernet
  //Ethernet.init(33);  // ESP32 with Adafruit Featherwing Ethernet

  // start the Ethernet connection and the server:
  Ethernet.begin(mac, ip);

  // Check for Ethernet hardware present
  while (Ethernet.hardwareStatus() == EthernetNoHardware || Ethernet.linkStatus() == LinkOFF) {
    delay(100); // do nothing, no point running without Ethernet hardware
  }

  // start the server
  server.begin();
}


void loop() {
  // listen for incoming clients
  EthernetClient client = server.available();
  if (client) {
    while (client.connected()) {
      if (client.available()) {
        uint8_t input = client.read();
        if (input & 0x80) {
          // 制御信号
          if (input & 0x40) {
            // ボタン入力
            if (input & 0x20) {
              // HAT
              inputHat(input & 0x0f);
            } else {
              // ボタン
              inputButton(input & 0x0f, (input & 0x10) >> 4);
            }
          } else if (client.available() >= 2) {
            // スティック入力
            uint8_t inputs[2];
            for (int i = 0; i < 2; i++) {
              inputs[i] = client.read();
            }
            inputStick((input & 0x10) >> 4, inputs[0], inputs[1]);
          }
        }
      }
    }
    client.stop();
    // give the web browser time to receive the data
    delay(1);
  }
}
