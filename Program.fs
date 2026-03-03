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
    let hanyszor = readInt "How intervalls do we have"
    let dbIntervallumok =
        [ for i in 1 ..hanyszor  ->
            printfn "\n%d. intervallum:" i
            bekeresIntervall() ]
    printfn "\nMegadott intervallumok:"
    dbIntervallumok
    |> List.iter (fun i ->
        printfn "[%d; %d]" i.Also i.Felso)
    0