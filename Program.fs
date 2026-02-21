module InputValueModule
open System

type Intervall = {
    Also: int
    Felso: int
}

let rec private readInt prompt =
    printf "%s" prompt
    let input= Console.ReadLine()
    match Int32.TryParse(input) with
    | true, value -> value 
    | false, _ ->
        printfn "Hibás szám, újra!"
        readInt prompt
let bekeresIntervall() =
    let also = readInt "Alsó határ:"
    let felso = readInt "Felso hatar:"
    { Also = also; Felso = felso }

[<EntryPoint>]
let main argv =
    let interval = bekeresIntervall()
    printfn "Az intervallum [%d; %d] " interval.Also interval.Felso
    0