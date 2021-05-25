<a href="https://play.google.com/store/apps/details?id=com.aircoookie.WLED&pcampaignid=MKT-Other-global-all-co-prtnr-py-PartBadge-Mar2515-1"><img alt="Get it on Google Play" src="https://play.google.com/intl/en_us/badges/images/generic/en_badge_web_generic.png" height="80" align="middle"/></a>
<a href="https://apps.apple.com/us/app/wled/id1475695033"><img alt="Download on the Apple App Store" src="https://raw.githubusercontent.com/Aircoookie/WLED-App/master/StoreImages/appstore_badge.svg?sanitize=true" height="55" align="middle"/></a>

## Welcome to the WLED app! (v1.0.4)

An app for Android, iPhone, iPad and UWP devices for discovering and controlling a [WLED](https://github.com/Aircoookie/WLED) devices easily!

### Features:
- Automatic device detection (mDNS)
- All lights are accessible from one list
- Opens control UI immediately if connected to WLED-AP
- Hide or delete devices

### FAQ:

#### My WLED lights are not found!

1. Ensure your phone/device is in the same network as the ESP.
1. The "mDNS address" WiFi settings entry in WLED must be unique for every device.
1. Try restarting both the ESP and your device.
1. Attempt manually adding the device via its IP address.

#### Can I control the lights when I'm not home?

In the default configuration this is not possible, the devices need to be in the same local network.
However, you can either use a VPN to connect to your home network (if your router offers this option) or use a [port forwarding](https://github.com/Aircoookie/WLED/wiki/Remote-Access-and-IFTTT).
Keep in mind that this exposes your light(s) to the public internet, so please be aware that it is not a secure solution.
If you want to risk it, at least take the precaution of enabling the WLED [OTA lock](https://github.com/Aircoookie/WLED/wiki/Security) feature and, if possible, only connect it to a guest network.

#### Where can I get the UWP app?

Right now my primary goal is Android support.
The UWP platform is tested and confirmed to work, but right now you need to install Xamarin.Forms for Visual Studio and build it yourself.
In the future, I will look into ways of distributing the UWP binaries via appstores or releases.

## Contribute

### Requirements

- [Visual Studio](https://visualstudio.microsoft.com/)
- [Visual Studio Tools for Xamarin](https://visualstudio.microsoft.com/xamarin/)

### Trademarks

Google Play and the Google Play logo are trademarks of Google LLC.

Apple, the Apple logo, iPhone, and iPad are trademarks of Apple Inc., registered in the U.S. and other countries and regions. App Store is a service mark of Apple Inc.