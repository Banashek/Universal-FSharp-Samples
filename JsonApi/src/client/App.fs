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

let getPokeBGHoverColor t =
  match t with
  | Normal -> "#DEDFDF"
  | Fire -> "#E31023"
  | Water -> "#67B0FF"
  | Electric -> "#C5A51C"
  | Grass -> "#67A369"
  | Ice -> "#84D9D9"
  | Fighting -> "#A14F45"
  | Poison -> "#9310EB"
  | Ground -> "#8E4618"
  | Flying -> "#8EBEFF"
  | Psychic -> "#381A55"
  | Bug -> "#778E43"
  | Rock -> "#494747"
  | Ghost -> "#6F65B9"
  | Dragon -> "#939A7B"
  | Dark -> "#939A7B"
  | Steel -> "#939A7B"
  | Fairy -> "#939A7B"

let pokeLabel =
  R.label [] [unbox "Pokemon"]

let pokeCardFront p =
  R.div
    [ Style
        [ BackfaceVisibility "hidden"
          Display "flex"
          FlexDirection "column"
          FlexItemAlign "center"
          JustifyContent "center"
          Position "absolute"
          Height "100%"
          CSSProp.Width "100%"
          ZIndex 1. ]]
    [ R.img
        [ Src p.img
          Style
            [ AlignSelf "center" ]] []
      R.label
        [ Style
            [ AlignSelf "center"
              Color "white"]]
            [ unbox p.name ]]

let pokeCardBack p =
  R.div
    [ Style
        [ BackfaceVisibility "hidden"
          Position "absolute"
          Height "100%"
          CSSProp.Width "100%"
          CSSProp.Transform "rotateY(-180deg)"
          ZIndex 2.]]
    [ R.label
        [ Style
            [ AlignSelf "center"
              Color "white"]]
            [ unbox p.name ]]

let pokeCard p =
  R.div
    [ ClassName "poke-card"
      Style
        [ CSSProp.Width "100%"
          Height "100%"
          TransformStyle "preserve-3d"
          Transition "0.5s" ]]
    [ pokeCardFront p
      pokeCardBack p ]

let flipComponent (c : obj) transform =
  c?style?webkitTransform <- transform
  c?style?MozTransform <- transform
  c?style?transform <- transform

let pokeComponent p =
  let bgColor = getPokeBGColor (List.head p.pokemonType)
  let bgHoverColor = getPokeBGHoverColor (List.head p.pokemonType)
  R.div
    [ ClassName "poke-component"
      Style
        [ BackgroundColor bgColor
          Border "1px solid black"
          BorderTopLeftRadius "10px"
          BorderTopRightRadius "10px"
          BorderBottomLeftRadius "10px"
          BorderBottomRightRadius "10px"
          Display "flex"
          Height "200px"
          CSSProp.Width "200px"
          FlexDirection "column"
          JustifyContent "center"
          FlexItemAlign "center"
          Margin "10px"
          Perspective "800" ]
      OnClick (fun e -> Browser.console.log p.name)
      OnMouseOver (fun e ->
        let pokeComponent = e.target?closest(".poke-component")
        pokeComponent?style?backgroundColor <- bgHoverColor
        let pokeCard =
          if isNull (e.target?closest(".poke-card"))
          then
            ((unbox<ResizeArray<Browser.Element>> (e.target?getElementsByClassName("poke-card"))).[0])
          else
            (unbox<Browser.Element> (e.target?closest(".poke-card")))
        flipComponent pokeCard "rotateY(-180deg)" )
      OnMouseOut (fun e ->
        let pokeComponent = e.target?closest(".poke-component")
        pokeComponent?style?backgroundColor <- bgColor
        let pokeCard = e.target?closest(".poke-card")
        flipComponent pokeCard "rotateY(0deg)") ]
    [ pokeCard p ]

let pokeList model =
  model.pokemon
  |> List.map (fun p -> pokeComponent p)

let pokeListComponent model =
  R.div
    [ Style
        [ Display "flex"
          FlexWrap "wrap" ]]
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
