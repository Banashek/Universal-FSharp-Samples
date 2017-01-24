open Suave
open Suave.Files
open Suave.Filters
open Suave.Logging
open Suave.Operators
open Suave.RequestErrors
open Suave.Successful
open System.IO

let app =
    choose [
        path "/" >=> Files.browseFileHome "index.html"
        path "/bundle.js" >=> Files.browseFileHome "bundle.js"
            >=> Writers.setHeader "Cache-Control" "no-cache, no-store, must-revalidate"
            >=> Writers.setHeader "Pragma" "no-cache"
            >=> Writers.setHeader "Expires" "0"
        NOT_FOUND "404 - Not found."
    ]


let serverConfig =
  { defaultConfig with
      logger = Targets.create LogLevel.Debug [||]
      homeFolder = Some (Path.GetFullPath "./public") }

[<EntryPoint>]
let main argv =
    startWebServer serverConfig app
    0
