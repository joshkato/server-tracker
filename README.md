# server-tracker

Simple application to keep track of servers and which environment each server resides in.

## Requirements

- .NET Core 3.0 Preview 6
- node.js
- npm
- yarn

## Build

From within the `src/ServerTracker` directory run:

**Windows**

```
dotnet publish -r win-x64 -c Release -o ..\..\dist
```

**Linux**

```
dotnet publish -r linux-x64 -c Release -o ../../dist
```