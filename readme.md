![WLED logo](https://raw.githubusercontent.com/Aircoookie/WLED/master/wled_logo.png)

## Welcome to the WLED app! (v1.0.2)

A brand new app for Android and UWP devices for discovering and controlling your [WLED](https://github.com/Aircoookie/WLED) devices easily!

### Features:
- Automatic device detection (mDNS)
- All lights are accessible from one list
- Opens control UI immediately if connected to WLED-AP
- Hide or delete devices

### FAQ:

#### My WLED lights are not found!

Ensure your phone/device is in the same network as the ESP.  
The "mDNS address" WiFi settings entry in WLED must be unique for every device.  
Try restarting both the ESP and your device.  
Attempt manually adding the device via its IP.

#### Can I control the lights when I'm not home?

In the default configuration this is not possible, the devices need to be in the same local network.  
However, you can either use a VPN to connect to your home network (if your router offers this option) or use a [port forwarding](https://github.com/Aircoookie/WLED/wiki/Remote-Access-and-IFTTT).  
Keep in mind that this exposes your light(s) to the public internet, so please be aware that it is not a secure solution.  
If you want to risk it, at least take the precaution of enabling the WLED [OTA lock](https://github.com/Aircoookie/WLED/wiki/Security) feature and, if possible, only connect it to a guest network.  

#### Why is there no iOS support?

While I would certainly see the appeal of an iOS version, Apple's Mac-development-only and Appstore-only policies make it hard for me personally to support the platform.  
That said, since this is a cross-platform app based on the Xamarin.Forms framework, adding iOS support is relatively easy if you have the infrastructure!  
(iOS testing device(s), MacOS computer for developing and already paying the annual store fee for another project)  
If you would like to help the project, feel free to PR any required changes for iOS support and/or publish it to the AppStore!  