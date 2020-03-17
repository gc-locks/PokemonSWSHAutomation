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

uint8_t Mul(uint8_t* M, int rows, int columns, uint8_t x) {
  std::vector<uint8_t> Mx(rows);
  for (int i=0; i < rows; i++) {
    Mx[i] = x & M[i];
  }

  uint8_t y = 0;
  for (int j=0; j < columns; j++) {
    uint8_t z = 0;
    for (int i=0; i < rows; i++) {
      z |= ((M[i] >> j) & 1) << i;
    }
    y ^= z;
  }
  return y;
}

uint8_t H[] = {
  0xff, 0x1f, 0x66, 0xaa
};

bool TryDecode(uint8_t x, uint8_t& decoded) {
  uint8_t Hx = Mul(H, 4, 8, x);
  if (!Hx) {
    decoded = ((x & 0x20) >> 2) | ((x & 0x0e) >> 1);
    return true;
  }

  uint8_t find = 0;
  for (int i=0; i < 4; i++) {
    find |= H[i] ^ ((Hx & (1 << i)) ? 0xff : 0x00);
  }
  find ^= 0xff;

  int index = -1;
  for (int i = 0; i < 8; i++) {
    if (find & (1 << i)) {
      if (index >= 0) {
        return false;
      }
      index = i;
    }
  }

  x ^= 1 << index;
  decoded = ((x & 0x20) >> 2) | ((x & 0x0e) >> 1);
  return true;
}

bool TryDecode2(uint8_t x[2], uint8_t& decoded) {
  uint8_t upper;
  if (!TryDecode(x[0], upper)) {
    return false;
  }

  if (!TryDecode(x[1], decoded)) {
    return false;
  }

  decoded = ((upper & 0x0f) << 4) | (decoded & 0x0f);
  return true;
}

void loop() {
  while (mySerial.available() >= 2) {
    uint8_t data[2];
    data[0] = mySerial.read();
    data[1] = mySerial.read();
    uint8_t input;
    if (!TryDecode2(data, input)) {
      continue;
    }

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
      } else if (mySerial.available() >= 4) {
        // スティック入力
        uint8_t inputs[2];
        for (int i = 0; i < 2; i++) {
          data[0] = mySerial.read();
          data[1] = mySerial.read();
          if (!TryDecode2(data, inputs[i])) {
            inputs[i] = 64;
          }
        }
        if (!(inputs[1] & 0x80) && !(inputs[2] & 0x80)) {
          inputStick((input & 0x10) >> 4, inputs[1] << 1, inputs[2] << 1);
        }
      }
    }
  }
}
