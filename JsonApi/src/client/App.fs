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

let getPokeBGColor t =
  match t with
  | Normal -> "#CDCDCD"
  | Fire -> "#D00006"
  | Water -> "#4A82FF"
  | Electric -> "#B79D04"
  | Grass -> "#549057"
  | Ice -> "#73C4CB"
  | Fighting -> "#924036"
  | Poison -> "#8400DC"
  | Ground -> "#803909"
  | Flying -> "#80ADFF"
  | Psychic -> "#2B0F48"
  | Bug -> "#687F35"
  | Rock -> "#363636"
  | Ghost -> "#6257AD"
  | Dragon -> "#818C6B"
  | Dark -> "#818C6B"
  | Steel -> "#818C6B"
  | Fairy -> "#818C6B"

let pokeLabel =
  R.label [] [unbox "Pokemon"]

let pokeComponent p =
  R.div
    [ Style
        [ BackgroundColor (unbox getPokeBGColor (List.head p.pokemonType)) 
          Border "1px solid black"
          BorderTopLeftRadius "10px"
          BorderTopRightRadius "10px"
          BorderBottomLeftRadius "10px"
          BorderBottomRightRadius "10px"
          Display "flex"
          FlexShrink 1.
          FlexDirection "column"
          Margin "10px"
          Padding "10px" ]
      OnClick (fun e -> Browser.console.log p.name) ]
    [ R.img
        [ Src p.img
          Style
            [ AlignSelf "center" ]] []
      R.label
        [ Style
            [ AlignSelf "center"
              Color "white"]]
            [ unbox p.name ]]

let pokeList model =
  model.pokemon
  |> List.map (fun p -> pokeComponent p)

let pokeListComponent model =
  R.div
    [ Style
        [ Display "flex" ]]
    (pokeList model)

let layout model =
  R.div
    [ Style
        [ Display "flex"
          FlexDirection "column" ]]
    [ R.label [] [unbox "Pokemon"]
      R.br [] []
      pokeListComponent model ]

let view model dispatch =
  layout model

open Elmish.React

// App
Program.mkProgram init update view
|> Program.withReact "elmish-app"
|> Program.run
