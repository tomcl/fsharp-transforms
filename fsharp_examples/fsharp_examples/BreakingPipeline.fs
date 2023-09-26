// -------------------------------- 12. Breaking a Pipeline -------------------------------- //

// A pipeline may lead to code that is dificult to understand if its stages do not make sense individually.
// In that case breaking the pipeline with a let defined identifier that documents an intermediate value can make code simpler. (DUI).

module pipelines
// Consider this long pipeline
let result = [1; 2; 3; 4; 5]
                |> List.map (fun x -> x * x)
                |> List.filter (fun x -> x % 2 = 0)
                |> List.sum
                |> string
                |> printfn "The result is %s"

// Now let's break it up
let squared = [1; 2; 3; 4; 5] |> List.map (fun x -> x * x)


let filtered = squared |> List.filter (fun x -> x % 2 = 0)


let result2 = filtered
              |> List.sum
              |> string
              |> printfn "The result is %s"