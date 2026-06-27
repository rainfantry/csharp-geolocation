# C# Geolocation Example

A minimal Console app that grabs your GPS coordinates using the Windows Location API.

## What It Does

Click a button, get your latitude and longitude in a popup. That's it.

## How It Works

1. **Asks permission** — `Geolocator.RequestAccessAsync()` triggers the Windows location permission prompt
2. **Gets position** — `geolocator.GetGeopositionAsync()` asks Windows to figure out where you are
3. **Shows result** — coordinates displayed in a `MessageBox`

The code never touches hardware directly. Windows Location Services does the heavy lifting using whatever's available:

| Source | Accuracy | When It's Used |
|--------|----------|----------------|
| GPS chip | ~1-10m | Phones, some tablets |
| WiFi positioning | ~30-100m | Laptops with WiFi (most common on desktop) |
| IP address lookup | ~1-10km | Ethernet-only, no WiFi adapter |

`DesiredAccuracyInMeters = 10` tells Windows "try your best" — it won't magically add GPS to a laptop that doesn't have one.

## Key Concepts

### async / await
The geolocation calls take time (talking to hardware). `async` marks the method as "contains waiting." `await` marks the exact line where you pause. While waiting, the UI stays responsive — no frozen window.

### WinRT APIs in Console
`Windows.Devices.Geolocation` is a WinRT (Windows Runtime) API, not a standard .NET API. To access it from a Console project, the `.csproj` needs a Windows-specific target framework.

## Project Requirements

**.csproj target framework:**
```xml
<TargetFramework>net8.0-windows10.0.17763.0</TargetFramework>
```

- `net8.0` = the .NET version (runtime/compiler)
- `-windows10.0.17763.0` = unlocks WinRT APIs like Geolocation
- Without the `-windows` suffix, the project can't see `Windows.Devices.Geolocation`

**Minimum OS:** Windows 10 version 1809 (build 17763)

**Location Services** must be enabled in Windows Settings > Privacy > Location

## Files

- `MainWindow.xaml.cs` — the code-behind with full inline comments explaining every line
- `MainWindow.xaml` — the UI layout (not included here — just a Window with a Button wired to `ShowMsg`)

---

## TODO — Release Blackops

_Automated read-only assessment — what a full public-release pass would do for this repo. Suggestions only; nothing above has been changed or removed._

- [ ] Audit git history for AI/Claude attribution; scrub if any is found.
- [ ] Add discovery topics for SEO (`gh repo edit --add-topic ...`, up to 20).
- [ ] Cut a tagged release (`v1.0.0`); attach a build artifact if this ships a binary/app.
- [ ] Add a screenshot or diagram to the README if there's a GUI or visual output.
- [ ] Verify a clean from-scratch build/run against the README quick start (produce a real artifact, don't trust the docs).
- [ ] If this is a desktop app, make a self-contained build (bundle runtime assets/models into the binary; confirm it runs with no external files).

<sub>Workflow: https://github.com/rainfantry/release-blackops-skill</sub>
