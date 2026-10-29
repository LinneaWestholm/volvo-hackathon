# volvo-hackathon-2026
GUI for the Volvo hackathon, built as an ASP.NET Core Blazor web app (.NET 10, interactive server rendering).

## Run

Requires the [.NET 10 SDK](https://dotnet.microsoft.com/download).

Double-click `Start.bat` in File Explorer, or run it from a terminal (`.\Start.bat`). It works from any folder.

Or run the command yourself from the `volvo-hackathon-2026` folder:

```powershell
cd volvo-hackathon-2026
dotnet watch --project src/VolvoHackathon
```

This opens the app in your browser and reloads it when you save a file. Use `dotnet run --project src/VolvoHackathon` to run without reloading.
The app listens on http://localhost:5167 (https://localhost:7056 with `--launch-profile https`).

In VS Code with the C# Dev Kit extension you can also press **F5**.

## Project layout

```
VolvoHackathon.slnx
src/VolvoHackathon/
  Program.cs                 Startup: registers services and Blazor
  Components/
    App.razor                HTML shell (render mode is set here: interactive server everywhere)
    Layout/
      MainLayout.razor       Centered content area (70% wide, full height)
    Pages/
      FactoryMonitoring.razor  The app's only page (at /): logo and title, agent cards, and the agent chat
    Shared/
      AgentCard.razor        One agent: robot, name, status icon
      AgentChat.razor        WhatsApp-style chat of messages from the factory API
      AgentColors.cs         Each agent's colour, shared by its card and chat bubbles
  Models/                    Plain data classes (FactoryResponse matches the API server)
  Services/
    FactoryPollingService.cs Polls the factory API (FactoryApi in appsettings.json)
    AgentChatService.cs      Chat messages, shared by all users
    AgentStatusService.cs    Agents' running state from the API
  wwwroot/
    app.css                  Global styles and theme colours (CSS variables in :root)
    images/                  Volvo logo and chat background
```

## Adding a page

1. Create `src/VolvoHackathon/Components/Pages/Reports.razor`:

   ```razor
   @page "/reports"

   <PageTitle>Reports</PageTitle>
   <h1>Reports</h1>
   ```

2. Open it at http://localhost:5167/reports. There is no navigation menu, so add a link
   (`<a href="reports">Reports</a>`) wherever you want one.

## Tips

- Register new services in `Program.cs`. Use `AddSingleton` for data shared by everyone and `AddScoped` for data that belongs to a single browser tab.
- Styles in `Foo.razor.css` apply only to `Foo.razor`. Use `wwwroot/app.css` for global styles.
