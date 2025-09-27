# Zenith Fan Utility

<img src="https://github.com/user-attachments/assets/2b680b93-4c25-46db-a384-e90ab6d851ff" alt="Zenith Fan Utility Screenshot" width="45%" />

A modern, reliable, and feature-rich fan control utility for ASUS laptops, rebuilt from the ground up for stability, performance, and a polished user experience.

> [!NOTE]
> ### NOTE
> The latest Asus System Control Interface v3.1.**41+** restricts access to the driver required for "manual" fan control to SYSTEM processes only. So if it doesn't work for you, you can:
> 
> ### Method 1
> 1. [Uninstall the latest Asus System Control Interface](https://github.com/seerge/g-helper/wiki/Troubleshooting#reinstalling-asus-system-control-interface)
> 2. Install the previous [version 3.1.40](https://dlcdnets.asus.com/pub/ASUS/GamingNB/Image/Software/SoftwareandUtility/16402/ASUSSystemControlInterfacev3_ASUS_Z_V3.1.40.0_16402.exe) (or earlier)
> 3. If Windows Update still overrides it, go to `Device Manager -> System Devices -> Asus System Control Interface`, then select `Driver -> Roll Back Driver` to 3.1.40 
>    
> #### Method 2
> 1. Download and extract [PSTools](https://download.sysinternals.com/files/PSTools.zip)
> 2. Place `PsExec.exe` into the same folder as `GHelper.exe`
> 3. Run in PowerShell as Administrator:  
>    ```powershell
>    ./psexec -i -s "$PWD\GHelper.exe"

# [:floppy_disk: Download](https://github.com/realMoai/ZenithFanUtility/releases)

---
## Features

Zenith Fan Utility provides a comprehensive toolset to take full control of your laptop's cooling system.

* 📈 **Custom Fan Curves:** Create and switch between two distinct fan curve profiles for automated, temperature-based fan control.
* 🖱️ **Interactive Curve Editor:** A smooth, intuitive editor to add, remove, and drag points to define your perfect fan curve.
* 💨 **Manual Control:** Instantly take over with a simple slider, or use the "Max Speed" and "Fans Off" buttons for quick actions.
* 🌡️ **Live Monitoring:** Real-time display of CPU & GPU temperatures and fan RPMs, right on the main window.
* 🧠 **Hysteresis Control:** Prevents annoying fan "flapping" by adding a configurable temperature buffer, ensuring fan speeds change smoothly.
* ⚙️ **Configurable Refresh Rate:** Adjust the stats refresh interval to your liking, from rapid updates to power-saving intervals.
* 🎨 **Polished Dark UI:** A beautiful, custom-built dark theme that is **DPI-aware**, ensuring it looks sharp and crisp on modern high-resolution displays.
* 🖥️ **Persistent Tray Icon:** A G-Helper-style tray icon that provides live stats on hover and toggles the main window's visibility with a single click.
* 🚀 **Startup Ready:** Configure the app to start automatically with Windows, minimized to the tray.

---
## How to Use

* **Master Control:** The "Enable Custom Fan Control" checkbox is the master switch. When checked, the app takes over your system's default fan behavior.
* **Mode Selection:** Use the radio buttons to switch between **Manual Mode** (slider is active), **Fan Curve 1**, or **Fan Curve 2**. When a fan curve is active, the manual slider is disabled.
* **Tray Icon:** The app lives in your system tray. A single left-click on the icon will show or hide the main window. Right-clicking provides options to show or exit the application completely.

---
## Acknowledgments

This project was built to be a modern, stable fan control utility. It would not have been possible without the foundational work of several open-source developers:

* **[Karmel0x](https://github.com/Karmel0x/AsusFanControl):** Created the original proof-of-concept and the core library for hardware interaction.
* **[Pgian88](https://github.com/pgain88/Asus-Fan):** Provided a foundational update with many new features in a prior version.
* **[Darren80](https://github.com/Darren80/AsusFanControlEnhanced):** Developed the original visual fan curve editor concept.
* **[BrightenedDay](https://github.com/BrightenedDay/AsusFanControl):** Implemented the initial theming features.

---
## License

Distributed under the MIT License.
