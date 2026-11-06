# KeyLocker

A lightweight Windows desktop app built with C# and WinForms (.NET 8) that lets you lock and unlock keyboard input with a single action.

> **Status:** Early development. The project scaffold is in place and core features are being built.

<img width="355" height="358" alt="image" src="https://github.com/user-attachments/assets/332c0108-749f-4b32-a9b1-eac70c871f04" />
<img width="193" height="67" alt="image" src="https://github.com/user-attachments/assets/7213d595-7275-4409-8cd8-441c6d549795" />


## Features

- Lock keyboard input on demand
- Unlock quickly with a button or hotkey
- Minimal, native Windows UI (WinForms)
- No external dependencies

> Planned features are listed under [Roadmap](#roadmap).

## Tech Stack

- C#
- .NET 8 (`net8.0-windows`)
- Windows Forms

## Requirements

- Windows 10 or later
- [.NET 8 SDK](https://dotnet.microsoft.com/download/dotnet/8.0) (to build from source)

## Getting Started

```bash
# Clone the repository
git clone https://github.com/MohammadMosavat/KeyLocker.git
cd KeyLocker

# Restore and build
dotnet build

# Run
dotnet run
```

You can also open `KeyLocker.csproj` in Visual Studio 2022 and press **F5**.

## Project Structure

```
KeyLocker/
├── Program.cs           # Application entry point
├── Form1.cs             # Main window logic
├── Form1.Designer.cs    # Main window UI (auto-generated)
└── KeyLocker.csproj     # Project configuration
```

## Roadmap

- [ ] Global keyboard hook to block input
- [ ] Unlock hotkey / password
- [ ] System tray support
- [ ] Settings (auto-lock, startup with Windows)
- [ ] Installer / release builds

## Contributing

Contributions are welcome. Open an issue to discuss what you'd like to change, then submit a pull request.

## License

No license has been specified yet. Consider adding one (e.g. [MIT](https://choosealicense.com/licenses/mit/)).

## Author

**Mohammad Mosavat** — [@MohammadMosavat](https://github.com/MohammadMosavat)
