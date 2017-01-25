open Newtonsoft.Json
open Suave
open Suave.Files
open Suave.Filters
open Suave.Logging
open Suave.Operators
open Suave.RequestErrors
open Suave.Successful
open Suave.Writers
open System
open System.IO
open System.Net

open JsonConverterHelper
open JsonApiServer.PokeUtil

let app =
    choose [
        path "/" >=> Files.browseFileHome "index.html"
        path "/bundle.js" >=> Files.browseFileHome "bundle.js"
            >=> Writers.setHeader "Cache-Control" "no-cache, no-store, must-revalidate"
            >=> Writers.setHeader "Pragma" "no-cache"
            >=> Writers.setHeader "Expires" "0"
        path "/pokemon" >=> (OK (JsonConvert.SerializeObject(allPokemon, JsonConverter())))
            >=> setMimeType "application/json; charset=utf-8"
        NOT_FOUND "404 - Not found."
    ]


let serverConfig port =
  { defaultConfig with
      logger = Targets.create LogLevel.Debug [||]
      homeFolder = Some (Path.GetFullPath "./public")
      bindings = [ HttpBinding.create HTTP (IPAddress.Parse "0.0.0.0") port] }

let rec startServer port =
    try 
        printfn "Starting with port %A" port
        startWebServer (serverConfig port) app
        0
    with
    | :? System.Net.Sockets.SocketException -> 
        printfn "%A in use, incrementing port number." port
        startServer (port + 1us)
    | _ -> -1

[<EntryPoint>]
let main argv = 
    startServer 8080us
