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
let rec bekeresIntervall() =
    let also = readInt "Alsó határ:"
    let felso = readInt "Felso hatar:"
    if felso < also then
        printfn "Hiba: a felső határ nem lehet kisebb, mint az alsó! Próbáld újra.\n"
        bekeresIntervall()
    else 
        { Also = also; Felso = felso }
let (|@) intervalls db =
    List.iter db intervalls

[<EntryPoint>]
let main argv =
    let hanyszor = readInt "How many intervalls do we have"
    let dbIntervallumok =
        [ for i in 1 ..hanyszor  ->
            printfn "\n%d. intervallum:" i
            bekeresIntervall() ]
    printfn "\nThe given intervalls:"
    let printDbIntervallumok i =
        printfn "[%d; %d]" i.Also i.Felso
    dbIntervallumok |@ printDbIntervallumok
    let rendezett = dbIntervallumok |> List.sortBy _.Felso
    rendezett |@ printDbIntervallumok
    0