module App
open Fable.Core
open Fable.Import
open Elmish

// MODEL

type Msg =
  | Increment
  | Decrement

let init () = 0

// UPDATE

let update (msg:Msg) count =
  match msg with
  | Increment -> count + 1
  | Decrement -> count - 1

// rendering views with React
module R = Fable.Helpers.React
open Fable.Core.JsInterop
open Fable.Helpers.React.Props

let view count dispatch =
  let onClick msg =
    OnClick <| fun _ -> msg |> dispatch

  R.div []
    [ R.button [ onClick Decrement ] [ unbox "-" ]
      R.div [] [ unbox (string count) ]
      R.button [ onClick Increment ] [ unbox "+" ] ]

open Elmish.React

// App
Program.mkSimple init update view
|> Program.withReact "elmish-app"
|> Program.run