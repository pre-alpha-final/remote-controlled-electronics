# Raspberry Pi Zero W Servo
Code for controlling the Feetech FS90 servo. 

# Notes
- add "dtoverlay=pwm-2chan" to /boot/config.txt (verify with ls /sys/class/pwm)
- red is plus, brown is minus, orange is control
- 50Hz, 180deg, duty 2.5%-12%
- servo pins

![](/docs/servo-pins.png)

# Example usage

![](/docs/the-homer.jpg)
