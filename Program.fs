module InputValueModule
open System
open System.Diagnostics
open System.Windows.Forms
open System.Drawing
open System.IO

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

        let rec greedy lista aktualisPont megoldas selected =
            match lista with
            | [] -> List.rev megoldas, List.rev selected

            | fej :: maradek ->
              match aktualisPont with
              | Some p when fej.Also <= p ->
                  // már lefedett intervallum
                  greedy maradek aktualisPont megoldas selected
              | _ ->
                  // új vágópont kell
                  let ujPont = fej.Felso
                  greedy maradek (Some ujPont) (ujPont :: megoldas) (fej :: selected)

        greedy rendezett None [] []

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

let graf (nodeIntervall: Intervall list): IntervallumGraf =
        { Nodes =
          nodeIntervall
          |> List.mapi (fun i intervall -> (i+1, intervall))
        }
let exportGraphviz (graf: IntervallumGraf) (cutIntval:Intervall list)=
        let nodes =
            graf.Nodes
            |> List.map (fun (id,i) ->

                let containsCutPoint = List.contains i cutIntval

                if containsCutPoint then
                    
                    sprintf "  %d [label=\"[%d,%d]\", style=filled, fillcolor=red];"
                        id i.Also i.Felso
                else
                    
                     sprintf "  %d [label=\"[%d,%d]\", style=filled, fillcolor=lightblue];"
                         id i.Also i.Felso
        )

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
let loadImage path =
    use fs = new FileStream(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite)
    Image.FromStream(fs)
let drawIntervals (intervals: Intervall list) =
    let width = 800
    let height = 400

    let bmp = new Bitmap(width, height)
    use g = Graphics.FromImage(bmp)

    g.Clear(Color.White)

    let pen = new Pen(Color.Blue, 2.0f)
    let axisPen = new Pen(Color.Black, 2.0f)

    g.DrawLine(axisPen, 50, height - 50, width - 50, height - 50)

    let minInterval = intervals |> List.minBy (fun i -> i.Also)
    let maxInterval = intervals |> List.maxBy (fun i -> i.Felso)

    let minX = minInterval.Also
    let maxX = maxInterval.Felso

    let scale x =
        50 + (x - minX) * (width - 100) / (maxX - minX + 1)

    let mutable rows : (int list) list = []

    let getRow (start, finish) =
        let rec findRow i =
            if i >= rows.Length then
                rows <- rows @ [[finish]]
                i
            else
                let lastEnd = rows.[i] |> List.last
                if start > lastEnd then
                    rows <- rows |> List.mapi (fun idx r ->
                        if idx = i then r @ [finish] else r)
                    i
                else
                    findRow (i + 1)
        findRow 0

    for i in intervals do
        let s = i.Also
        let e = i.Felso

        let row = getRow (s, e)
        let y = height - 70 - row * 30

        let x1 = scale s
        let x2 = scale e

        g.DrawLine(pen, x1, y, x2, y)
        g.FillEllipse(Brushes.Red, x1 - 3, y - 3, 6, 6)
        g.FillEllipse(Brushes.Red, x2 - 3, y - 3, 6, 6)

    bmp
