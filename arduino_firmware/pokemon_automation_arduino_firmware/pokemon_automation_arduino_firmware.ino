#include <SoftwareSerial.h>
#include <SwitchControlLibrary.h>

enum {
  HAT = 0,
  RSTICK,
  LSTICK,
  A = 4,
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
  PRESS = 0,
  RELEASE
};

SoftwareSerial mySerial(11, 10); // RX, TX

void setup() {
  //connect to switch
  for (int i = 0; i < 3; ++i) {
    SwitchControlLibrary().PressButtonZL();
    SwitchControlLibrary().PressButtonZR();
    delay(500);
    SwitchControlLibrary().ReleaseButtonZL();
    SwitchControlLibrary().ReleaseButtonZR();
    delay(500);
  }
  
  mySerial.begin(115200);
}

void loop() {
  // put your main code here, to run repeatedly:
  if (mySerial.available() > 0) {
    int button_type, data0, data1;

    button_type = mySerial.read();
    while (mySerial.available() <= 0) {}
    data0 = mySerial.read();
    while (mySerial.available() <= 0) {}
    data1 = mySerial.read();

    switch (button_type) {
      case HAT:
        SwitchControlLibrary().MoveHat(data0);
        break;
      case RSTICK:
        SwitchControlLibrary().MoveRightStick(data0, data1);
        break;
      case LSTICK:
        SwitchControlLibrary().MoveLeftStick(data0, data1);
        break;
      case A:
        if (data0 == PRESS) {
          SwitchControlLibrary().PressButtonA();
        } else if (data0 == RELEASE) {
          SwitchControlLibrary().ReleaseButtonA();
        }
        break;
      case B:
        if (data0 == PRESS) {
          SwitchControlLibrary().PressButtonB();
        } else if (data0 == RELEASE) {
          SwitchControlLibrary().ReleaseButtonB();
        }
        break;
      case X:
        if (data0 == PRESS) {
          SwitchControlLibrary().PressButtonX();
        } else if (data0 == RELEASE) {
          SwitchControlLibrary().ReleaseButtonX();
        }
        break;
      case Y:
        if (data0 == PRESS) {
          SwitchControlLibrary().PressButtonY();
        } else if (data0 == RELEASE) {
          SwitchControlLibrary().ReleaseButtonY();
        }
        break;
      case R:
        if (data0 == PRESS) {
          SwitchControlLibrary().PressButtonR();
        } else if (data0 == RELEASE) {
          SwitchControlLibrary().ReleaseButtonR();
        }
        break;
      case L:
        if (data0 == PRESS) {
          SwitchControlLibrary().PressButtonL();
        } else if (data0 == RELEASE) {
          SwitchControlLibrary().ReleaseButtonL();
        }
        break;
      case ZR:
        if (data0 == PRESS) {
          SwitchControlLibrary().PressButtonZR();
        } else if (data0 == RELEASE) {
          SwitchControlLibrary().ReleaseButtonZR();
        }
        break;
      case ZL:
        if (data0 == PRESS) {
          SwitchControlLibrary().PressButtonZL();
        } else if (data0 == RELEASE) {
          SwitchControlLibrary().ReleaseButtonZL();
        }
        break;  
      case RCLICK:
        if (data0 == PRESS) {
          SwitchControlLibrary().PressButtonRClick();
        } else if (data0 == RELEASE) {
          SwitchControlLibrary().ReleaseButtonRClick();
        }
        break;        
      case LCLICK:
        if (data0 == PRESS) {
          SwitchControlLibrary().PressButtonLClick();
        } else if (data0 == RELEASE) {
          SwitchControlLibrary().ReleaseButtonLClick();
        }
        break;
      case HOME:
        if (data0 == PRESS) {
          SwitchControlLibrary().PressButtonHome();
        } else if (data0 == RELEASE) {
          SwitchControlLibrary().ReleaseButtonHome();
        }
        break;
      case CAPTURE:
        if (data0 == PRESS) {
          SwitchControlLibrary().PressButtonCapture();
        } else if (data0 == RELEASE) {
          SwitchControlLibrary().ReleaseButtonCapture();
        }
        break;
      case PLUS:
        if (data0 == PRESS) {
          SwitchControlLibrary().PressButtonPlus();
        } else if (data0 == RELEASE) {
          SwitchControlLibrary().ReleaseButtonPlus();
        }
        break;
      case MINUS:
        if (data0 == PRESS) {
          SwitchControlLibrary().PressButtonMinus();
        } else if (data0 == RELEASE) {
          SwitchControlLibrary().ReleaseButtonMinus();
        }
        break;                
    }
  }
}
