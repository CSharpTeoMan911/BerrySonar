<div align=center>
  <img src="https://github.com/user-attachments/assets/383854aa-2a29-4cba-85da-935c21985f74"/>
</div>



# 🍓 BerrySonar

BerrySonar is a Linux-based C# application developed for the Raspberry Pi, designed to act as a DIY LIDAR system—similar in concept to the object detection systems used in autonomous vehicles like Teslas. It uses an ultrasonic sensor rotated by a stepper motor to scan a 180° field and detect nearby objects. The data is transmitted securely to a Firebase Realtime Database for monitoring and analysis. The GUI of the application is served as a **React** based application called [**berry-sonar-view**](https://github.com/CSharpTeoMan911/berry-sonar-view) that is getting the **LIDAR** metadata in real time using **Firebase real-time database** and rendering the data in a sonar UI element that is drew and redered dynamically in the browser 

<div align=center>
  <img src="https://github.com/user-attachments/assets/9cc8cfaa-de67-43fd-a39a-d0862947d435"/>
</div>

<br/>
<br/>

https://github.com/user-attachments/assets/8e78da88-58d7-42f9-b4d3-cf6c0d95aeeb



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

<br/>
<br/>

# ⚙️ Configuration

## 🖥️ Software Configuration

### Driver dependencies

Install the required **GPIO** drivers using APT.
```
sudo apt install gpio -y || sudo apt install wiringpi -y && sudo apt install libgpiod-dev -y
```

### Download the release

Download the application from the repository **[Release section](https://github.com/CSharpTeoMan911/BerrySonar/releases/tag/BerrySonar-1.0.0)** and run the application as sudo

```
sudo -i && ./BerrySonar
```

## 🔩 Hardware Setup

### 🧱 Required Hardware 🧠🔩
<br/>

#### 🔁 28BYJ-48 Electric Stepper Motor

🌀 Tiny but mighty! This 5V stepper motor rotates the ultrasonic sensor to scan the surroundings.

<div align=left >
  <img width=500 src="https://github.com/user-attachments/assets/fbac3535-ee24-4bf6-b57c-ff5c545c7df9"/>
</div>


<br/>
<br/>

#### ⚙️ ULN2003A Stepper Motor Driver Board

🧠 The brain behind the motor! Controls the motor's stepping sequence and power delivery.

<div align=left >
  <img width=500 src="https://github.com/user-attachments/assets/86c08075-af29-41e2-974a-137d16061d17"/>
</div>


<br/>
<br/>

#### 📡 HC-SR04 Ultrasonic Distance Sensor

👀 The “eyes” of BerrySonar. Sends sound waves and listens for echoes to measure object distances.

<div align=left >
  <img width=500 src="https://github.com/user-attachments/assets/357da6d0-c4aa-4fcc-992a-714fb9cbc2a5"/>
</div>


<br/>

🔩🧰 Hardware Configuration and Wiring

![BerrySonar pinnout legent schematic](https://github.com/user-attachments/assets/c9c23ffa-060f-4d21-9f7d-6c69aede8bb8)