let runGui () =
 
    let form = new Form(Text="Intervallum vizualizáló", Width=900, Height=600)
    
    
    let input = new TextBox(Text="10")
    let btn = new Button(Text="Generate & Run")
    let picture = new PictureBox()
    picture.SizeMode <- PictureBoxSizeMode.Zoom
    let split = new SplitContainer()
    split.Dock <- DockStyle.Fill
    split.Orientation <- Orientation.Vertical
    split.SplitterDistance <- 450

    let status = new StatusStrip()
    let statusLabel = new ToolStripStatusLabel("")

    let pictureLeft = new PictureBox()
    pictureLeft.Dock <- DockStyle.Fill
    pictureLeft.SizeMode <- PictureBoxSizeMode.Zoom

    let pictureRight = new PictureBox()
    pictureRight.Dock <- DockStyle.Fill
    pictureRight.SizeMode <- PictureBoxSizeMode.Zoom

    status.Items.Add(statusLabel) |> ignore
    input.Dock <- DockStyle.Top
    btn.Dock <- DockStyle.Top
    
    status.Dock <- DockStyle.Bottom
    combo.Dock <- DockStyle.Top
    split.Panel1.Controls.Add(pictureLeft)
    split.Panel2.Controls.Add(pictureRight)
    form.Controls.Add(status)
    form.Controls.Add(split)
    form.Resize.Add(fun _ ->
    split.SplitterDistance <- form.ClientSize.Width / 2
    )

    btn.Click.Add(fun _ ->
    let db = Int32.Parse(input.Text)

    let data = randomIntervallumok db
    let (_, selected) = minimumVagopont data
    
    

    [pictureLeft; pictureRight]
    |> List.iter (fun pic ->
        match pic.Image with
        | null -> ()
        | img -> img.Dispose()
    )

    let intervalImage = drawIntervals data

    let graphImage =
        let g = graf data
        exportGraphviz g selected
        runGraphviz()
        loadImage "graf.png"

    pictureLeft.SizeMode <- PictureBoxSizeMode.Zoom
    pictureLeft.Dock <- DockStyle.Fill
    pictureLeft.Image <- intervalImage

    pictureRight.SizeMode <- PictureBoxSizeMode.Zoom
    pictureRight.Dock <- DockStyle.Fill
    pictureRight.Image <- graphImage

    statusLabel.Text <- "Split view megjelenítve"
    )
        
    picture.MouseEnter.Add(fun _ ->
    if picture.Image <> null then
        statusLabel.Text <- "Kattints a képre a nagyításhoz"
    )
    picture.Click.Add(fun _ ->
    statusLabel.Text <- "Kép megnyitva új ablakban"
    )
    picture.MouseLeave.Add(fun _ ->
    statusLabel.Text <- "Kész"
    )
    

    pictureLeft.Click.Add(fun _ ->
    if pictureLeft.Image <> null then
        let viewer = new Form(Text="Nagyított nézet", Width=1200, Height=800)

        let bigPic = new PictureBox(Dock=DockStyle.Fill)
        bigPic.Image <- pictureLeft.Image
        bigPic.SizeMode <- PictureBoxSizeMode.Zoom

        viewer.Controls.Add(bigPic)
        viewer.Show()
    )

    pictureRight.Click.Add(fun _ ->
        if pictureRight.Image <> null then
            let viewer = new Form(Text="Nagyított nézet", Width=1200, Height=800)

            let bigPic = new PictureBox(Dock=DockStyle.Fill)
            bigPic.Image <- pictureRight.Image
            bigPic.SizeMode <- PictureBoxSizeMode.Zoom

            viewer.Controls.Add(bigPic)
            viewer.Show()
    )  
    
    
    form.Controls.Add(btn)
    form.Controls.Add(input)
    form.Controls.Add(status)
    form.Controls.Add(combo)
    form.Controls.Add(split)

    split.SplitterDistance <- form.ClientSize.Width / 2

    form.Resize.Add(fun _ ->
    split.SplitterDistance <- form.ClientSize.Width / 2
    )

    Application.Run(form)
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
        let hanyszor = readInt "How many intervalls do we have"
        let dbIntervallumok =
            [ for i in 1 ..hanyszor  ->
                printfn "\n%d. intervallum:" i
                bekeresIntervall() ]
        printfn "\nThe given intervalls:"
        dbIntervallumok |@ printDbIntervallumok
        let rendezett = dbIntervallumok |> List.sortBy _.Felso
        rendezett |@ printDbIntervallumok

        let (vagopontok, selected) = minimumVagopont dbIntervallumok
        vagopontok |@ (fun p -> printfn "Vágópont: %d" p)
        let actualGraf = graf dbIntervallumok
        exportGraphviz actualGraf selected
        runGraphviz()
        Process.Start(ProcessStartInfo("graf.png", UseShellExecute = true)) |> ignore
        menu()

    | "2" ->
        printfn "Greedy algoritmus fut"
        menu()

    | "3" ->
        printfn "random intervallum generálása..."
        let db = readInt "Hány intervallumot generáljunk? "
        let randomLista = randomIntervallumok db
        printfn "Generált intervallumok:"
        randomLista |@ printDbIntervallumok
        let (vagopontok,selected) = minimumVagopont randomLista
        printfn "Vágópontok:"
        vagopontok |@ (fun p -> printfn "%d" p)
        let actualGraf = graf randomLista
        exportGraphviz actualGraf selected
        runGraphviz()
        Process.Start(ProcessStartInfo("graf.png", UseShellExecute = true)) |> ignore
        menu()
    | "4" ->
        printfn "Greedy vs BruteForce összehasonlítás"

        let db = readInt "Hány intervallumot generáljunk? (max 15 ajánlott!) "

        let intervallumok = randomIntervallumok db

        printfn "\nIntervallumok:"
        intervallumok |> List.take (min db 20) |@ printDbIntervallumok

        // GREEDY
        let sw1 = Stopwatch.StartNew()
        let (greedyPoints, greedyIntervals) = minimumVagopont intervallumok
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
               true
            else
                let union = Set.union onlyGreedy onlybruteForce
                printfn "The point that are different: %A" union
                false 

        printfn "\n--- EREDMÉNYEK ---"

        printfn "Greedy vágópontok száma: %d" greedyPoints.Length
        printfn "Greedy idő: %d ms" sw1.ElapsedMilliseconds

        printfn "\nBrute force vágópontok száma: %d" bruteResult.Length
        printfn "Brute force idő: %d ms" sw2.ElapsedMilliseconds

        printfn "\nMegoldások egyeznek? %b" (isSame greedyPoints bruteResult)

        menu()
    | "0" ->
        printfn "Kilépés"

    | _ ->
        printfn "Hibás választás"
        menu()
let runApp argv =
        match argv with
        | [| "gui" |] -> runGui()
        | _ -> menu()
[<EntryPoint>]
let main argv =
    runApp argv
    0