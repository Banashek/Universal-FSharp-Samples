module JsonApiServer.PokeUtil

open Newtonsoft.Json
open SharedTypes.Types

let allPokemon =
    [
        { id = 1; num = "001"; name = "Bulbasaur"; img = "http://www.serebii.net/pokemongo/pokemon/001.png"; pokemonType = [ Grass; Poison ]; height = 0.71; weight = 6.9; weaknesses = [ Fire; Ice; Flying; Psychic ]; evolutions = [{ num = "002"; name = "Ivysaur"}; { num = "003"; name = "Venasaur"}] }
    ]