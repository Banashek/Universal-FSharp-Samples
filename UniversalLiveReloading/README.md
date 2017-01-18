# Universal Live Reloading Example

This example shows how to get live-reloading working with both a Suave backend and a Fable-Elmish frontend.

Steps to setup:

1. Install the latest dotnet core cli tools (msbuild based). My version is 1.0.0-preview5-004460.
    * [https://github.com/dotnet/cli](https://github.com/dotnet/cli)
2. Clone the repository
    * `git clone https://github.com/Banashek/Universal-FSharp-Samples`
3. Restore and run the projects ( I typically do this in two terminal tabs )
    1. Server
        * `cd src`
        * `dotnet restore`
        * `dotnet watch run`
    2. Client
        * `cd src/client`
        * `npm run dev`

## Things to know
* Server
    * Watch the output of the server command to see which port it binds to. It will increment the port to find an unused one upon restarting.