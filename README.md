<div align=center>
  <img src="https://github.com/user-attachments/assets/383854aa-2a29-4cba-85da-935c21985f74"/>
</div>



# 🍓 BerrySonar

BerrySonar is a Linux-based C# application developed for the Raspberry Pi, designed to act as a DIY LIDAR system—similar in concept to the object detection systems used in autonomous vehicles like Teslas. It uses an ultrasonic sensor rotated by a stepper motor to scan a 180° field and detect nearby objects. The data is transmitted securely to a Firebase Realtime Database for monitoring and analysis.

## 🚀 Features
* 🧠 LIDAR-like Scanning – Scans a 180° arc using a rotating ultrasonic sensor.

* 🔄 Stepper Motor Rotation – Uses a 28BYJ-48 stepper motor to rotate the sensor precisely.

* 🧰 Raspberry Pi-Controlled – All hardware is interfaced and controlled via GPIO pins.

* ☁️ Firebase Realtime Database – Secure, cloud-based storage of distance and angle data.

* 🔐 Secure Writes – Only the super-admin can write data.

* 👀 Read-Only Access – A restricted admin account can read data.
  

## 🧱 Tech Stack
* Programming Language: C# (.NET Core)

* Platform: Linux (Raspberry Pi OS)

* Cloud: Firebase Realtime Database

* Security: Firebase Authentication & Realtime Database Rules

* Communication: GPIO with PWM for motor control & timing for ultrasonic pulse detection

## 🔩 Hardware Requirements
* 🧠 Raspberry Pi (any model with GPIO support; tested on Raspberry Pi 3/4)

* 📏 HC-SR04 Ultrasonic Sensor – For distance measurement

* 🔁 28BYJ-48 Stepper Motor with ULN2003 Driver Board – For rotating the sensor

* 🪛 Breadboard & Jumper Wires – For circuit connections
*🔌 5V Power Supply – To power the motor and sensor

## ⚙️ Configuration

### 🖥️ Software Configuration






