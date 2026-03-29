module InputValueModule
open System
open System.Diagnostics


type Intervall = {
    Also: int
    Felso: int
}

type NodeId = int

type IntervallumGraf = {
    Nodes : (NodeId * Intervall) list
}

let minimumVagopont intervallumok =
        let rendezett = intervallumok |> List.sortBy _.Felso

        let rec greedy lista aktualisPont megoldas =
            match lista with
            | [] -> List.rev megoldas

            | fej :: maradek ->
              match aktualisPont with
              | Some p when fej.Also <= p ->
                  // már lefedett intervallum
                  greedy maradek aktualisPont megoldas
              | _ ->
                  // új vágópont kell
                  let ujPont = fej.Felso
                  greedy maradek (Some ujPont) (ujPont :: megoldas)

        greedy rendezett None []

let flashError () =
    let oldBg = Console.BackgroundColor
    let oldFg = Console.ForegroundColor

    Console.BackgroundColor <- ConsoleColor.Red
    Console.ForegroundColor <- ConsoleColor.White
    printf " "
    System.Threading.Thread.Sleep(80)
    printf "\b"

    Console.BackgroundColor <- oldBg
    Console.ForegroundColor <- oldFg

let readInt prompt =
    printf "%s" prompt
    let rec loop (acc:string) =
        let key = Console.ReadKey(true)

        match key.Key with
        | ConsoleKey.Enter ->
            printfn ""
            if acc = "" then 0 else Int32.Parse(acc)

        | ConsoleKey.Backspace ->
            if acc.Length > 0 then
                printf "\b \b"
                loop (acc.Substring(0, acc.Length - 1))
            else
                Console.Beep()
                loop acc

        | _ ->
            if Char.IsDigit(key.KeyChar) then
                printf "%c" key.KeyChar
                loop (acc + string key.KeyChar)
            else
                flashError()
                loop acc

    loop ""
let randomIntervallumok db =
    let rnd = Random()

    [ for _ in 1..db ->
        let a = rnd.Next(0,100)
        let b = rnd.Next(a, a + rnd.Next(1,50))
        { Also = a; Felso = b } ]
let printDbIntervallumok i =
        printfn "[%d; %d]" i.Also i.Felso
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
let lefed (pontok:int list) (intervallumok:Intervall list) =
    intervallumok
    |> List.forall (fun i ->
        pontok |> List.exists (fun p -> i.Also <= p && p <= i.Felso))
let rec subsets list =
    match list with
    | [] -> [[]]
    | x::xs ->
        let rest = subsets xs
        rest @ (rest |> List.map (fun r -> x::r))
let minimumVagopontBrute intervallumok =
    let pontok =
        intervallumok
        |> List.map (fun i -> i.Felso)
        |> List.distinct

    let osszes = subsets pontok

    osszes
    |> List.filter (fun p -> lefed p intervallumok)
    |> List.minBy List.length        
let rec menu () =
    printfn ""
    printfn "1 - Intervallumok"
    printfn "2 - Greedy"
    printfn "3 - Random teszt"
    printfn "4 - Greedy and BruteForce simaulation" 
    printfn "0 - Kilépés"

    match Console.ReadLine() with
    | "1" ->
        printfn "Intervallum bevitel"
        menu()

    | "2" ->
        printfn "Greedy algoritmus fut"
        menu()

    | "3" ->
        printfn "random intervallum generálása..."
        let db = readInt "Hány intervallumot generáljunk? "
        let randomLista = randomIntervallumok 10
        printfn "Generált intervallumok:"
        randomLista |@ printDbIntervallumok
        let vagopontok = minimumVagopont randomLista
        printfn "Vágópontok:"
        vagopontok |@ (fun p -> printfn "%d" p)
        menu()
    | "4" ->
        printfn "Greedy vs BruteForce összehasonlítás"

        let db = readInt "Hány intervallumot generáljunk? (max 15 ajánlott!) "

        let intervallumok = randomIntervallumok db

        printfn "\nIntervallumok:"
        intervallumok |> List.take (min db 20) |@ printDbIntervallumok

        // GREEDY
        let sw1 = Stopwatch.StartNew()
        let greedyResult = minimumVagopont intervallumok
        sw1.Stop()

        // BRUTE FORCE
        let sw2 = Stopwatch.StartNew()
        let bruteResult = minimumVagopontBrute intervallumok
        sw2.Stop()

        // Check Brute Force and greedy
        let isSame (a:int list) (b:int list) =
            let setGreedy     = Set.ofList a
            let setbruteForce = Set.ofList b
            
            let onlyGreedy     = Set.difference setGreedy setbruteForce
            let onlybruteForce = Set.difference setbruteForce setGreedy

            if Set.isEmpty onlyGreedy && Set.isEmpty onlybruteForce then
               printfn("The two solution sare the same")
            else
                let union = Set.union setGreedy setbruteForce
                printfn "The point that are different: %A" union 

        printfn "\n--- EREDMÉNYEK ---"

        printfn "Greedy vágópontok száma: %d" greedyResult.Length
        printfn "Greedy idő: %d ms" sw1.ElapsedMilliseconds

        printfn "\nBrute force vágópontok száma: %d" bruteResult.Length
        printfn "Brute force idő: %d ms" sw2.ElapsedMilliseconds

        printfn "\nMegoldások egyeznek? %b" (isSame greedyResult bruteResult)

        menu()
    | "0" ->
        printfn "Kilépés"

    | _ ->
        printfn "Hibás választás"
        menu()

[<EntryPoint>]
let main argv =
    menu()
    let hanyszor = readInt "How many intervalls do we have"
    let dbIntervallumok =
        [ for i in 1 ..hanyszor  ->
            printfn "\n%d. intervallum:" i
            bekeresIntervall() ]
    printfn "\nThe given intervalls:"
    dbIntervallumok |@ printDbIntervallumok
    let rendezett = dbIntervallumok |> List.sortBy _.Felso
    rendezett |@ printDbIntervallumok

    
    let vagopontok = minimumVagopont dbIntervallumok
    vagopontok |@ (fun p -> printfn "Vágópont: %d" p)
    let graf : IntervallumGraf =
        { Nodes =
          dbIntervallumok
          |> List.mapi (fun i intervall -> (i+1, intervall))
        }
    let exportGraphviz (graf: IntervallumGraf) =
        let nodes =
            graf.Nodes
            |> List.map (fun (id,i) ->
                sprintf "  %d [label=\"%d\", style=filled, fillcolor=lightblue, tooltip=\"[%d,%d]\"];" 
                  id id i.Also i.Felso)

        let edges =
            graf.Nodes
            |> List.collect (fun (id1,i1) ->
               graf.Nodes
               |> List.choose (fun (id2,i2) ->
                  if id1 < id2 && i1.Also <= i2.Felso && i2.Also <= i1.Felso then
                     Some (sprintf "  %d -- %d;" id1 id2)
                  else None))

        let dot =
            ["graph G {"
             "  layout=neato;"
             "  overlap=false;"
             "  splines=true;"]
             @ nodes
             @ edges
             @ ["}"]

        System.IO.File.WriteAllLines("intervallumgraf.dot", dot)
    let runGraphviz () =
            let psi = ProcessStartInfo()
            psi.FileName <- "dot"
            psi.Arguments <- "-Tpng intervallumgraf.dot -o graf.png"
            psi.UseShellExecute <- false
            psi.CreateNoWindow <- true

            let p = Process.Start(psi)
            p.WaitForExit()   
    exportGraphviz graf
    runGraphviz()
    Process.Start("graf.png") |> ignore
    0