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
  { pokemon = [] }, Cmd.ofMsg QueryPokemon

let getPokemon (f : string) =
  promise {
    return! Fable.PowerPack.Fetch.fetchAs<PokeList> ("/pokemon") []
  }

let update msg model : Model * Cmd<Msg> =
  match msg with
  | QueryPokemon ->
    { model with pokemon = []}, Cmd.ofPromise getPokemon "" FetchSuccess FetchFailure
  | FetchSuccess pokeList ->
    { model with pokemon = pokeList}, []
  | FetchFailure ex ->
    Browser.console.log (unbox ex.Message)
    Browser.console.log "exception occured" |> ignore
    model, []

// Views

let pokeLabel =
  R.label [] [unbox "Pokemon"]

let pokeList model =
  model.pokemon
  |> List.map (fun p ->
    R.div []
      [ R.img [ Src p.img ] []
        R.label [] [ unbox p.name]
        R.br [] []])

let pokeListComponent model =
  R.div []
    ( [ R.label [] [unbox "Pokemon"]
        R.br [] [] ]
      @ (pokeList model))

let view model dispatch =
  pokeListComponent model

open Elmish.React

// App
Program.mkProgram init update view
|> Program.withReact "elmish-app"
|> Program.run
