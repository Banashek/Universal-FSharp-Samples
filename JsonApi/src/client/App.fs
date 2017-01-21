module JsonApiClientWeb.App

open Fable.Core
open Fable.Core.JsInterop
open Fable.Helpers.React.Props
open Fable.Import
open Fable.PowerPack
open Elmish

open SharedTypes.Types

module R = Fable.Helpers.React

type PokeList = Pokemon list

type Model = 
  { pokemon : PokeList }

type Msg =
  | QueryPokemon
  | FetchSuccess of PokeList
  | FetchFailure of exn

let init () = 
  { pokemon = [] }, Cmd.none

let getPokemon =
  promise {
    return! Fable.PowerPack.Fetch.fetchAs<PokeList> ("/pokemon") []
  }

let update msg model : Model * Cmd<Msg> =
  match msg with
  | GetPokemon -> 
    { model with pokemon = []}, Cmd.ofPromise getQuery FetchSuccess FetchFailure
  | FetchSuccess PokeList ->
    { model with pokemon = PokeList}, []
  | FetchFailure _ ->
    Browser.console.log "exception occured" |> ignore
    model, []

let view model dispatch =
  let onClick msg =
    OnClick <| fun _ -> msg |> dispatch

  R.div []
    [ R.label [] [ unbox "Pokemon" ]
      // model.pokemon |> List.map (fun p -> R.label [] [ unbox p.name ])
    ]

open Elmish.React

// App
Program.mkProgram init update view
|> Program.withReact "elmish-app"
|> Program.run