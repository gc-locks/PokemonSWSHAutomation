#include <SoftwareSerial.h>
#include <SwitchControlLibrary.h>

enum {
  RSTICK = 0,
  LSTICK,
};

enum {
  A = 0,
  B,
  X,
  Y,
  R,
  L,
  ZR,
  ZL,
  RCLICK,
  LCLICK,
  HOME,
  CAPTURE,
  PLUS,
  MINUS
};

enum {
  RELEASE = 0,
  PRESS
};

SoftwareSerial mySerial(11, 10); // RX, TX

void setup() {
  mySerial.begin(115200);
}

void inputHat(uint8_t dir) {
  SwitchControlLibrary().MoveHat(dir);
}

void inputButton(uint8_t button, uint8_t state) {
    switch (button) {
      case A:
        if (state == PRESS) {
          SwitchControlLibrary().PressButtonA();
        } else if (state == RELEASE) {
          SwitchControlLibrary().ReleaseButtonA();
        }
        break;
      case B:
        if (state == PRESS) {
          SwitchControlLibrary().PressButtonB();
        } else if (state == RELEASE) {
          SwitchControlLibrary().ReleaseButtonB();
        }
        break;
      case X:
        if (state == PRESS) {
          SwitchControlLibrary().PressButtonX();
        } else if (state == RELEASE) {
          SwitchControlLibrary().ReleaseButtonX();
        }
        break;
      case Y:
        if (state == PRESS) {
          SwitchControlLibrary().PressButtonY();
        } else if (state == RELEASE) {
          SwitchControlLibrary().ReleaseButtonY();
        }
        break;
      case R:
        if (state == PRESS) {
          SwitchControlLibrary().PressButtonR();
        } else if (state == RELEASE) {
          SwitchControlLibrary().ReleaseButtonR();
        }
        break;
      case L:
        if (state == PRESS) {
          SwitchControlLibrary().PressButtonL();
        } else if (state == RELEASE) {
          SwitchControlLibrary().ReleaseButtonL();
        }
        break;
      case ZR:
        if (state == PRESS) {
          SwitchControlLibrary().PressButtonZR();
        } else if (state == RELEASE) {
          SwitchControlLibrary().ReleaseButtonZR();
        }
        break;
      case ZL:
        if (state == PRESS) {
          SwitchControlLibrary().PressButtonZL();
        } else if (state == RELEASE) {
          SwitchControlLibrary().ReleaseButtonZL();
        }
        break;  
      case RCLICK:
        if (state == PRESS) {
          SwitchControlLibrary().PressButtonRClick();
        } else if (state == RELEASE) {
          SwitchControlLibrary().ReleaseButtonRClick();
        }
        break;        
      case LCLICK:
        if (state == PRESS) {
          SwitchControlLibrary().PressButtonLClick();
        } else if (state == RELEASE) {
          SwitchControlLibrary().ReleaseButtonLClick();
        }
        break;
      case HOME:
        if (state == PRESS) {
          SwitchControlLibrary().PressButtonHome();
        } else if (state == RELEASE) {
          SwitchControlLibrary().ReleaseButtonHome();
        }
        break;
      case CAPTURE:
        if (state == PRESS) {
          SwitchControlLibrary().PressButtonCapture();
        } else if (state == RELEASE) {
          SwitchControlLibrary().ReleaseButtonCapture();
        }
        break;
      case PLUS:
        if (state == PRESS) {
          SwitchControlLibrary().PressButtonPlus();
        } else if (state == RELEASE) {
          SwitchControlLibrary().ReleaseButtonPlus();
        }
        break;
      case MINUS:
        if (state == PRESS) {
          SwitchControlLibrary().PressButtonMinus();
        } else if (state == RELEASE) {
          SwitchControlLibrary().ReleaseButtonMinus();
        }
        break;                
    }
}

void inputStick(uint8_t stick, uint8_t stateX, uint8_t stateY) {
    switch (stick) {
      case RSTICK:
        SwitchControlLibrary().MoveRightStick(stateX, stateY);
        break;
      case LSTICK:
        SwitchControlLibrary().MoveLeftStick(stateX, stateY);
        break;
    }
}

void loop() {
  while (mySerial.available() > 0) {
    uint8_t data[3];
    data[0] = mySerial.read();
    if (data[0] & 0x80) {
      // 制御信号
      if (data[0] & 0x40) {
        // ボタン入力
        if (data[0] & 0x20) {
          // HAT
          inputHat(data[0] & 0x0f);
        } else {
          // ボタン
          inputButton(data[0] & 0x0f, (data[0] & 0x10) >> 4);
        }
      } else if (mySerial.available() >= 2) {
        // スティック入力
        for (int i = 0; i < 2; i++) {
          data[i+1] = mySerial.read();
        }
        if (!(data[1] & 0x80) && !(data[2] & 0x80)) {
          inputStick((data[0] & 0x10) >> 4, data[1] << 1, data[2] << 1);
        }
      }
    }
  }
}
