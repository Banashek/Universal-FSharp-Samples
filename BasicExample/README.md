# Basic Example

This example shows how to get a basic sample setup with both a Suave backend and a Fable-Elmish frontend.

Steps to setup:

1. Install the latest dotnet core cli tools (msbuild based). My version is 1.0.0-preview5-004460.
    * [https://github.com/dotnet/cli](https://github.com/dotnet/cli)
2. Install the Yarn package manager for node
    * [https://yarnpkg.com/en/docs/install](https://yarnpkg.com/en/docs/install)
3. Clone the repository
    * `git clone https://github.com/Banashek/Universal-FSharp-Samples`
4. Restore both projects
    1. Server
        * `cd BasicExample/src`
        * `dotnet restore`
    2. Client
        * `cd BasicExample/src/client`
        * `yarn install`
        * `npm run build`
5. Run the server
    1. `cd BasicExample/src`
    2. `dotnet run`
6. Browser to `localhost:8080`

## Things to know
* Server
    * Because the server always starts on the same port (8080), if you stop the application and then start it again you will receive a `SocketException`. This is because by default sockets are held open for a certain amount of time after the application using it stops listening on it.
